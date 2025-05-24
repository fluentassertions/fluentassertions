using System;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class DateTimePropertiesSpecs
{
    [Fact]
    public void When_two_properties_are_datetime_and_both_are_nullable_and_both_are_null_it_should_succeed()
    {
        // Arrange
        var subject =
            new { Time = (DateTime?)null };

        var other =
            new { Time = (DateTime?)null };

        // Act
        Action act = () =>
            subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_two_properties_are_datetime_and_both_are_nullable_and_are_equal_it_should_succeed()
    {
        // Arrange
        var subject =
            new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

        var other =
            new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void
        When_two_properties_are_datetime_and_both_are_nullable_and_expectation_is_null_it_should_throw_and_state_the_difference()
    {
        // Arrange
        var subject =
            new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

        var other =
            new { Time = (DateTime?)null };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected*Time to be <null>, but found <2013-12-09 15:58:00>.*");
    }

    [Fact]
    public void
        When_two_properties_are_datetime_and_both_are_nullable_and_subject_is_null_it_should_throw_and_state_the_difference()
    {
        // Arrange
        var subject =
            new { Time = (DateTime?)null };

        var other =
            new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected*Time*to be <2013-12-09 15:58:00>, but found <null>.*");
    }

    [Fact]
    public void When_two_properties_are_datetime_and_expectation_is_nullable_and_are_equal_it_should_succeed()
    {
        // Arrange
        var subject =
            new { Time = new DateTime(2013, 12, 9, 15, 58, 0) };

        var other =
            new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void
        When_two_properties_are_datetime_and_expectation_is_nullable_and_expectation_is_null_it_should_throw_and_state_the_difference()
    {
        // Arrange
        var subject =
            new { Time = new DateTime(2013, 12, 9, 15, 58, 0) };

        var other =
            new { Time = (DateTime?)null };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected*Time*to be <null>, but found <2013-12-09 15:58:00>.*");
    }

    [Fact]
    public void When_two_properties_are_datetime_and_subject_is_nullable_and_are_equal_it_should_succeed()
    {
        // Arrange
        var subject =
            new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

        var other =
            new { Time = new DateTime(2013, 12, 9, 15, 58, 0) };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void
        When_two_properties_are_datetime_and_subject_is_nullable_and_subject_is_null_it_should_throw_and_state_the_difference()
    {
        // Arrange
        var subject =
            new { Time = (DateTime?)null };

        var other =
            new { Time = new DateTime(2013, 12, 9, 15, 58, 0) };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(other);

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected*Time*to be <2013-12-09 15:58:00>, but found <null>.*");
    }

#if NET6_0_OR_GREATER
    [Fact]
    public void Clarifies_that_a_date_time_is_compared_with_a_date_only()
    {
        // Arrange
        var subject = new
        {
            SomeDate = new DateOnly(2020, 1, 2)
        };

        // Act
        var act = () => subject.Should().BeEquivalentTo(new
        {
            SomeDate = 2.January(2020)
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*SomeDate*DateTime*found*DateOnly*");
    }

    [Fact]
    public void Clarifies_that_a_date_only_is_compared_with_a_date_time()
    {
        // Arrange
        var subject = new
        {
            SomeDate = 2.January(2020)
        };

        // Act
        var act = () => subject.Should().BeEquivalentTo(new
        {
            SomeDate = new DateOnly(2020, 1, 2)
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*SomeDate*DateOnly*found*DateTime*");
    }

    [Fact]
    public void Clarifies_that_a_time_only_is_compared_with_a_date_time()
    {
        // Arrange
        var subject = new
        {
            SomeDate = new DateTime(2020, 1, 2).At(9, 30)
        };

        // Act
        var act = () => subject.Should().BeEquivalentTo(new
        {
            SomeDate = new TimeOnly(9, 30)
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*SomeDate*TimeOnly*found*DateTime*");
    }

    [Fact]
    public void Clarifies_that_a_date_time_is_compared_with_a_time_only()
    {
        // Arrange
        var subject = new
        {
            SomeDate = new TimeOnly(9, 30)
        };

        // Act
        var act = () => subject.Should().BeEquivalentTo(new
        {
            SomeDate = new DateTime(2020, 1, 2).At(9, 30)
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*SomeDate*DateTime*found*TimeOnly*");
    }

    [Fact]
    public void Clarifies_that_a_time_only_is_compared_with_a_time_span()
    {
        // Arrange
        var subject = new
        {
            SomeTime = new TimeOnly(9, 30)
        };

        // Act
        var act = () => subject.Should().BeEquivalentTo(new
        {
            SomeTime = new TimeSpan(9, 30, 0)
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*SomeTime*TimeSpan*found*TimeOnly*");
    }

    [Fact]
    public void Clarifies_that_a_time_span_is_compared_with_a_time_only()
    {
        // Arrange
        var subject = new
        {
            SomeTime = new TimeSpan(9, 30, 0)
        };

        // Act
        var act = () => subject.Should().BeEquivalentTo(new
        {
            SomeTime = new TimeOnly(9, 30)
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*SomeTime*TimeOnly*found*TimeSpan*");
    }

#endif

    [Fact]
    public void Clarifies_that_a_date_time_is_compared_to_a_date_time_offset()
    {
        // Arrange
        var subject = new
        {
            SomeDate = 2.January(2020)
        };

        // Act
        var act = () => subject.Should().BeEquivalentTo(new
        {
            SomeDate = new DateTimeOffset(2.January(2020), TimeSpan.Zero)
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*SomeDate*DateTimeOffset*found*DateTime*");
    }

    [Fact]
    public void Clarifies_that_a_date_time_offset_is_compared_to_a_date_time()
    {
        // Arrange
        var subject = new
        {
            SomeDate = new DateTimeOffset(2.January(2020), TimeSpan.Zero)
        };

        // Act
        var act = () => subject.Should().BeEquivalentTo(new
        {
            SomeDate = 2.January(2020)
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*SomeDate*DateTime*found*DateTimeOffset*");
    }
}
