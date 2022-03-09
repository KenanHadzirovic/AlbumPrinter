using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Dto
{
    /// <summary>
    /// Minimal Dto of Base properties representing Order upon creation
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OrderBaseResponseDto
    {
        /// <summary>
        /// Order Identifier
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// Required width for storage of the Order
        /// </summary>
        public decimal BinWidth { get; set; }
    }
}
