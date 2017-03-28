using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FluentAssertions.Formatting
{
    public class DoubleValueFormatter : IValueFormatter
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
            return value is double;
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
            double doubleValue = (double)value;

            if (double.IsPositiveInfinity(doubleValue))
            {
                return typeof(double).Name + "." + nameof(double.PositiveInfinity);
            }

            if (double.IsNegativeInfinity(doubleValue))
            {
                return typeof(double).Name + "." + nameof(double.NegativeInfinity);
            }

            if (double.IsNaN(doubleValue))
            {
                return doubleValue.ToString(CultureInfo.InvariantCulture);
            }

            string formattedValue = doubleValue.ToString("R", CultureInfo.InvariantCulture);

            return (formattedValue.IndexOf('.') == -1) && (formattedValue.IndexOf('E') == -1)
                ? formattedValue + ".0"
                : formattedValue;
        }
    }
}