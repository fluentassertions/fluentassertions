using System;
using System.Globalization;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Primitives;
using FluentAssertions.Specs.Equivalency;
using FluentAssertions.Specs.Primitives;
using FluentAssertions.Types;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Types
{
    public class TypeAssertionSpecs
    {
        #region Be

        [Fact]
        public void When_type_is_equal_to_the_same_type_it_succeeds()
        {
            // Arrange
            Type type = typeof(ClassWithAttribute);
            Type sameType = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                type.Should().Be(sameType);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_equal_to_another_type_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithAttribute);
            Type differentType = typeof(ClassWithoutAttribute);

            // Act
            Action act = () =>
                type.Should().Be(differentType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be *.ClassWithoutAttribute *failure message*, but found *.ClassWithAttribute.");
        }

        [Fact]
        public void When_asserting_equality_of_two_null_types_it_succeeds()
        {
            // Arrange
            Type nullType = null;
            Type someType = null;

            // Act
            Action act = () => nullType.Should().Be(someType);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_equality_of_a_type_but_the_type_is_null_it_fails()
        {
            // Arrange
            Type nullType = null;
            Type someType = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                nullType.Should().Be(someType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be *.ClassWithAttribute *failure message*, but found <null>.");
        }

        [Fact]
        public void When_asserting_equality_of_a_type_with_null_it_fails()
        {
            // Arrange
            Type someType = typeof(ClassWithAttribute);
            Type nullType = null;

            // Act
            Action act = () =>
                someType.Should().Be(nullType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be <null> *failure message*, but found *.ClassWithAttribute.");
        }

        [Fact]
        public void When_type_is_equal_to_same_type_from_different_assembly_it_fails_with_assembly_qualified_name()
        {
            // Arrange
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

            Type typeFromThisAssembly = typeof(ObjectAssertions);

            Type typeFromOtherAssembly =
                typeof(TypeAssertions).Assembly.GetType("FluentAssertions.Primitives.ObjectAssertions");

#pragma warning restore 436

            // Act
            Action act = () =>
                typeFromThisAssembly.Should().Be(typeFromOtherAssembly, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be [*.ObjectAssertions, *] *failure message*, but found [*.ObjectAssertions, *].");
        }

        [Fact]
        public void When_type_is_equal_to_the_same_type_using_generics_it_succeeds()
        {
            // Arrange
            Type type = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                type.Should().Be<ClassWithAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_equal_to_another_type_using_generics_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                type.Should().Be<ClassWithoutAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be *.ClassWithoutAttribute *failure message*, but found *.ClassWithAttribute.");
        }

        #endregion

        #region NotBe

        [Fact]
        public void When_type_is_not_equal_to_the_another_type_it_succeeds()
        {
            // Arrange
            Type type = typeof(ClassWithAttribute);
            Type otherType = typeof(ClassWithoutAttribute);

            // Act
            Action act = () =>
                type.Should().NotBe(otherType);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_not_equal_to_the_same_type_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithAttribute);
            Type sameType = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                type.Should().NotBe(sameType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type not to be [*.ClassWithAttribute*] *failure message*, but it is.");
        }

        [Fact]
        public void When_type_is_not_equal_to_the_same_null_type_it_fails()
        {
            // Arrange
            Type type = null;
            Type sameType = null;

            // Act
            Action act = () =>
                type.Should().NotBe(sameType);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_type_is_not_equal_to_another_type_using_generics_it_succeeds()
        {
            // Arrange
            Type type = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                type.Should().NotBe<ClassWithoutAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_not_equal_to_the_same_type_using_generics_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                type.Should().NotBe<ClassWithAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type not to be [*.ClassWithAttribute*] *failure message*, but it is.");
        }

        #endregion

        #region BeAssignableTo

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

        #endregion

        #region NotBeAssignableTo

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

        #endregion

        #region BeDerivedFrom

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

        #endregion

        #region BeDerivedFromOfT

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

        #endregion

        #region NotBeDerivedFrom

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

        #endregion

        #region NotBeDerivedFromOfT

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

        #endregion

        #region BeDecoratedWith

        [Fact]
        public void When_type_is_decorated_with_expected_attribute_it_succeeds()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should().BeDecoratedWith<DummyClassAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_decorated_with_expected_attribute_it_should_allow_chaining()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should().BeDecoratedWith<DummyClassAttribute>()
                    .Which.IsEnabled.Should().BeTrue();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_not_decorated_with_expected_attribute_it_fails()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithoutAttribute);

            // Act
            Action act = () =>
                typeWithoutAttribute.Should().BeDecoratedWith<DummyClassAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithoutAttribute to be decorated with *.DummyClassAttribute *failure message*" +
                    ", but the attribute was not found.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_BeDecoratedWith_it_should_throw()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should().BeDecoratedWith<DummyClassAttribute>(isMatchingAttributePredicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }

        [Fact]
        public void When_type_is_decorated_with_expected_attribute_with_the_expected_properties_it_succeeds()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should()
                    .BeDecoratedWith<DummyClassAttribute>(a => (a.Name == "Expected") && a.IsEnabled);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_decorated_with_expected_attribute_with_the_expected_properties_it_should_allow_chaining()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should()
                    .BeDecoratedWith<DummyClassAttribute>(a => a.Name == "Expected")
                        .Which.IsEnabled.Should().BeTrue();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_decorated_with_expected_attribute_that_has_an_unexpected_property_it_fails()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should()
                    .BeDecoratedWith<DummyClassAttribute>(a => (a.Name == "Unexpected") && a.IsEnabled);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithAttribute to be decorated with *.DummyClassAttribute that matches " +
                    "((a.Name == \"Unexpected\")*a.IsEnabled), but no matching attribute was found.");
        }

        #endregion

        #region BeDecoratedWithOrInherit

        [Fact]
        public void When_type_inherits_expected_attribute_it_succeeds()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should().BeDecoratedWithOrInherit<DummyClassAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_inherits_expected_attribute_it_should_allow_chaining()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should().BeDecoratedWithOrInherit<DummyClassAttribute>()
                    .Which.IsEnabled.Should().BeTrue();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_does_not_inherit_expected_attribute_it_fails()
        {
            // Arrange
            Type type = typeof(ClassWithoutAttribute);

            // Act
            Action act = () =>
                type.Should().BeDecoratedWithOrInherit<DummyClassAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithoutAttribute to be decorated with or inherit *.DummyClassAttribute " +
                    "*failure message*, but the attribute was not found.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_BeDecoratedWithOrInherit_it_should_throw()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () => typeWithAttribute.Should()
                .BeDecoratedWithOrInherit<DummyClassAttribute>(isMatchingAttributePredicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }

        [Fact]
        public void When_type_inherits_expected_attribute_with_the_expected_properties_it_succeeds()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should()
                    .BeDecoratedWithOrInherit<DummyClassAttribute>(a => (a.Name == "Expected") && a.IsEnabled);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_inherits_expected_attribute_with_the_expected_properties_it_should_allow_chaining()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should()
                    .BeDecoratedWithOrInherit<DummyClassAttribute>(a => a.Name == "Expected")
                        .Which.IsEnabled.Should().BeTrue();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_inherits_expected_attribute_that_has_an_unexpected_property_it_fails()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should()
                    .BeDecoratedWithOrInherit<DummyClassAttribute>(a => (a.Name == "Unexpected") && a.IsEnabled);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithInheritedAttribute to be decorated with or inherit *.DummyClassAttribute " +
                    "that matches ((a.Name == \"Unexpected\")*a.IsEnabled), but no matching attribute was found.");
        }

        #endregion

        #region NotBeDecoratedWith

        [Fact]
        public void When_type_is_not_decorated_with_unexpected_attribute_it_succeeds()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithoutAttribute);

            // Act
            Action act = () =>
                typeWithoutAttribute.Should().NotBeDecoratedWith<DummyClassAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_decorated_with_unexpected_attribute_it_fails()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should().NotBeDecoratedWith<DummyClassAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithAttribute to not be decorated with *.DummyClassAttribute* *failure message*" +
                    ", but the attribute was found.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_NotBeDecoratedWith_it_should_throw()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () => typeWithoutAttribute.Should()
                .NotBeDecoratedWith<DummyClassAttribute>(isMatchingAttributePredicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }

        [Fact]
        public void When_type_is_not_decorated_with_unexpected_attribute_with_the_expected_properties_it_succeeds()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithoutAttribute.Should()
                    .NotBeDecoratedWith<DummyClassAttribute>(a => (a.Name == "Unexpected") && a.IsEnabled);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_is_not_decorated_with_expected_attribute_that_has_an_unexpected_property_it_fails()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithAttribute);

            // Act
            Action act = () =>
                typeWithoutAttribute.Should()
                    .NotBeDecoratedWith<DummyClassAttribute>(a => (a.Name == "Expected") && a.IsEnabled);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithAttribute to not be decorated with *.DummyClassAttribute that matches " +
                    "((a.Name == \"Expected\") * a.IsEnabled), but a matching attribute was found.");
        }

        #endregion

        #region NotBeDecoratedWithOrInherit

        [Fact]
        public void When_type_does_not_inherit_unexpected_attribute_it_succeeds()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithoutAttribute);

            // Act
            Action act = () =>
                typeWithoutAttribute.Should().NotBeDecoratedWithOrInherit<DummyClassAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_inherits_unexpected_attribute_it_fails()
        {
            // Arrange
            Type typeWithAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () =>
                typeWithAttribute.Should()
                    .NotBeDecoratedWithOrInherit<DummyClassAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithInheritedAttribute to not be decorated with or inherit *.DummyClassAttribute " +
                    "*failure message* attribute was found.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_NotBeDecoratedWithOrInherit_it_should_throw()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () => typeWithoutAttribute.Should()
                .NotBeDecoratedWithOrInherit<DummyClassAttribute>(isMatchingAttributePredicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }

        [Fact]
        public void When_type_does_not_inherit_unexpected_attribute_with_the_expected_properties_it_succeeds()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () =>
                typeWithoutAttribute.Should()
                    .NotBeDecoratedWithOrInherit<DummyClassAttribute>(a => (a.Name == "Unexpected") && a.IsEnabled);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_type_does_not_inherit_expected_attribute_that_has_an_unexpected_property_it_fails()
        {
            // Arrange
            Type typeWithoutAttribute = typeof(ClassWithInheritedAttribute);

            // Act
            Action act = () =>
                typeWithoutAttribute.Should()
                    .NotBeDecoratedWithOrInherit<DummyClassAttribute>(a => (a.Name == "Expected") && a.IsEnabled);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithInheritedAttribute to not be decorated with or inherit *.DummyClassAttribute " +
                    "that matches ((a.Name == \"Expected\") * a.IsEnabled), but a matching attribute was found.");
        }

        #endregion

        #region BeSealed

        [Fact]
        public void When_type_is_sealed_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(Sealed).Should().BeSealed();
        }

        [Theory]
        [InlineData(typeof(ClassWithoutMembers), "Expected type *.ClassWithoutMembers to be sealed.")]
        [InlineData(typeof(Abstract), "Expected type *.Abstract to be sealed.")]
        [InlineData(typeof(Static), "Expected type *.Static to be sealed.")]
        public void When_type_is_not_sealed_it_fails(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().BeSealed();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_type_is_not_sealed_it_fails_with_a_meaningful_message()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () => type.Should().BeSealed("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.ClassWithoutMembers to be sealed *failure message*.");
        }

        [Theory]
        [InlineData(typeof(IDummyInterface), "*.IDummyInterface must be a class.")]
        [InlineData(typeof(Struct), "*.Struct must be a class.")]
        [InlineData(typeof(ExampleDelegate), "*.ExampleDelegate must be a class.")]
        public void When_type_is_not_valid_for_BeSealed_it_throws_exception(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().BeSealed();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_subject_is_null_be_sealed_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().BeSealed("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be sealed *failure message*, but type is <null>.");
        }

        #endregion

        #region NotBeSealed

        [Theory]
        [InlineData(typeof(ClassWithoutMembers))]
        [InlineData(typeof(Abstract))]
        [InlineData(typeof(Static))]
        public void When_type_is_not_sealed_it_succeeds(Type type)
        {
            // Arrange / Act / Assert
            type.Should().NotBeSealed();
        }

        [Fact]
        public void When_type_is_sealed_it_fails()
        {
            // Arrange
            var type = typeof(Sealed);

            // Act
            Action act = () => type.Should().NotBeSealed();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.Sealed not to be sealed.");
        }

        [Fact]
        public void When_type_is_sealed_it_fails_with_a_meaningful_message()
        {
            // Arrange
            var type = typeof(Sealed);

            // Act
            Action act = () => type.Should().NotBeSealed("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.Sealed not to be sealed *failure message*.");
        }

        [Theory]
        [InlineData(typeof(IDummyInterface), "*.IDummyInterface must be a class.")]
        [InlineData(typeof(Struct), "*.Struct must be a class.")]
        [InlineData(typeof(ExampleDelegate), "*.ExampleDelegate must be a class.")]
        public void When_type_is_not_valid_for_NotBeSealed_it_throws_exception(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().NotBeSealed();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_subject_is_null_not_be_sealed_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotBeSealed("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type not to be sealed *failure message*, but type is <null>.");
        }

        #endregion

        #region BeAbstract

        [Fact]
        public void When_type_is_abstract_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(Abstract).Should().BeAbstract();
        }

        [Theory]
        [InlineData(typeof(ClassWithoutMembers), "Expected type *.ClassWithoutMembers to be abstract.")]
        [InlineData(typeof(Sealed), "Expected type *.Sealed to be abstract.")]
        [InlineData(typeof(Static), "Expected type *.Static to be abstract.")]
        public void When_type_is_not_abstract_it_fails(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().BeAbstract();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_type_is_not_abstract_it_fails_with_a_meaningful_message()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () => type.Should().BeAbstract("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.ClassWithoutMembers to be abstract *failure message*.");
        }

        [Theory]
        [InlineData(typeof(IDummyInterface), "*.IDummyInterface must be a class.")]
        [InlineData(typeof(Struct), "*.Struct must be a class.")]
        [InlineData(typeof(ExampleDelegate), "*.ExampleDelegate must be a class.")]
        public void When_type_is_not_valid_for_BeAbstract_it_throws_exception(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().BeAbstract();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_subject_is_null_be_abstract_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().BeAbstract("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be abstract *failure message*, but type is <null>.");
        }

        #endregion

        #region NotBeAbstract

        [Theory]
        [InlineData(typeof(ClassWithoutMembers))]
        [InlineData(typeof(Sealed))]
        [InlineData(typeof(Static))]
        public void When_type_is_not_abstract_it_succeeds(Type type)
        {
            // Arrange / Act / Assert
            type.Should().NotBeAbstract();
        }

        [Fact]
        public void When_type_is_abstract_it_fails()
        {
            // Arrange
            var type = typeof(Abstract);

            // Act
            Action act = () => type.Should().NotBeAbstract();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.Abstract not to be abstract.");
        }

        [Fact]
        public void When_type_is_abstract_it_fails_with_a_meaningful_message()
        {
            // Arrange
            var type = typeof(Abstract);

            // Act
            Action act = () => type.Should().NotBeAbstract("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.Abstract not to be abstract *failure message*.");
        }

        [Theory]
        [InlineData(typeof(IDummyInterface), "*.IDummyInterface must be a class.")]
        [InlineData(typeof(Struct), "*.Struct must be a class.")]
        [InlineData(typeof(ExampleDelegate), "*.ExampleDelegate must be a class.")]
        public void When_type_is_not_valid_for_NotBeAbstract_it_throws_exception(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().NotBeAbstract();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_subject_is_null_not_be_abstract_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotBeAbstract("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type not to be abstract *failure message*, but type is <null>.");
        }

        #endregion

        #region BeStatic

        [Fact]
        public void When_type_is_static_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(Static).Should().BeStatic();
        }

        [Theory]
        [InlineData(typeof(ClassWithoutMembers), "Expected type *.ClassWithoutMembers to be static.")]
        [InlineData(typeof(Sealed), "Expected type *.Sealed to be static.")]
        [InlineData(typeof(Abstract), "Expected type *.Abstract to be static.")]
        public void When_type_is_not_static_it_fails(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().BeStatic();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_type_is_not_static_it_fails_with_a_meaningful_message()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () => type.Should().BeStatic("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.ClassWithoutMembers to be static *failure message*.");
        }

        [Theory]
        [InlineData(typeof(IDummyInterface), "*.IDummyInterface must be a class.")]
        [InlineData(typeof(Struct), "*.Struct must be a class.")]
        [InlineData(typeof(ExampleDelegate), "*.ExampleDelegate must be a class.")]
        public void When_type_is_not_valid_for_BeStatic_it_throws_exception(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().BeStatic();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_subject_is_null_be_static_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().BeStatic("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be static *failure message*, but type is <null>.");
        }

        #endregion

        #region NotBeStatic

        [Theory]
        [InlineData(typeof(ClassWithoutMembers))]
        [InlineData(typeof(Sealed))]
        [InlineData(typeof(Abstract))]
        public void When_type_is_not_static_it_succeeds(Type type)
        {
            // Arrange / Act / Assert
            type.Should().NotBeStatic();
        }

        [Fact]
        public void When_type_is_static_it_fails()
        {
            // Arrange
            var type = typeof(Static);

            // Act
            Action act = () => type.Should().NotBeStatic();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.Static not to be static.");
        }

        [Fact]
        public void When_type_is_static_it_fails_with_a_meaningful_message()
        {
            // Arrange
            var type = typeof(Static);

            // Act
            Action act = () => type.Should().NotBeStatic("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.Static not to be static *failure message*.");
        }

        [Theory]
        [InlineData(typeof(IDummyInterface), "*.IDummyInterface must be a class.")]
        [InlineData(typeof(Struct), "*.Struct must be a class.")]
        [InlineData(typeof(ExampleDelegate), "*.ExampleDelegate must be a class.")]
        public void When_type_is_not_valid_for_NotBeStatic_it_throws_exception(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().NotBeStatic();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_subject_is_null_not_be_static_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotBeStatic("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type not to be static *failure message*, but type is <null>.");
        }

        #endregion

        #region Implement

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

        #endregion

        #region ImplementOfT

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

        #endregion

        #region NotImplement

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

        #endregion

        #region NotImplementOfT

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

        #endregion

        #region HaveProperty

        [Fact]
        public void When_asserting_a_type_has_a_property_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveProperty(typeof(string), "PrivateWriteProtectedReadProperty")
                    .Which.Should()
                        .BeWritable(CSharpAccessModifier.Private)
                        .And.BeReadable(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), "PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected String *.ClassWithNoMembers.PublicProperty to exist *failure message*, but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_which_it_has_with_a_different_type_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveProperty(typeof(int), "PrivateWriteProtectedReadProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected String *.ClassWithMembers.PrivateWriteProtectedReadProperty to be of type System.Int32 " +
                    "*failure message*, but it is not.");
        }

        [Fact]
        public void When_subject_is_null_have_property_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), "PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected String type.PublicProperty to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_of_null_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(null, "PublicProperty");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("propertyType");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region HavePropertyOfT

        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveProperty<string>("PrivateWriteProtectedReadProperty")
                    .Which.Should()
                        .BeWritable(CSharpAccessModifier.Private)
                        .And.BeReadable(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty<string>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty<string>(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region NotHaveProperty

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty("Property");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty("PrivateWriteProtectedReadProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected String *.ClassWithMembers.PrivateWriteProtectedReadProperty to not exist *failure message*" +
                    ", but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_property_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveProperty("PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type.PublicProperty to not exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region HaveExplicitProperty

        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_property_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "ExplicitStringProperty");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_property_which_it_implements_implicitly_and_explicitly_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "ExplicitImplicitStringProperty");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_property_which_it_implements_implicitly_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "ImplicitStringProperty");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "*.IExplicitInterface.ImplicitStringProperty, but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_property_which_it_does_not_implement_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "NonExistentProperty");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "*.IExplicitInterface.NonExistentProperty, but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_explicitly_implements_a_property_from_an_unimplemented_interface_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IDummyInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "NonExistentProperty");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassExplicitlyImplementingInterface to implement interface *.IDummyInterface" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_explicit_property_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveExplicitProperty(
                    typeof(IExplicitInterface), "ExplicitStringProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to explicitly implement *.IExplicitInterface.ExplicitStringProperty *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_property_inherited_by_null_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitProperty(null, "ExplicitStringProperty");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("interfaceType");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitProperty(typeof(IExplicitInterface), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitProperty(typeof(IExplicitInterface), string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region HaveExplicitPropertyOfT

        [Fact]
        public void When_asserting_a_type_explicitlyOfT_implements_a_property_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty<IExplicitInterface>("ExplicitStringProperty");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicitlyOfT_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitProperty<IExplicitInterface>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicitlyOfT_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().HaveExplicitProperty<IExplicitInterface>(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_subject_is_null_have_explicit_propertyOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveExplicitProperty<IExplicitInterface>(
                    "ExplicitStringProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to explicitly implement *.IExplicitInterface.ExplicitStringProperty *failure message*" +
                    ", but type is <null>.");
        }

        #endregion

        #region NotHaveExplicitProperty

        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "ExplicitStringProperty");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "*.IExplicitInterface.ExplicitStringProperty, but it does.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_which_it_implements_implicitly_and_explicitly_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "ExplicitImplicitStringProperty");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "*.IExplicitInterface.ExplicitImplicitStringProperty, but it does.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_which_it_implements_implicitly_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "ImplicitStringProperty");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_which_it_does_not_implement_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "NonExistentProperty");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_from_an_unimplemented_interface_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IDummyInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "NonExistentProperty");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassExplicitlyImplementingInterface to implement interface *.IDummyInterface" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_not_have_explicit_property_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitProperty(
                    typeof(IExplicitInterface), "ExplicitStringProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to not explicitly implement *IExplicitInterface.ExplicitStringProperty *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_property_inherited_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitProperty(null, "ExplicitStringProperty");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("interfaceType");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitProperty(typeof(IExplicitInterface), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitProperty(typeof(IExplicitInterface), string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region NotHaveExplicitPropertyOfT

        [Fact]
        public void When_asserting_a_type_does_not_explicitlyOfT_implement_a_property_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty<IExplicitInterface>("ExplicitStringProperty");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected *.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "*.IExplicitInterface.ExplicitStringProperty, but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_explicit_propertyOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitProperty<IExplicitInterface>(
                    "ExplicitStringProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to not explicitly implement *.IExplicitInterface.ExplicitStringProperty *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicitlyOfT_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitProperty<IExplicitInterface>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicitlyOfT_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassExplicitlyImplementingInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitProperty<IExplicitInterface>(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region HaveExplicitMethod

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
        public void When_asserting_a_type_explicitly_implements_a_method_which_it_implements_implicitly_and_explicitly_it_succeeds()
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
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region HaveExplicitMethodOfT

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
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region NotHaveExplicitMethod

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
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_implements_implicitly_and_explicitly_it_fails()
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
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region NotHaveExplicitMethodOfT

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
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        #endregion

        #region HaveIndexer

        [Fact]
        public void When_asserting_a_type_has_an_indexer_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveIndexer(typeof(string), new[] { typeof(string) })
                    .Which.Should()
                        .BeWritable(CSharpAccessModifier.Internal)
                        .And.BeReadable(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_indexer_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveIndexer(
                    typeof(string), new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected String *.ClassWithNoMembers[System.Int32, System.Type] to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_indexer_with_different_parameters_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveIndexer(
                    typeof(string), new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected String *.ClassWithMembers[System.Int32, System.Type] to exist *failure message*, but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_indexer_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveIndexer(typeof(string), new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected String type[System.String] to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_indexer_for_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveIndexer(null, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("indexerType");
        }

        [Fact]
        public void When_asserting_a_type_has_an_indexer_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveIndexer(typeof(string), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        #endregion

        #region NotHaveIndexer

        [Fact]
        public void When_asserting_a_type_does_not_have_an_indexer_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveIndexer(new[] { typeof(string) });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_indexer_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveIndexer(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected indexer *.ClassWithMembers[System.String] to not exist *failure message*, but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_indexer_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveIndexer(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected indexer type[System.String] to not exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_indexer_for_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveIndexer(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        #endregion

        #region HaveConstructor

        [Fact]
        public void When_asserting_a_type_has_a_constructor_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveConstructor(new Type[] { typeof(string) })
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_constructor_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveConstructor(new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected constructor *.ClassWithNoMembers(System.Int32, System.Type) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_constructor_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveConstructor(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected constructor type(System.String) to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_constructor_of_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveConstructor(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        #endregion

        #region HaveDefaultConstructor

        [Fact]
        public void When_asserting_a_type_has_a_default_constructor_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveDefaultConstructor()
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_default_constructor_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveDefaultConstructor()
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_default_constructor_which_it_does_not_and_a_cctor_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithCctor);

            // Act
            type.Should()
                    .HaveDefaultConstructor()
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifier.Public);
            Action act = () =>
                type.Should()
                    .HaveDefaultConstructor()
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_default_constructor_which_it_does_not_and_a_cctor_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithCctorAndNonDefaultConstructor);

            // Act
            Action act = () =>
                type.Should().HaveDefaultConstructor("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected constructor *.ClassWithCctorAndNonDefaultConstructor() to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_default_constructor_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveDefaultConstructor("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected constructor type() to exist *failure message*, but type is <null>.");
        }

        #endregion

        #region NotHaveConstructor

        [Fact]
        public void When_asserting_a_type_does_not_have_a_constructor_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveConstructor(new Type[] { typeof(string) });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_constructor_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveConstructor(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected constructor *.ClassWithMembers(System.String) not to exist *failure message*, but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_constructor_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveConstructor(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected constructor type(System.String) not to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_constructor_of_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveConstructor(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        #endregion

        #region NotHaveDefaultConstructor

        [Fact]
        public void When_asserting_a_type_does_not_have_a_default_constructor_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithCctorAndNonDefaultConstructor);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveDefaultConstructor();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_default_constructor_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveDefaultConstructor("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected constructor *.ClassWithMembers() not to exist *failure message*, but it does.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_default_constructor_which_it_does_and_a_cctor_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithCctor);

            // Act
            Action act = () =>
                type.Should().NotHaveDefaultConstructor("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected constructor *.ClassWithCctor*() not to exist *failure message*, but it does.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_default_constructor_which_it_does_not_and_a_cctor_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithCctorAndNonDefaultConstructor);

            // Act
            Action act = () =>
                type.Should().NotHaveDefaultConstructor();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_null_not_have_default_constructor_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveDefaultConstructor("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected constructor type() not to exist *failure message*, but type is <null>.");
        }

        #endregion

        #region HaveMethod

        [Fact]
        public void When_asserting_a_type_has_a_method_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveMethod("VoidMethod", new Type[] { })
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifier.Private)
                        .And.ReturnVoid();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod(
                        "NonExistentMethod", new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method *.ClassWithNoMembers.NonExistentMethod(*.Int32, *.Type) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_with_different_parameter_types_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod(
                    "VoidMethod", new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method *.ClassWithMembers.VoidMethod(*.Int32, *.Type) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_method_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveMethod("Name", new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method type.Name(System.String) to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod(null, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod(string.Empty, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod("Name", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        #endregion

        #region NotHaveMethod

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("NonExistentMethod", new Type[] { });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_which_it_has_with_different_parameter_types_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("VoidMethod", new[] { typeof(int) });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_that_method_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("VoidMethod", new Type[] { }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method Void *.ClassWithMembers.VoidMethod() to not exist *failure message*, but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_method_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("Name", new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method type.Name(System.String) to not exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod(null, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod(string.Empty, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("Name", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }

        #endregion

        #region HaveAccessModifier

        [Fact]
        public void When_asserting_a_public_type_is_public_it_succeeds()
        {
            // Arrange
            Type type = typeof(IPublicInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_public_member_is_internal_it_throws()
        {
            // Arrange
            Type type = typeof(IPublicInterface);

            // Act
            Action act = () =>
                type
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type IPublicInterface to be Internal *failure message*, but it is Public.");
        }

        [Fact]
        public void When_asserting_an_internal_type_is_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(InternalClass);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_internal_type_is_protected_internal_it_throws()
        {
            // Arrange
            Type type = typeof(InternalClass);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalClass to be ProtectedInternal *failure message*, but it is Internal.");
        }

        [Fact]
        public void When_asserting_a_nested_private_type_is_private_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_private_type_is_protected_it_throws()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Protected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type PrivateClass to be Protected *failure message*, but it is Private.");
        }

        [Fact]
        public void When_asserting_a_nested_protected_type_is_protected_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_protected_type_is_public_it_throws()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type ProtectedEnum to be Public, but it is Protected.");
        }

        [Fact]
        public void When_asserting_a_nested_public_type_is_public_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.IPublicInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_public_member_is_internal_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.IPublicInterface);

            // Act
            Action act = () =>
                type
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type IPublicInterface to be Internal *failure message*, but it is Public.");
        }

        [Fact]
        public void When_asserting_a_nested_internal_type_is_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.InternalClass);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_internal_type_is_protected_internal_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.InternalClass);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalClass to be ProtectedInternal *failure message*, but it is Internal.");
        }

        [Fact]
        public void When_asserting_a_nested_protected_internal_member_is_protected_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.IProtectedInternalInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_protected_internal_member_is_private_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.IProtectedInternalInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Private, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type IProtectedInternalInterface to be Private *failure message*, but it is ProtectedInternal.");
        }

        [Fact]
        public void When_subject_is_null_have_access_modifier_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be Public *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_access_modifier_with_an_invalid_enum_value_it_should_throw()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier((CSharpAccessModifier)int.MaxValue);

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithParameterName("accessModifier");
        }

        #endregion

        #region NotHaveAccessModifier

        [Fact]
        public void When_asserting_a_public_type_is_not_private_it_succeeds()
        {
            // Arrange
            Type type = typeof(IPublicInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_public_member_is_not_public_it_throws()
        {
            // Arrange
            Type type = typeof(IPublicInterface);

            // Act
            Action act = () =>
                type
                    .Should()
                    .NotHaveAccessModifier(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type IPublicInterface not to be Public *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_an_internal_type_is_not_protected_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(InternalClass);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_internal_type_is_not_internal_it_throws()
        {
            // Arrange
            Type type = typeof(InternalClass);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalClass not to be Internal *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_private_type_is_not_protected_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_private_type_is_not_private_it_throws()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Private, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type PrivateClass not to be Private *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_protected_type_is_not_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Internal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_protected_type_is_not_protected_it_throws()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Protected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type ProtectedEnum not to be Protected, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_public_type_is_not_private_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.IPublicInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_public_member_is_not_public_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.IPublicInterface);

            // Act
            Action act = () =>
                type
                    .Should()
                    .NotHaveAccessModifier(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type IPublicInterface not to be Public *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_internal_type_is_not_protected_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.InternalClass);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_internal_type_is_not_internal_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.InternalClass);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalClass not to be Internal *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_protected_internal_member_is_not_public_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.IProtectedInternalInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_protected_internal_member_is_not_protected_internal_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.IProtectedInternalInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type IProtectedInternalInterface not to be ProtectedInternal *failure message*, but it is.");
        }

        [Fact]
        public void When_subject_is_null_not_have_access_modifier_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type not to be Public *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_access_modifier_with_an_invalid_enum_value_it_should_throw()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier((CSharpAccessModifier)int.MaxValue);

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithParameterName("accessModifier");
        }

        #endregion

        #region HaveExplicitConversionOperator

        [Fact]
        public void When_asserting_a_type_has_an_explicit_conversion_operator_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(byte);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitConversionOperator(sourceType, targetType)
                    .Which.Should()
                        .NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_conversion_operator_which_it_does_not_it_fails_with_a_useful_message()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(string);

            // Act
            Action act = () =>
                type.Should().HaveExplicitConversionOperator(
                    sourceType, targetType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static explicit System.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_explicit_conversion_operator_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveExplicitConversionOperator(
                    typeof(TypeWithConversionOperators), typeof(string), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static explicit System.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_conversion_operator_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveExplicitConversionOperator(null, typeof(string));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("sourceType");
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_conversion_operator_to_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveExplicitConversionOperator(typeof(TypeWithConversionOperators), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("targetType");
        }

        #endregion

        #region HaveExplicitConversionOperatorOfT

        [Fact]
        public void When_asserting_a_type_has_an_explicit_conversion_operatorOfT_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should()
                    .HaveExplicitConversionOperator<TypeWithConversionOperators, byte>()
                    .Which.Should()
                        .NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_explicit_conversion_operatorOfT_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveExplicitConversionOperator<TypeWithConversionOperators, string>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static explicit System.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_explicit_conversion_operatorOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveExplicitConversionOperator<TypeWithConversionOperators, string>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static explicit System.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but type is <null>.");
        }

        #endregion

        #region NotHaveExplicitConversionOperator

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_conversion_operator_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(string);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitConversionOperator(sourceType, targetType);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_conversion_operator_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(byte);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitConversionOperator(
                    sourceType, targetType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static explicit *.Byte(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_explicit_conversion_operator_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitConversionOperator(
                    typeof(TypeWithConversionOperators), typeof(string), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static explicit *.String(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_conversion_operator_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitConversionOperator(null, typeof(string));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("sourceType");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_conversion_operator_to_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitConversionOperator(typeof(TypeWithConversionOperators), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("targetType");
        }

        #endregion

        #region NotHaveExplicitConversionOperatorOfT

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_conversion_operatorOfT_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveExplicitConversionOperator<TypeWithConversionOperators, string>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_explicit_conversion_operatorOfT_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveExplicitConversionOperator<TypeWithConversionOperators, byte>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static explicit *.Byte(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but it does.");
        }

        #endregion

        #region HaveImplicitConversionOperator

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operator_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(int);

            // Act
            Action act = () =>
                type.Should()
                    .HaveImplicitConversionOperator(sourceType, targetType)
                    .Which.Should()
                        .NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operator_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(string);

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator(
                    sourceType, targetType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_implicit_conversion_operator_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator(
                    typeof(TypeWithConversionOperators), typeof(string), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operator_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator(null, typeof(string));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("sourceType");
        }

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operator_to_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator(typeof(TypeWithConversionOperators), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("targetType");
        }

        #endregion

        #region HaveImplicitConversionOperatorOfT

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operatorOfT_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should()
                    .HaveImplicitConversionOperator<TypeWithConversionOperators, int>()
                    .Which.Should()
                        .NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operatorOfT_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator<TypeWithConversionOperators, string>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_implicit_conversion_operatorOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator<TypeWithConversionOperators, string>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but type is <null>.");
        }

        #endregion

        #region NotHaveImplicitConversionOperator

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operator_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(string);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveImplicitConversionOperator(sourceType, targetType);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operator_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(int);

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator(
                    sourceType, targetType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.Int32(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_implicit_conversion_operator_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator(
                    typeof(TypeWithConversionOperators), typeof(string), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operator_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator(null, typeof(string));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("sourceType");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operator_to_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator(typeof(TypeWithConversionOperators), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("targetType");
        }

        #endregion

        #region NotHaveImplicitConversionOperatorOfT

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operatorOfT_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveImplicitConversionOperator<TypeWithConversionOperators, string>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operatorOfT_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator<TypeWithConversionOperators, int>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.Int32(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_implicit_conversion_operatorOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator<TypeWithConversionOperators, string>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but type is <null>.");
        }

        #endregion
    }

    #region Internal classes used in unit tests

    [DummyClass("Expected", true)]
    public class ClassWithAttribute
    {
    }

    public class ClassWithInheritedAttribute : ClassWithAttribute
    {
    }

    public class ClassWithoutAttribute
    {
    }

    public class OtherClassWithoutAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DummyClassAttribute : Attribute
    {
        public string Name { get; }

        public bool IsEnabled { get; }

        public DummyClassAttribute(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }
    }

    public interface IDummyInterface
    {
    }

    public interface IDummyInterface<T>
    {
    }

    public class ClassThatImplementsInterface : IDummyInterface, IDummyInterface<IDummyInterface>
    {
    }

    public class ClassThatDoesNotImplementInterface
    {
    }

    public class DummyBaseType<T> : IDummyInterface<IDummyInterface>
    {
    }

    public class ClassWithGenericBaseType : DummyBaseType<ClassWithGenericBaseType>
    {
    }

    public class ClassWithMembers
    {
        protected internal ClassWithMembers() { }

        private ClassWithMembers(string _) { }

        protected string PrivateWriteProtectedReadProperty { get => null; private set { } }

        internal string this[string str] { private get => str; set { } }

        protected internal string this[int i] { get => i.ToString(CultureInfo.InvariantCulture); private set { } }

        private void VoidMethod() { }

        private void VoidMethod(string _) { }
    }

    public class ClassExplicitlyImplementingInterface : IExplicitInterface
    {
        public string ImplicitStringProperty { get => null; private set { } }

        string IExplicitInterface.ExplicitStringProperty { set { } }

        public string ExplicitImplicitStringProperty { get; set; }

        string IExplicitInterface.ExplicitImplicitStringProperty { get; set; }

        public void ImplicitMethod() { }

        public void ImplicitMethod(string overload) { }

        void IExplicitInterface.ExplicitMethod() { }

        void IExplicitInterface.ExplicitMethod(string overload) { }

        public void ExplicitImplicitMethod() { }

        public void ExplicitImplicitMethod(string _) { }

        void IExplicitInterface.ExplicitImplicitMethod() { }

        void IExplicitInterface.ExplicitImplicitMethod(string overload) { }
    }

    public interface IExplicitInterface
    {
        string ImplicitStringProperty { get; }

        string ExplicitStringProperty { set; }

        string ExplicitImplicitStringProperty { get; set; }

        void ImplicitMethod();

        void ImplicitMethod(string overload);

        void ExplicitMethod();

        void ExplicitMethod(string overload);

        void ExplicitImplicitMethod();

        void ExplicitImplicitMethod(string overload);
    }

    public class ClassWithoutMembers { }

    public interface IPublicInterface { }

    internal class InternalClass { }

    internal class Nested
    {
        private class PrivateClass { }

        protected enum ProtectedEnum { }

        public interface IPublicInterface { }

        internal class InternalClass { }

        protected internal interface IProtectedInternalInterface { }
    }

    internal readonly struct TypeWithConversionOperators
    {
        private readonly int value;

        private TypeWithConversionOperators(int value)
        {
            this.value = value;
        }

        public static implicit operator int(TypeWithConversionOperators typeWithConversionOperators) => typeWithConversionOperators.value;

        public static explicit operator byte(TypeWithConversionOperators typeWithConversionOperators) => (byte)typeWithConversionOperators.value;
    }

    internal sealed class Sealed { }

    internal abstract class Abstract { }

    internal static class Static { }

    internal struct Struct { }

    public delegate void ExampleDelegate();

    internal class ClassNotInDummyNamespace { }

    internal class OtherClassNotInDummyNamespace { }

    #endregion
}

namespace FluentAssertions.Primitives
{
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

    /// <summary>
    /// A class that intentionally has the exact same name and namespace as the ObjectAssertions from the FluentAssertions
    /// assembly. This class is used to test the behavior of comparisons on such types.
    /// </summary>
    internal class ObjectAssertions
    {
    }

#pragma warning restore 436
}

#region Internal classes used in unit tests

namespace DummyNamespace
{
    internal class ClassInDummyNamespace { }

    namespace InnerDummyNamespace
    {
        internal class ClassInInnerDummyNamespace { }
    }
}

namespace DummyNamespaceTwo
{
    internal class ClassInDummyNamespaceTwo { }
}

#endregion
