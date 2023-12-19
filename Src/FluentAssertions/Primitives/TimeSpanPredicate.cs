using System;

namespace FluentAssertionsAsync.Primitives;

/// <summary>
/// Provides the logic and the display text for a <see cref="TimeSpanCondition"/>.
/// </summary>
internal class TimeSpanPredicate
{
    private readonly Func<TimeSpan, TimeSpan, bool> lambda;

    public TimeSpanPredicate(Func<TimeSpan, TimeSpan, bool> lambda, string displayText)
    {
        this.lambda = lambda;
        DisplayText = displayText;
    }

    public string DisplayText { get; }

    public bool IsMatchedBy(TimeSpan actual, TimeSpan expected)
    {
        return lambda(actual, expected) && actual >= TimeSpan.Zero;
    }
}
