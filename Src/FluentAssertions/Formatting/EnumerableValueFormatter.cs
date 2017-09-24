using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Formatting
{
    public class EnumerableValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is IEnumerable;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var enumerable = ((IEnumerable)value).Cast<object>().ToArray();
            if (enumerable.Any())
            {
                string postfix = "";

                int maxItems = 32;
                if (enumerable.Length > maxItems)
                {
                    postfix = $", ...{enumerable.Length - maxItems} more...";
                    enumerable = enumerable.Take(maxItems).ToArray();
                }

                return "{" + string.Join(", ", enumerable.Select((item, index) => formatChild(index.ToString(), item)).ToArray()) + postfix + "}";
            }
            else
            {
                return "{empty}";
            }
        }
    }
}