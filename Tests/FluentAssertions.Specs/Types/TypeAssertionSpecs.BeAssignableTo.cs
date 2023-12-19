using System;
using FluentAssertionsAsync.Specs.Primitives;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]BeAssignableTo specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class BeAssignableTo
    {
        [Fact]
        public void When_its_own_type_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyImplementingClass).Should().BeAssignableTo<DummyImplementingClass>();
        }

        [Fact]
        public void When_its_base_type_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyImplementingClass).Should().BeAssignableTo<DummyBaseClass>();
        }

        [Fact]
        public void When_implemented_interface_type_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyImplementingClass).Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void When_an_unrelated_type_it_fails()
        {
            // Arrange
            Type someType = typeof(DummyImplementingClass);
            Action act = () => someType.Should().BeAssignableTo<DateTime>("we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected someType *.DummyImplementingClass to be assignable to *.DateTime *failure message*" +
                    ", but it is not.");
        }

        [Fact]
        public void When_its_own_type_instance_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyImplementingClass).Should().BeAssignableTo(typeof(DummyImplementingClass));
        }

        [Fact]
        public void When_its_base_type_instance_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyImplementingClass).Should().BeAssignableTo(typeof(DummyBaseClass));
        }

        [Fact]
        public void When_an_implemented_interface_type_instance_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyImplementingClass).Should().BeAssignableTo(typeof(IDisposable));
        }

        [Fact]
        public void When_an_unrelated_type_instance_it_fails()
        {
            // Arrange
            Type someType = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                someType.Should().BeAssignableTo(typeof(DateTime), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*.DummyImplementingClass to be assignable to *.DateTime *failure message*");
        }

        [Fact]
        public void When_constructed_of_open_generic_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(IDummyInterface<IDummyInterface>).Should().BeAssignableTo(typeof(IDummyInterface<>));
        }

        [Fact]
        public void When_implementation_of_open_generic_interface_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(ClassWithGenericBaseType).Should().BeAssignableTo(typeof(IDummyInterface<>));
        }

        [Fact]
        public void When_derived_of_open_generic_class_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(ClassWithGenericBaseType).Should().BeAssignableTo(typeof(DummyBaseType<>));
        }

        [Fact]
        public void When_unrelated_to_open_generic_interface_it_fails()
        {
            // Arrange
            Type someType = typeof(IDummyInterface);

            // Act
            Action act = () =>
                someType.Should().BeAssignableTo(typeof(IDummyInterface<>), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected someType *.IDummyInterface to be assignable to *.IDummyInterface`1[T] *failure message*" +
                    ", but it is not.");
        }

        [Fact]
        public void When_unrelated_to_open_generic_type_it_fails()
        {
            // Arrange
            Type someType = typeof(ClassWithAttribute);

            Action act = () =>
                someType.Should().BeAssignableTo(typeof(DummyBaseType<>), "we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*.ClassWithAttribute to be assignable to *.DummyBaseType* *failure message*");
        }

        [Fact]
        public void When_asserting_an_open_generic_class_is_assignable_to_itself_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyBaseType<>).Should().BeAssignableTo(typeof(DummyBaseType<>));
        }

        [Fact]
        public void When_asserting_a_type_to_be_assignable_to_null_it_should_throw()
        {
            // Arrange
            var type = typeof(DummyBaseType<>);

            // Act
            Action act = () =>
                type.Should().BeAssignableTo(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("type");
        }
    }

    public class NotBeAssignableTo
    {
        [Fact]
        public void When_its_own_type_and_asserting_not_assignable_it_fails()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().NotBeAssignableTo<DummyImplementingClass>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyImplementingClass to not be assignable to *.DummyImplementingClass *failure message*");
        }

        [Fact]
        public void When_its_base_type_and_asserting_not_assignable_it_fails()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().NotBeAssignableTo<DummyBaseClass>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyImplementingClass to not be assignable to *.DummyBaseClass *failure message*" +
                    ", but it is.");
        }

        [Fact]
        public void When_implemented_interface_type_and_asserting_not_assignable_it_fails()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().NotBeAssignableTo<IDisposable>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyImplementingClass to not be assignable to *.IDisposable *failure message*, but it is.");
        }

        [Fact]
        public void When_an_unrelated_type_and_asserting_not_assignable_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyImplementingClass).Should().NotBeAssignableTo<DateTime>();
        }

        [Fact]
        public void When_its_own_type_instance_and_asserting_not_assignable_it_fails()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().NotBeAssignableTo(typeof(DummyImplementingClass), "we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyImplementingClass to not be assignable to *.DummyImplementingClass *failure message*" +
                    ", but it is.");
        }

        [Fact]
        public void When_its_base_type_instance_and_asserting_not_assignable_it_fails()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().NotBeAssignableTo(typeof(DummyBaseClass), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyImplementingClass to not be assignable to *.DummyBaseClass *failure message*" +
                    ", but it is.");
        }

        [Fact]
        public void When_an_implemented_interface_type_instance_and_asserting_not_assignable_it_fails()
        {
            // Arrange
            var type = typeof(DummyImplementingClass);

            // Act
            Action act = () =>
                type.Should().NotBeAssignableTo(typeof(IDisposable), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.DummyImplementingClass to not be assignable to *.IDisposable *failure message*, but it is.");
        }

        [Fact]
        public void When_an_unrelated_type_instance_and_asserting_not_assignable_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(DummyImplementingClass).Should().NotBeAssignableTo(typeof(DateTime));
        }

        [Fact]
        public void When_unrelated_to_open_generic_interface_and_asserting_not_assignable_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(ClassWithAttribute).Should().NotBeAssignableTo(typeof(IDummyInterface<>));
        }

        [Fact]
        public void When_unrelated_to_open_generic_class_and_asserting_not_assignable_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(ClassWithAttribute).Should().NotBeAssignableTo(typeof(DummyBaseType<>));
        }

        [Fact]
        public void When_implementation_of_open_generic_interface_and_asserting_not_assignable_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithGenericBaseType);

            // Act
            Action act = () =>
                type.Should().NotBeAssignableTo(typeof(IDummyInterface<>), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithGenericBaseType to not be assignable to *.IDummyInterface`1[T] *failure message*" +
                    ", but it is.");
        }

        [Fact]
        public void When_derived_from_open_generic_class_and_asserting_not_assignable_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithGenericBaseType);

            // Act
            Action act = () =>
                type.Should().NotBeAssignableTo(typeof(IDummyInterface<>), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithGenericBaseType to not be assignable to *.IDummyInterface`1[T] *failure message*" +
                    ", but it is.");
        }

        [Fact]
        public void When_asserting_a_type_not_to_be_assignable_to_null_it_should_throw()
        {
            // Arrange
            var type = typeof(DummyBaseType<>);

            // Act
            Action act =
                () => type.Should().NotBeAssignableTo(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("type");
        }
    }
}
