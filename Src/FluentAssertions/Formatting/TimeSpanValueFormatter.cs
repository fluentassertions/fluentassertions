using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FluentAssertions.Formatting
{
    public class TimeSpanValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="string"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is TimeSpan;
        }

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            var timeSpan = (TimeSpan)value;

            if (timeSpan == TimeSpan.MinValue)
            {
                formattedGraph.AddFragment("min time span");
                return;
            }

            if (timeSpan == TimeSpan.MaxValue)
            {
                formattedGraph.AddFragment("max time span");
                return;
            }

            List<string> fragments = GetNonZeroFragments(timeSpan);

            if (!fragments.Any())
            {
                formattedGraph.AddFragment("default");
            }

            string sign = (timeSpan.Ticks >= 0) ? string.Empty : "-";

            if (fragments.Count == 1)
            {
                formattedGraph.AddFragment(sign + fragments.Single());
            }
            else
            {
                formattedGraph.AddFragment(sign + fragments.JoinUsingWritingStyle());
            }
        }

        private static List<string> GetNonZeroFragments(TimeSpan timeSpan)
        {
            TimeSpan absoluteTimespan = timeSpan.Duration();

            var fragments = new List<string>();

            AddDaysIfNotZero(absoluteTimespan, fragments);
            AddHoursIfNotZero(absoluteTimespan, fragments);
            AddMinutesIfNotZero(absoluteTimespan, fragments);
            AddSecondsIfNotZero(absoluteTimespan, fragments);
            AddMilliSecondsIfNotZero(absoluteTimespan, fragments);
            AddMicrosecondsIfNotZero(absoluteTimespan, fragments);

            return fragments;
        }

        private static void AddMicrosecondsIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            var ticks = timeSpan.Ticks % TimeSpan.TicksPerMillisecond;
            if (ticks > 0)
            {
                var microSeconds = ticks / (double)TimeSpan.TicksPerMillisecond * 1000;
                fragments.Add(microSeconds.ToString("0.0", CultureInfo.InvariantCulture) + "µs");
            }
        }

        private static void AddSecondsIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if (timeSpan.Seconds > 0)
            {
                string result = timeSpan.Seconds.ToString(CultureInfo.InvariantCulture);

                fragments.Add(result + "s");
            }
        }

        private static void AddMilliSecondsIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if (timeSpan.Milliseconds > 0)
            {
                var result = timeSpan.Milliseconds.ToString(CultureInfo.InvariantCulture);

                fragments.Add(result + "ms");
            }
        }

        private static void AddMinutesIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if (timeSpan.Minutes > 0)
            {
                fragments.Add(timeSpan.Minutes.ToString(CultureInfo.InvariantCulture) + "m");
            }
        }

        private static void AddHoursIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if (timeSpan.Hours > 0)
            {
                fragments.Add(timeSpan.Hours.ToString(CultureInfo.InvariantCulture) + "h");
            }
        }

        private static void AddDaysIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if (timeSpan.Days > 0)
            {
                fragments.Add(timeSpan.Days.ToString(CultureInfo.InvariantCulture) + "d");
            }
        }
    }
}
