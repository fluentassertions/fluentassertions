using System;

using FluentAssertions.Primitives;
using FluentAssertions.Types;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class TypeAssertionSpecs
    {
        #region Be

        [TestMethod]
        public void When_type_is_equal_to_the_same_type_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);
            Type sameType = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be(sameType);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_type_is_equal_to_another_type_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);
            Type differentType = typeof (ClassWithoutAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be(differentType);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_type_is_equal_to_another_type_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);
            Type differentType = typeof (ClassWithoutAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be(differentType, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------

            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be FluentAssertions.Specs.ClassWithoutAttribute" +
                " because we want to test the error message, but found FluentAssertions.Specs.ClassWithAttribute.");
        }

        [TestMethod]
        public void When_asserting_equality_of_a_type_but_the_type_is_null_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type nullType = null;
            Type someType = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                nullType.Should().Be(someType, "because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be FluentAssertions.Specs.ClassWithAttribute" +
                " because we want to test the error message, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_equality_of_a_type_with_null_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type someType = typeof (ClassWithAttribute);
            Type nullType = null;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                someType.Should().Be(nullType, "because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be <null>" +
                " because we want to test the error message, but found FluentAssertions.Specs.ClassWithAttribute.");
        }

        [TestMethod]
        public void When_type_is_equal_to_same_type_from_different_assembly_it_should_throw_with_assembly_qualified_name
            ()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

            Type typeFromThisAssembly = typeof (ObjectAssertions);
#if !WINRT && !WINDOWS_PHONE_APP
            Type typeFromOtherAssembly =
                typeof (TypeAssertions).Assembly.GetType("FluentAssertions.Primitives.ObjectAssertions");
#else
            Type typeFromOtherAssembly =
                Type.GetType("FluentAssertions.Primitives.ObjectAssertions,FluentAssertions.Core");
#endif

