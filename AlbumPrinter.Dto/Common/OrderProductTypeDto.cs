using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Dto
{
    /// <summary>
    /// Single Order Product
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class OrderProductTypeDto
    {
        /// <summary>
        /// Quantity of the ProductType in the Order
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Name of the ProductType
        /// </summary>
        public string ProductType { get; set; } = null!;
    }
}
