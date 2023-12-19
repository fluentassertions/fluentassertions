using System;
using FluentAssertionsAsync.Specs.Primitives;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]BeDerivedFrom specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class BeDerivedFrom
    {
        [Fact]
        public void When_asserting_a_type_is_derived_from_its_base_class_it_succeeds()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(DummyBaseClass));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_is_derived_from_an_unrelated_class_it_fails()
        {
            // Arrange
            var type = typeof(DummyBaseClass);

            // Act
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(ClassWithMembers), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyBaseClass to be derived from *.ClassWithMembers *failure message*, but it is not.");
        }

        [Fact]
        public void When_asserting_a_type_is_derived_from_an_interface_it_fails()
        {
            // Arrange
            var type = typeof(ClassThatImplementsInterface);

            // Act
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(IDummyInterface), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassThatImplementsInterface to be derived from *.IDummyInterface *failure message*" +
                    ", but *.IDummyInterface is an interface.");
        }

        [Fact]
        public void When_asserting_a_type_is_derived_from_an_open_generic_it_succeeds()
        {
            // Arrange
            var type = typeof(DummyBaseType<ClassWithGenericBaseType>);

            // Act
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(DummyBaseType<>));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_is_derived_from_an_open_generic_base_class_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithGenericBaseType);

            // Act
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(DummyBaseType<>));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_is_derived_from_an_unrelated_open_generic_class_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(DummyBaseType<>), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithMembers to be derived from *.DummyBaseType`* *failure message*, but it is not.");
        }

        [Fact]
        public void When_asserting_a_type_to_be_derived_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(DummyBaseType<>);

            // Act
            Action act =
                () => type.Should().BeDerivedFrom(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("baseType");
        }
    }

    public class BeDerivedFromOfT
    {
        [Fact]
        public void When_asserting_a_type_is_DerivedFromOfT_its_base_class_it_succeeds()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().BeDerivedFrom<DummyBaseClass>();

            // Assert
            act.Should().NotThrow();
        }
    }

    public class NotBeDerivedFrom
    {
        [Fact]
        public void When_asserting_a_type_is_not_derived_from_an_unrelated_class_it_succeeds()
        {
            // Arrange
            var type = typeof(DummyBaseClass);

            // Act
            Action act = () =>
                type.Should().NotBeDerivedFrom(typeof(ClassWithMembers));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_is_not_derived_from_its_base_class_it_fails()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().NotBeDerivedFrom(typeof(DummyBaseClass), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyImplementingClass not to be derived from *.DummyBaseClass *failure message*" +
                    ", but it is.");
        }

        [Fact]
        public void When_asserting_a_type_is_not_derived_from_an_interface_it_fails()
        {
            // Arrange
            var type = typeof(ClassThatImplementsInterface);

            // Act
            Action act = () =>
                type.Should().NotBeDerivedFrom(typeof(IDummyInterface), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassThatImplementsInterface not to be derived from *.IDummyInterface *failure message*" +
                    ", but *.IDummyInterface is an interface.");
        }

        [Fact]
        public void When_asserting_a_type_is_not_derived_from_an_unrelated_open_generic_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotBeDerivedFrom(typeof(DummyBaseType<>));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_open_generic_type_is_not_derived_from_itself_it_succeeds()
        {
            // Arrange
            var type = typeof(DummyBaseType<>);

            // Act
            Action act = () =>
                type.Should().NotBeDerivedFrom(typeof(DummyBaseType<>));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_is_not_derived_from_its_open_generic_it_fails()
        {
            // Arrange
            var type = typeof(DummyBaseType<ClassWithGenericBaseType>);

            // Act
            Action act = () =>
                type.Should().NotBeDerivedFrom(typeof(DummyBaseType<>), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyBaseType`1[*.ClassWithGenericBaseType] not to be derived from *.DummyBaseType`1[T] " +
                    "*failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_type_not_to_be_derived_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(DummyBaseType<>);

            // Act
            Action act =
                () => type.Should().NotBeDerivedFrom(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("baseType");
        }
    }

    public class NotBeDerivedFromOfT
    {
        [Fact]
        public void When_asserting_a_type_is_not_DerivedFromOfT_an_unrelated_class_it_succeeds()
        {
            // Arrange
            var type = typeof(DummyBaseClass);

            // Act
            Action act = () =>
                type.Should().NotBeDerivedFrom<ClassWithMembers>();

            // Assert
            act.Should().NotThrow();
        }
    }
}
