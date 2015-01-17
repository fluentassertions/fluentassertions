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
            try
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), TimeSpan.Zero);
            }
        }
    }
}