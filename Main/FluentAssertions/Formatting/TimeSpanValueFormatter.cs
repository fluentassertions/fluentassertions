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
            var timeSpan = (TimeSpan) value;
            var fragments = new List<string>();

            AddDaysIfNotZero(timeSpan, fragments);
            AddHoursIfNotZero(timeSpan, fragments);
            AddMinutesIfNotZero(timeSpan, fragments);
            AddSecondsIfNotZero(timeSpan, fragments);

            if (fragments.Count == 1)
            {
                return fragments.Single();
            }
            else
            {
                return JoinUsingWritingStyle(fragments);
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

        private static string JoinUsingWritingStyle(ICollection<string> fragments)
        {
            return string.Join(", ", fragments.Take(fragments.Count - 1).ToArray()) + " and " + fragments.Last();
        }
    }
}