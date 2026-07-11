using System.Diagnostics;

namespace FluentAssertions.Common;

[StackTraceHidden]
internal static class IntegerExtensions
{
    public static string Times(this int count) => count == 1 ? "1 time" : $"{count} times";

    internal static bool IsConsecutiveTo(this int startNumber, int endNumber) => endNumber == startNumber + 1;
}
