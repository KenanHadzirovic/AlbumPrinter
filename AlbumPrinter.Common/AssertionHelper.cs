using AlbumPrinter.Common.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.Common
{
    /// <summary>
    /// Helper class for Asserting / validating common tasks
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AssertionHelper
    {
        /// <summary>
        /// Validate whether string parameter is null or empty
        /// </summary>
        /// <param name="param">Input parameter</param>
        /// <param name="exceptionMessage">(Optional) Custom exception message</param>
        /// <exception cref="StringArgumentException"></exception>
        public static void AssertString(string param, string? exceptionMessage = null)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new StringArgumentException(exceptionMessage ?? CommonExceptionMessages.StringArgumentNullOrEmpty);
            }
        }

        /// <summary>
        /// Validate whether Guid parameter is empty
        /// </summary>
        /// <param name="param">Input parameter</param>
        /// <param name="exceptionMessage">(Optional) Custom exception message</param>
        /// <exception cref="GuidArgumentException"></exception>
        public static void AssertGuid(Guid param, string? exceptionMessage = null)
        {
            if (param == Guid.Empty)
            {
                throw new GuidArgumentException(exceptionMessage ?? CommonExceptionMessages.GuidArgumentEmpty);
            }
        }

        /// <summary>
        /// Validate whether string is null, empty or just whitespace
        /// </summary>
        /// <param name="param">Input parameter</param>
        /// <param name="exceptionMessage">(Optional) Custom exception message</param>
        /// <exception cref="StringArgumentException"></exception>
        public static void AssertStringWhitespace(string param, string? exceptionMessage = null)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                throw new StringArgumentException(exceptionMessage ?? CommonExceptionMessages.StringArgumentNullOrEmpty);
            }
        }

        /// <summary>
        /// Validate whether int parameter is less than 0
        /// </summary>
        /// <param name="param">Input parameter</param>
        /// <param name="exceptionMessage">(Optional) Custom exception message</param>
        /// <exception cref="IntArgumentException"></exception>
        public static void AssertInt(int param, string? exceptionMessage = null)
        {
            if (param <= 0)
            {
                throw new IntArgumentException(exceptionMessage ?? CommonExceptionMessages.IntegerNegative);
            }
        }

        /// <summary>
        /// Validate whether object is null
        /// </summary>
        /// <param name="param">Input parameter</param>
        /// <param name="exceptionMessage">(Optional) Custom exception message</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AssertObject(object param, string? exceptionMessage = null)
        {
            if (param.IsNull())
            {
                throw new ArgumentNullException(exceptionMessage ?? CommonExceptionMessages.ObjectNull);
            }
        }
    }
}