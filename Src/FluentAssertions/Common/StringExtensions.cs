using System;
using System.Linq;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    internal static class StringExtensions
    {
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
            string formattedString = Formatter.ToString(value.Substring(index, length));

            return $"{formattedString} (index {index})".Replace("{", "{{").Replace("}", "}}");
        }

        /// <summary>
        /// Replaces all characters that might conflict with formatting placeholders with their escaped counterparts.
        /// </summary>
        public static string EscapePlaceholders(this string value) =>
            value.Replace("{", "{{").Replace("}", "}}");

        /// <summary>
        /// Replaces all characters that might conflict with formatting placeholders with their escaped counterparts.
        /// </summary>
        internal static string UnescapePlaceholders(this string value) =>
            value.Replace("{{", "{").Replace("}}", "}");

        /// <summary>
        /// Joins a string with one or more other strings using a specified separator.
        /// </summary>
        /// <remarks>
        /// Any string that is empty (including the original string) is ignored.
        /// </remarks>
        public static string Combine(this string @this, string other, string separator = ".")
        {
            if (@this.Length == 0)
            {
                return (other.Length != 0) ? other : string.Empty;
            }

            if (other.Length == 0)
            {
                return @this;
            }

            return @this + separator + other;
        }

        /// <summary>
        /// Changes the first character of a string to uppercase.
        /// </summary>
        public static string Capitalize(this string @this)
        {
            if (@this.Length == 0)
            {
                return @this;
            }

            char[] charArray = @this.ToCharArray();
            charArray[0] = char.ToUpper(charArray[0]);
            return new string(charArray);
        }

        /// <summary>
        /// Appends tab character at the beginning of each line in a string.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string IndentLines(this string @this)
        {
            return string.Join(Environment.NewLine,
                @this.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => $"\t{x}"));
        }

        public static string RemoveNewLines(this string @this)
        {
            return @this.Replace("\n", "").Replace("\r", "").Replace("\\r\\n", "");
        }
    }
}
