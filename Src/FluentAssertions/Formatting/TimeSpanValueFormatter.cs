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
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is TimeSpan;
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var timeSpan = (TimeSpan)value;

            if (timeSpan == TimeSpan.MinValue)
            {
                return "min time span";
            }

            if (timeSpan == TimeSpan.MaxValue)
            {
                return "max time span";
            }

            List<string> fragments = GetNonZeroFragments(timeSpan);

            if (!fragments.Any())
            {
                return "default";
            }

            string sign = (timeSpan.Ticks >= 0) ? "" : "-";

            if (fragments.Count == 1)
            {
                return sign + fragments.Single();
            }
            else
            {
                return sign + JoinUsingWritingStyle(fragments);
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
            if ((timeSpan.Seconds > 0) || (timeSpan.Milliseconds > 0))
            {
                string result = timeSpan.Seconds.ToString(CultureInfo.InvariantCulture);

                if (timeSpan.Milliseconds > 0)
                {
                    result += "." + timeSpan.Milliseconds.ToString("000", CultureInfo.InvariantCulture);
                }

                fragments.Add(result + "s");
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

        private static string JoinUsingWritingStyle(IEnumerable<string> fragments)
        {
            return string.Join(", ", AllButLastFragment(fragments)) + " and " + fragments.Last();
        }

        private static string[] AllButLastFragment(IEnumerable<string> fragments)
        {
            return fragments.Take(fragments.Count() - 1).ToArray();
        }
    }
}
