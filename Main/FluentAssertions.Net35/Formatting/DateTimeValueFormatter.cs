using System;
using System.Collections.Generic;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    internal class DateTimeValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return (value is DateTime);
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