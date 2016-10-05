using System;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
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
        public void When_type_is_equal_to_the_same_type_it_succeeds()
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
        public void When_type_is_equal_to_another_type_it_fails()
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
        public void When_type_is_equal_to_another_type_it_fails_with_a_useful_message()
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
        public void When_asserting_equality_of_a_type_but_the_type_is_null_it_fails()
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
                nullType.Should().Be(someType, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be FluentAssertions.Specs.ClassWithAttribute" +
                    " because we want to test the error message, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_equality_of_a_type_with_null_it_fails()
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
                someType.Should().Be(nullType, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be <null>" +
                    " because we want to test the error message, but found FluentAssertions.Specs.ClassWithAttribute.");
        }

        [TestMethod]
        public void When_type_is_equal_to_same_type_from_different_assembly_it_fails_with_assembly_qualified_name
            ()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

            Type typeFromThisAssembly = typeof (ObjectAssertions);
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
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
        public void When_type_is_equal_to_the_same_type_using_generics_it_succeeds()
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
        public void When_type_is_equal_to_another_type_using_generics_it_fails()
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
        public void When_type_is_equal_to_another_type_using_generics_it_fails_with_a_useful_message()
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
        public void When_type_is_not_equal_to_the_another_type_it_succeeds()
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
        public void When_type_is_not_equal_to_the_same_type_it_fails()
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
        public void When_type_is_not_equal_to_the_same_type_it_fails_with_a_useful_message()
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
                             " because we want to test the error message, but it is.");
        }

        [TestMethod]
        public void When_type_is_not_equal_to_another_type_using_generics_it_succeeds()
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
        public void When_type_is_not_equal_to_the_same_type_using_generics_it_fails()
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
        public void When_type_is_not_equal_to_the_same_type_using_generics_it_fails_with_a_useful_message()
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
                    "the error message, but it is.");
        }

        #endregion

        #region BeAssignableTo

        [TestMethod]
        public void When_its_own_type_it_succeeds()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof (DummyImplementingClass).Should().BeAssignableTo<DummyImplementingClass>();
        }

        [TestMethod]
        public void When_its_base_type_it_succeeds()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof (DummyImplementingClass).Should().BeAssignableTo<DummyBaseClass>();
        }

        [TestMethod]
        public void When_implemented_interface_type_it_succeeds()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof (DummyImplementingClass).Should().BeAssignableTo<IDisposable>();
        }

        [TestMethod]
        public void When_an_unrelated_type_it_fails_with_a_useful_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Type someType = typeof (DummyImplementingClass);
            Action act = () => someType.Should().BeAssignableTo<DateTime>("because we want to test the failure {0}", "message");
            
            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage($"*{typeof (DummyImplementingClass)} to be assignable to {typeof (DateTime)}*failure message*");
        }

        [TestMethod]
        public void When_its_own_type_instance_it_succeeds()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof(DummyImplementingClass).Should().BeAssignableTo(typeof(DummyImplementingClass));
        }

        [TestMethod]
        public void When_its_base_type_instance_it_succeeds()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof(DummyImplementingClass).Should().BeAssignableTo(typeof(DummyBaseClass));
        }

        [TestMethod]
        public void When_an_implemented_interface_type_instance_it_succeeds()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            typeof(DummyImplementingClass).Should().BeAssignableTo(typeof(IDisposable));
        }

        [TestMethod]
        public void When_an_unrelated_type_instance_it_fails_with_a_useful_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Type someType = typeof(DummyImplementingClass);
            Action act = () => someType.Should().BeAssignableTo(typeof(DateTime), "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage($"*{typeof(DummyImplementingClass)} to be assignable to {typeof(DateTime)}*failure message*");
        }

        #endregion

        #region BeDerivedFrom

        [TestMethod]
        public void When_asserting_a_type_is_derived_from_its_base_class_it_succeeds()
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
        public void When_asserting_a_type_is_derived_from_an_unrelated_class_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(DummyBaseClass);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(ClassWithMembers), "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.DummyBaseClass to be derived from " +
                             "FluentAssertions.Specs.ClassWithMembers because we want to test the error message, but it is not.");
        }

        [TestMethod]
        public void When_asserting_a_type_is_derived_from_an_interface_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatImplementsInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().BeDerivedFrom(typeof(IDummyInterface), "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Must not be an interface Type.\r\nParameter name: baseType");
        }

        #endregion

        #region BeDerivedFromOfT

        [TestMethod]
        public void When_asserting_a_type_is_DerivedFromOfT_its_base_class_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(DummyImplementingClass);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().BeDerivedFrom<DummyBaseClass>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region BeDecoratedWith

        [TestMethod]
        public void When_type_is_decorated_with_expected_attribute_it_succeeds()
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
        public void When_type_is_not_decorated_with_expected_attribute_it_fails()
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
        public void When_type_is_not_decorated_with_expected_attribute_it_fails_with_a_useful_message()
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
        public void When_type_is_decorated_with_expected_attribute_with_the_expected_properties_it_succeeds()
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
        public void When_type_is_decorated_with_expected_attribute_that_has_an_unexpected_property_it_fails()
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
        public void When_asserting_a_selection_of_decorated_types_is_decorated_with_an_attribute_it_succeeds()
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
        public void When_asserting_a_selection_of_non_decorated_types_is_decorated_with_an_attribute_it_fails()
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
        public void When_asserting_a_selection_of_types_with_unexpected_attribute_property_it_fails()
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
        public void When_asserting_a_type_implements_an_interface_which_it_does_then_it_succeeds()
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
        public void When_asserting_a_type_does_not_implement_an_interface_which_it_does_then_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof (ClassThatDoesNotImplementInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Implement(typeof (IDummyInterface), "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassThatDoesNotImplementInterface to implement " +
                             "interface FluentAssertions.Specs.IDummyInterface because we want to test the error message, " +
                             "but it does not.");
        }

        [TestMethod]
        public void When_asserting_a_type_implements_a_NonInterface_type_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatDoesNotImplementInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Implement(typeof(DateTime), "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Must be an interface Type.\r\nParameter name: interfaceType");
        }

        #endregion

        #region ImplementOfT

        [TestMethod]
        public void When_asserting_a_type_implementsOfT_an_interface_which_it_does_then_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatImplementsInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Implement<IDummyInterface>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region NotImplement

        [TestMethod]
        public void When_asserting_a_type_does_not_implement_an_interface_which_it_does_not_then_it_succeeds()
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
        public void When_asserting_a_type_implements_an_interface_which_it_does_not_then_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatImplementsInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotImplement(typeof(IDummyInterface), "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassThatImplementsInterface to not implement interface " +
                             "FluentAssertions.Specs.IDummyInterface because we want to test the error message, but it does.");
    }

        [TestMethod]
        public void When_asserting_a_type_does_not_implement_a_NonInterface_type_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatDoesNotImplementInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotImplement(typeof(DateTime), "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Must be an interface Type.\r\nParameter name: interfaceType");
        }

        #endregion

        #region NotImplementOfT

        [TestMethod]
        public void When_asserting_a_type_does_not_implementOfT_an_interface_which_it_does_not_then_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassThatDoesNotImplementInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotImplement<IDummyInterface>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region HaveProperty

        [TestMethod]
        public void When_asserting_a_type_has_a_property_which_it_does_then_it_succeeds()
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
                        .BeWritable(CSharpAccessModifier.Private)
                        .And.BeReadable(CSharpAccessModifier.Protected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_has_a_property_which_it_does_not_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_has_a_property_which_it_has_with_a_different_type_it_fails_with_a_useful_message()
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

        #region HavePropertyOfT

        [TestMethod]
        public void When_asserting_a_type_has_a_propertyOfT_which_it_does_then_it_succeeds()
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
                    .HaveProperty<string>("PrivateWriteProtectedReadProperty")
                    .Which.Should()
                        .BeWritable(CSharpAccessModifier.Private)
                        .And.BeReadable(CSharpAccessModifier.Protected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region NotHaveProperty

        [TestMethod]
        public void When_asserting_a_type_does_not_have_a_property_which_it_does_not_it_succeeds()
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
        public void When_asserting_a_type_does_not_have_a_property_which_it_does_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_explicitly_implements_a_property_which_it_does_it_succeeds()
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
        public void When_asserting_a_type_explicitly_implements_a_property_which_it_implements_implicitly_and_explicitly_it_succeeds()
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
        public void When_asserting_a_type_explicitly_implements_a_property_which_it_implements_implicitly_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_explicitly_implements_a_property_which_it_does_not_implement_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_explicitly_implements_a_property_from_an_unimplemented_interface_it_fails_with_a_useful_message()
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
                    "FluentAssertions.Specs.IDummyInterface, but it does not.");
        }

        #endregion

        #region HaveExplicitPropertyOfT

        [TestMethod]
        public void When_asserting_a_type_explicitlyOfT_implements_a_property_which_it_does_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitProperty<IExplicitInterface>("ExplicitStringProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }
        
        #endregion

        #region NotHaveExplicitProperty

        [TestMethod]
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_which_it_does_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_which_it_implements_implicitly_and_explicitly_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_which_it_implements_implicitly_it_succeeds()
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
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_which_it_does_not_implement_it_succeeds()
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
        public void When_asserting_a_type_does_not_explicitly_implement_a_property_from_an_unimplemented_interface_it_succeeds()
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
                             "FluentAssertions.Specs.IDummyInterface, but it does not.");
        }

        #endregion

        #region NotHaveExplicitPropertyOfT

        [TestMethod]
        public void When_asserting_a_type_does_not_explicitlyOfT_implement_a_property_which_it_does_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitProperty<IExplicitInterface>("ExplicitStringProperty");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ExplicitStringProperty, but it does.");
        }

        #endregion
        
        #region HaveExplicitMethod

        [TestMethod]
        public void When_asserting_a_type_explicitly_implements_a_method_which_it_does_it_succeeds()
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
                    .HaveExplicitMethod(interfaceType, "ExplicitMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_explicitly_implements_a_method_which_it_implements_implicitly_and_explicitly_it_succeeds()
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
                    .HaveExplicitMethod(interfaceType, "ExplicitImplicitMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_explicitly_implements_a_method_which_it_implements_implicitly_it_fails_with_a_useful_message()
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
                    .HaveExplicitMethod(interfaceType, "ImplicitMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ImplicitMethod, but it does not.");
        }

        [TestMethod]
        public void When_asserting_a_type_explicitly_implements_a_method_which_it_does_not_implement_it_fails_with_a_useful_message()
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
                    .HaveExplicitMethod(interfaceType, "NonExistantMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.NonExistantMethod, but it does not.");
        }

        [TestMethod]
        public void When_asserting_a_type_explicitly_implements_a_method_from_an_unimplemented_interface_it_fails_with_a_useful_message()
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
                    .HaveExplicitMethod(interfaceType, "NonExistantProperty", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected type FluentAssertions.Specs.ClassExplicitlyImplementingInterface to implement interface " +
                    "FluentAssertions.Specs.IDummyInterface, but it does not.");
        }

        #endregion

        #region HaveExplicitMethodOfT

        [TestMethod]
        public void When_asserting_a_type_explicitly_implementsOfT_a_method_which_it_does_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .HaveExplicitMethod<IExplicitInterface>("ExplicitMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region NotHaveExplicitMethod

        [TestMethod]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_does_it_fails_with_a_useful_message()
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
                    .NotHaveExplicitMethod(interfaceType, "ExplicitMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ExplicitMethod, but it does.");
        }

        [TestMethod]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_implements_implicitly_and_explicitly_it_fails_with_a_useful_message()
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
                    .NotHaveExplicitMethod(interfaceType, "ExplicitImplicitMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ExplicitImplicitMethod, but it does.");
        }

        [TestMethod]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_implements_implicitly_it_succeeds()
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
                    .NotHaveExplicitMethod(interfaceType, "ImplicitMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_which_it_does_not_implement_it_succeeds()
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
                    .NotHaveExplicitMethod(interfaceType, "NonExistantMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_does_not_explicitly_implement_a_method_from_an_unimplemented_interface_it_succeeds()
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
                    .NotHaveExplicitMethod(interfaceType, "NonExistantMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassExplicitlyImplementingInterface to implement interface " +
                             "FluentAssertions.Specs.IDummyInterface, but it does not.");
        }

        #endregion

        #region NotHaveExplicitMethodOfT

        [TestMethod]
        public void When_asserting_a_type_does_not_explicitly_implementOfT_a_method_which_it_does_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassExplicitlyImplementingInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should()
                    .NotHaveExplicitMethod< IExplicitInterface>("ExplicitMethod", Enumerable.Empty<Type>());

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected FluentAssertions.Specs.ClassExplicitlyImplementingInterface to not explicitly implement " +
                    "FluentAssertions.Specs.IExplicitInterface.ExplicitMethod, but it does.");
        }

        #endregion
        
        #region HaveIndexer

        [TestMethod]
        public void When_asserting_a_type_has_an_indexer_which_it_does_then_it_succeeds()
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
                        .BeWritable(CSharpAccessModifier.Internal)
                        .And.BeReadable(CSharpAccessModifier.Private);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_has_an_indexer_which_it_does_not_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_has_an_indexer_with_different_parameters_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_does_not_have_an_indexer_which_it_does_not_it_succeeds()
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
        public void When_asserting_a_type_does_not_have_an_indexer_which_it_does_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_has_a_constructor_which_it_does_it_succeeds()
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
                        .HaveAccessModifier(CSharpAccessModifier.Private);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_has_a_constructor_which_it_does_not_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_has_a_default_constructor_which_it_does_it_succeeds()
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
                        .HaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_has_a_default_constructor_which_it_does_not_it_succeeds()
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
                        .HaveAccessModifier(CSharpAccessModifier.Public);


            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        #endregion

        #region HaveMethod

        [TestMethod]
        public void When_asserting_a_type_has_a_method_which_it_does_it_succeeds()
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
                        .HaveAccessModifier(CSharpAccessModifier.Private)
                        .And.ReturnVoid();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_has_a_method_which_it_does_not_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_has_a_method_with_different_parameter_types_it_fails_with_a_useful_message()
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
        public void When_asserting_a_type_does_not_have_a_method_which_it_does_not_it_succeeds()
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
        public void When_asserting_a_type_does_not_have_a_method_which_it_has_with_different_parameter_types_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var type = typeof(ClassWithMembers);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotHaveMethod("VoidMethod", new [] { typeof(int) });

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_type_does_not_have_that_method_which_it_does_it_fails_with_a_useful_message()
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

        #region HaveAccessModifier
        
        [TestMethod]
        public void When_asserting_a_public_type_is_public_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(IPublicInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_public_member_is_not_public_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(IPublicInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type IPublicInterface to be Internal because we want to test the error message, but it " +
                             "is Public.");
        }

        [TestMethod]
        public void When_asserting_an_internal_type_is_internal_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(InternalClass);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_internal_type_is_not_internal_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(InternalClass);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal, "because we want to test the" +
                                                                                                " error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type InternalClass to be ProtectedInternal because we want to test the error message, " +
                             "but it is Internal.");
        }
        
        [TestMethod]
        public void When_asserting_a_nested_private_type_is_private_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);
