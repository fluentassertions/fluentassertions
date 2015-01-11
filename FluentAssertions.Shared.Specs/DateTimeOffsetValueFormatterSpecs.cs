using System;

using FluentAssertions.Formatting;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class DateTimeOffsetValueFormatterSpecs
    {
        [TestMethod]
        public void When_time_is_not_relevant_it_should_not_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = formatter.ToString(new DateTime(1973, 9, 20), false);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<1973-09-20>");
        }

        [TestMethod]
        public void When_date_is_not_relevant_it_should_not_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            DateTime emptyDate = 1.January(0001);
            var dateTime = emptyDate.At(08, 20, 01);
            string result = formatter.ToString(dateTime, false);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<08:20:01>");
        }

        [TestMethod]
        public void When_a_full_date_and_time_is_specified_all_parts_should_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var dateTime = 1.May(2012).At(20, 15, 30, 318);
            string result = formatter.ToString(dateTime, false);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be(dateTime.ToString("<yyyy-MM-dd HH:mm:ss.fff>"));
        }

        [TestMethod]
        public void When_milliseconds_are_not_relevant_they_should_not_be_included_in_the_output()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var dateTime = 1.May(2012).At(20, 15, 30);
            string result = formatter.ToString(dateTime, false);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("<2012-05-01 20:15:30>");
        }

        [TestMethod]
        public void
            When_a_DateTimeOffset_is_formatted_it_should_mention_offset()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var nonUtcFormatter = new DateTimeOffsetValueFormatter { TimeZoneOffset = TimeSpan.FromHours(8) };
            var utcFormatter = new DateTimeOffsetValueFormatter { TimeZoneOffset = TimeSpan.Zero };

            Func<DateTimeOffset, string> formatNonUtc = value => nonUtcFormatter.ToString(value, false);
            Func<DateTimeOffset, string> formatUtc = value => utcFormatter.ToString(value, false);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            formatNonUtc(new DateTimeOffset(2015, 11, 15, 0, 0, 0, TimeSpan.FromHours(13))).Should().Be("<2015-11-15 UTC+13>");
            formatNonUtc(new DateTimeOffset(2015, 11, 1, 21, 0, 0, -TimeSpan.FromHours(1))).Should().Be("<2015-11-01 21:00:00 UTC-1>");
            formatNonUtc(new DateTimeOffset(2014, 12, 30, 15, 41, 58, -new TimeSpan(7, 2, 0))).Should().Be("<2014-12-30 15:41:58 UTC-7:02>");
            formatNonUtc(new DateTimeOffset(2014, 12, 30, 0, 0, 0, new TimeSpan(8, 48, 0))).Should().Be("<2014-12-30 UTC+8:48>");
            formatNonUtc(new DateTimeOffset(1996, 3, 8, 11, 35, 14, TimeSpan.Zero)).Should().Be("<1996-03-08 11:35:14 UTC>");
            formatNonUtc(new DateTimeOffset(2015, 11, 15, 0, 0, 0, TimeSpan.FromHours(8))).Should().Be("<2015-11-15>");

            formatUtc(new DateTimeOffset(1987, 3, 15, 13, 16, 19, TimeSpan.Zero)).Should().Be("<1987-03-15 13:16:19>");
            formatUtc(new DateTimeOffset(2015, 11, 15, 0, 0, 0, TimeSpan.FromHours(8))).Should().Be("<2015-11-15 UTC+8>");
        }

        [TestMethod]
        public void
            When_a_DateTimeOffsetValueFormatter_is_created_it_should_default_TimeZoneOffset_to_current_local()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            formatter.TimeZoneOffset.Should().Be(TimeZoneInfo.Local.BaseUtcOffset);
        }

        [TestMethod]
        public void
            When_a_DateTime_is_used_it_should_format_the_same_as_a_DateTimeOffset()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var formatter = new DateTimeOffsetValueFormatter();

            var dateOnly = new DateTime(1973, 9, 20);
            var timeOnly = 1.January(0001).At(08, 20, 01);
            var witoutMilliseconds = 1.May(2012).At(20, 15, 30);
            var withMilliseconds = 1.May(2012).At(20, 15, 30, 318);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            formatter.ToString(dateOnly, false).Should().Be(formatter.ToString((DateTimeOffset) dateOnly, false));
            formatter.ToString(timeOnly, false).Should().Be(formatter.ToString((DateTimeOffset) timeOnly, false));
            formatter.ToString(witoutMilliseconds, false).Should().Be(formatter.ToString((DateTimeOffset) witoutMilliseconds, false));
            formatter.ToString(withMilliseconds, false).Should().Be(formatter.ToString((DateTimeOffset) withMilliseconds, false));
        }

        private static DateTime ToUtcWithoutChangingTime(DateTime date)
        {
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }
    }
}