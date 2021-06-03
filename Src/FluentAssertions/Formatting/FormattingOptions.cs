namespace FluentAssertions.Formatting
{
    public class FormattingOptions
    {
        /// <summary>
        /// Indicates whether the formatter should use line breaks when the <see cref="IValueFormatter"/> supports it.
        /// </summary>
        public bool UseLineBreaks { get; set; }

        /// <summary>
        /// Determines the depth until which the library should try to render an object graph.
        /// </summary>
        /// <value>
        /// A depth of 1 will only the display the members of the root object.
        /// </value>
        public int MaxDepth { get; set; } = 5;

        /// <summary>
        /// Sets the maximum number of lines of the failure message.
        /// </summary>
        /// <remarks>
        /// Because of technical reasons, the actual output may be one or two lines longer.
        /// </remarks>
        public int MaxLines { get; set; } = 100;

        internal FormattingOptions Clone()
        {
            return new()
            {
                UseLineBreaks = UseLineBreaks,
                MaxDepth = MaxDepth,
                MaxLines = MaxLines
            };
        }
    }
}
