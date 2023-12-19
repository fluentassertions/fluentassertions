using System;
using System.Globalization;
using FluentAssertionsAsync.Formatting;
using Xunit;

namespace FluentAssertionsAsync.Specs.Formatting;

public class TimeSpanFormatterSpecs
{
    [Fact]
    public void When_zero_time_span_it_should_return_a_literal()
    {
        // Act
        string result = Formatter.ToString(TimeSpan.Zero);

        // Assert
        result.Should().Be("default");
    }

    [Fact]
    public void When_max_time_span_it_should_return_a_literal()
    {
        // Act
        string result = Formatter.ToString(TimeSpan.MaxValue);

        // Assert
        result.Should().Be("max time span");
    }

    [Fact]
    public void When_min_time_span_it_should_return_a_literal()
    {
        // Act
        string result = Formatter.ToString(TimeSpan.MinValue);

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
    [InlineData("00:00:00.123", "123ms")]
    [InlineData("-00:00:00.123", "-123ms")]
    [InlineData("00:00:00.123456", "123ms and 456.0µs")]
    [InlineData("-00:00:00.123456", "-123ms and 456.0µs")]
    [InlineData("00:00:00.1234567", "123ms and 456.7µs")]
    [InlineData("-00:00:00.1234567", "-123ms and 456.7µs")]
    [InlineData("00:00:04", "4s")]
    [InlineData("-00:00:04", "-4s")]
    [InlineData("00:03:04", "3m and 4s")]
    [InlineData("-00:03:04", "-3m and 4s")]
    [InlineData("1.02:03:04", "1d, 2h, 3m and 4s")]
    [InlineData("-1.02:03:04", "-1d, 2h, 3m and 4s")]
    [InlineData("01:02:03", "1h, 2m and 3s")]
    [InlineData("-01:02:03", "-1h, 2m and 3s")]
    [InlineData("01:02:03.123", "1h, 2m, 3s and 123ms")]
    [InlineData("-01:02:03.123", "-1h, 2m, 3s and 123ms")]
    [InlineData("01:02:03.123456", "1h, 2m, 3s, 123ms and 456.0µs")]
    [InlineData("-01:02:03.123456", "-1h, 2m, 3s, 123ms and 456.0µs")]
    [InlineData("01:02:03.1234567", "1h, 2m, 3s, 123ms and 456.7µs")]
    [InlineData("-01:02:03.1234567", "-1h, 2m, 3s, 123ms and 456.7µs")]
    public void When_timespan_components_are_not_relevant_they_should_not_be_included_in_the_output(string actual,
        string expected)
    {
        // Arrange
        var value = TimeSpan.Parse(actual, CultureInfo.InvariantCulture);

        // Act
        string result = Formatter.ToString(value);

        // Assert
        result.Should().Be(expected);
    }
}
