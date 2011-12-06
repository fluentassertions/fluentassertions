using System;

using FluentAssertions.Assertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
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
            Type type = typeof (ClasWithAttribute);
            Type sameType = typeof (ClasWithAttribute);

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
            Type type = typeof (ClasWithAttribute);
            Type differentType = typeof (ClasWithoutAttribute);

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
            Type type = typeof (ClasWithAttribute);
            Type differentType = typeof (ClasWithoutAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be(differentType, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------

            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be FluentAssertions.specs.ClasWithoutAttribute" +
                    " because we want to test the error message, but found FluentAssertions.specs.ClasWithAttribute.");
        }

        [TestMethod]
        public void When_type_is_equal_to_same_type_from_different_assembly_it_should_throw_with_assembly_qualified_name()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type typeFromThisAssembly = typeof (ObjectAssertions);
            Type typeFromOtherAssembly = typeof (TypeAssertions).Assembly.GetType("FluentAssertions.Assertions.ObjectAssertions");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                typeFromThisAssembly.Should().Be(typeFromOtherAssembly, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            const string expectedMessage =
                "Expected type to be [FluentAssertions.Assertions.ObjectAssertions, FluentAssertions, Version=*]" +
                    " because we want to test the error message, but found " +
                        "[FluentAssertions.Assertions.ObjectAssertions, FluentAssertions.Specs, Version=*].";

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage, ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_type_is_equal_to_the_same_type_using_generics_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClasWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be<ClasWithAttribute>();

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
            Type type = typeof (ClasWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be<ClasWithoutAttribute>();

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
            Type type = typeof (ClasWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().Be<ClasWithoutAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type to be FluentAssertions.specs.ClasWithoutAttribute because we want to test " +
                    "the error message, but found FluentAssertions.specs.ClasWithAttribute.");
        }

        #endregion

        #region NotBe

        [TestMethod]
        public void When_type_is_not_equal_to_the_another_type_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClasWithAttribute);
            Type otherType = typeof (ClasWithoutAttribute);

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
            Type type = typeof (ClasWithAttribute);
            Type sameType = typeof (ClasWithAttribute);

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
            Type type = typeof (ClasWithAttribute);
            Type sameType = typeof (ClasWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe(sameType, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type not to be [FluentAssertions.specs.ClasWithAttribute*]" +
                    " because we want to test the error message.", ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_type_is_not_equal_to_another_type_using_generics_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof (ClasWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe<ClasWithoutAttribute>();

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
            Type type = typeof (ClasWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe<ClasWithAttribute>();

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
            Type type = typeof (ClasWithAttribute);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                type.Should().NotBe<ClasWithAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type not to be [FluentAssertions.specs.ClasWithAttribute*] because we want to test " +
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
            Type typeWithAttribute = typeof (ClasWithAttribute);

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
            Type typeWithoutAttribute = typeof (ClasWithoutAttribute);

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
            Type typeWithoutAttribute = typeof (ClasWithoutAttribute);

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
                .WithMessage("Expected type FluentAssertions.specs.ClasWithoutAttribute to be decorated with " +
                    "FluentAssertions.specs.DummyClassAttribute because we want to test the error message, but the attribute " +
                        "was not found.");
        }

        #endregion
    }

    #region Internal classes used in unit tests

    [DummyClass]
    public class ClasWithAttribute
    {
    }

    public class ClasWithoutAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DummyClassAttribute : Attribute
    {
    }

    #endregion
}

namespace FluentAssertions.Assertions
{
    internal class ObjectAssertions
    {
    }
}