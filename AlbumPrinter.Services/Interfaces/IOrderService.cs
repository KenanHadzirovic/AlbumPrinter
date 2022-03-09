using AlbumPrinter.Dto;

namespace AlbumPrinter.Services.Interfaces
{
    /// <summary>
    /// Service that handles logic responsible for Order domain
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Retrieves OrderInfo for Order with provided identifier
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns></returns>
        Task<OrderDto> GetOrderInfo(Guid orderId);
        /// <summary>
        /// Creates new Order in the system
        /// </summary>
        /// <param name="createOrderRequestDto">Request for creating new Order object in the system</param>
        /// <returns></returns>
        Task<OrderBaseResponseDto> CreateOrder(CreateOrderRequestDto createOrderRequestDto);
    }
}
