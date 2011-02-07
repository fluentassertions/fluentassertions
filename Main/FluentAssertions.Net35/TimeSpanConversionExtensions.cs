using System;

namespace FluentAssertions
{
    public static class TimeSpanConversionExtensions
    {
        /// <summary>
        /// Returns a <see cref = "TimeSpan" /> for the current number of minutes
        /// </summary>
        public static TimeSpan Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Returns a <see cref = "TimeSpan" /> for the current number of hours
        /// </summary>
        public static TimeSpan Hours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// Returns a <see cref = "TimeSpan" /> for the current number of days
        /// </summary>
        public static TimeSpan Days(this int days)
        {
            return TimeSpan.FromDays(days);
        }
    }
}