using System.Collections.Generic;
using FluentAssertions.Common;
using FluentAssertions.Formatting;
using Newtonsoft.Json.Linq;

namespace FluentAssertions.Json
{
    /// <summary>
    /// A <see cref="IValueFormatter"/> for <see cref="JToken" />.
    /// </summary>
    public class JTokenFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="string"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is JToken;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="string"/>.</param>
        /// <param name="useLineBreaks"> </param>
        /// <param name="processedObjects">
        /// A collection of objects that 
        /// </param>
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToString(object value, bool useLineBreaks = false, IList<object> processedObjects = null,
            int nestedPropertyLevel = 0)
        {
            var jToken = (JToken)value;
            return useLineBreaks ? jToken.ToString(Newtonsoft.Json.Formatting.Indented) : jToken.ToString().RemoveNewLines();
        }
    }
}