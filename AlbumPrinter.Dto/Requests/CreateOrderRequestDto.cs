using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Dto
{
    /// <summary>
    /// Request for creating a new Order in the system
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CreateOrderRequestDto
    {
        /// <summary>
        /// Order identifier for the new object
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// List of Products that belong to the Order
        /// </summary>
        public List<CreateOrderProductRequestDto>? Products { get; set; }
    }
}
