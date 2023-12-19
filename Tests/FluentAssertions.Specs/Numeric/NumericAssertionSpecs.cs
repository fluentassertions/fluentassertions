using System;
using Xunit;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    [Fact]
    public void When_chaining_constraints_with_and_should_not_throw()
    {
        // Arrange
        int value = 2;
        int greaterValue = 3;
        int smallerValue = 1;

        // Act
        Action action = () => value.Should()
            .BePositive()
            .And
            .BeGreaterThan(smallerValue)
            .And
            .BeLessThan(greaterValue);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Should_throw_a_helpful_error_when_accidentally_using_equals()
    {
        // Arrange
        int value = 1;

        // Act
        Action action = () => value.Should().Equals(1);

        // Assert
        action.Should().Throw<NotSupportedException>()
            .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
    }
}
