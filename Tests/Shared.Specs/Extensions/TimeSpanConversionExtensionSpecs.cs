using System;
using System.Globalization;
using FluentAssertions.Extensions;
using Xunit;

namespace FluentAssertions.Specs
{
    public class TimeSpanConversionExtensionSpecs
    {
        [Fact]
        public void When_getting_the_number_of_days_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.Days();

            // Assert
            time.Should().Be(TimeSpan.FromDays(4));
        }

        [Fact]
        public void When_getting_the_number_of_days_from_a_double_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.5.Days();

            // Assert
            time.Should().Be(TimeSpan.FromDays(4.5));
        }

        [Fact]
        public void When_getting_the_number_of_hours_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.Hours();

            // Assert
            time.Should().Be(TimeSpan.FromHours(4));
        }

        [Fact]
        public void When_getting_the_number_of_hours_from_a_double_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.5.Hours();

            // Assert
            time.Should().Be(TimeSpan.FromHours(4.5));
        }

        [Fact]
        public void When_getting_the_number_of_minutes_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.Minutes();

            // Assert
            time.Should().Be(TimeSpan.FromMinutes(4));
        }

        [Fact]
        public void When_getting_the_number_of_minutes_from_a_double_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.5.Minutes();

            // Assert
            time.Should().Be(TimeSpan.FromMinutes(4.5));
        }

        [Fact]
        public void When_getting_the_number_of_seconds_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.Seconds();

            // Assert
            time.Should().Be(TimeSpan.FromSeconds(4));
        }

        [Fact]
        public void When_getting_the_number_of_seconds_from_a_double_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.5.Seconds();

            // Assert
            time.Should().Be(TimeSpan.FromSeconds(4.5));
        }

        [Fact]
        public void When_getting_the_nanoseconds_component_it_should_return_the_correct_value()
        {
            // Arrange
            var time = TimeSpan.Parse("01:02:03.1234567", CultureInfo.InvariantCulture);

            // Act
            var value = time.Nanoseconds();

            // Assert
            value.Should().Be(700);
        }

        [Fact]
        public void When_getting_the_number_of_nanoseconds_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 200.Nanoseconds();

            // Assert
            time.Should().Be(TimeSpan.Parse("00:00:00.0000002", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void When_getting_the_number_of_nanoseconds_from_a_long_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 200L.Nanoseconds();

            // Assert
            time.Should().Be(TimeSpan.Parse("00:00:00.0000002", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void When_getting_the_total_number_of_nanoseconds_should_return_the_correct_double_value()
        {
            // Arrange
            TimeSpan time = 1.Milliseconds();

            // Act
            double total = time.TotalNanoseconds();

            // Assert
            total.Should().Be(1_000_000);
        }

        [Fact]
        public void When_getting_the_microseconds_component_it_should_return_the_correct_value()
        {
            // Arrange
            var time = TimeSpan.Parse("01:02:03.1234567", CultureInfo.InvariantCulture);

            // Act
            var value = time.Microseconds();

            // Assert
            value.Should().Be(456);
        }

        [Fact]
        public void When_getting_the_number_of_microseconds_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.Microseconds();

            // Assert
            time.Should().Be(TimeSpan.Parse("00:00:00.000004", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void When_getting_the_number_of_microseconds_from_a_long_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4L.Microseconds();

            // Assert
            time.Should().Be(TimeSpan.Parse("00:00:00.000004", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void When_getting_the_total_number_of_microseconds_should_return_the_correct_double_value()
        {
            // Arrange
            TimeSpan time = 1.Milliseconds();

            // Act
            double total = time.TotalMicroseconds();

            // Assert
            total.Should().Be(1_000);
        }

        [Fact]
        public void When_getting_the_number_of_milliseconds_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.Milliseconds();

            // Assert
            time.Should().Be(TimeSpan.FromMilliseconds(4));
        }

        [Fact]
        public void When_getting_the_number_of_milliseconds_from_a_double_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.5.Milliseconds();

            // Assert
            time.Should().Be(TimeSpan.FromMilliseconds(4.5));
        }

        [Fact]
        public void When_getting_the_number_of_ticks_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4.Ticks();

            // Assert
            time.Should().Be(TimeSpan.FromTicks(4));
        }

        [Fact]
        public void When_getting_the_number_of_ticks_from_a_long_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time = 4L.Ticks();

            // Assert
            time.Should().Be(TimeSpan.FromTicks(4));
        }

        [Fact]
        public void When_combining_fluent_time_methods_it_should_return_the_correct_time_span_value()
        {
            // Act
            TimeSpan time1 = 23.Hours().And(59.Minutes());
            TimeSpan time2 = 23.Hours(59.Minutes()).And(20.Seconds());
            TimeSpan time3 = 1.Days(2.Hours(33.Minutes(44.Seconds()))).And(99.Milliseconds());

            // Assert
            time1.Should().Be(new TimeSpan(23, 59, 0));
            time2.Should().Be(new TimeSpan(23, 59, 20));
            time3.Should().Be(new TimeSpan(1, 2, 33, 44, 99));
        }

        [Fact]
        public void When_specifying_a_time_before_another_time_it_should_return_the_correct_time()
        {
            // Act
            DateTime now = 21.September(2011).At(07, 35);

            DateTime twoHoursAgo = 2.Hours().Before(now);

            // Assert
            twoHoursAgo.Should().Be(new DateTime(2011, 9, 21, 05, 35, 00));
        }

        [Fact]
        public void When_specifying_a_time_after_another_time_it_should_return_the_correct_time()
        {
            // Act
            DateTime now = 21.September(2011).At(07, 35);

            DateTime twoHoursLater = 2.Hours().After(now);

            // Assert
            twoHoursLater.Should().Be(new DateTime(2011, 9, 21, 09, 35, 00));
        }
    }
}
