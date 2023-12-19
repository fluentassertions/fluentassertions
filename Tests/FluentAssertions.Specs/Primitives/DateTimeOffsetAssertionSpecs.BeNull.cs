using System;
using FluentAssertionsAsync.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeOffsetAssertionSpecs
{
    public class BeNull
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_datetimeoffset_value_without_a_value_to_be_null()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = null;

            // Act
            Action action = () =>
                nullableDateTime.Should().BeNull();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetimeoffset_value_with_a_value_to_be_null()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = new DateTime(2016, 06, 04).ToDateTimeOffset();

            // Act
            Action action = () =>
                nullableDateTime.Should().BeNull();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }

    public class NotBeNull
    {
        [Fact]
        public void When_nullable_datetimeoffset_value_with_a_value_not_be_null_it_should_succeed()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = new DateTime(2016, 06, 04).ToDateTimeOffset();

            // Act
            Action action = () => nullableDateTime.Should().NotBeNull();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetimeoffset_value_without_a_value_to_not_be_null()
        {
            // Arrange
            DateTimeOffset? nullableDateTime = null;

            // Act
            Action action = () => nullableDateTime.Should().NotBeNull();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
