using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    public class DateTimeOffsetValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return (value is DateTime) || (value is DateTimeOffset);
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            DateTimeOffset dateTimeOffset;

            if (value is DateTime dateTime)
            {
                dateTimeOffset = dateTime.ToDateTimeOffset();
            }
            else
            {
                dateTimeOffset = (DateTimeOffset)value;
            }

            var fragments = new List<string>();

            if (HasDate(dateTimeOffset))
            {
                fragments.Add(dateTimeOffset.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            if (HasTime(dateTimeOffset))
            {
                if (HasNanoSeconds(dateTimeOffset))
                {
                    fragments.Add(dateTimeOffset.ToString("HH:mm:ss.fffffff", CultureInfo.InvariantCulture));
                }
                else if (HasMicroSeconds(dateTimeOffset))
                {
                    fragments.Add(dateTimeOffset.ToString("HH:mm:ss.ffffff", CultureInfo.InvariantCulture));
                }
                else if (HasMilliSeconds(dateTimeOffset))
                {
                    fragments.Add(dateTimeOffset.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
                }
                else
                {
                    fragments.Add(dateTimeOffset.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
                }
            }

            if (dateTimeOffset.Offset > TimeSpan.Zero)
            {
                fragments.Add("+" + formatChild("offset", dateTimeOffset.Offset));
            }

            if (dateTimeOffset.Offset < TimeSpan.Zero)
            {
                fragments.Add(formatChild("offset", dateTimeOffset.Offset));
            }

            if (!fragments.Any())
            {
                if (HasMilliSeconds(dateTimeOffset))
                {
                    fragments.Add("0001-01-01 00:00:00." + dateTimeOffset.ToString("fff", CultureInfo.InvariantCulture));
                }
                else
                {
                    fragments.Add("0001-01-01 00:00:00.000");
                }
            }

            return "<" + string.Join(" ", fragments.ToArray()) + ">";
        }

        private static bool HasTime(DateTimeOffset dateTime)
        {
            return (dateTime.Hour != 0)
                || (dateTime.Minute != 0)
                || (dateTime.Second != 0)
                || HasMilliSeconds(dateTime)
                || HasMicroSeconds(dateTime)
                || HasNanoSeconds(dateTime);
        }

        private static bool HasDate(DateTimeOffset dateTime)
        {
            return (dateTime.Day != 1) || (dateTime.Month != 1) || (dateTime.Year != 1);
        }

        private static bool HasMilliSeconds(DateTimeOffset dateTime)
        {
            return (dateTime.Millisecond > 0);
        }

        private static bool HasMicroSeconds(DateTimeOffset dateTime)
        {
            return (dateTime.Ticks % TimeSpan.FromMilliseconds(1).Ticks) > 0;
        }

        private static bool HasNanoSeconds(DateTimeOffset dateTime)
        {
            return (dateTime.Ticks % (TimeSpan.FromMilliseconds(1).Ticks / 1000)) > 0;
        }
    }
}
