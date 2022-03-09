using AlbumPrinter.Models;

namespace AlbumPrinter.DataAccess.Interfaces
{
    /// <summary>
    /// Repository for handling data operations of ProductType domain model
    /// </summary>
    public interface IProductTypeRepository
    {
        /// <summary>
        /// Retrieve all Product Types from database
        /// </summary>
        /// <returns>Collection of ProductType objects</returns>
        Task<ICollection<ProductType>> GetProductTypes();
    }
}
