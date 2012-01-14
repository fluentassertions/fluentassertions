using System;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Formatting;

using System.Linq;

namespace FluentAssertions.Common
{
    internal static class Extensions
    {
        public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> expression)
        {
            if (ReferenceEquals(expression, null))
            {
                throw new NullReferenceException("Expected a property expression, but found <null>.");
            }

            PropertyInfo propertyInfo = AttemptToGetPropertyInfoFromCastExpression(expression);
            if (propertyInfo == null)
            {
                propertyInfo = AttemptToGetPropertyInfoFromPropertyExpression(expression);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("Cannot use <" + expression.Body + "> when a property expression is expected.");
            }

            return propertyInfo;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromPropertyExpression<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return (PropertyInfo)memberExpression.Member;
            }

            return null;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromCastExpression<T>(Expression<Func<T, object>> expression)
        {
            var castExpression = expression.Body as UnaryExpression;
            if (castExpression != null)
            {
                return (PropertyInfo)((MemberExpression)castExpression.Operand).Member;
            }

            return null;
        }

        /// <summary>
        /// Finds the first index at which the <paramref name="value"/> does not match the <paramref name="expected"/>
        /// string anymore, including the exact casing.
        /// </summary>
        public static int IndexOfFirstMismatch(this string value, string expected)
        {
            return IndexOfFirstMismatch(value, expected, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Finds the first index at which the <paramref name="value"/> does not match the <paramref name="expected"/>
        /// string anymore, accounting for the specified <paramref name="stringComparison"/>.
        /// </summary>
        public static int IndexOfFirstMismatch(this string value, string expected, StringComparison stringComparison)
        {
            for (int index = 0; index < value.Length; index++)
            {
                if ((index >= expected.Length) || !value[index].ToString().Equals(expected[index].ToString(), stringComparison))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the quoted three characters at the specified index of a string, including the index itself.
        /// </summary>
        public static string IndexedSegmentAt(this string value, int index)
        {
            int length = Math.Min(value.Length - index, 3);

            return string.Format("{0} (index {1})", Formatter.ToString(value.Substring(index, length)), index);
        }

        public static bool IsSameOrEqualTo(this object actual, object expected)
        {
            if (ReferenceEquals(actual, null) && ReferenceEquals(expected, null))
            {
                return true;
            }

            if (ReferenceEquals(actual, null))
            {
                return false;
            }

            return actual.Equals(expected);
        }

        public static bool IsSameOrInherits(this Type actualType, Type expectedType)
        {
            return (actualType == expectedType) || (expectedType.IsSubclassOf(actualType));
        }

        /// <summary>
        /// Replaces all characters that might conflict with formatting placeholders and newlines with their escaped counterparts.
        /// </summary>
        public static string FirstLine(this string value)
        {
            string [] lines = value.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return lines.First();
        }

        /// <summary>
        /// Replaces all characters that might conflict with formatting placeholders and newlines with their escaped counterparts.
        /// </summary>
        public static string Escape(this string value)
        {
            return value.Replace("\"", "\\\"").Replace("\n", @"\n").Replace("\r", @"\r").Replace("{", "{{").Replace(
                "}", "}}");
        }
    }
}
