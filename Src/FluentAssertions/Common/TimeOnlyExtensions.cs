#if NET6_0_OR_GREATER
using System;

namespace FluentAssertionsAsync.Common;

internal static class TimeOnlyExtensions
{
    /// <summary>
    /// Determines if <paramref name="subject"/> is close to <paramref name="other"/> within a given <paramref name="precision"/>.
    /// </summary>
    /// <param name="subject">The time to check</param>
    /// <param name="other">The time to be close to</param>
    /// <param name="precision">The precision that <paramref name="other"/> may differ from <paramref name="subject"/></param>
    /// <remarks>
    /// <see cref="TimeOnly.IsBetween(TimeOnly, TimeOnly)" /> checks the right-open interval, whereas this method checks the closed interval.
    /// </remarks>
    public static bool IsCloseTo(this TimeOnly subject, TimeOnly other, TimeSpan precision)
    {
        long startTicks = other.Add(-precision).Ticks;
        long endTicks = other.Add(precision).Ticks;
        long ticks = subject.Ticks;

        return startTicks <= endTicks
            ? startTicks <= ticks && endTicks >= ticks
            : startTicks <= ticks || endTicks >= ticks;
    }
}

#endif
