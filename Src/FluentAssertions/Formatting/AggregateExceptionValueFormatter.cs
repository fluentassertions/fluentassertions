using System;
using System.Text;
using FluentAssertions.Localization;

namespace FluentAssertions.Formatting
{
    public class AggregateExceptionValueFormatter : IValueFormatter
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
            return value is AggregateException;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="context"> </param>
        /// <param name="formatChild"></param>
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
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var exception = (AggregateException)value;
            if (exception.InnerExceptions.Count == 1)
            {
                return Resources.Formatter_Aggregated + formatChild("inner", exception.InnerException);
            }
            else
            {
                var builder = new StringBuilder();

                builder.AppendFormat(Resources.Formatter_X0AggregatedExceptionsX1Format, exception.InnerExceptions.Count, Environment.NewLine);

                foreach (Exception innerException in exception.InnerExceptions)
                {
                    builder.AppendLine();
                    builder.AppendLine(formatChild("InnerException", innerException));
                }

                return builder.ToString();
            }
        }
    }
}
