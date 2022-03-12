using System;
using System.Diagnostics;

#if NET6_0_OR_GREATER

namespace FluentAssertions.Extensions
{
    /// <summary>
    /// Extension methods on <see cref="int"/> to allow for a more fluent way of specifying a <see cref="DateOnly"/>.
    /// </summary>
    [DebuggerNonUserCode]
    public static class FluentDateOnlyExtensions
    {
        /// <summary>
        /// Returns a new <see cref="DateOnly"/> value that is the current <see cref="TimeSpan"/> before the
        /// specified <paramref name="sourceDateOnly"/>.
        /// </summary>
        public static DateOnly Before(this TimeSpan timeDifference, DateOnly sourceDateOnly)
        {
            var dayDifference = (int)timeDifference.TotalDays;
            return sourceDateOnly.AddDays(-1 * dayDifference);
        }

        /// <summary>
        /// Returns a new <see cref="DateOnly"/> value that is the current <see cref="TimeSpan"/> after the
        /// specified <paramref name="sourceDateOnly"/>.
        /// </summary>
        public static DateOnly After(this TimeSpan timeDifference, DateOnly sourceDateOnly)
        {
            var dayDifference = (int)timeDifference.TotalDays;
            return sourceDateOnly.AddDays(dayDifference);
        }
    }
}

#endif
