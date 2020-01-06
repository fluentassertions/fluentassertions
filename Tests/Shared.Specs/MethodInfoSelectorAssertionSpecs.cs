using System;
using FluentAssertions.Types;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class MethodInfoSelectorAssertionSpecs
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
        public void When_asserting_methods_are_virtual_but_non_virtual_methods_are_found_it_should_throw_with_descriptive_message()
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
                             "Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.PublicDoNothing*" +
                             "Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.InternalDoNothing*" +
                             "Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.ProtectedDoNothing");
        }

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
        public void When_asserting_methods_are_not_virtual_but_virtual_methods_are_found_it_should_throw_with_descriptive_message()
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
                .Which.ParamName.Should().Be("isMatchingAttributePredicate");
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
        public void When_asserting_methods_are_decorated_with_attribute_but_they_are_not_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                methodSelector.Should().BeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods to be decorated with" +
                             " FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                             " but the following methods are not:*" +
                             "Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing*" +
                             "Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.ProtectedDoNothing*" +
                             "Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PrivateDoNothing");
        }

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
                .Which.ParamName.Should().Be("isMatchingAttributePredicate");
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
        public void When_asserting_methods_are_not_decorated_with_attribute_but_they_are_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var methodSelector = new MethodInfoSelector(typeof(ClassWithAllMethodsDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                methodSelector.Should().NotBeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected methods to not be decorated*DummyMethodAttribute*because we want to test the error message" +
                             "*ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothing*" +
                             "*ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothingWithSameAttributeTwice*" +
                             "*ClassWithAllMethodsDecoratedWithDummyAttribute.ProtectedDoNothing*" +
                             "*ClassWithAllMethodsDecoratedWithDummyAttribute.PrivateDoNothing");
        }
    }
}
