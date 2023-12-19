using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class DateTimeAssertionSpecs
{
    public class HaveValue
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_datetime_value_with_a_value_to_have_a_value()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act
            Action action = () => nullableDateTime.Should().HaveValue();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetime_value_without_a_value_to_have_a_value()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () => nullableDateTime.Should().HaveValue();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }

    public class NotHaveValue
    {
        [Fact]
        public void Should_succeed_when_asserting_nullable_datetime_value_without_a_value_to_not_have_a_value()
        {
            // Arrange
            DateTime? nullableDateTime = null;

            // Act
            Action action = () =>
                nullableDateTime.Should().NotHaveValue();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_datetime_value_with_a_value_to_not_have_a_value()
        {
            // Arrange
            DateTime? nullableDateTime = new DateTime(2016, 06, 04);

            // Act
            Action action = () =>
                nullableDateTime.Should().NotHaveValue();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