#pragma warning restore 436

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeFromThisAssembly.Should().Be(typeFromOtherAssembly, "because we want to test the error {0}",
                    "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            const string expectedMessage =
                "Expected type to be [FluentAssertions.Primitives.ObjectAssertions, FluentAssertions*]" +
                " because we want to test the error message, but found " +
                "[FluentAssertions.Primitives.ObjectAssertions, FluentAssertions*].";

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        [TestMethod]
        public void When_type_is_equal_to_the_same_type_using_generics_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be<ClassWithAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_type_is_equal_to_another_type_using_generics_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be<ClassWithoutAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_type_is_equal_to_another_type_using_generics_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be<ClassWithoutAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected type to be FluentAssertions.Specs.ClassWithoutAttribute because we want to test " +
                    "the error message, but found FluentAssertions.Specs.ClassWithAttribute.");
        }

        #endregion

        #region NotBe

        [TestMethod]
        public void When_type_is_not_equal_to_the_another_type_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);
            Type otherType = typeof (ClassWithoutAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe(otherType);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_type_is_not_equal_to_the_same_type_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);
            Type sameType = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe(sameType);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_type_is_not_equal_to_the_same_type_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);
            Type sameType = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe(sameType, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type not to be [FluentAssertions.Specs.ClassWithAttribute*]" +
                             " because we want to test the error message.");
        }

        [TestMethod]
        public void When_type_is_not_equal_to_another_type_using_generics_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe<ClassWithoutAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_type_is_not_equal_to_the_same_type_using_generics_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe<ClassWithAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_type_is_not_equal_to_the_same_type_using_generics_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe<ClassWithAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected type not to be [FluentAssertions.Specs.ClassWithAttribute*] because we want to test " +
                    "the error message.");
        }

        #endregion

        #region BeAssignableTo

        [TestMethod]
        public void When_asserting_an_object_is_assignable_its_own_type_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof (DummyImplementingClass).Should().BeAssignableTo<DummyImplementingClass>();
        }

        [TestMethod]
        public void When_asserting_an_object_is_assignable_to_its_base_type_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof (DummyImplementingClass).Should().BeAssignableTo<DummyBaseClass>();
        }

        [TestMethod]
        public void When_asserting_an_object_is_assignable_to_an_implemented_interface_type_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof (DummyImplementingClass).Should().BeAssignableTo<IDisposable>();
        }

        [TestMethod]
        public void When_asserting_an_object_is_assignable_to_an_unrelated_type_it_should_fail_with_a_descriptive_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Type someType = typeof (DummyImplementingClass);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            someType.Invoking(
                x => x.Should().BeAssignableTo<DateTime>("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format(
                    "Expected type {0} to be assignable to {1} because we want to test the failure message, but it is not",
                    typeof (DummyImplementingClass), typeof (DateTime)));
        }

        #endregion

        #region BeDerivedFrom

        [TestMethod]
        public void When_asserting_a_type_is_derived_from_its_base_class_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(DummyImplementingClass);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(DummyBaseClass));

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_is_derived_from_an_unrelated_class_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(DummyBaseClass);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(ClassWithMembers), "because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.DummyBaseClass to be derived from " +
                             "FluentAssertions.Specs.ClassWithMembers because we want to test the error message.");
        }

        [TestMethod]
        public void When_asserting_a_type_is_derived_from_an_interface_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatImplementsInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(IDummyInterface), "because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Must not be an interface Type.\r\nParameter name: baseType");
        }

        #endregion

        #region BeDecoratedWith

        [TestMethod]
        public void When_type_is_decorated_with_expected_attribute_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type typeWithAttribute = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeWithAttribute.Should().BeDecoratedWith<DummyClassAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_type_is_not_decorated_with_expected_attribute_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type typeWithoutAttribute = typeof (ClassWithoutAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeWithoutAttribute.Should().BeDecoratedWith<DummyClassAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_type_is_not_decorated_with_expected_attribute_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type typeWithoutAttribute = typeof (ClassWithoutAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeWithoutAttribute.Should().BeDecoratedWith<DummyClassAttribute>(
                    "because we want to test the error {0}",
                    "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassWithoutAttribute to be decorated with " +
                             "FluentAssertions.Specs.DummyClassAttribute because we want to test the error message, but the attribute " +
                             "was not found.");
        }

        [TestMethod]
        public void When_type_is_decorated_with_expected_attribute_with_the_expected_properties_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type typeWithAttribute = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeWithAttribute.Should()
                    .BeDecoratedWith<DummyClassAttribute>(a => ((a.Name == "Expected") && a.IsEnabled));

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_type_is_decorated_with_expected_attribute_that_has_an_unexpected_property_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type typeWithAttribute = typeof (ClassWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeWithAttribute.Should()
                    .BeDecoratedWith<DummyClassAttribute>(a => ((a.Name == "Unexpected") && a.IsEnabled));

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassWithAttribute to be decorated with " +
                             "FluentAssertions.Specs.DummyClassAttribute that matches ((a.Name == \"Unexpected\")*a.IsEnabled), " +
                             "but no matching attribute was found.");
        }

        [TestMethod]
        public void When_asserting_a_selection_of_decorated_types_is_decorated_with_an_attribute_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var types = new TypeSelector(new[]
            {
                typeof (ClassWithAttribute)
            });

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                types.Should().BeDecoratedWith<DummyClassAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_selection_of_non_decorated_types_is_decorated_with_an_attribute_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var types = new TypeSelector(new[]
            {
                typeof (ClassWithAttribute),
                typeof (ClassWithoutAttribute),
                typeof (OtherClassWithoutAttribute)
            });

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                types.Should().BeDecoratedWith<DummyClassAttribute>("because we do");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected all types to be decorated with FluentAssertions.Specs.DummyClassAttribute" +
                             " because we do, but the attribute was not found on the following types:\r\n" +
                             "FluentAssertions.Specs.ClassWithoutAttribute\r\n" +
                             "FluentAssertions.Specs.OtherClassWithoutAttribute");
        }


        [TestMethod]
        public void When_asserting_a_selection_of_types_with_unexpected_attribute_property_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var types = new TypeSelector(new[]
            {
                typeof (ClassWithAttribute),
                typeof (ClassWithoutAttribute),
                typeof (OtherClassWithoutAttribute)
            });

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                types.Should()
                    .BeDecoratedWith<DummyClassAttribute>(a => ((a.Name == "Expected") && a.IsEnabled), "because we do");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected all types to be decorated with FluentAssertions.Specs.DummyClassAttribute" +
                             " that matches ((a.Name == \"Expected\")*a.IsEnabled) because we do," +
                             " but no matching attribute was found on the following types:\r\n" +
                             "FluentAssertions.Specs.ClassWithoutAttribute\r\n" +
                             "FluentAssertions.Specs.OtherClassWithoutAttribute");
        }

        #endregion

        #region Implement

        [TestMethod]
        public void When_asserting_that_a_type_that_implements_an_interface_implements_that_interface_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof (ClassThatImplementsInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Implement(typeof (IDummyInterface));

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_not_implement_an_interface_implements_that_interface_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof (ClassThatDoesNotImplementInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Implement(typeof (IDummyInterface), "because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassThatDoesNotImplementInterface to implement " +
                             "interface FluentAssertions.Specs.IDummyInterface because we want to test the error message.");
        }

        [TestMethod]
        public void When_asserting_a_type_implements_a_NonInterface_type_Then_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatDoesNotImplementInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Implement(typeof(DateTime), "because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Must be an interface Type.\r\nParameter name: interfaceType");
        }

        #endregion

        #region NotImplement

        [TestMethod]
        public void When_asserting_that_a_type_that_does_not_implement_an_interface_does_not_implement_that_interface_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatDoesNotImplementInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotImplement(typeof(IDummyInterface));

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_implements_an_interface_does_not_implement_that_interface_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatImplementsInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotImplement(typeof(IDummyInterface), "because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassThatImplementsInterface to not implement interface " +
                             "FluentAssertions.Specs.IDummyInterface because we want to test the error message.");
        }

        [TestMethod]
        public void When_asserting_a_type_does_not_implement_a_NonInterface_type_Then_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatDoesNotImplementInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotImplement(typeof(DateTime), "because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Must be an interface Type.\r\nParameter name: interfaceType");
        }

        #endregion

        #region HaveProperty

        [TestMethod]
        public void When_asserting_a_type_that_has_a_property_does_have_a_named_property_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveProperty(typeof(string), "PrivateWriteProtectedReadProperty")
                    .Which.Should()
                        .BeWritable(CSharpAccessModifiers.Private)
                        .And.BeReadable(CSharpAccessModifiers.Protected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_not_have_a_property_does_have_a_property_with_that_name_and_type_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithNoMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveProperty(typeof(string), "PublicProperty", "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected String FluentAssertions.Specs.ClassWithNoMembers.PublicProperty to exist because we want to " +
                    "test the error message, but it does not.");
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_have_a_property_does_have_a_property_with_that_name_but_different_type_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveProperty(typeof(int), "PrivateWriteProtectedReadProperty", "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected String FluentAssertions.Specs.ClassWithMembers.PrivateWriteProtectedReadProperty to be of type System.Int32 because we want to test the error message, but it is not.");
        }

        #endregion

        #region NotHaveProperty

        [TestMethod]
        public void When_asserting_a_type_that_has_no_properties_does_not_have_a_named_property_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof (ClassWithoutMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotHaveProperty("Property");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void
            When_asserting_a_type_that_has_a_property_does_not_have_a_property_with_that_name_it_should_throw_with_descriptive_message
            ()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof (ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotHaveProperty("PrivateWriteProtectedReadProperty", "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected String FluentAssertions.Specs.ClassWithMembers.PrivateWriteProtectedReadProperty to not exist because we want to " +
                    "test the error message, but it does.");
        }

        #endregion

        #region HaveExplicitProperty

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_property_which_it_does_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof (IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "ExplicitStringProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_property_which_it_implements_implicitly_and_explicitly_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "ExplicitImplicitStringProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_property_which_it_implements_implicitly_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "ImplicitStringProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ImplicitStringProperty, but it does not.");
        }

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_property_which_it_does_not_implement_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "NonExistantProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.NonExistantProperty, but it does not.");
        }

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_property_from_an_unimplemented_interface_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IDummyInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty(interfaceType, "NonExistantProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected type FluentAssertions.Specs.ClassExplicitlyImplementingInterface to implement interface " +
                    "FluentAssertions.Specs.IDummyInterface.");
        }

        #endregion

        #region NotHaveExplicitProperty

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_property_which_it_does_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "ExplicitStringProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ExplicitStringProperty, but it does.");
        }

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_property_which_it_implements_implicitly_and_explicitly_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "ExplicitImplicitStringProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ExplicitImplicitStringProperty, but it does."); 
        }

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_property_which_it_implements_implicitly_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "ImplicitStringProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_property_which_it_does_not_implement_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "NonExistantProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_property_from_an_unimplemented_interface_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IDummyInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty(interfaceType, "NonExistantProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassExplicitlyImplementingInterface to implement interface " +
                             "FluentAssertions.Specs.IDummyInterface.");
        }

        #endregion

        #region HaveExplicitMethod

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_method_which_it_does_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "ExplicitMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_method_which_it_implements_implicitly_and_explicitly_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "ExplicitImplicitMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_method_which_it_implements_implicitly_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "ImplicitMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ImplicitMethod, but it does not.");
        }

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_method_which_it_does_not_implement_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "NonExistantMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.NonExistantMethod, but it does not.");
        }

        [TestMethod]
        public void When_asserting_that_a_type_explicitly_implements_a_method_from_an_unimplemented_interface_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IDummyInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod(interfaceType, "NonExistantProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected type FluentAssertions.Specs.ClassExplicitlyImplementingInterface to implement interface " +
                    "FluentAssertions.Specs.IDummyInterface.");
        }

        #endregion

        #region NotHaveExplicitProperty

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_method_which_it_does_it_should_fail_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "ExplicitMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ExplicitMethod, but it does.");
        }

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_method_which_it_implements_implicitly_and_explicitly_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "ExplicitImplicitMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ExplicitImplicitMethod, but it does.");
        }

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_method_which_it_implements_implicitly_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "ImplicitMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_method_which_it_does_not_implement_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IExplicitInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "NonExistantMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_that_a_type_does_not_explicitly_implement_a_method_from_an_unimplemented_interface_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            var interfaceType = typeof(IDummyInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod(interfaceType, "NonExistantMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassExplicitlyImplementingInterface to implement interface " +
                             "FluentAssertions.Specs.IDummyInterface.");
        }

        #endregion

        #region HaveIndexer

        [TestMethod]
        public void When_asserting_a_type_that_has_an_indexer_does_have_an_indexer_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveIndexer(typeof(string), new[] { typeof(string) })
                    .Which.Should()
                        .BeWritable(CSharpAccessModifiers.Internal)
                        .And.BeReadable(CSharpAccessModifiers.Private);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_not_have_an_indexer_does_have_an_indexer_with_that_type_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithNoMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveIndexer(typeof(string), new [] {typeof(int), typeof(Type)}, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected String FluentAssertions.Specs.ClassWithNoMembers[System.Int32, System.Type] to exist because we want to test the error" +
                    " message, but it does not.");
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_have_an_indexer_with_different_parameters_does_have_an_indexer_with_that_type_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveIndexer(typeof(string), new[] { typeof(int), typeof(Type) }, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected String FluentAssertions.Specs.ClassWithMembers[System.Int32, System.Type] to exist because we want to test the error" +
                    " message, but it does not.");
        }

        #endregion

        #region NotHaveIndexer

        [TestMethod]
        public void When_asserting_a_type_that_has_no_indexers_does_not_have_an_indexer_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithoutMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotHaveIndexer(new [] {typeof(string)});

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_has_an_indexer_does_not_have_an_indexer_with_those_parameters_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotHaveIndexer(new [] {typeof(string)}, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected indexer FluentAssertions.Specs.ClassWithMembers[System.String] to not exist because we want to " +
                    "test the error message, but it does.");
        }

        #endregion

        #region HaveConstructor

        [TestMethod]
        public void When_asserting_a_type_that_has_a_constructor_does_have_that_constructor_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveConstructor(new Type[] { typeof(string) })
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifiers.Private);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_not_have_a_constructor_does_have_that_constructor_with_the_specified_parameter_types_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithNoMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveConstructor(new[] { typeof(int), typeof(Type) }, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected constructor FluentAssertions.Specs.ClassWithNoMembers(System.Int32, System.Type) to exist because " +
                    "we want to test the error message, but it does not.");
        }

        #endregion

        #region HaveDefaultConstructor

        [TestMethod]
        public void When_asserting_a_type_that_has_a_default_constructor_does_have_that_default_constructor_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveDefaultConstructor()
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifiers.ProtectedInternal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_not_have_a_default_constructor_does_have_a_default_constructor_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithNoMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveDefaultConstructor("because the compiler generates one even if not explicitly defined.")
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifiers.Public);


            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region HaveMethod

        [TestMethod]
        public void When_asserting_a_type_that_has_an_method_does_have_that_method_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveMethod("VoidMethod", new Type[] { })
                    .Which.Should()
                        .HaveAccessModifier(CSharpAccessModifiers.Private)
                        .And.ReturnVoid();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_not_have_a_method_does_have_that_method_with_the_specified_parameter_types_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithNoMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveMethod("NonExistantMethod", new[] { typeof(int), typeof(Type) }, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected method FluentAssertions.Specs.ClassWithNoMembers.NonExistantMethod(System.Int32, System.Type) to exist " +
                    "because we want to test the error message, but it does not.");
        }

        [TestMethod]
        public void When_asserting_a_type_that_does_have_a_method_with_different_parameters_does_have_a_method_with_that_type_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveMethod("VoidMethod", new[] { typeof(int), typeof(Type) }, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected method FluentAssertions.Specs.ClassWithMembers.VoidMethod(System.Int32, System.Type) to exist " +
                    "because we want to test the error message, but it does not.");
        }

        #endregion

        #region NotHaveMethod

        [TestMethod]
        public void When_asserting_a_type_that_has_no_methods_does_not_have_a_method_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithoutMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotHaveMethod("NonExistantMethod", new Type[] {});

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_has_a_methods_does_not_have_an_overload_of_that_method_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotHaveMethod("VoidMethod", new [] { typeof(string) });

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_that_has_a_method_does_not_have_that_method_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotHaveMethod("VoidMethod", new Type[] {}, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMembers.VoidMethod() to not exist because we want to " +
                    "test the error message, but it does.");
        }

        #endregion
    }

    #region Internal classes used in unit tests

    [DummyClass("Expected", true)]
    public class ClassWithAttribute
    {
    }

    public class ClassWithoutAttribute
    {
    }

    public class OtherClassWithoutAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DummyClassAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        public DummyClassAttribute(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }
    }

    public interface IDummyInterface
    {
    }

    public class ClassThatImplementsInterface : IDummyInterface
    {
    }

    public class ClassThatDoesNotImplementInterface
    {
    }

    public class ClassWithMembers
    {
        protected internal ClassWithMembers() { }
        private ClassWithMembers(String str) { }
        protected string PrivateWriteProtectedReadProperty { get { return null; } private set { } }
        internal string this[string str] { private get { return str; } set { } }
        protected internal string this[int i] { get { return i.ToString(); } private set { } }
        private void VoidMethod() { }
    }

    public class ClassExplicitlyImplementingInterface : IExplicitInterface
    {
        public string ImplicitStringProperty { get { return null; } private set { } }
        string IExplicitInterface.ExplicitStringProperty { set { } }
        public string ExplicitImplicitStringProperty { get; set; }
        string IExplicitInterface.ExplicitImplicitStringProperty { get; set; }
        public void ImplicitMethod() { } 
        void IExplicitInterface.ExplicitMethod() { }
        public void ExplicitImplicitMethod() { }
        void IExplicitInterface.ExplicitImplicitMethod() { }
    }

    public interface IExplicitInterface
    {
        string ImplicitStringProperty { get; }
        string ExplicitStringProperty { set; }
        string ExplicitImplicitStringProperty { get; set; }
        void ImplicitMethod();
        void ExplicitMethod();
        void ExplicitImplicitMethod();
    }

    public class ClassWithoutMembers { }

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
