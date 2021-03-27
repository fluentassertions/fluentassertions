namespace FluentAssertions.Formatting
{
    public class StringValueFormatter : IValueFormatter
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
            return value is string;
        }

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            string result = "\"" + value + "\"";

            if (context.UseLineBreaks)
            {
                formattedGraph.AddFragmentOnNewLine(result);
            }
            else
            {
                formattedGraph.AddFragment(result);
            }
        }
    }
}
