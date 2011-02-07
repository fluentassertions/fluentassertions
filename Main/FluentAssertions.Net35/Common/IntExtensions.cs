using System;

namespace FluentAssertions.Common
{
    public static class IntExtensions
    {
        /// <summary>
        /// Returns a <see cref = "TimeSpan" /> for the current number of minutes
        /// </summary>
        public static TimeSpan Minutes(this int source)
        {
            return TimeSpan.FromMinutes(source);
        }

        /// <summary>
        /// Returns a <see cref = "TimeSpan" /> for the current number of hours
        /// </summary>
        public static TimeSpan Hours(this int source)
        {
            return TimeSpan.FromHours(source);
        }

        /// <summary>
        /// Returns a <see cref = "TimeSpan" /> for the current number of days
        /// </summary>
        public static TimeSpan Days(this int source)
        {
            return TimeSpan.FromDays(source);
        }
    }
}