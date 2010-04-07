using System;
using System.Collections.Generic;

namespace FluentAssertions.Formatting
{
    internal class TimeSpanFormatter : IFormatter
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

            if ((timeSpan.Hours > 0) || (timeSpan.Minutes > 0))
            {
                fragments.Add(string.Format("{0}h{1}", timeSpan.Hours, timeSpan.Minutes.ToString("00")));
            }

            if ((timeSpan.Seconds > 0) || (timeSpan.Milliseconds> 0))
            {
                fragments.Add(string.Format("{0}.{1}s", timeSpan.Seconds, timeSpan.Milliseconds.ToString("00")));
            }

            return string.Join(" ", fragments.ToArray());
        }
    }
}