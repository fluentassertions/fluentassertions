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
        /// Indicates
        /// whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="string"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandle(object value);

        /// <summary>
        /// Returns a human-readable representation of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to format into a human-readable representation</param>
        /// <param name="formattedGraph">
        ///     An object to write the textual representation to.
        /// </param>
        /// <param name="context">
        ///     Contains additional information that the implementation should take into account.
        /// </param>
        /// <param name="formatChild">
        ///     Allows the formatter to recursively format any child objects.
        /// </param>
        /// <remarks>
        /// DO NOT CALL <see cref="Formatter.ToString(object,FormattingOptions)"/> directly, but use <paramref name="formatChild"/>
        /// instead. This will ensure cyclic dependencies are properly detected.
        /// Also, the <see cref="FormattedObjectGraph"/> may throw
        /// an <see cref="MaxLinesExceededException"/> that must be ignored by implementations of this interface.
        /// </remarks>
        void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild);
    }
}
