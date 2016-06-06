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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="useLineBreaks"> </param>
        /// <param name="processedObjects">
        /// A collection of objects that 
        /// </param>
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, bool useLineBreaks, IList<object> processedObjects = null, int nestedPropertyLevel = 0)
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
                string format = HasMilliSeconds(dateTime) ? "HH:mm:ss.fff" : "HH:mm:ss";
                fragments.Add(dateTime.ToString(format));
            }

            if (dateTime.Offset > TimeSpan.Zero)
            {
                fragments.Add("+" + Formatter.ToString(dateTime.Offset));
            }

            if (dateTime.Offset < TimeSpan.Zero)
            {
                fragments.Add(Formatter.ToString(dateTime.Offset));
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
    }
}