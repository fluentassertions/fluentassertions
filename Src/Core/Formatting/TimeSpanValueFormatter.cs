using System;
using System.Collections.Generic;
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
            var timeSpan = (TimeSpan) value;

            if (timeSpan == TimeSpan.MinValue)
            {
                return "min time span";
            }

            if (timeSpan == TimeSpan.MaxValue)
            {
                return "max time span";
            }

            IEnumerable<string> fragments = GetNonZeroFragments(timeSpan);

            if (!fragments.Any())
            {
                return "default";
            }

            string sign = (timeSpan.TotalMilliseconds >= 0) ? "" : "-";

            if (fragments.Count() == 1)
            {
                return sign + fragments.Single();
            }
            else
            {
                return sign + JoinUsingWritingStyle(fragments);
            }
        }

        private static IEnumerable<string> GetNonZeroFragments(TimeSpan timeSpan)
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
            if (timeSpan.Ticks > 0 && timeSpan.Ticks < TimeSpan.TicksPerMillisecond)
            {
                var microSeconds = timeSpan.Ticks / (double)TimeSpan.TicksPerMillisecond * 1000;
                fragments.Add(microSeconds.ToString("0.0") + "us");
            }
        }

        private static void AddSecondsIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if ((timeSpan.Seconds > 0) || (timeSpan.Milliseconds > 0))
            {
                string result = timeSpan.Seconds.ToString();

                if (timeSpan.Milliseconds > 0)
                {
                    result += "." + timeSpan.Milliseconds.ToString("000");
                }

                fragments.Add(result + "s");
            }
        }

        private static void AddMinutesIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if (timeSpan.Minutes > 0)
            {
                fragments.Add(timeSpan.Minutes + "m");
            }
        }

        private static void AddHoursIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if (timeSpan.Hours > 0)
            {
                fragments.Add(timeSpan.Hours + "h");
            }
        }

        private static void AddDaysIfNotZero(TimeSpan timeSpan, List<string> fragments)
        {
            if (timeSpan.Days > 0)
            {
                fragments.Add(timeSpan.Days + "d");
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