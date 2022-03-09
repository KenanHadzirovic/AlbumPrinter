using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Common
{
    /// <summary>
    /// Common helper methods for object class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ObjectExtensions
    {
        /// <summary>
        /// Check whether object is null through fluent syntax
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(this object value)
        {
            return value == null;
        }

        /// <summary>
        /// Check whether object is not null through fluent syntax
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object value)
        {
            return !IsNull(value);
        }
    }
}