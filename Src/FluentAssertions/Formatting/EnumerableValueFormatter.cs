using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    public class EnumerableValueFormatter : IValueFormatter
    {
        /// <summary>
        /// The number of items to include when formatting this object.
        /// </summary>
        /// <remarks>The default value is 32.</remarks>
        protected virtual int MaxItems { get; } = 32;

        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="string"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanHandle(object value)
        {
            return value is IEnumerable;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            ICollection<object> enumerable = ((IEnumerable)value).ConvertOrCastToCollection<object>();

            if (enumerable.Any())
            {
                string postfix = string.Empty;

                if (enumerable.Count > MaxItems)
                {
                    postfix = $", …{enumerable.Count - MaxItems} more…";
                    enumerable = enumerable.Take(MaxItems).ToArray();
                }

                return "{" + string.Join(", ", enumerable.Select((item, index) => formatChild(index.ToString(CultureInfo.InvariantCulture), item))) + postfix + "}";
            }
            else
            {
                return "{empty}";
            }
        }
    }
}
