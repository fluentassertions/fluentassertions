using System;
using AssemblyA;
using AssemblyB;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeOfType
    {
        [Fact]
        public void When_object_type_is_matched_against_null_type_exactly_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("expectedType");
        }

        [Fact]
        public void When_object_type_is_exactly_equal_to_the_specified_type_it_should_not_fail()
        {
            // Arrange
            var someObject = new Exception();

            // Act
            Action act = () => someObject.Should().BeOfType<Exception>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_object_type_is_value_type_and_matches_received_type_should_not_fail_and_assert_correctly()
        {
            // Arrange
            int valueTypeObject = 42;

            // Act
            Action act = () => valueTypeObject.Should().BeOfType(typeof(int));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_object_is_matched_against_a_null_type_it_should_throw()
        {
            // Arrange
            int valueTypeObject = 42;

            // Act
            Action act = () => valueTypeObject.Should().BeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("expectedType");
        }

        [Fact]
        public void When_null_object_is_matched_against_a_type_it_should_throw()
        {
            // Arrange
            int? valueTypeObject = null;

            // Act
            Action act = () =>
                valueTypeObject.Should().BeOfType(typeof(int), "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*type to be System.Int32*because we want to test the failure message*");
        }

        [Fact]
        public void When_object_type_is_value_type_and_doesnt_match_received_type_should_fail()
        {
            // Arrange
            int valueTypeObject = 42;
            var doubleType = typeof(double);

            // Act
            Action act = () => valueTypeObject.Should().BeOfType(doubleType);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected type to be {doubleType}, but found {valueTypeObject.GetType()}.");
        }

        [Fact]
        public void When_object_is_of_the_expected_type_it_should_cast_the_returned_object_for_chaining()
        {
            // Arrange
            var someObject = new Exception("Actual Message");

            // Act
            Action act = () => someObject.Should().BeOfType<Exception>().Which.Message.Should().Be("Other Message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Expected*Actual*Other*");
        }

        [Fact]
        public void When_object_type_is_different_than_expected_type_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeOfType<int>("because they are {0} {1}", "of different", "type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be System.Int32 because they are of different type, but found System.Object.");
        }

        [Fact]
        public void When_asserting_the_type_of_a_null_object_it_should_throw()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () => someObject.Should().BeOfType<int>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected someObject to be System.Int32, but found <null>.");
        }

        [Fact]
        public void
            When_object_type_is_same_as_expected_type_but_in_different_assembly_it_should_fail_with_assembly_qualified_name()
        {
            // Arrange
            var typeFromOtherAssembly =
                new ClassA().ReturnClassC();

            // Act
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

            Action act = () =>
                typeFromOtherAssembly.Should().BeOfType<ClassC>();

#pragma warning restore 436

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type to be [AssemblyB.ClassC, FluentAssertionsAsync.Specs*], but found [AssemblyB.ClassC, AssemblyB*].");
        }

        [Fact]
        public void When_object_type_is_a_subclass_of_the_expected_type_it_should_fail()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act
            Action act = () => someObject.Should().BeOfType<DummyBaseClass>();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be FluentAssertionsAsync*DummyBaseClass, but found FluentAssertionsAsync*DummyImplementingClass.");
        }
    }

    public class NotBeOfType
    {
        [Fact]
        public void When_object_type_is_matched_negatively_against_null_type_exactly_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().NotBeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("unexpectedType");
        }

        [Fact]
        public void When_object_is_matched_negatively_against_a_null_type_it_should_throw()
        {
            // Arrange
            int valueTypeObject = 42;

            // Act
            Action act = () => valueTypeObject.Should().NotBeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("unexpectedType");
        }

        [Fact]
        public void
            When_object_type_is_value_type_and_doesnt_match_received_type_as_expected_should_not_fail_and_assert_correctly()
        {
            // Arrange
            int valueTypeObject = 42;

            // Act
            Action act = () => valueTypeObject.Should().NotBeOfType(typeof(double));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_null_object_is_matched_negatively_against_a_type_it_should_throw()
        {
            // Arrange
            int? valueTypeObject = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                valueTypeObject.Should().NotBeOfType(typeof(int), "because we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*type not to be System.Int32*because we want to test the failure message*");
        }

        [Fact]
        public void When_object_type_is_value_type_and_matches_received_type_not_as_expected_should_fail()
        {
            // Arrange
            int valueTypeObject = 42;
            var expectedType = typeof(int);

            // Act
            Action act = () => valueTypeObject.Should().NotBeOfType(expectedType);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected type not to be [{expectedType.AssemblyQualifiedName}], but it is.");
        }
    }
}
