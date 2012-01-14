using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    internal class TimeSpanValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is TimeSpan;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="uniqueObjectTracker">
        /// An object that is passed through recursive calls and which should be used to detect circular references
        /// in the object graph that is being converted to a string representation.</param>
        /// <param name="nestedPropertyLevel">
        ///     The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        ///     no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, UniqueObjectTracker uniqueObjectTracker = null, int nestedPropertyLevel = 0)
        {
            var timeSpan = (TimeSpan)value;

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
            
            return fragments;
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