using System;

namespace FluentAssertions.Extensions
{
    /// <summary>
    /// Extension methods on <see cref="int"/> to allow for a more fluent way of specifying a <see cref="TimeSpan"/>.
    /// </summary>
    /// <example>
    /// Instead of<br />
    /// <br />
    /// TimeSpan.FromHours(12)<br />
    /// <br />
    /// you can write<br />
    /// <br />
    /// 12.Hours()<br />
    /// <br />
    /// Or even<br />
    /// <br />
    /// 12.Hours().And(30.Minutes()).
    /// </example>
    /// <seealso cref="FluentDateTimeExtensions"/>
    public static class FluentTimeSpanExtensions
    {
        /// <summary>
        /// Represents the number of ticks that are in 1 microsecond.
        /// </summary>
        public const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

        /// <summary>
        /// Represents the number of ticks that are in 1 nanosecond.
        /// </summary>
        public const double TicksPerNanosecond = TicksPerMicrosecond / 1000d;

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of ticks.
        /// </summary>
        public static TimeSpan Ticks(this int ticks)
        {
            return TimeSpan.FromTicks(ticks);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of ticks.
        /// </summary>
        public static TimeSpan Ticks(this long ticks)
        {
            return TimeSpan.FromTicks(ticks);
        }

        /// <summary>
        /// Gets the nanoseconds component of the time interval represented by the current <see cref="TimeSpan" /> structure.
        /// </summary>
        public static int Nanoseconds(this TimeSpan self)
        {
            return (int)((self.Ticks % TicksPerMicrosecond) * (1d / TicksPerNanosecond));
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of nanoseconds.
        /// </summary>
        /// <remarks>
        /// .NET's smallest resolutions is 100 nanoseconds. Any nanoseconds passed in
        /// lower than .NET's resolution will be rounded using the default rounding
        /// algorithm in Math.Round().
        /// </remarks>
        public static TimeSpan Nanoseconds(this int nanoseconds)
        {
            return ((long)Math.Round(nanoseconds * TicksPerNanosecond)).Ticks();
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of nanoseconds.
        /// </summary>
        /// <remarks>
        /// .NET's smallest resolutions is 100 nanoseconds. Any nanoseconds passed in
        /// lower than .NET's resolution will be rounded using the default rounding
        /// algorithm in Math.Round().
        /// </remarks>
        public static TimeSpan Nanoseconds(this long nanoseconds)
        {
            return ((long)Math.Round(nanoseconds * TicksPerNanosecond)).Ticks();
        }

        /// <summary>
        /// Gets the value of the current <see cref="TimeSpan" /> structure expressed in whole and fractional nanoseconds.
        /// </summary>
        public static double TotalNanoseconds(this TimeSpan self)
        {
            return self.Ticks * (1d / TicksPerNanosecond);
        }

        /// <summary>
        /// Gets the microseconds component of the time interval represented by the current <see cref="TimeSpan" /> structure.
        /// </summary>
        public static int Microseconds(this TimeSpan self)
        {
            return (int)((self.Ticks % TimeSpan.TicksPerMillisecond) * (1d / TicksPerMicrosecond));
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of microseconds.
        /// </summary>
        public static TimeSpan Microseconds(this int microseconds)
        {
            return (microseconds * TicksPerMicrosecond).Ticks();
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of microseconds.
        /// </summary>
        public static TimeSpan Microseconds(this long microseconds)
        {
            return (microseconds * TicksPerMicrosecond).Ticks();
        }

        /// <summary>
        /// Gets the value of the current <see cref="TimeSpan" /> structure expressed in whole and fractional microseconds.
        /// </summary>
        public static double TotalMicroseconds(this TimeSpan self)
        {
            return self.Ticks * (1d / TicksPerMicrosecond);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of milliseconds.
        /// </summary>
        public static TimeSpan Milliseconds(this int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of milliseconds.
        /// </summary>
        public static TimeSpan Milliseconds(this double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of seconds.
        /// </summary>
        public static TimeSpan Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of seconds.
        /// </summary>
        public static TimeSpan Seconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of seconds, and add the specified
        /// <paramref name="offset"/>.
        /// </summary>
        public static TimeSpan Seconds(this int seconds, TimeSpan offset)
        {
            return TimeSpan.FromSeconds(seconds).Add(offset);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of minutes.
        /// </summary>
        public static TimeSpan Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of minutes.
        /// </summary>
        public static TimeSpan Minutes(this double minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of minutes, and add the specified
        /// <paramref name="offset"/>.
        /// </summary>
        public static TimeSpan Minutes(this int minutes, TimeSpan offset)
        {
            return TimeSpan.FromMinutes(minutes).Add(offset);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of hours.
        /// </summary>
        public static TimeSpan Hours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of hours.
        /// </summary>
        public static TimeSpan Hours(this double hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of hours, and add the specified
        /// <paramref name="offset"/>.
        /// </summary>
        public static TimeSpan Hours(this int hours, TimeSpan offset)
        {
            return TimeSpan.FromHours(hours).Add(offset);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of days.
        /// </summary>
        public static TimeSpan Days(this int days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of days.
        /// </summary>
        public static TimeSpan Days(this double days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of days, and add the specified
        /// <paramref name="offset"/>.
        /// </summary>
        public static TimeSpan Days(this int days, TimeSpan offset)
        {
            return TimeSpan.FromDays(days).Add(offset);
        }

        /// <summary>
        /// Convenience method for chaining multiple calls to the methods provided by this class.
        /// </summary>
        /// <example>
        /// 23.Hours().And(59.Minutes())
        /// </example>
        public static TimeSpan And(this TimeSpan sourceTime, TimeSpan offset)
        {
            return sourceTime.Add(offset);
        }
    }
}
