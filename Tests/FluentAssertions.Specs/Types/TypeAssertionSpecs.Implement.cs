using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]Implement specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class Implement
    {
        [Fact]
        public void When_asserting_a_type_implements_an_interface_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassThatImplementsInterface);

            // Act
            Action act = () =>
                type.Should().Implement(typeof(IDummyInterface));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_implement_an_interface_which_it_does_then_it_fails()
        {
            // Arrange
            var type = typeof(ClassThatDoesNotImplementInterface);

            // Act
            Action act = () =>
                type.Should().Implement(typeof(IDummyInterface), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassThatDoesNotImplementInterface to implement interface *.IDummyInterface " +
                    "*failure message*, but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_implements_a_NonInterface_type_it_fails()
        {
            // Arrange
            var type = typeof(ClassThatDoesNotImplementInterface);

            // Act
            Action act = () =>
                type.Should().Implement(typeof(DateTime), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassThatDoesNotImplementInterface to implement interface *.DateTime *failure message*" +
                    ", but *.DateTime is not an interface.");
        }

        [Fact]
        public void When_asserting_a_type_to_implement_null_it_should_throw()
        {
            // Arrange
            var type = typeof(DummyBaseType<>);

            // Act
            Action act = () =>
                type.Should().Implement(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("interfaceType");
        }

        [Fact]
        public void An_interface_does_not_implement_itself()
        {
            // Arrange
            var type = typeof(IDummyInterface);

            // Act
            Action act = () =>
                type.Should().Implement(typeof(IDummyInterface));

            // Assert
            act.Should().Throw<XunitException>();
        }
    }

    public class ImplementOfT
    {
        [Fact]
        public void When_asserting_a_type_implementsOfT_an_interface_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassThatImplementsInterface);

            // Act
            Action act = () =>
                type.Should().Implement<IDummyInterface>();

            // Assert
            act.Should().NotThrow();
        }
    }

    public class NotImplement
    {
        [Fact]
        public void When_asserting_a_type_does_not_implement_an_interface_which_it_does_not_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassThatDoesNotImplementInterface);

            // Act
            Action act = () =>
                type.Should().NotImplement(typeof(IDummyInterface));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_implements_an_interface_which_it_does_not_then_it_fails()
        {
            // Arrange
            var type = typeof(ClassThatImplementsInterface);

            // Act
            Action act = () =>
                type.Should().NotImplement(typeof(IDummyInterface), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassThatImplementsInterface to not implement interface *.IDummyInterface " +
                    "*failure message*, but it does.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_implement_a_NonInterface_type_it_fails()
        {
            // Arrange
            var type = typeof(ClassThatDoesNotImplementInterface);

            // Act
            Action act = () =>
                type.Should().NotImplement(typeof(DateTime), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassThatDoesNotImplementInterface to not implement interface *.DateTime *failure message*" +
                    ", but *.DateTime is not an interface.");
        }

        [Fact]
        public void When_asserting_a_type_not_to_implement_null_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassThatDoesNotImplementInterface);

            // Act
            Action act = () =>
                type.Should().NotImplement(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("interfaceType");
        }

        [Fact]
        public void An_interface_does_not_implement_itself()
        {
            // Arrange
            var type = typeof(IDummyInterface);

            // Act / Assert
            type.Should().NotImplement(typeof(IDummyInterface));
        }
    }

    public class NotImplementOfT
    {
        [Fact]
        public void When_asserting_a_type_does_not_implementOfT_an_interface_which_it_does_not_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassThatDoesNotImplementInterface);

            // Act
            Action act = () =>
                type.Should().NotImplement<IDummyInterface>();

            // Assert
            act.Should().NotThrow();
        }
    }
}
