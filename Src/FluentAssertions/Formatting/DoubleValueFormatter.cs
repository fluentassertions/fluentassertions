using System.Globalization;

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

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
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
