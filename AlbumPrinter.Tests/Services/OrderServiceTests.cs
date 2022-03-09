using AlbumPrinter.API.MappingProfiles;
using AlbumPrinter.DataAccess.Interfaces;
using AlbumPrinter.Dto;
using AlbumPrinter.Dto.Enums;
using AlbumPrinter.Models;
using AlbumPrinter.Resources;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumPrinter.Tests.Services
{
    [TestClass]
    public class OrderServiceTests
    {
        private readonly AlbumPrinter.Services.OrderService _orderService;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IProductTypeRepository> _productTypeRepositoryMock;
        private readonly Fixture _fixture;
        private readonly IMapper _mapper;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _productTypeRepositoryMock = new Mock<IProductTypeRepository>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });

            _mapper = mockMapper.CreateMapper();
            _orderService = new AlbumPrinter.Services.OrderService(_mapper, _orderRepositoryMock.Object, _productTypeRepositoryMock.Object);
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        }


        #region GetOrderInfo
        [TestMethod]
        public async Task GetOrderInfo_ValidOrderId_ShouldReturnSuccess()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            Order expectedOrder = _fixture.Create<Order>();
            OrderDto mappedExpectedOrder = _mapper.Map<OrderDto>(expectedOrder);
            _orderRepositoryMock.Setup(_ => _.GetOrderInfo(orderId)).ReturnsAsync(expectedOrder);

            // Act
            var result = await _orderService.GetOrderInfo(orderId);

            // Assert
            result.Should().BeEquivalentTo(mappedExpectedOrder);
        }

        [TestMethod]
        public void GetOrderInfo_EmptyOrderId_ShouldThrowArgumentException()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();

            // Act
            var result = () => _orderService.GetOrderInfo(orderId);

            // Assert
            result.Should().ThrowAsync<ArgumentException>();
        }
        #endregion GetOrderInfo

        #region CreateOrder
        [TestMethod]
        public void CreateOrder_EmptyOrder_ShouldThrowArgumentException()
        {
            // Arrange
            CreateOrderRequestDto? createOrderRequestDto = null;

            // Act
            var result = () => _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public void CreateOrder_EmptyOrderId_ShouldThrowArgumentException()
        {
            // Arrange
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.OrderId, new Guid())
                                                                  .Create();

            // Act
            var result = () => _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public void CreateOrder_DuplicateProducts_ShouldThrowArgumentException()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderProductRequestDto orderProduct = _fixture.Create<CreateOrderProductRequestDto>();
            orderProducts.Add(orderProduct);
            orderProducts.Add(orderProduct);
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();

            // Act
            var result = () => _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage(DomainExceptionMessages.DuplicateProductsNotAllowed);
        }

        [TestMethod]
        public void CreateOrder_InvalidProducts_ShouldThrowArgumentException()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderProductRequestDto orderProduct = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "Invalid")
                                                                .Create();
            orderProducts.Add(orderProduct);
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();

            // Act
            var result = () => _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage(DomainExceptionMessages.InvalidProductTypeName);
        }

        [TestMethod]
        public void CreateOrder_EmptyCart_ShouldThrowArgumentException()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();

            // Act
            var result = () => _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage(DomainExceptionMessages.EmptyCart);
        }

        [TestMethod]
        public void CreateOrder_InvalidQuantity_ShouldThrowArgumentException()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderProductRequestDto orderProduct = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "mug")
                                                                .With(x => x.Quantity, 0)
                                                                .Create();
            orderProducts.Add(orderProduct);
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();

            // Act
            var result = () => _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage(DomainExceptionMessages.InvalidProductQuantity);
        }

        [TestMethod]
        public void CreateOrder_MissingProductType_ShouldThrowArgumentException()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderProductRequestDto orderProduct = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "mug")
                                                                .With(x => x.Quantity, 5)
                                                                .Create();
            orderProducts.Add(orderProduct);
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();
            List<ProductType> productTypes = _fixture.CreateMany<ProductType>(2).ToList();
            _productTypeRepositoryMock.Setup(_ => _.GetProductTypes()).ReturnsAsync(productTypes);

            // Act
            var result = () => _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage(DomainExceptionMessages.InvalidProductTypeName);
        }

        [TestMethod]
        public async Task CreateOrder_ValidOrderDouble_ShouldReturnSuccess()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderProductRequestDto orderProduct = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "mug")
                                                                .With(x => x.Quantity, 5)
                                                                .Create();
            orderProducts.Add(orderProduct);
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();
            ProductType mugProductType = _fixture.Build<ProductType>()
                                                 .With(x => x.ProductTypeId, (int)ProductTypeEnum.Mugs)
                                                 .With(x => x.ProductTypeName, "mug")
                                                 .With(x => x.BinWidth, 94)
                                                 .Create();
            List<ProductType> productTypes = new List<ProductType>() { mugProductType };
            _productTypeRepositoryMock.Setup(_ => _.GetProductTypes()).ReturnsAsync(productTypes);

            // Act
            var result = await _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.BinWidth.Should().NotBe(null);
            result.BinWidth.Should().Be(mugProductType.BinWidth * 2);
        }

        [TestMethod]
        public async Task CreateOrder_ValidOrder_ShouldReturnSuccess()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderProductRequestDto orderProduct = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "mug")
                                                                .With(x => x.Quantity, 4)
                                                                .Create();
            orderProducts.Add(orderProduct);
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();
            ProductType mugProductType = _fixture.Build<ProductType>()
                                                 .With(x => x.ProductTypeId, (int)ProductTypeEnum.Mugs)
                                                 .With(x => x.ProductTypeName, "mug")
                                                 .With(x => x.BinWidth, 94)
                                                 .Create();
            List<ProductType> productTypes = new List<ProductType>() { mugProductType };
            _productTypeRepositoryMock.Setup(_ => _.GetProductTypes()).ReturnsAsync(productTypes);

            // Act
            var result = await _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.BinWidth.Should().NotBe(null);
            result.BinWidth.Should().Be(mugProductType.BinWidth);
        }


        [TestMethod]
        public async Task CreateOrder_ValidOrderMultipleItems_ShouldReturnSuccess()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderProductRequestDto orderProductMug = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "mug")
                                                                .With(x => x.Quantity, 4)
                                                                .Create();
            CreateOrderProductRequestDto orderProductCanvas = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "canvas")
                                                                .With(x => x.Quantity, 4)
                                                                .Create();
            orderProducts.Add(orderProductCanvas);
            orderProducts.Add(orderProductMug);
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();
            ProductType mugProductType = _fixture.Build<ProductType>()
                                                 .With(x => x.ProductTypeId, (int)ProductTypeEnum.Mugs)
                                                 .With(x => x.ProductTypeName, "mug")
                                                 .With(x => x.BinWidth, 94)
                                                 .Create();
            ProductType canvas = _fixture.Build<ProductType>()
                                                 .With(x => x.ProductTypeId, (int)ProductTypeEnum.Canvas)
                                                 .With(x => x.ProductTypeName, "canvas")
                                                 .With(x => x.BinWidth, 16)
                                                 .Create();
            List<ProductType> productTypes = new List<ProductType>() { canvas, mugProductType };
            _productTypeRepositoryMock.Setup(_ => _.GetProductTypes()).ReturnsAsync(productTypes);

            // Act
            var result = await _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.BinWidth.Should().NotBe(null);
            result.BinWidth.Should().Be(mugProductType.BinWidth + canvas.BinWidth * 4);
        }

        [TestMethod]
        public async Task CreateOrder_ValidOrderMultipleItemsWithMugs_ShouldReturnSuccess()
        {
            // Arrange
            List<CreateOrderProductRequestDto> orderProducts = new List<CreateOrderProductRequestDto>();
            CreateOrderProductRequestDto orderProductMug = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "mug")
                                                                .With(x => x.Quantity, 6)
                                                                .Create();
            CreateOrderProductRequestDto orderProductCanvas = _fixture.Build<CreateOrderProductRequestDto>()
                                                                .With(x => x.ProductType, "canvas")
                                                                .With(x => x.Quantity, 4)
                                                                .Create();
            orderProducts.Add(orderProductCanvas);
            orderProducts.Add(orderProductMug);
            CreateOrderRequestDto createOrderRequestDto = _fixture.Build<CreateOrderRequestDto>()
                                                                  .With(x => x.Products, orderProducts)
                                                                  .Create();
            ProductType mugProductType = _fixture.Build<ProductType>()
                                                 .With(x => x.ProductTypeId, (int)ProductTypeEnum.Mugs)
                                                 .With(x => x.ProductTypeName, "mug")
                                                 .With(x => x.BinWidth, 94)
                                                 .Create();
            ProductType canvas = _fixture.Build<ProductType>()
                                                 .With(x => x.ProductTypeId, (int)ProductTypeEnum.Canvas)
                                                 .With(x => x.ProductTypeName, "canvas")
                                                 .With(x => x.BinWidth, 16)
                                                 .Create();
            List<ProductType> productTypes = new List<ProductType>() { canvas, mugProductType };
            _productTypeRepositoryMock.Setup(_ => _.GetProductTypes()).ReturnsAsync(productTypes);

            // Act
            var result = await _orderService.CreateOrder(createOrderRequestDto);

            // Assert
            result.BinWidth.Should().NotBe(null);
            result.BinWidth.Should().Be(mugProductType.BinWidth * 2 + canvas.BinWidth * 4);
        }
        #endregion CreateOrder
    }
}
