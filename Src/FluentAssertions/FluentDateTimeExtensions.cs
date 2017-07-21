using System;
using System.Diagnostics;

namespace FluentAssertions
{
    /// <summary>
    /// Extension methods on <see cref="int"/> to allow for a more fluent way of specifying a <see cref="DateTime"/>.
    /// </summary>
    /// <example>
    /// Instead of<br />
    /// <br />
    /// new DateTime(2011, 3, 10)<br />
    /// <br />
    /// you can write 3.March(2011)<br />
    /// <br />
    /// Or even<br />
    /// <br />
    /// 3.March(2011).At(09, 30)
    /// </example>
    /// <seealso cref="TimeSpanConversionExtensions"/>
    [DebuggerNonUserCode]
    public static class FluentDateTimeExtensions
    {
        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month January.
        /// </summary>
        public static DateTime January(this int day, int year)
        {
            return new DateTime(year, 1, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month February.
        /// </summary>
        public static DateTime February(this int day, int year)
        {
            return new DateTime(year, 2, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month March.
        /// </summary>
        public static DateTime March(this int day, int year)
        {
            return new DateTime(year, 3, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month April.
        /// </summary>
        public static DateTime April(this int day, int year)
        {
            return new DateTime(year, 4, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month May.
        /// </summary>
        public static DateTime May(this int day, int year)
        {
            return new DateTime(year, 5, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month June.
        /// </summary>
        public static DateTime June(this int day, int year)
        {
            return new DateTime(year, 6, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month July.
        /// </summary>
        public static DateTime July(this int day, int year)
        {
            return new DateTime(year, 7, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month August.
        /// </summary>
        public static DateTime August(this int day, int year)
        {
            return new DateTime(year, 8, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month September.
        /// </summary>
        public static DateTime September(this int day, int year)
        {
            return new DateTime(year, 9, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month October.
        /// </summary>
        public static DateTime October(this int day, int year)
        {
            return new DateTime(year, 10, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month November.
        /// </summary>
        public static DateTime November(this int day, int year)
        {
            return new DateTime(year, 11, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="day"/> and <paramref name="year"/>
        /// in the month December.
        /// </summary>
        public static DateTime December(this int day, int year)
        {
            return new DateTime(year, 12, day);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="date"/> and <paramref name="time"/>.
        /// </summary>
        public static DateTime At(this DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="date"/> and time with the specified
        /// <paramref name="hours"/>, <paramref name="minutes"/> and optionally <paramref name="seconds"/>.
        /// </summary>
        public static DateTime At(this DateTime date, int hours, int minutes, int seconds = 0, int milliseconds = 0)
        {
            return new DateTime(date.Year, date.Month, date.Day, hours, minutes, seconds, milliseconds);
        }

        /// <summary>
        /// Returns a new <see cref="DateTimeOffset"/> value for the specified <paramref name="date"/> and time with the specified
        /// <paramref name="hours"/>, <paramref name="minutes"/> and optionally <paramref name="seconds"/>.
        /// </summary>
        public static DateTimeOffset At(this DateTimeOffset date, int hours, int minutes, int seconds = 0, int milliseconds = 0)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, hours, minutes, seconds, milliseconds, date.Offset);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="dateTime"/> and time with
        /// the kind set to <see cref="DateTimeKind.Utc"/>.
        /// </summary>
        public static DateTime AsUtc(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value for the specified <paramref name="dateTime"/> and time with
        /// the kind set to <see cref="DateTimeKind.Local"/>.
        /// </summary>
        public static DateTime AsLocal(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value that is the current <see cref="TimeSpan"/> before the
        /// specified <paramref name="sourceDateTime"/>.
        /// </summary>
        public static DateTime Before(this TimeSpan timeDifference, DateTime sourceDateTime)
        {
            return sourceDateTime.Subtract(timeDifference);
        }

        /// <summary>
        /// Returns a new <see cref="DateTime"/> value that is the current <see cref="TimeSpan"/> after the
        /// specified <paramref name="sourceDateTime"/>.
        /// </summary>
        public static DateTime After(this TimeSpan timeDifference, DateTime sourceDateTime)
        {
            return sourceDateTime.Add(timeDifference);
        }
    }
}