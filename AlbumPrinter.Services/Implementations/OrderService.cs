using AlbumPrinter.Common;
using AlbumPrinter.DataAccess.Interfaces;
using AlbumPrinter.Dto;
using AlbumPrinter.Dto.Enums;
using AlbumPrinter.Models;
using AlbumPrinter.Resources;
using AlbumPrinter.Services.Interfaces;
using AutoMapper;

namespace AlbumPrinter.Services
{
    /// <inheritdoc/>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, IOrderRepository orderRepository, IProductTypeRepository productTypeRepository)
        {
            _orderRepository = orderRepository;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> GetOrderInfo(Guid orderId)
        {
            AssertionHelper.AssertGuid(orderId, DomainExceptionMessages.OrderIdEmpty);

            Order? orderInfo = await _orderRepository.GetOrderInfo(orderId);

            return _mapper.Map<OrderDto>(orderInfo);
        }

        public async Task<OrderBaseResponseDto> CreateOrder(CreateOrderRequestDto createOrderRequestDto)
        {
            // Validate input data
            AssertionHelper.AssertObject(createOrderRequestDto, DomainExceptionMessages.OrderRequestInvalid);
            AssertionHelper.AssertGuid(createOrderRequestDto.OrderId, DomainExceptionMessages.OrderIdEmpty);
            if (AnyDuplicateOrderProducts(createOrderRequestDto.Products))
            {
                throw new ArgumentException(DomainExceptionMessages.DuplicateProductsNotAllowed);
            }

            if(!createOrderRequestDto.Products.Any())
            {
                throw new ArgumentException(DomainExceptionMessages.EmptyCart);
            }

            if (createOrderRequestDto.Products.Any(x => x.Quantity <= 0))
            {
                throw new ArgumentException(DomainExceptionMessages.InvalidProductQuantity);
            }

            // Get product definitions
            ICollection<ProductType> productTypes = await _productTypeRepository.GetProductTypes();

            if (CheckForInvalidProductTypes(createOrderRequestDto.Products, productTypes))
            {
                throw new ArgumentException(DomainExceptionMessages.InvalidProductTypeName);
            }

            // Format order
            Order newOrder = CreateOrderProductsWithTypes(createOrderRequestDto, productTypes);

            // Save to DB
            await _orderRepository.SaveOrder(newOrder);

            // Calculate response
            decimal calculatedBinWidth = CalculateBinWidth(newOrder.OrderProductTypes, productTypes);

            return new OrderBaseResponseDto()
            {
                BinWidth = calculatedBinWidth,
                OrderId = createOrderRequestDto.OrderId,
            };
        }

        private bool CheckForInvalidProductTypes(ICollection<CreateOrderProductRequestDto> orderProducts, ICollection<ProductType> productTypes)
        {
            return orderProducts.Any(orderProduct => !productTypes.Any(productType => productType.ProductTypeName == orderProduct.ProductType));
        }

        private bool AnyDuplicateOrderProducts(ICollection<CreateOrderProductRequestDto> orderProducts)
        {
            return orderProducts.GroupBy(x => x.ProductType)
                                .Any(g => g.Count() > 1);
        }

        private Order CreateOrderProductsWithTypes(CreateOrderRequestDto createOrderRequestDto, ICollection<ProductType> productTypes)
        {
            Order newOrder = new Order()
            {
                OrderId = createOrderRequestDto.OrderId
            };

            foreach(var product in createOrderRequestDto.Products)
            {
                ProductType? productType = productTypes.FirstOrDefault(x => x.ProductTypeName == product.ProductType);
                
                if (productType == null)
                {
                    throw new ArgumentException(DomainExceptionMessages.InvalidProductTypeName);
                }

                newOrder.OrderProductTypes.Add(new OrderProductType()
                {
                    OrderProductId = Guid.NewGuid(),
                    OrderId = createOrderRequestDto.OrderId,
                    ProductTypeId = productType.ProductTypeId,
                    Quantity = product.Quantity
                });
            }

            return newOrder;
        }

        private decimal CalculateBinWidth(ICollection<OrderProductType> orderProducts, ICollection<ProductType> productTypes)
        {
            decimal binWidth = 0;

            // Calculate common width (without mugs)
            foreach(OrderProductType product in orderProducts.Where(x => x.ProductTypeId != (int)ProductTypeEnum.Mugs))
            {
                decimal productWidth = productTypes.First(x => x.ProductTypeId == product.ProductTypeId).BinWidth;
                binWidth += product.Quantity * productWidth;
            }

            // Calculate mug width
            OrderProductType? mugProduct = orderProducts.FirstOrDefault(x => x.ProductTypeId == (int)ProductTypeEnum.Mugs);
            if (mugProduct != null)
            {
                ProductType mugProductTypeDefinition = productTypes.First(x => x.ProductTypeId == (int)ProductTypeEnum.Mugs);

                binWidth += CalculateParallelItemsWidth(mugProduct.Quantity, mugProductTypeDefinition.BinWidth);
            }

            return binWidth;
        }

        private decimal CalculateParallelItemsWidth(int quantity, decimal binWidth, int parallelStack = 4)
        {
            // Calculate amount of rows of parallel items
            decimal width = quantity / parallelStack * binWidth;
            
            // Add extra row for more items that dont fit in parallel
            if(quantity % parallelStack != 0)
            {
                width += binWidth;
            }

            return width;
        }
    }
}