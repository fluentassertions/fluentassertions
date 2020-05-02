using System;

namespace FluentAssertions.Common
{
    internal static class Guard
    {
        public static void ThrowIfArgumentIsNull<T>([ValidatedNotNull] T obj, string paramName)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void ThrowIfArgumentIsNull<T>([ValidatedNotNull] T obj, string paramName, string message)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static void ThrowIfArgumentIsNullOrEmpty([ValidatedNotNull] string str, string paramName)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void ThrowIfArgumentIsNullOrEmpty([ValidatedNotNull] string str, string paramName, string message)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        /// <summary>
        /// Workaround to make dotnet_code_quality.null_check_validation_methods work
        /// https://github.com/dotnet/roslyn-analyzers/issues/3451#issuecomment-606690452
        /// </summary>
        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class ValidatedNotNullAttribute : Attribute
        {
        }
    }
}
