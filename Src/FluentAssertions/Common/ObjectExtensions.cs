using System;
using System.Globalization;

namespace FluentAssertions.Common
{
    public static class ObjectExtensions
    {
        public static bool IsSameOrEqualTo(this object actual, object expected)
        {
            if (actual is null && expected is null)
            {
                return true;
            }

            if (actual is null)
            {
                return false;
            }

            if (expected is null)
            {
                return false;
            }

            if (actual.Equals(expected))
            {
                return true;
            }

            Type expectedType = expected.GetType();
            Type actualType = actual.GetType();

            return actualType != expectedType
                && IsNumericType(actual)
                && IsNumericType(expected)
                && CanConvert(actual, expected, actualType, expectedType)
                && CanConvert(expected, actual, expectedType, actualType);
        }

        private static bool CanConvert(object source, object target, Type sourceType, Type targetType)
        {
            try
            {
                var converted = Convert.ChangeType(source, targetType, CultureInfo.InvariantCulture);

                return source.Equals(Convert.ChangeType(converted, sourceType, CultureInfo.InvariantCulture))
                     && converted.Equals(target);
            }
            catch
            {
                // ignored
                return false;
            }
        }

        private static bool IsNumericType(object obj)
        {
            switch (obj)
            {
                case int _:
                case long _:
                case float _:
                case double _:
                case decimal _:
                case sbyte _:
                case byte _:
                case short _:
                case ushort _:
                case uint _:
                case ulong _:
                    return true;
                default:
                    return false;
            }
        }
    }
}
