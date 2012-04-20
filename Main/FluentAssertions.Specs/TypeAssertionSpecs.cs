using System;

#if WINRT
using System.Reflection;
#endif
using FluentAssertions.Primitives;
using FluentAssertions.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void When_type_is_equal_to_same_type_from_different_assembly_it_should_throw_with_assembly_qualified_name()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

            Type typeFromThisAssembly = typeof(ObjectAssertions);
#if !WINRT
            Type typeFromOtherAssembly = typeof(TypeAssertions).Assembly.GetType("FluentAssertions.Primitives.ObjectAssertions");
#else
            Type typeFromOtherAssembly =
                typeof (TypeAssertions).GetTypeInfo().Assembly.GetType("FluentAssertions.Assertions.ObjectAssertions");
#endif


#pragma warning restore 436

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeFromThisAssembly.Should().Be(typeFromOtherAssembly, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            const string expectedMessage =
                "Expected type to be [FluentAssertions.Primitives.ObjectAssertions, FluentAssertions*]" +
                    " because we want to test the error message, but found " +
                        "[FluentAssertions.Primitives.ObjectAssertions, FluentAssertions*].";

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage, ComparisonMode.Wildcard);
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
                .WithMessage("Expected type to be FluentAssertions.Specs.ClassWithoutAttribute because we want to test " +
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
                    " because we want to test the error message.", ComparisonMode.Wildcard);
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
                .WithMessage("Expected type not to be [FluentAssertions.Specs.ClassWithAttribute*] because we want to test " +
                    "the error message.", ComparisonMode.Wildcard);
        }

        #endregion

        #region BeDecoratedWith

        [TestMethod]
        public void When_type_is_decorated_with_a_specific_attribute_and_it_is_it_should_succeed()
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
        public void When_type_is_decorated_with_an_attribute_and_it_is_not_it_should_throw()
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
        public void When_type_is_decorated_with_an_attribute_and_it_is_not_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type typeWithoutAttribute = typeof (ClassWithoutAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeWithoutAttribute.Should().BeDecoratedWith<DummyClassAttribute>("because we want to test the error {0}",
                    "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type FluentAssertions.Specs.ClassWithoutAttribute to be decorated with " +
                    "FluentAssertions.Specs.DummyClassAttribute because we want to test the error message, but the attribute " +
                        "was not found.");
        }

        #endregion
    }

    #region Internal classes used in unit tests

    [DummyClass]
    public class ClassWithAttribute
    {
    }

    public class ClassWithoutAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DummyClassAttribute : Attribute
    {
    }

    #endregion
}

namespace FluentAssertions.Primitives
{
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

    /// <summary>
    /// A class that intentianalty has the exact same name and namespace as the ObjectAssertions from the FluentAssertions
    /// assembly. This class is used to test the behavior of comparisons on such types.
    /// </summary>
    internal class ObjectAssertions
    {
    }

#pragma warning restore 436
}