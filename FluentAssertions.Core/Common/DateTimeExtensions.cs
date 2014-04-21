using System;

namespace FluentAssertions.Common
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts an existing <see cref="DateTime"/> to a <see cref="DateTimeOffset"/> but normalizes the <see cref="DateTimeKind"/> 
        /// so that comparisons of converted <see cref="DateTime"/> instances retain the UTC/local agnostic behavior.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTimeOffset.MinValue;
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTimeOffset.MaxValue;
            }
            else
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }
        }
    }
}