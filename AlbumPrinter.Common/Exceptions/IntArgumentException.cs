using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Common.Exceptions
{
    /// <summary>
    /// Exception occurred with handling int data type
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IntArgumentException : ArgumentException
    {
        public IntArgumentException()
        {
        }

        public IntArgumentException(string message)
            : base(message)
        {
        }

        public IntArgumentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}