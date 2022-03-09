using AlbumPrinter.DataAccess.Interfaces;
using AlbumPrinter.Models;
using Microsoft.EntityFrameworkCore;

namespace AlbumPrinter.DataAccess
{
    /// <inheritdoc/>
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly AlbumPrinterContext _context;

        public ProductTypeRepository(AlbumPrinterContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ProductType>> GetProductTypes()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}
