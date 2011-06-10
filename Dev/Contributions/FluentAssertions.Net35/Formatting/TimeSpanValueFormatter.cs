using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Formatting
{
    internal class TimeSpanValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is TimeSpan;
        }

        public string ToString(object value)
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