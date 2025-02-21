namespace FluentAssertions.Execution;

internal static class StringExtensions
{
    /// <summary>
    /// Can be used
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static WithoutFormattingWrapper AsNonFormattable(this string value)
    {
        return new WithoutFormattingWrapper(value);
    }

    /// <summary>
    /// Can be used
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static WithoutFormattingWrapper AsNonFormattable(this object value)
    {
        return new WithoutFormattingWrapper(value?.ToString());
    }
}
