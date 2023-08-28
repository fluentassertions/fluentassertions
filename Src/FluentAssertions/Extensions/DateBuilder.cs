#nullable enable

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace FluentAssertions.Extensions;

/// <summary>A date builder that implicitly casts both to a date only and a date time.</summary>
public readonly struct DateBuilder : IEquatable<DateBuilder>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly DateTime date;

    /// <summary>Initializes a new instance of the <see cref="DateBuilder"/> struct.</summary>
    internal DateBuilder(int year, int month, int day)
        => date = new DateTime(year, month, day, 00, 00, 00, DateTimeKind.Unspecified);

    /// <summary>
    /// Returns a new <see cref="DateTime"/> value for the <paramref name="time"/>.
    /// </summary>
    public DateTime At(TimeSpan time) => date.Date + time;

    /// <summary>
    /// Returns a new <see cref="DateTime"/> value for time with the specified
    /// <paramref name="hours"/>, <paramref name="minutes"/> and optionally <paramref name="seconds"/>.
    /// </summary>
    public DateTime At(int hours, int minutes, int seconds = 0, int milliseconds = 0,
        int microseconds = 0, int nanoseconds = 0)
    {
        if (microseconds is < 0 or > 999)
        {
            throw new ArgumentOutOfRangeException(nameof(microseconds), "Valid values are between 0 and 999");
        }

        if (nanoseconds is < 0 or > 999)
        {
            throw new ArgumentOutOfRangeException(nameof(nanoseconds), "Valid values are between 0 and 999");
        }

        var value = new DateTime(date.Year, date.Month, date.Day, hours, minutes, seconds, milliseconds, date.Kind);

        if (microseconds != 0)
        {
            value += microseconds.Microseconds();
        }

        if (nanoseconds != 0)
        {
            value += nanoseconds.Nanoseconds();
        }

        return value;
    }

    /// <summary>
    /// Returns a new <see cref="DateTime"/> with the kind set to <see cref="DateTimeKind.Utc"/>.
    /// </summary>
    public DateTime AsUtc() => date.AsUtc();

    /// <summary>
    /// Returns a new <see cref="DateTimeOffset"/> value without an offset.
    /// </summary>
    public DateTimeOffset AsOffset() => WithOffset(0.Hours());

    /// <summary>
    /// Returns a new <see cref="DateTimeOffset"/> value with specified offset.
    /// </summary>
    public DateTimeOffset WithOffset(TimeSpan offset) => new(date, offset);

    /// <inheritdoc />
    public override string ToString() => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    /// <inheritdoc />
    public override int GetHashCode() => date.GetHashCode();

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is DateBuilder other && Equals(other);

    /// <inheritdoc />
    public bool Equals(DateBuilder other)
        => date.Equals(other.date);

    /// <summary>True if both <see cref="DateBuilder"/>s represent the same date.</summary>
    public static bool operator ==(DateBuilder left, DateBuilder right)
        => left.Equals(right);

    /// <summary>False if both <see cref="DateBuilder"/>s represent the same date.</summary>
    public static bool operator !=(DateBuilder left, DateBuilder right)
        => !(left == right);

    /// <summary>Adds a <see cref="TimeSpan"/> to the <see cref="DateTime"/>.</summary>
    public static DateTime operator +(DateBuilder builder, TimeSpan time) => builder.At(+time);

    /// <summary>Implicitly casts the <see cref="DateBuilder"/> to a <see cref="DateTime"/>.</summary>
    public static implicit operator DateTime(DateBuilder builder) => builder.date;

#if NET6_0_OR_GREATER
    /// <summary>Implicitly casts the <see cref="DateBuilder"/> to a <see cref="DateOnly"/>.</summary>
    public static implicit operator DateOnly(DateBuilder builder) => DateOnly.FromDateTime(builder.date);
#endif
}
