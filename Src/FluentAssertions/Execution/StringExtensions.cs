namespace FluentAssertions.Execution;

internal static class StringExtensions
{
    /// <summary>
    /// Can be used
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static WithoutFormattingWrapper AsNonFormatable(this string value)
    {
        return new WithoutFormattingWrapper(value);
    }
}
