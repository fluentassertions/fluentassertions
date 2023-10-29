using System;
using FluentAssertions.Extensions;
using Xunit;

namespace FluentAssertions.Specs.Extensions;

public class DateBuilderSpecs
{
    public class ByMonth
    {
        [Fact]
        public void January_creates_month_01()
        {
            // Act
            DateTime date = 10.January(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 01, 10));
        }

        [Fact]
        public void February_creates_month_02()
        {
            // Act
            DateTime date = 10.February(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 02, 10));
        }

        [Fact]
        public void March_creates_month_03()
        {
            // Act
            DateTime date = 10.March(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 03, 10));
        }

        [Fact]
        public void April_creates_month_04()
        {
            // Act
            DateTime date = 10.April(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 04, 10));
        }

        [Fact]
        public void May_creates_month_05()
        {
            // Act
            DateTime date = 10.May(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 05, 10));
        }

        [Fact]
        public void June_creates_month_06()
        {
            // Act
            DateTime date = 10.June(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 06, 10));
        }

        [Fact]
        public void July_crates_month_07()
        {
            // Act
            DateTime date = 10.July(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 07, 10));
        }

        [Fact]
        public void August_creates_month_08()
        {
            // Act
            DateTime date = 10.August(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 08, 10));
        }

        [Fact]
        public void September_creates_month_09()
        {
            // Act
            DateTime date = 10.September(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 09, 10));
        }

        [Fact]
        public void October_creates_month_10()
        {
            // Act
            DateTime date = 03.October(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 10, 03));
        }

        [Fact]
        public void November_creates_month_11()
        {
            // Act
            DateTime date = 10.November(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 11, 10));
        }

        [Fact]
        public void December_creates_month_12()
        {
            // Act
            DateTime date = 10.December(2011);

            // Assert
            date.Should().Be(new DateTime(2011, 12, 10));
        }
    }

    public class ToDateTime
    {
        [Fact]
        public void When_fluently_specifying_a_date_and_time_it_should_return_the_correct_date_time_value()
        {
            // Act
            DateTime dateTime = 10.December(2011).At(09, 30, 45, 123, 456, 700);

            // Assert
            dateTime.Should().Be(new DateTime(2011, 12, 10, 9, 30, 45, 123).AddMicroseconds(456).AddNanoseconds(700));
            dateTime.Microsecond().Should().Be(456);
            dateTime.Nanosecond().Should().Be(700);
            dateTime.Should().BeIn(DateTimeKind.Unspecified);
        }

        [InlineData(-1)]
        [InlineData(1000)]
        [Theory]
        public void When_fluently_specifying_a_datetime_with_out_of_range_microseconds_it_should_throw(int microseconds)
        {
            // Act
            string expectedParameterName = "microseconds";
            Action act = () => 10.December(2011).At(0, 0, 0, 0, microseconds);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(expectedParameterName);
        }

        [InlineData(0)]
        [InlineData(999)]
        [Theory]
        public void When_fluently_specifying_a_datetime_with_inrange_microseconds_it_should_not_throw(int microseconds)
        {
            // Act
            Action act = () => 10.December(2011).At(0, 0, 0, 0, microseconds);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(-1)]
        [InlineData(1000)]
        [Theory]
        public void When_fluently_specifying_a_datetime_with_out_of_range_nanoseconds_it_should_throw(int nanoseconds)
        {
            // Act
            var expectedParameterName = "nanoseconds";
            Action act = () => 10.December(2011).At(0, 0, 0, 0, 0, nanoseconds);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(expectedParameterName);
        }

        [InlineData(0)]
        [InlineData(999)]
        [Theory]
        public void When_fluently_specifying_a_datetime_with_inrange_nanoseconds_it_should_not_throw(int nanoseconds)
        {
            // Act
            Action act = () => 10.December(2011).At(0, 0, 0, 0, 0, nanoseconds);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_fluently_specifying_a_date_and_time_as_utc_it_should_return_the_date_time_value_with_utc_kind()
        {
            // Act
            DateTime dateTime = 10.December(2011).At(09, 30, 45, 123, 456, 700).AsUtc();

            // Assert
            dateTime.Should().Be(new DateTime(2011, 12, 10, 9, 30, 45, 123).AddMicroseconds(456).AddNanoseconds(700));
            dateTime.Microsecond().Should().Be(456);
            dateTime.Nanosecond().Should().Be(700);
            dateTime.Should().BeIn(DateTimeKind.Utc);
        }

        [Fact]
        public void When_fluently_specifying_a_date_and_time_as_local_it_should_return_the_date_time_value_with_local_kind()
        {
            // Act
            DateTime dateTime = 10.December(2011).At(09, 30, 45, 123, 456, 700).AsLocal();

            // Assert
            dateTime.Should().Be(new DateTime(2011, 12, 10, 9, 30, 45, 123).AddMicroseconds(456).AddNanoseconds(700));
            dateTime.Microsecond().Should().Be(456);
            dateTime.Nanosecond().Should().Be(700);
            dateTime.Should().BeIn(DateTimeKind.Local);
        }

        [Fact]
        public void When_fluently_specifying_a_date_and_timespan_it_should_return_the_correct_date_time_value()
        {
            // Act
            var time = 9.Hours().And(30.Minutes()).And(45.Seconds());
            DateTime dateTime = 10.December(2011).At(time);

            // Assert
            dateTime.Should().Be(new DateTime(2011, 12, 10, 9, 30, 45));
        }
    }

    public class ToDateTimeOffset
    {
        [Fact]
        public void When_fluently_specifying_a_datetimeoffset_and_time_it_should_return_the_correct_date_time_value()
        {
            // Act
            DateTimeOffset dateTime = 10.December(2011).AsOffset().At(09, 30, 45, 123, 456, 700);

            // Assert
            dateTime.Should().Be(new DateTimeOffset(2011, 12, 10, 9, 30, 45, 123, TimeSpan.Zero).AddMicroseconds(456)
                .AddNanoseconds(700));

            dateTime.Microsecond().Should().Be(456);
            dateTime.Nanosecond().Should().Be(700);
        }

        [InlineData(-1)]
        [InlineData(1000)]
        [Theory]
        public void When_fluently_specifying_a_datetimeoffset_with_out_of_range_microseconds_it_should_throw(int microseconds)
        {
            // Act
            var expectedParameterName = "microseconds";
            Action act = () => 10.December(2011).AsOffset().At(0, 0, 0, 0, microseconds);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(expectedParameterName);
        }

        [InlineData(0)]
        [InlineData(999)]
        [Theory]
        public void When_fluently_specifying_a_datetimeoffset_with_inrange_microseconds_it_should_not_throw(int microseconds)
        {
            // Act
            Action act = () => 10.December(2011).AsOffset().At(0, 0, 0, 0, microseconds);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(-1)]
        [InlineData(1000)]
        [Theory]
        public void When_fluently_specifying_a_datetimeoffset_with_out_of_range_nanoseconds_it_should_throw(int nanoseconds)
        {
            // Act
            var expectedParameterName = "nanoseconds";
            Action act = () => 10.December(2011).AsOffset().At(0, 0, 0, 0, 0, nanoseconds);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(expectedParameterName);
        }

        [InlineData(0)]
        [InlineData(999)]
        [Theory]
        public void When_fluently_specifying_a_datetimeoffset_with_inrange_nanoseconds_it_should_not_throw(int nanoseconds)
        {
            // Act
            Action act = () => 10.December(2011).AsOffset().At(0, 0, 0, 0, 0, nanoseconds);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class TimeOfDay
    {
        [Fact]
        public void Specifying_the_time_of_day_retains_the_full_precision()
        {
            // Act
            DateTime subject = 10.December(2011).At(1.Ticks());
            DateTime expected = 10.December(2011) + 1.Ticks();

            // Assert
            subject.Should().Be(expected);
        }

        [Fact]
        public void Specifying_the_time_of_day_retains_the_datetime_kind()
        {
            // Act
            DateTime dateTime = 10.December(2011).AsUtc().At(TimeSpan.Zero);

            // Assert
            dateTime.Should().BeIn(DateTimeKind.Utc);
        }

        [Fact]
        public void Specifying_the_time_of_day_in_hours_and_minutes_retains_the_datetime_kind()
        {
            // Act
            DateTime dateTime = 10.December(2011).AsUtc().At(0, 0);

            // Assert
            dateTime.Should().BeIn(DateTimeKind.Utc);
        }
    }

    public class EquivalentTo
    {
        [Fact]
        public void DateTime()
        {
            // Arrange
            DateBuilder builder = 11.June(2017);
            object date = new DateTime(2017, 06, 11);

            // Act + assert
            date.Should().BeEquivalentTo(builder);
        }

#if NET6_0_OR_GREATER
        [Fact]
        public void DateOnly()
        {
            // Arrange
            DateBuilder builder = 11.June(2017);
            object date = new DateOnly(2017, 06, 11);

            // Act + assert
            date.Should().BeEquivalentTo(builder);
        }
#endif
    }
}
