using System;
using System.Globalization;
using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs
{
    public class TimeSpanFormatterSpecs
    {
        [Fact]
        public void When_zero_time_span_it_should_return_a_literal()
        {
            // Arrange
            var formatter = new TimeSpanValueFormatter();

            // Act
            string result = formatter.Format(TimeSpan.Zero, new FormattingContext(), null);

            // Assert
            result.Should().Be("default");
        }

        [Fact]
        public void When_max_time_span_it_should_return_a_literal()
        {
            // Arrange
            var formatter = new TimeSpanValueFormatter();

            // Act
            string result = formatter.Format(TimeSpan.MaxValue, new FormattingContext(), null);

            // Assert
            result.Should().Be("max time span");
        }

        [Fact]
        public void When_min_time_span_it_should_return_a_literal()
        {
            // Arrange
            var formatter = new TimeSpanValueFormatter();

            // Act
            string result = formatter.Format(TimeSpan.MinValue, new FormattingContext(), null);

            // Assert
            result.Should().Be("min time span");
        }

        [Theory]
        [InlineData("00:00:00.0000007", "0.7µs")]
        [InlineData("-00:00:00.0000007", "-0.7µs")]
        [InlineData("00:00:00.000456", "456.0µs")]
        [InlineData("-00:00:00.000456", "-456.0µs")]
        [InlineData("00:00:00.0004567", "456.7µs")]
        [InlineData("-00:00:00.0004567", "-456.7µs")]
        [InlineData("00:00:00.123", "0.123s")]
        [InlineData("-00:00:00.123", "-0.123s")]
        [InlineData("00:00:00.123456", "0.123s and 456.0µs")]
        [InlineData("-00:00:00.123456", "-0.123s and 456.0µs")]
        [InlineData("00:00:00.1234567", "0.123s and 456.7µs")]
        [InlineData("-00:00:00.1234567", "-0.123s and 456.7µs")]
        [InlineData("00:00:04", "4s")]
        [InlineData("-00:00:04", "-4s")]
        [InlineData("00:03:04", "3m and 4s")]
        [InlineData("-00:03:04", "-3m and 4s")]
        [InlineData("1.02:03:04", "1d, 2h, 3m and 4s")]
        [InlineData("-1.02:03:04", "-1d, 2h, 3m and 4s")]
        [InlineData("01:02:03", "1h, 2m and 3s")]
        [InlineData("-01:02:03", "-1h, 2m and 3s")]
        [InlineData("01:02:03.123", "1h, 2m and 3.123s")]
        [InlineData("-01:02:03.123", "-1h, 2m and 3.123s")]
        [InlineData("01:02:03.123456", "1h, 2m, 3.123s and 456.0µs")]
        [InlineData("-01:02:03.123456", "-1h, 2m, 3.123s and 456.0µs")]
        [InlineData("01:02:03.1234567", "1h, 2m, 3.123s and 456.7µs")]
        [InlineData("-01:02:03.1234567", "-1h, 2m, 3.123s and 456.7µs")]
        public void When_timespan_components_are_not_relevant_they_should_not_be_included_in_the_output(string actual, string expected)
        {
            // Arrange
            var formatter = new TimeSpanValueFormatter();
            var value = TimeSpan.Parse(actual, CultureInfo.InvariantCulture);

            // Act
            string result = formatter.Format(value, new FormattingContext(), null);

            // Assert
            result.Should().Be(expected);
        }
    }
}
