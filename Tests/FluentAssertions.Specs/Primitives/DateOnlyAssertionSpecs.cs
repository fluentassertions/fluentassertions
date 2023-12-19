#if NET6_0_OR_GREATER

using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateOnlyAssertionSpecs
{
    [Fact]
    public void Should_succeed_when_asserting_nullable_dateonly_value_with_value_to_have_a_value()
    {
        // Arrange
        DateOnly? dateOnly = new(2016, 06, 04);

        // Act/Assert
        dateOnly.Should().HaveValue();
    }

    [Fact]
    public void Should_succeed_when_asserting_nullable_dateonly_value_with_value_to_not_be_null()
    {
        // Arrange
        DateOnly? dateOnly = new(2016, 06, 04);

        // Act/Assert
        dateOnly.Should().NotBeNull();
    }

    [Fact]
    public void Should_succeed_when_asserting_nullable_dateonly_value_with_null_to_be_null()
    {
        // Arrange
        DateOnly? dateOnly = null;

        // Act/Assert
        dateOnly.Should().BeNull();
    }

    [Fact]
    public void Should_support_chaining_constraints_with_and()
    {
        // Arrange
        DateOnly earlierDateOnly = new(2016, 06, 03);
        DateOnly? nullableDateOnly = new(2016, 06, 04);

        // Act/Assert
        nullableDateOnly.Should()
            .HaveValue()
            .And
            .BeAfter(earlierDateOnly);
    }

    [Fact]
    public void Should_throw_a_helpful_error_when_accidentally_using_equals()
    {
        // Arrange
        DateOnly someDateOnly = new(2022, 9, 25);

        // Act
        Action action = () => someDateOnly.Should().Equals(someDateOnly);

        // Assert
        action.Should().Throw<NotSupportedException>()
            .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
    }
}

#endif
