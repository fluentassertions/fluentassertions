using System;
using System.Collections.Generic;
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
            DateTimeOffset dateTime;

            if (value is DateTime)
            {
                dateTime = ((DateTime)value).ToDateTimeOffset();
            }
            else
            {
                dateTime = (DateTimeOffset) value;
            }

            var fragments = new List<string>();

            if (HasDate(dateTime))
            {
                fragments.Add(dateTime.ToString("yyyy-MM-dd"));
            }

            if (HasTime(dateTime))
            {
                if (HasNanoSeconds(dateTime))
                {
                    fragments.Add(dateTime.ToString("HH:mm:ss.fffffff"));
                }
                else if (HasMicroSeconds(dateTime))
                {
                    fragments.Add(dateTime.ToString("HH:mm:ss.ffffff"));
                }
                else if(HasMilliSeconds(dateTime))
                {
                    fragments.Add(dateTime.ToString("HH:mm:ss.fff"));
                }
                else
                {
                    fragments.Add(dateTime.ToString("HH:mm:ss"));
                }
            }

            if (dateTime.Offset > TimeSpan.Zero)
            {
                fragments.Add("+" + formatChild("offset", dateTime.Offset));
            }

            if (dateTime.Offset < TimeSpan.Zero)
            {
                fragments.Add(formatChild("offset", dateTime.Offset));
            }

            if (!fragments.Any())
            {
                if (HasMilliSeconds(dateTime))
                {
                    fragments.Add("0001-01-01 00:00:00." + dateTime.ToString("fff"));
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
            return (dateTime.Hour != 0) || (dateTime.Minute != 0) || (dateTime.Second != 0);
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
