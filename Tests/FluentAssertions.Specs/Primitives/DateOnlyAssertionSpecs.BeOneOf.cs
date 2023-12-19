#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateOnlyAssertionSpecs
{
    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateOnly value = new(2016, 12, 20);

            // Act
            Action action = () => value.Should().BeOneOf(value.AddDays(1), value.AddMonths(-1));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<2016-12-21>, <2016-11-20>}, but found <2016-12-20>.");
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            DateOnly value = new(2016, 12, 20);

            // Act
            Action action = () =>
                value.Should().BeOneOf(new[] { value.AddDays(1), value.AddDays(2) }, "because it's true");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be one of {<2016-12-21>, <2016-12-22>} because it's true, but found <2016-12-20>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            DateOnly value = new(2016, 12, 30);

            // Act/Assert
            value.Should().BeOneOf(new DateOnly(2216, 1, 30), new DateOnly(2016, 12, 30));
        }

        [Fact]
        public void When_a_null_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateOnly? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new DateOnly(2216, 1, 30), new DateOnly(1116, 4, 10));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<2216-01-30>, <1116-04-10>}, but found <null>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed_when_dateonly_is_null()
        {
            // Arrange
            DateOnly? value = null;

            // Act/Assert
            value.Should().BeOneOf(new DateOnly(2216, 1, 30), null);
        }
    }
}

#endif
