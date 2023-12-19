#if NET6_0_OR_GREATER
using System;
using Xunit;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class TimeOnlyAssertionSpecs
{
    [Fact]
    public void Should_succeed_when_asserting_nullable_timeonly_value_with_value_to_have_a_value()
    {
        // Arrange
        TimeOnly? timeOnly = new(15, 06, 04);

        // Act/Assert
        timeOnly.Should().HaveValue();
    }

    [Fact]
    public void Should_succeed_when_asserting_nullable_timeonly_value_with_value_to_not_be_null()
    {
        // Arrange
        TimeOnly? timeOnly = new(15, 06, 04);

        // Act/Assert
        timeOnly.Should().NotBeNull();
    }

    [Fact]
    public void Should_succeed_when_asserting_nullable_timeonly_value_with_null_to_be_null()
    {
        // Arrange
        TimeOnly? timeOnly = null;

        // Act/Assert
        timeOnly.Should().BeNull();
    }

    [Fact]
    public void Should_support_chaining_constraints_with_and()
    {
        // Arrange
        TimeOnly earlierTimeOnly = new(15, 06, 03);
        TimeOnly? nullableTimeOnly = new(15, 06, 04);

        // Act/Assert
        nullableTimeOnly.Should()
            .HaveValue()
            .And
            .BeAfter(earlierTimeOnly);
    }

    [Fact]
    public void Should_throw_a_helpful_error_when_accidentally_using_equals()
    {
        // Arrange
        TimeOnly someTimeOnly = new(21, 1);

        // Act
        var act = () => someTimeOnly.Should().Equals(null);

        // Assert
        act.Should().Throw<NotSupportedException>()
            .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
    }
}

#endif
