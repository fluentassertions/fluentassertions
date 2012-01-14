using System;
using System.Collections.Generic;

namespace FluentAssertions.Formatting
{
    public class DateTimeValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return (value is DateTime);
        }

        public string ToString(object value)
        {
            var dateTime = (DateTime)value;

            var fragments = new List<string>();

            if (HasDate(dateTime))
            {
                fragments.Add(dateTime.ToString("yyyy-MM-dd"));
            }

            if (HasTime(dateTime))
            {
                fragments.Add(dateTime.ToString("HH:mm:ss"));
            }

            return "<" + string.Join(" ", fragments.ToArray()) + ">";
        }

        private static bool HasTime(DateTime dateTime)
        {
            return (dateTime.Hour != 0) || (dateTime.Minute != 0) || (dateTime.Second != 0);
        }

        private static bool HasDate(DateTime dateTime)
        {
            return (dateTime.Day != 1) || (dateTime.Month != 1) || (dateTime.Year != 1);
        }
    }
}