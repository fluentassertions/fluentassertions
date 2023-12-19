using System;
using FluentAssertionsAsync.Extensions;
using FluentAssertionsAsync.Primitives;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public class SimpleTimeSpanAssertionSpecs
{
    [Fact]
    public void When_asserting_positive_value_to_be_positive_it_should_succeed()
    {
        // Arrange
        TimeSpan timeSpan = 1.Seconds();

        // Act / Assert
        timeSpan.Should().BePositive();
    }

    [Fact]
    public void When_asserting_negative_value_to_be_positive_it_should_fail()
    {
        // Arrange
        TimeSpan negatedTimeSpan = 1.Seconds().Negate();

        // Act
        Action act = () => negatedTimeSpan.Should().BePositive();

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_zero_value_to_be_positive_it_should_fail()
    {
        // Arrange
        TimeSpan negatedTimeSpan = 0.Seconds();

        // Act
        Action act = () => negatedTimeSpan.Should().BePositive();

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_null_value_to_be_positive_it_should_fail()
    {
        // Arrange
        TimeSpan? nullTimeSpan = null;

        // Act
        Action act = () => nullTimeSpan.Should().BePositive("because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected nullTimeSpan to be positive because we want to test the failure message, but found <null>.");
    }

    [Fact]
    public void When_asserting_negative_value_to_be_positive_it_should_fail_with_descriptive_message()
    {
        // Arrange
        TimeSpan timeSpan = 1.Seconds().Negate();

        // Act
        Action act = () => timeSpan.Should().BePositive("because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected timeSpan to be positive because we want to test the failure message, but found -1s.");
    }

    [Fact]
    public void When_asserting_negative_value_to_be_negative_it_should_succeed()
    {
        // Arrange
        TimeSpan timeSpan = 1.Seconds().Negate();

        // Act / Assert
        timeSpan.Should().BeNegative();
    }

    [Fact]
    public void When_asserting_positive_value_to_be_negative_it_should_fail()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();

        // Act
        Action act = () => actual.Should().BeNegative();

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_zero_value_to_be_negative_it_should_fail()
    {
        // Arrange
        TimeSpan actual = 0.Seconds();

        // Act
        Action act = () => actual.Should().BeNegative();

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_null_value_to_be_negative_it_should_fail()
    {
        // Arrange
        TimeSpan? nullTimeSpan = null;

        // Act
        Action act = () => nullTimeSpan.Should().BeNegative("because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected nullTimeSpan to be negative because we want to test the failure message, but found <null>.");
    }

    [Fact]
    public void When_asserting_positive_value_to_be_negative_it_should_fail_with_descriptive_message()
    {
        // Arrange
        TimeSpan timeSpan = 1.Seconds();

        // Act
        Action act = () => timeSpan.Should().BeNegative("because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected timeSpan to be negative because we want to test the failure message, but found 1s.");
    }

    [Fact]
    public void When_asserting_value_to_be_equal_to_same_value_it_should_succeed()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();
        var expected = TimeSpan.FromSeconds(1);

        // Act / Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void When_asserting_value_to_be_equal_to_different_value_it_should_fail()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();
        TimeSpan expected = 2.Seconds();

        // Act
        Action act = () => actual.Should().Be(expected);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void A_null_is_not_equal_to_another_value()
    {
        // Arrange
        var subject = new SimpleTimeSpanAssertions(null);
        TimeSpan expected = 2.Seconds();

        // Act
        Action act = () => subject.Be(expected);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_null_value_to_be_equal_to_different_value_it_should_fail()
    {
        // Arrange
        TimeSpan? nullTimeSpan = null;
        TimeSpan expected = 1.Seconds();

        // Act
        Action act = () => nullTimeSpan.Should().Be(expected, "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected 1s because we want to test the failure message, but found <null>.");
    }

    [Fact]
    public void When_asserting_value_to_be_equal_to_different_value_it_should_fail_with_descriptive_message()
    {
        // Arrange
        TimeSpan timeSpan = 1.Seconds();

        // Act
        Action act = () => timeSpan.Should().Be(2.Seconds(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected 2s because we want to test the failure message, but found 1s.");
    }

    [Fact]
    public void When_asserting_value_to_be_not_equal_to_different_value_it_should_succeed()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();
        TimeSpan unexpected = 2.Seconds();

        // Act / Assert
        actual.Should().NotBe(unexpected);
    }

    [Fact]
    public void When_asserting_null_value_to_be_not_equal_to_different_value_it_should_succeed()
    {
        // Arrange
        TimeSpan? nullTimeSpan = null;
        TimeSpan expected = 1.Seconds();

        // Act / Assert
        nullTimeSpan.Should().NotBe(expected);
    }

    [Fact]
    public void When_asserting_value_to_be_not_equal_to_the_same_value_it_should_fail()
    {
        // Arrange
        var oneSecond = 1.Seconds();

        // Act
        Action act = () => oneSecond.Should().NotBe(oneSecond);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_value_to_be_not_equal_to_the_same_value_it_should_fail_with_descriptive_message()
    {
        // Arrange
        var oneSecond = 1.Seconds();

        // Act
        Action act = () => oneSecond.Should().NotBe(oneSecond, "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Did not expect 1s because we want to test the failure message.");
    }

    [Fact]
    public void When_asserting_value_to_be_greater_than_smaller_value_it_should_succeed()
    {
        // Arrange
        TimeSpan actual = 2.Seconds();
        TimeSpan smaller = 1.Seconds();

        // Act / Assert
        actual.Should().BeGreaterThan(smaller);
    }

    [Fact]
    public void When_asserting_value_to_be_greater_than_greater_value_it_should_fail()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();
        TimeSpan expected = 2.Seconds();

        // Act
        Action act = () => actual.Should().BeGreaterThan(expected);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_null_value_to_be_greater_than_other_value_it_should_fail()
    {
        // Arrange
        TimeSpan? nullTimeSpan = null;
        TimeSpan expected = 1.Seconds();

        // Act
        Action act = () => nullTimeSpan.Should().BeGreaterThan(expected, "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected nullTimeSpan to be greater than 1s because we want to test the failure message, but found <null>.");
    }

    [Fact]
    public void When_asserting_value_to_be_greater_than_same_value_it_should_fail()
    {
        // Arrange
        var twoSeconds = 2.Seconds();

        // Act
        Action act = () => twoSeconds.Should().BeGreaterThan(twoSeconds);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_value_to_be_greater_than_greater_value_it_should_fail_with_descriptive_message()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();

        // Act
        Action act = () => actual.Should().BeGreaterThan(2.Seconds(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected actual to be greater than 2s because we want to test the failure message, but found 1s.");
    }

    [Fact]
    public void When_asserting_value_to_be_greater_than_or_equal_to_smaller_value_it_should_succeed()
    {
        // Arrange
        TimeSpan actual = 2.Seconds();
        TimeSpan smaller = 1.Seconds();

        // Act / Assert
        actual.Should().BeGreaterThanOrEqualTo(smaller);
    }

    [Fact]
    public void When_asserting_null_value_to_be_greater_than_or_equal_to_other_value_it_should_fail()
    {
        // Arrange
        TimeSpan? nullTimeSpan = null;
        TimeSpan expected = 1.Seconds();

        // Act
        Action act = () =>
            nullTimeSpan.Should().BeGreaterThanOrEqualTo(expected, "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected nullTimeSpan to be greater than or equal to 1s because we want to test the failure message, but found <null>.");
    }

    [Fact]
    public void When_asserting_value_to_be_greater_than_or_equal_to_same_value_it_should_succeed()
    {
        // Arrange
        var twoSeconds = 2.Seconds();

        // Act / Assert
        twoSeconds.Should().BeGreaterThanOrEqualTo(twoSeconds);
    }

    [Fact]
    public void Chaining_after_one_assertion_1()
    {
        // Arrange
        var twoSeconds = 2.Seconds();

        // Act / Assert
        twoSeconds.Should().BeGreaterThanOrEqualTo(twoSeconds).And.Be(2.Seconds());
    }

    [Fact]
    public void When_asserting_value_to_be_greater_than_or_equal_to_greater_value_it_should_fail()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();
        TimeSpan expected = 2.Seconds();

        // Act
        Action act = () => actual.Should().BeGreaterThanOrEqualTo(expected);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_value_to_be_greater_than_or_equal_to_greater_value_it_should_fail_with_descriptive_message()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();

        // Act
        Action act = () =>
            actual.Should().BeGreaterThanOrEqualTo(2.Seconds(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected actual to be greater than or equal to 2s because we want to test the failure message, but found 1s.");
    }

    [Fact]
    public void When_asserting_value_to_be_less_than_greater_value_it_should_succeed()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();
        TimeSpan greater = 2.Seconds();

        // Act / Assert
        actual.Should().BeLessThan(greater);
    }

    [Fact]
    public void When_asserting_value_to_be_less_than_smaller_value_it_should_fail()
    {
        // Arrange
        TimeSpan actual = 2.Seconds();
        TimeSpan expected = 1.Seconds();

        // Act
        Action act = () => actual.Should().BeLessThan(expected);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_null_value_to_be_less_than_other_value_it_should_fail()
    {
        // Arrange
        TimeSpan? nullTimeSpan = null;
        TimeSpan expected = 1.Seconds();

        // Act
        Action act = () => nullTimeSpan.Should().BeLessThan(expected, "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected nullTimeSpan to be less than 1s because we want to test the failure message, but found <null>.");
    }

    [Fact]
    public void When_asserting_value_to_be_less_than_same_value_it_should_fail()
    {
        // Arrange
        var twoSeconds = 2.Seconds();

        // Act
        Action act = () => twoSeconds.Should().BeLessThan(twoSeconds);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_value_to_be_less_than_smaller_value_it_should_fail_with_descriptive_message()
    {
        // Arrange
        TimeSpan actual = 2.Seconds();

        // Act
        Action act = () => actual.Should().BeLessThan(1.Seconds(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected actual to be less than 1s because we want to test the failure message, but found 2s.");
    }

    [Fact]
    public void When_asserting_value_to_be_less_than_or_equal_to_greater_value_it_should_succeed()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();
        TimeSpan greater = 2.Seconds();

        // Act / Assert
        actual.Should().BeLessThanOrEqualTo(greater);
    }

    [Fact]
    public void Chaining_after_one_assertion_2()
    {
        // Arrange
        TimeSpan actual = 1.Seconds();
        TimeSpan greater = 2.Seconds();

        // Act / Assert
        actual.Should().BeLessThanOrEqualTo(greater).And.Be(1.Seconds());
    }

    [Fact]
    public void When_asserting_null_value_to_be_less_than_or_equal_to_other_value_it_should_fail()
    {
        // Arrange
        TimeSpan? nullTimeSpan = null;
        TimeSpan expected = 1.Seconds();

        // Act
        Action act = () =>
            nullTimeSpan.Should().BeLessThanOrEqualTo(expected, "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected nullTimeSpan to be less than or equal to 1s because we want to test the failure message, but found <null>.");
    }

    [Fact]
    public void When_asserting_value_to_be_less_than_or_equal_to_same_value_it_should_succeed()
    {
        // Arrange
        var twoSeconds = 2.Seconds();

        // Act / Assert
        twoSeconds.Should().BeLessThanOrEqualTo(twoSeconds);
    }

    [Fact]
    public void When_asserting_value_to_be_less_than_or_equal_to_smaller_value_it_should_fail()
    {
        // Arrange
        TimeSpan actual = 2.Seconds();
        TimeSpan expected = 1.Seconds();

        // Act
        Action act = () => actual.Should().BeLessThanOrEqualTo(expected);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_asserting_value_to_be_less_than_or_equal_to_smaller_value_it_should_fail_with_descriptive_message()
    {
        // Arrange
        TimeSpan actual = 2.Seconds();

        // Act
        Action act = () => actual.Should().BeLessThanOrEqualTo(1.Seconds(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected actual to be less than or equal to 1s because we want to test the failure message, but found 2s.");
    }

    [Fact]
    public void When_accidentally_using_equals_it_should_throw_a_helpful_error()
    {
        // Arrange
        TimeSpan someTimeSpan = 2.Seconds();

        // Act
        Action act = () => someTimeSpan.Should().Equals(someTimeSpan);

        // Assert
        act.Should().Throw<NotSupportedException>()
            .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
    }

    #region Be Close To

    [Fact]
    public void When_asserting_that_time_is_close_to_a_negative_precision_it_should_throw()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 30, 980);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().BeCloseTo(nearbyTime, -1.Ticks());

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("precision")
            .WithMessage("*must be non-negative*");
    }

    [Fact]
    public void When_time_is_less_then_but_close_to_another_value_it_should_succeed()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 30, 980);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_time_is_greater_then_but_close_to_another_value_it_should_succeed()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 31, 020);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_time_is_less_then_and_not_close_to_another_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 30, 979);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds(), "we want to test the error message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected time to be within 20ms from 1d, 12h, 15m and 31s because we want to test the error message, but found 1d, 12h, 15m, 30s and 979ms.");
    }

    [Fact]
    public void When_time_is_greater_then_and_not_close_to_another_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 31, 021);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().BeCloseTo(nearbyTime, 20.Milliseconds(), "we want to test the error message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected time to be within 20ms from 1d, 12h, 15m and 31s because we want to test the error message, but found 1d, 12h, 15m, 31s and 21ms.");
    }

    [Fact]
    public void When_time_is_within_specified_number_of_milliseconds_from_another_value_it_should_succeed()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 31, 035);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().BeCloseTo(nearbyTime, 35.Milliseconds());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_a_null_time_is_asserted_to_be_close_to_another_it_should_throw()
    {
        // Arrange
        TimeSpan? time = null;
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().BeCloseTo(nearbyTime, 35.Milliseconds());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected*, but found <null>.");
    }

    [Fact]
    public void When_time_away_from_another_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 30, 979);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () =>
            time.Should().BeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(20), "we want to test the error message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected time to be within 20ms from 1d, 12h, 15m and 31s because we want to test the error message, but found 1d, 12h, 15m, 30s and 979ms.");
    }

    #endregion

    #region Not Be Close To

    [Fact]
    public void When_asserting_that_time_is_not_close_to_a_negative_precision_it_should_throw()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 30, 980);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().NotBeCloseTo(nearbyTime, -1.Ticks());

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("precision")
            .WithMessage("*must be non-negative*");
    }

    [Fact]
    public void When_asserting_subject_time_is_not_close_to_a_later_time_it_should_throw()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 30, 980);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected time to not be within 20ms from 1d, 12h, 15m and 31s, but found 1d, 12h, 15m, 30s and 980ms.");
    }

    [Fact]
    public void When_asserting_subject_time_is_not_close_to_an_earlier_time_it_should_throw()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 31, 020);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected time to not be within 20ms from 1d, 12h, 15m and 31s, but found 1d, 12h, 15m, 31s and 20ms.");
    }

    [Fact]
    public void When_asserting_subject_time_is_not_close_to_an_earlier_time_by_a_20ms_timespan_it_should_throw()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 31, 020);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().NotBeCloseTo(nearbyTime, TimeSpan.FromMilliseconds(20));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected time to not be within 20ms from 1d, 12h, 15m and 31s, but found 1d, 12h, 15m, 31s and 20ms.");
    }

    [Fact]
    public void When_asserting_subject_time_is_not_close_to_another_value_that_is_later_by_more_than_20ms_it_should_succeed()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 30, 979);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_subject_time_is_not_close_to_another_value_that_is_earlier_by_more_than_20ms_it_should_succeed()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 31, 021);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().NotBeCloseTo(nearbyTime, 20.Milliseconds());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_subject_time_is_not_close_to_an_earlier_time_by_35ms_it_should_throw()
    {
        // Arrange
        var time = new TimeSpan(1, 12, 15, 31, 035);
        var nearbyTime = new TimeSpan(1, 12, 15, 31, 000);

        // Act
        Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35.Milliseconds());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected time to not be within 35ms from 1d, 12h, 15m and 31s, but found 1d, 12h, 15m, 31s and 35ms.");
    }

    [Fact]
    public void When_asserting_subject_null_time_is_not_close_to_another_it_should_throw()
    {
        // Arrange
        TimeSpan? time = null;
        TimeSpan nearbyTime = TimeSpan.FromHours(1);

        // Act
        Action act = () => time.Should().NotBeCloseTo(nearbyTime, 35.Milliseconds());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected*, but found <null>.");
    }

    #endregion
}
