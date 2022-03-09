using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Dto
{
    /// <summary>
    /// Dto representation of Order object
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class OrderDto
    {
        /// <summary>
        /// Order identifier
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// UTC DateTime representing creation of Order
        /// </summary>
        public DateTime DateCreated { get; set; }

        public ICollection<OrderProductTypeDto>? OrderProductTypes { get; set; }
    }
}
