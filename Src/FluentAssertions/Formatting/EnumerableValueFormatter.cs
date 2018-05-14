using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

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
            ICollection<object> enumerable = ((IEnumerable)value).ConvertOrCastToCollection<object>();

            if (enumerable.Any())
            {
                string postfix = "";

                const int maxItems = 32;
                if (enumerable.Count > maxItems)
                {
                    postfix = $", …{enumerable.Count - maxItems} more…";
                    enumerable = enumerable.Take(maxItems).ToArray();
                }

                return "{" + string.Join(", ", enumerable.Select((item, index) => formatChild(index.ToString(), item))) + postfix + "}";
            }
            else
            {
                return "{empty}";
            }
        }
    }
}
