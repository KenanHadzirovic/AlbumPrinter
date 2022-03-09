using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Common.Exceptions
{
    /// <summary>
    /// Exception occurred with handling Guid data type
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GuidArgumentException : ArgumentException
    {
        public GuidArgumentException()
        {
        }

        public GuidArgumentException(string message)
            : base(message)
        {
        }

        public GuidArgumentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}