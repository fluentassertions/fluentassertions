using System;

namespace FluentAssertions.Common
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
    public static class TimeSpanConversionExtensions
    {
        /// <summary>
        /// Returns a <see cref="TimeSpan" /> based on a number of milliseconds.
        /// </summary>
        public static TimeSpan Milliseconds(this int milliseconds)
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