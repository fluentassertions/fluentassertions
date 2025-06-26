namespace FluentAssertions.Execution;

internal static class StringExtensions
{
    /// <summary>
    /// Wraps the specified string in a <see cref="WithoutFormattingWrapper"/> to prevent any formatting applied during output.
    /// </summary>
    public static WithoutFormattingWrapper AsNonFormattable(this string value)
    {
        return new WithoutFormattingWrapper(value);
    }

    /// <summary>
    /// Wraps the specified value in a <see cref="WithoutFormattingWrapper"/> to prevent any formatting applied during output.
    /// </summary>
    public static WithoutFormattingWrapper AsNonFormattable(this object value)
    {
        return new WithoutFormattingWrapper(value?.ToString());
    }
}
