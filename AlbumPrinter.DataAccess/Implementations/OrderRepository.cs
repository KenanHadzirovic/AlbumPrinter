using AlbumPrinter.Common;
using AlbumPrinter.DataAccess.Interfaces;
using AlbumPrinter.Models;
using AlbumPrinter.Resources;
using Microsoft.EntityFrameworkCore;

namespace AlbumPrinter.DataAccess
{
    /// <inheritdoc/>
    public class OrderRepository : IOrderRepository
    {
        private readonly AlbumPrinterContext _context;

        public OrderRepository(AlbumPrinterContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderInfo(Guid orderId)
        {
            AssertionHelper.AssertGuid(orderId, DomainExceptionMessages.OrderIdEmpty);

            return await _context.Orders
                                 .Include(x => x.OrderProductTypes)
                                 .ThenInclude(y => y.ProductType)
                                 .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order> SaveOrder(Order order)
        {
            // add validation
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (await _context.Orders.AnyAsync(x => x.OrderId == order.OrderId))
            {
                throw new ArgumentException(string.Format(DomainExceptionMessages.OrderExists, order.OrderId));
            }

            order.DateCreated = DateTime.UtcNow;
            var addedOrder = _context.Orders.Add(order).Entity;
            await _context.SaveChangesAsync();
            
            return addedOrder;
        }
    }
}
