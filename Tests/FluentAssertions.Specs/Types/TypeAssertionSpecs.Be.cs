using System;
using AssemblyA;
using AssemblyB;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]Be specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class Be
    {
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

            Type typeFromThisAssembly = typeof(ClassC);

            Type typeFromOtherAssembly =
                new ClassA().ReturnClassC().GetType();

#pragma warning restore 436

            // Act
            Action act = () =>
                typeFromThisAssembly.Should().Be(typeFromOtherAssembly, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to be [AssemblyB.ClassC, AssemblyB*] *failure message*, but found [AssemblyB.ClassC, FluentAssertionsAsync.Specs*].");
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
    }

    public class NotBe
    {
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
    }
}
