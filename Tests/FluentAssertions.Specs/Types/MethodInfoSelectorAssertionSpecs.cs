using System;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Types;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

public class MethodInfoSelectorAssertionSpecs
{
    public class BeVirtual
    {
        [Fact]
        public void When_asserting_methods_are_virtual_and_they_are_it_should_succeed()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsVirtual));

            // Act
            Action act = () =>
                methodSelector.Should().BeVirtual();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_methods_are_virtual_but_non_virtual_methods_are_found_it_should_throw()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonVirtualPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().BeVirtual();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_methods_are_virtual_but_non_virtual_methods_are_found_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonVirtualPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().BeVirtual("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods" +
                    " to be virtual because we want to test the error message," +
                    " but the following methods are not virtual:*" +
                    "Void FluentAssertionsAsync*ClassWithNonVirtualPublicMethods.PublicDoNothing*" +
                    "Void FluentAssertionsAsync*ClassWithNonVirtualPublicMethods.InternalDoNothing*" +
                    "Void FluentAssertionsAsync*ClassWithNonVirtualPublicMethods.ProtectedDoNothing");
        }
    }

    public class NotBeVirtual
    {
        [Fact]
        public void When_asserting_methods_are_not_virtual_and_they_are_not_it_should_succeed()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonVirtualPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().NotBeVirtual();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_methods_are_not_virtual_but_virtual_methods_are_found_it_should_throw()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsVirtual));

            // Act
            Action act = () =>
                methodSelector.Should().NotBeVirtual();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_methods_are_not_virtual_but_virtual_methods_are_found_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsVirtual));

            // Act
            Action act = () =>
                methodSelector.Should().NotBeVirtual("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods" +
                    " not to be virtual because we want to test the error message," +
                    " but the following methods are virtual" +
                    "*ClassWithAllMethodsVirtual.PublicVirtualDoNothing" +
                    "*ClassWithAllMethodsVirtual.InternalVirtualDoNothing" +
                    "*ClassWithAllMethodsVirtual.ProtectedVirtualDoNothing*");
        }
    }

    public class BeDecoratedWith
    {
        [Fact]
        public void When_injecting_a_null_predicate_into_BeDecoratedWith_it_should_throw()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                methodSelector.Should().BeDecoratedWith<DummyMethodAttribute>(isMatchingAttributePredicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }

        [Fact]
        public void When_asserting_methods_are_decorated_with_attribute_and_they_are_it_should_succeed()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                methodSelector.Should().BeDecoratedWith<DummyMethodAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_methods_are_decorated_with_attribute_but_they_are_not_it_should_throw()
        {
            // Arrange
            MethodInfoSelector methodSelector =
                new MethodInfoSelector(typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute))
                    .ThatArePublicOrInternal;

            // Act
            Action act = () =>
                methodSelector.Should().BeDecoratedWith<DummyMethodAttribute>();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_methods_are_decorated_with_attribute_but_they_are_not_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                methodSelector.Should().BeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods to be decorated with" +
                    " FluentAssertionsAsync*DummyMethodAttribute because we want to test the error message," +
                    " but the following methods are not:*" +
                    "Void FluentAssertionsAsync*ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing*" +
                    "Void FluentAssertionsAsync*ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.ProtectedDoNothing*" +
                    "Void FluentAssertionsAsync*ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PrivateDoNothing");
        }
    }

    public class NotBeDecoratedWith
    {
        [Fact]
        public void When_injecting_a_null_predicate_into_NotBeDecoratedWith_it_should_throw()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                methodSelector.Should().NotBeDecoratedWith<DummyMethodAttribute>(isMatchingAttributePredicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }

        [Fact]
        public void When_asserting_methods_are_not_decorated_with_attribute_and_they_are_not_it_should_succeed()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                methodSelector.Should().NotBeDecoratedWith<DummyMethodAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_methods_are_not_decorated_with_attribute_but_they_are_it_should_throw()
        {
            // Arrange
            MethodInfoSelector methodSelector =
                new MethodInfoSelector(typeof(ClassWithAllMethodsDecoratedWithDummyAttribute))
                    .ThatArePublicOrInternal;

            // Act
            Action act = () =>
                methodSelector.Should().NotBeDecoratedWith<DummyMethodAttribute>();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_methods_are_not_decorated_with_attribute_but_they_are_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsDecoratedWithDummyAttribute));

            // Act
            Action act = () => methodSelector.Should()
                    .NotBeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected all selected methods to not be decorated*DummyMethodAttribute*because we want to test the error message" +
                    "*ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothing*" +
                    "*ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothingWithSameAttributeTwice*" +
                    "*ClassWithAllMethodsDecoratedWithDummyAttribute.ProtectedDoNothing*" +
                    "*ClassWithAllMethodsDecoratedWithDummyAttribute.PrivateDoNothing");
        }
    }

    public class Be
    {
        [Fact]
        public void When_all_methods_have_specified_accessor_it_should_succeed()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().Be(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_not_all_methods_have_specified_accessor_it_should_throw()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().Be(CSharpAccessModifier.Public);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods to be Public" +
                    ", but the following methods are not:*" +
                    "Void FluentAssertionsAsync*ClassWithNonPublicMethods.PublicDoNothing*" +
                    "Void FluentAssertionsAsync*ClassWithNonPublicMethods.DoNothingWithParameter*" +
                    "Void FluentAssertionsAsync*ClassWithNonPublicMethods.DoNothingWithAnotherParameter");
        }

        [Fact]
        public void When_not_all_methods_have_specified_accessor_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().Be(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods to be Public" +
                    " because we want to test the error message" +
                    ", but the following methods are not:*" +
                    "Void FluentAssertionsAsync*ClassWithNonPublicMethods.PublicDoNothing*" +
                    "Void FluentAssertionsAsync*ClassWithNonPublicMethods.DoNothingWithParameter*" +
                    "Void FluentAssertionsAsync*ClassWithNonPublicMethods.DoNothingWithAnotherParameter");
        }
    }

    public class NotBe
    {
        [Fact]
        public void When_all_methods_does_not_have_specified_accessor_it_should_succeed()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().NotBe(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_any_method_have_specified_accessor_it_should_throw()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().NotBe(CSharpAccessModifier.Public);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods to not be Public" +
                    ", but the following methods are:*" +
                    "Void FluentAssertionsAsync*ClassWithPublicMethods.PublicDoNothing*");
        }

        [Fact]
        public void When_any_method_have_specified_accessor_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithPublicMethods));

            // Act
            Action act = () =>
                methodSelector.Should().NotBe(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods to not be Public" +
                    " because we want to test the error message" +
                    ", but the following methods are:*" +
                    "Void FluentAssertionsAsync*ClassWithPublicMethods.PublicDoNothing*");
        }
    }

    public class BeAsync
    {
        [Fact]
        public void When_asserting_methods_are_async_and_they_are_then_it_succeeds()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsAsync));

            // Act
            Action act = () => methodSelector.Should().BeAsync();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_methods_are_async_but_non_async_methods_are_found_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonAsyncMethods));

            // Act
            Action act = () => methodSelector.Should().BeAsync("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods" +
                    " to be async because we want to test the error message," +
                    " but the following methods are not:" + Environment.NewLine +
                    "Task FluentAssertionsAsync.Specs.Types.ClassWithNonAsyncMethods.PublicDoNothing" + Environment.NewLine +
                    "Task FluentAssertionsAsync.Specs.Types.ClassWithNonAsyncMethods.InternalDoNothing" + Environment.NewLine +
                    "Task FluentAssertionsAsync.Specs.Types.ClassWithNonAsyncMethods.ProtectedDoNothing");
        }
    }

    public class NotBeAsync
    {
        [Fact]
        public void When_asserting_methods_are_not_async_and_they_are_not_then_it_succeeds()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithNonAsyncMethods));

            // Act
            Action act = () => methodSelector.Should().NotBeAsync();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_methods_are_not_async_but_async_methods_are_found_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsAsync));

            // Act
            Action act = () => methodSelector.Should().NotBeAsync("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods" +
                    " not to be async because we want to test the error message," +
                    " but the following methods are:" + Environment.NewLine +
                    "Task FluentAssertionsAsync.Specs.Types.ClassWithAllMethodsAsync.PublicAsyncDoNothing" + Environment.NewLine +
                    "Task FluentAssertionsAsync.Specs.Types.ClassWithAllMethodsAsync.InternalAsyncDoNothing" + Environment.NewLine +
                    "Task FluentAssertionsAsync.Specs.Types.ClassWithAllMethodsAsync.ProtectedAsyncDoNothing");
        }
    }
}
