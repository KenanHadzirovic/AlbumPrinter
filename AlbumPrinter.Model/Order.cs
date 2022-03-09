using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Models
{
    /// <summary>
    /// Order object representation in the database
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class Order
    {
        public Order()
        {
            OrderProductTypes = new HashSet<OrderProductType>();
        }

        /// <summary>
        /// Order identifier
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// UTC DateTime set upon object creation in the database
        /// </summary>
        public DateTime DateCreated { get; set; }
        
        /// <summary>
        /// Navigation collection of Products for a specific Order
        /// </summary>
        public virtual ICollection<OrderProductType> OrderProductTypes { get; set; }
    }
}