#else
            Type type = typeof(Nested).GetTypeInfo().DeclaredNestedTypes.First(t => t.Name == "PrivateClass").AsType();
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Private);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_nested_private_type_is_not_private_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);
#else
            Type type = typeof(Nested).GetTypeInfo().DeclaredNestedTypes.First(t => t.Name == "PrivateClass").AsType();
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Protected, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type PrivateClass to be Protected because we want to test the error message, but it " +
                             "is Private.");
        }

        [TestMethod]
        public void When_asserting_a_nested_protected_type_is_protected_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);
#else
            Type type = typeof(Nested).GetTypeInfo().DeclaredNestedTypes.First(t => t.Name == "ProtectedEnum").AsType();
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Protected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_nested_protected_type_is_not_protected_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);
#else
            Type type = typeof(Nested).GetTypeInfo().DeclaredNestedTypes.First(t => t.Name == "ProtectedEnum").AsType();
#endif
            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type ProtectedEnum to be Public, but it is Protected.");
        }

        [TestMethod]
        public void When_asserting_a_nested_public_type_is_public_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(Nested.IPublicInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_nested_public_member_is_not_public_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(Nested.IPublicInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type IPublicInterface to be Internal because we want to test the error message, " +
                             "but it is Public.");
        }

        [TestMethod]
        public void When_asserting_a_nested_internal_type_is_internal_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(Nested.InternalClass);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_nested_internal_type_is_not_internal_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(Nested.InternalClass);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal, "because we want to test the" +
                                                                                                " error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type InternalClass to be ProtectedInternal because we want to test the error message, " +
                             "but it is Internal.");
        }

        [TestMethod]
        public void When_asserting_a_nested_protected_internal_member_is_protected_internal_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(Nested.IProtectedInternalInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_nested_protected_internal_member_is_not_protected_internal_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(Nested.IProtectedInternalInterface);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Private, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type IProtectedInternalInterface to be Private because we want to test the error " +
                             "message, but it is ProtectedInternal.");
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
        private ClassWithMembers(String overload) { }
        protected string PrivateWriteProtectedReadProperty { get { return null; } private set { } }
        internal string this[string str] { private get { return str; } set { } }
        protected internal string this[int i] { get { return i.ToString(); } private set { } }
        private void VoidMethod() { }
        private void VoidMethod(string overload) { }
    }

    public class ClassExplicitlyImplementingInterface : IExplicitInterface
    {
        public string ImplicitStringProperty { get { return null; } private set { } }
        string IExplicitInterface.ExplicitStringProperty { set { } }
        public string ExplicitImplicitStringProperty { get; set; }
        string IExplicitInterface.ExplicitImplicitStringProperty { get; set; }
        public void ImplicitMethod() { }
        public void ImplicitMethod(string overload) { } 
        void IExplicitInterface.ExplicitMethod() { }
        void IExplicitInterface.ExplicitMethod(string overload) { }
        public void ExplicitImplicitMethod() { }
        public void ExplicitImplicitMethod(string overload) { }
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

    class Nested
    {
        class PrivateClass { }

        protected enum ProtectedEnum { }

        public interface IPublicInterface { }

        internal class InternalClass { }

        protected internal interface IProtectedInternalInterface { }
    }

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
