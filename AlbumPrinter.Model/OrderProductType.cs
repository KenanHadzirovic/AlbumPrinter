using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Models
{
    /// <summary>
    /// Helper model for linking Orders and Products in the database. Represents many-to-many link between these domains.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class OrderProductType
    {
        /// <summary>
        /// Primary Key identifier of the relationship
        /// </summary>
        public Guid OrderProductId { get; set; }
        /// <summary>
        /// Quantity of the ordered product
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Order identifier for the parent
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// ProductType identifier
        /// </summary>
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Navigation property for parent Order
        /// </summary>
        public virtual Order Order { get; set; } = null!;
        /// <summary>
        /// Navigation property for child ProductType
        /// </summary>
        public virtual ProductType ProductType { get; set; } = null!;
    }
}
