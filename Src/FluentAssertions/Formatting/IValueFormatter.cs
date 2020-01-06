namespace FluentAssertions.Formatting
{
    /// <summary>
    /// Represents a strategy for formatting an arbitrary value into a human-readable string representation.
    /// </summary>
    /// <remarks>
    /// Add custom formatters using <see cref="Formatter.AddFormatter"/>.
    /// </remarks>
    public interface IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandle(object value);

        /// <summary>
        /// Returns a human-readable representation of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to format.</param>
        /// <param name="context">
        /// Contains additional information about the formatting task.
        /// </param>
        /// <param name="formatChild">
        /// Allows the formatter to recursively format any child objects.
        /// </param>
        /// <remarks>
        /// DO NOT CALL <see cref="Formatter.ToString(object,bool)"/> directly, but use <paramref name="formatChild"/>
        /// instead. This will ensure cyclic dependencies are properly detected.
        /// </remarks>
        string Format(object value, FormattingContext context, FormatChild formatChild);
    }

    /// <summary>
    /// Provides information about the current formatting action.
    /// </summary>
    public class FormattingContext
    {
        public int Depth { get; set; }

        public bool UseLineBreaks { get; set; }
    }

    /// <summary>
    /// Represents a method that can be used to format child values from inside a <see cref="IValueFormatter"/>.
    /// </summary>
    /// <param name="childPath">
    /// Represents the path from the current location to the child value.
    /// </param>
    /// <param name="value">
    /// The child value to run through the configured <see cref="IValueFormatter"/>s.
    /// </param>
    public delegate string FormatChild(string childPath, object value);
}
