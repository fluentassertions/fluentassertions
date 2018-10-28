using System.Globalization;

namespace FluentAssertions.Formatting
{
    public class SingleValueFormatter : IValueFormatter
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
            return value is float;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            float singleValue = (float)value;

            if (float.IsPositiveInfinity(singleValue))
            {
                return typeof(float).Name + "." + nameof(float.PositiveInfinity);
            }

            if (float.IsNegativeInfinity(singleValue))
            {
                return typeof(float).Name + "." + nameof(float.NegativeInfinity);
            }

            if (float.IsNaN(singleValue))
            {
                return singleValue.ToString(CultureInfo.InvariantCulture);
            }

            return singleValue.ToString("R", CultureInfo.InvariantCulture) + "F";
        }
    }
}
