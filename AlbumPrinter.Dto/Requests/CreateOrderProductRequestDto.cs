using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Dto
{
    /// <summary>
    /// Request child object used in Order creation 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CreateOrderProductRequestDto
    {
        /// <summary>
        /// Name of the Product
        /// </summary>
        public string? ProductType { get; set; }
        /// <summary>
        /// Quantity of the Ordered product
        /// </summary>
        public int Quantity { get; set; }
    }
}
