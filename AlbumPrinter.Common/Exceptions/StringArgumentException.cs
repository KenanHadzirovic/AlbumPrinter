using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Common.Exceptions
{
    /// <summary>
    /// Exception occurred with handling string data type
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StringArgumentException : ArgumentException
    {
        public StringArgumentException()
        {
        }

        public StringArgumentException(string message)
            : base(message)
        {
        }

        public StringArgumentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}