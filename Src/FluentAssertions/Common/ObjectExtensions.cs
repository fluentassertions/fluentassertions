using System;
using System.Collections.Generic;
using System.Globalization;

namespace FluentAssertions.Common
{
    internal static class ObjectExtensions
    {
        public static Func<T, T, bool> GetComparer<T>()
        {
            if (typeof(T).IsValueType)
            {
                // Avoid causing any boxing for value types
                return (actual, expected) => EqualityComparer<T>.Default.Equals(actual, expected);
            }

            if (typeof(T) != typeof(object))
            {
                // CompareNumerics is only relevant for numerics boxed in an object.
                return (actual, expected) => actual is null
                    ? expected is null
                    : expected is not null && EqualityComparer<T>.Default.Equals(actual, expected);
            }

            return (actual, expected) => actual is null
                    ? expected is null
                    : expected is not null
                        && (EqualityComparer<T>.Default.Equals(actual, expected) || CompareNumerics(actual, expected));
        }

        private static bool CompareNumerics(object actual, object expected)
        {
            Type expectedType = expected.GetType();
            Type actualType = actual.GetType();

            return actualType != expectedType
                && actual.IsNumericType()
                && expected.IsNumericType()
                && CanConvert(actual, expected, actualType, expectedType)
                && CanConvert(expected, actual, expectedType, actualType);
        }

        private static bool CanConvert(object source, object target, Type sourceType, Type targetType)
        {
            try
            {
                var converted = source.ConvertTo(targetType);

                return source.Equals(converted.ConvertTo(sourceType))
                     && converted.Equals(target);
            }
            catch
            {
                // ignored
                return false;
            }
        }

        private static object ConvertTo(this object source, Type targetType)
        {
            return Convert.ChangeType(source, targetType, CultureInfo.InvariantCulture);
        }

        private static bool IsNumericType(this object obj)
        {
            // "is not null" is due to https://github.com/dotnet/runtime/issues/47920#issuecomment-774481505
            return obj is not null and (
                int or
                long or
                float or
                double or
                decimal or
                sbyte or
                byte or
                short or
                ushort or
                uint or
                ulong);
        }
    }
}
