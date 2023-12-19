#if NET6_0_OR_GREATER
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class TimeOnlyAssertionSpecs
{
    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            TimeOnly value = new(15, 12, 20);

            // Act
            Action action = () => value.Should().BeOneOf(value.AddHours(1), value.AddMinutes(-1));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<16:12:20.000>, <15:11:20.000>}, but found <15:12:20.000>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            TimeOnly value = new(15, 12, 30);

            // Act/Assert
            value.Should().BeOneOf(new TimeOnly(4, 1, 30), new TimeOnly(15, 12, 30));
        }

        [Fact]
        public void When_a_null_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            TimeOnly? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new TimeOnly(15, 1, 30), new TimeOnly(5, 4, 10, 123));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<15:01:30.000>, <05:04:10.123>}, but found <null>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed_when_timeonly_is_null()
        {
            // Arrange
            TimeOnly? value = null;

            // Act/Assert
            value.Should().BeOneOf(new TimeOnly(15, 1, 30), null);
        }
    }
}

#endif
