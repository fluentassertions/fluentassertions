using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class BeNull
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_datetime_value_without_a_value_to_be_null()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () =>
                nullableDateTime.Should().BeNull();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetime_value_with_a_value_to_be_null()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

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
        public void Should_succeed_when_asserting_nullable_datetime_value_with_a_value_to_not_be_null()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act
            Action action = () => nullableDateTime.Should().NotBeNull();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetime_value_without_a_value_to_not_be_null()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () => nullableDateTime.Should().NotBeNull();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
