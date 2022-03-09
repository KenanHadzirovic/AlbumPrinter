using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Models
{
    /// <summary>
    /// List of available ProductTypes in the database
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ProductType
    {
        public ProductType()
        {
            OrderProductTypes = new HashSet<OrderProductType>();
        }

        /// <summary>
        /// ProductType identifier
        /// </summary>
        public int ProductTypeId { get; set; }
        /// <summary>
        /// Name of the ProductType
        /// </summary>
        public string ProductTypeName { get; set; } = null!;
        /// <summary>
        /// Description of the ProductType
        /// </summary>
        public string? ProductTypeDescription { get; set; }
        /// <summary>
        /// Width of the specific ProductType
        /// </summary>
        public decimal BinWidth { get; set; }

        /// <summary>
        /// Navigation property for Order - ProductType relationship
        /// </summary>
        public virtual ICollection<OrderProductType> OrderProductTypes { get; set; }
    }
}
