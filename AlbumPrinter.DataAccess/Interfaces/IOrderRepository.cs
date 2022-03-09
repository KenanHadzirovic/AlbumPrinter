using AlbumPrinter.Models;

namespace AlbumPrinter.DataAccess.Interfaces
{
    /// <summary>
    /// Repository for handling data operations of Order domain model
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Retrieve Order details from database for provided Order Id 
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>Order</returns>
        Task<Order?> GetOrderInfo(Guid orderId);
        /// <summary>
        /// Save new Order object into database
        /// </summary>
        /// <param name="order">New order object</param>
        /// <returns>Saved object</returns>
        Task<Order> SaveOrder(Order order);
    }
}
