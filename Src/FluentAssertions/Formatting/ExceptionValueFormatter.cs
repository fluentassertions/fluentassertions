using System;
using System.Text;

namespace FluentAssertions.Formatting
{
    public class ExceptionValueFormatter : IValueFormatter
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
            return value is Exception;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var exception = (Exception)value;

            var builder = new StringBuilder();
            builder.AppendFormat("{0} with message \"{1}\"\n", exception.GetType().FullName, exception.Message);

            if (exception.StackTrace != null)
            {
                foreach (string line in exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    builder.Append("  ").AppendLine(line);
                }
            }

            return builder.ToString();
        }
    }
}
