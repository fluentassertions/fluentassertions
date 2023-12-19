using System;

namespace FluentAssertionsAsync.Extensions;

/// <summary>
/// Provides extensions to write <see cref="OccurrenceConstraint" />s with fluent syntax
/// </summary>
public static class OccurrenceConstraintExtensions
{
    /// <summary>
    /// This is the equivalent to <see cref="Exactly.Times(int)" />
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="times"/> is less than zero.</exception>
    public static OccurrenceConstraint TimesExactly(this int times)
    {
        return Exactly.Times(times);
    }

    /// <summary>
    /// This is the equivalent to <see cref="AtMost.Times(int)" />
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="times"/> is less than zero.</exception>
    public static OccurrenceConstraint TimesOrLess(this int times)
    {
        return AtMost.Times(times);
    }

    /// <summary>
    /// This is the equivalent to <see cref="AtLeast.Times(int)" />
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="times"/> is less than zero.</exception>
    public static OccurrenceConstraint TimesOrMore(this int times)
    {
        return AtLeast.Times(times);
    }
}
