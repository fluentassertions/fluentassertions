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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="useLineBreaks"> </param>
        /// <param name="processedObjects">
        /// A collection of objects that 
        /// </param>
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, bool useLineBreaks, IList<object> processedObjects = null, int nestedPropertyLevel = 0)
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

                return "{" + string.Join(", ", enumerable.Select(obj => Formatter.ToString(obj, useLineBreaks, processedObjects, nestedPropertyLevel)).ToArray()) + postfix + "}";
            }
            else
            {
                return "{empty}";
            }
        }
    }
}