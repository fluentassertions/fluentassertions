using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]HaveExplicitMethod specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class HaveExplicitMethod
    {
        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_method_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "ExplicitMethod", new Type[0]);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_a_type_explicitly_implements_a_method_which_it_implements_implicitly_and_explicitly_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "ExplicitImplicitMethod", new Type[0]);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_method_which_it_implements_implicitly_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "ImplicitMethod", new Type[0]);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "*.IExplicitInterface.ImplicitMethod(), but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_method_which_it_does_not_implement_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "NonExistentMethod", new Type[0]);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "*.IExplicitInterface.NonExistentMethod(), but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_method_from_an_unimplemented_interface_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IDummyInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "NonExistentProperty", new Type[0]);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassExplicitlyImplementingInterface to implement interface " +
                    "*.IDummyInterface, but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_explicit_method_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod(
                    typeof(IExplicitInterface), "ExplicitMethod", new Type[0], "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to explicitly implement *.IExplicitInterface.ExplicitMethod() *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_method_with_a_null_interface_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod(null, "ExplicitMethod", new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("interfaceType");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_method_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod(typeof(IExplicitInterface), "ExplicitMethod", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_method_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod(typeof(IExplicitInterface), null, new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_method_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod(typeof(IExplicitInterface), string.Empty, new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }

        [Fact]
        public void Does_not_continue_assertion_on_explicit_interface_implementation_if_not_implemented_at_all()
        {
            var act = () =>
            {
                using var _ = new AssertionScope();
                typeof(ClassWithMembers).Should().HaveExplicitMethod(typeof(IExplicitInterface), "Foo", new Type[0]);
            };

            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *ClassWithMembers* to*implement *IExplicitInterface, but it does not.");
        }
    }

    public class HaveExplicitMethodOfT
    {
        [Fact]
        public void When_asserting_a_type_explicitly_implementsOfT_a_method_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod<IExplicitInterface>("ExplicitMethod", new Type[0]);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_null_have_explicit_methodOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod<IExplicitInterface>(
                    "ExplicitMethod", new Type[0], "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to explicitly implement *.IExplicitInterface.ExplicitMethod() *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_methodOfT_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod<IExplicitInterface>("ExplicitMethod", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_methodOfT_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod<IExplicitInterface>(null, new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_methodOfT_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitMethod<IExplicitInterface>(string.Empty, new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }

    public class NotHaveExplicitMethod
    {
        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "ExplicitMethod", new Type[0]);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "*.IExplicitInterface.ExplicitMethod(), but it does.");
        }

        [Fact]
        public void
            When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_implements_implicitly_and_explicitly_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "ExplicitImplicitMethod", new Type[0]);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "*.IExplicitInterface.ExplicitImplicitMethod(), but it does.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_implements_implicitly_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "ImplicitMethod", new Type[0]);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_does_not_implement_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "NonExistentMethod", new Type[0]);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_from_an_unimplemented_interface_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IDummyInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "NonExistentMethod", new Type[0]);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassExplicitlyImplementingInterface to implement interface *.IDummyInterface" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_not_have_explicit_method_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod(
                    typeof(IExplicitInterface), "ExplicitMethod", new Type[0], "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to not explicitly implement *.IExplicitInterface.ExplicitMethod() *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_method_inherited_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod(null, "ExplicitMethod", new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("interfaceType");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_method_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod(typeof(IExplicitInterface), "ExplicitMethod", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_method_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod(typeof(IExplicitInterface), null, new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_method_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod(typeof(IExplicitInterface), string.Empty, new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }

        [Fact]
        public void Does_not_continue_assertion_on_explicit_interface_implementation_if_implemented()
        {
            var act = () =>
            {
                using var _ = new AssertionScope();
                typeof(ClassExplicitlyImplementingInterface)
                    .Should().NotHaveExplicitMethod(typeof(IExplicitInterface), "ExplicitMethod", new Type[0]);
            };

            act.Should().Throw<XunitException>()
                .WithMessage("Expected *ClassExplicitlyImplementingInterface* to not*implement " +
                    "*IExplicitInterface.ExplicitMethod(), but it does.");
        }
    }

    public class NotHaveExplicitMethodOfT
    {
        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implementOfT_a_method_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod<IExplicitInterface>("ExplicitMethod", new Type[0]);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "*.IExplicitInterface.ExplicitMethod(), but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_explicit_methodOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod<IExplicitInterface>(
                    "ExplicitMethod", new Type[0], "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to not explicitly implement *.IExplicitInterface.ExplicitMethod() *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_methodOfT_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod<IExplicitInterface>("ExplicitMethod", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_methodOfT_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod<IExplicitInterface>(null, new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_methodOfT_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitMethod<IExplicitInterface>(string.Empty, new Type[0]);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }
}
