using System;
using System.Linq;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            var value = new DateTimeOffset(31.December(2016), 1.Hours());

            // Act
            Action action = () => value.Should().BeOneOf(value + 1.Days(), value + 4.Hours());

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value to be one of {<2017-01-01 +1h>, <2016-12-31 04:00:00 +1h>}, but it was <2016-12-31 +1h>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_param_values_follow_up_assertions_works()
        {
            // Arrange
            var value = new DateTimeOffset(31.December(2016), 1.Hours());

            // Act / Assert
            value.Should().BeOneOf(value, value + 1.Hours())
                .And.Be(31.December(2016).WithOffset(1.Hours()));
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_nullable_params_values_follow_up_assertions_works()
        {
            // Arrange
            var value = new DateTimeOffset(31.December(2016), 1.Hours());

            // Act / Assert
            value.Should().BeOneOf(null, value, value + 1.Hours())
                .And.Be(31.December(2016).WithOffset(1.Hours()));
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_enumerable_values_follow_up_assertions_works()
        {
            // Arrange
            var value = new DateTimeOffset(31.December(2016), 1.Hours());
            var expected = new[] { value, value + 1.Hours() }.AsEnumerable();

            // Act / Assert
            value.Should().BeOneOf(expected)
                .And.Be(31.December(2016).WithOffset(1.Hours()));
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_nullable_enumerable_follow_up_assertions_works()
        {
            // Arrange
            var value = new DateTimeOffset(31.December(2016), 1.Hours());
            var expected = new DateTimeOffset?[] { null, value, value + 1.Hours() }.AsEnumerable();

            // Act / Assert
            value.Should().BeOneOf(expected)
                .And.Be(31.December(2016).WithOffset(1.Hours()));
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            DateTimeOffset value = 31.December(2016).WithOffset(1.Hours());

            // Act
            Action action = () => value.Should().BeOneOf(new[] { value + 1.Days(), value + 2.Days() }, "because it's true");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value to be one of {<2017-01-01 +1h>, <2017-01-02 +1h>} because it's true, but it was <2016-12-31 +1h>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            DateTimeOffset value = new(2016, 12, 30, 23, 58, 57, TimeSpan.FromHours(4));

            // Act
            Action action = () => value.Should().BeOneOf(new DateTimeOffset(2216, 1, 30, 0, 5, 7, TimeSpan.FromHours(2)),
                new DateTimeOffset(2016, 12, 30, 23, 58, 57, TimeSpan.FromHours(4)));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_a_null_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateTimeOffset? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new DateTimeOffset(2216, 1, 30, 0, 5, 7, TimeSpan.FromHours(1)),
                new DateTimeOffset(2016, 2, 10, 2, 45, 7, TimeSpan.FromHours(2)));

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected value to be one of {<2216-01-30 00:05:07 +1h>, <2016-02-10 02:45:07 +2h>}, but it was <null>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed_when_datetimeoffset_is_null()
        {
            // Arrange
            DateTimeOffset? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new DateTimeOffset(2216, 1, 30, 0, 5, 7, TimeSpan.Zero), null);

            // Assert
            action.Should().NotThrow();
        }
    }
}
