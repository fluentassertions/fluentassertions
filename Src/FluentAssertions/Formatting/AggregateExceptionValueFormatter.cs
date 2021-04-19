using System;
using static System.FormattableString;

namespace FluentAssertions.Formatting
{
    public class AggregateExceptionValueFormatter : IValueFormatter
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
            return value is AggregateException;
        }

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            var exception = (AggregateException)value;
            if (exception.InnerExceptions.Count == 1)
            {
                formattedGraph.AddFragment("(aggregated) ");

                formatChild("inner", exception.InnerException, formattedGraph);
            }
            else
            {
                formattedGraph.AddLine(Invariant($"{exception.InnerExceptions.Count} (aggregated) exceptions:"));

                foreach (Exception innerException in exception.InnerExceptions)
                {
                    formattedGraph.AddLine(string.Empty);
                    formatChild("InnerException", innerException, formattedGraph);
                }
            }
        }
    }
}
