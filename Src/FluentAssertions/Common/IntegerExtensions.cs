namespace FluentAssertionsAsync.Common;

internal static class IntegerExtensions
{
    public static string Times(this int count) => count == 1 ? "1 time" : $"{count} times";

    internal static bool IsConsecutiveTo(this int startNumber, int endNumber) => endNumber == (startNumber + 1);
}
