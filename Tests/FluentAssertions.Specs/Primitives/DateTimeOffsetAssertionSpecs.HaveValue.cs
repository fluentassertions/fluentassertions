using System;
using FluentAssertionsAsync.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class HaveValue
    {
        [Fact]
        public void When_nullable_datetimeoffset_value_with_a_value_to_have_a_value_it_should_succeed()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = new DateTime(2016, 06, 04).ToDateTimeOffset();

            // Act
            Action action = () => nullableDateTime.Should().HaveValue();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetimeoffset_value_without_a_value_to_have_a_value()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = null;

            // Act
            Action action = () => nullableDateTime.Should().HaveValue();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }

    public class NotHaveValue
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_datetimeoffset_value_without_a_value_to_not_have_a_value()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = null;

            // Act
            Action action = () =>
                nullableDateTime.Should().NotHaveValue();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetimeoffset_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = new DateTime(2016, 06, 04).ToDateTimeOffset();

            // Act
            Action action = () =>
                nullableDateTime.Should().NotHaveValue();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
