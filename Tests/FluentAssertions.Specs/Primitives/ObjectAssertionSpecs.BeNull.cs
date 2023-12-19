using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeNull
    {
        [Fact]
        public void Should_succeed_when_asserting_null_object_to_be_null()
        {
            // Arrange
            object someObject = null;

            // Act / Assert
            someObject.Should().BeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_non_null_object_to_be_null()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_non_null_object_is_expected_to_be_null_it_should_fail()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .Where(e => e.Message.StartsWith(
                    "Expected someObject to be <null> because we want to test the failure message, but found System.Object",
                    StringComparison.Ordinal));
        }
    }

    public class BeNotNull
    {
        [Fact]
        public void Should_succeed_when_asserting_non_null_object_not_to_be_null()
        {
            // Arrange
            var someObject = new object();

            // Act / Assert
            someObject.Should().NotBeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_null_object_not_to_be_null()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () => someObject.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_null_object_not_to_be_null()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () => someObject.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someObject not to be <null> because we want to test the failure message.");
        }
    }
}
