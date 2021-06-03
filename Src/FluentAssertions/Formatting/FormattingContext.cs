namespace FluentAssertions.Formatting
{
    /// <summary>
    /// Provides information about the current formatting action.
    /// </summary>
    public class FormattingContext
    {
        /// <summary>
        /// Indicates whether the formatter should use line breaks when the <see cref="IValueFormatter"/> supports it.
        /// </summary>
        public bool UseLineBreaks { get; set; }
    }
}
