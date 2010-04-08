using System;
using System.Collections.Generic;

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
            TimeSpan timeSpan = (TimeSpan) value;

            List<string> fragments = new List<string>();

            if (timeSpan.Days > 0)
            {
                fragments.Add(timeSpan.Days + "d");
            }

            if (timeSpan.Hours > 0)
            {
                fragments.Add(string.Format("{0}h{1}", timeSpan.Hours, timeSpan.Minutes.ToString("00")));
            }
            else if ((timeSpan.Minutes > 0) || (timeSpan.Seconds > 0))
            {
                string result = string.Format("{0}m{1}", timeSpan.Minutes, timeSpan.Seconds.ToString("00"));
                if (timeSpan.Milliseconds > 0)
                {
                    result += "." + timeSpan.Milliseconds;
                }

                fragments.Add(result);
            }
            else if ((timeSpan.Seconds > 0) || (timeSpan.Milliseconds> 0))
            {
                fragments.Add(string.Format("{0}.{1}s", timeSpan.Seconds.ToString("00"), timeSpan.Milliseconds.ToString("00")));
            }

            return string.Join(" ", fragments.ToArray());
        }
    }
}