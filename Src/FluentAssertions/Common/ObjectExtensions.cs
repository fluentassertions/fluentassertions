using System;
using System.Globalization;
using System.Reflection;

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
                && (actual.IsNumericType() || actualType.IsEnumType())
                && (expected.IsNumericType() || expectedType.IsEnumType())
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
            return IsEnumType(targetType)
                ? Enum.ToObject(targetType, source)
                : Convert.ChangeType(source, targetType, CultureInfo.InvariantCulture);
        }

        private static bool IsNumericType(this object obj)
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

        private static bool IsEnumType(this Type type) => type.GetTypeInfo().IsEnum;
    }
}
