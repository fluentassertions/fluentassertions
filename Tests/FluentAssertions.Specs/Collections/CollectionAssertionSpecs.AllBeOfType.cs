using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The AllBeOfType specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class AllBeOfType
    {
        [Fact]
        public void When_the_types_in_a_collection_is_matched_against_a_null_type_exactly_it_should_throw()
        {
            // Arrange
            var collection = Array.Empty<int>();

            // Act
            Action act = () => collection.Should().AllBeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("expectedType");
        }

        [Fact]
        public void All_items_in_an_empty_collection_are_of_a_generic_type()
        {
            // Arrange
            var collection = Array.Empty<int>();

            // Act / Assert
            collection.Should().AllBeOfType<int>();
        }

        [Fact]
        public void All_items_in_an_empty_collection_are_of_a_type()
        {
            // Arrange
            var collection = Array.Empty<int>();

            // Act / Assert
            collection.Should().AllBeOfType(typeof(int));
        }

        [Fact]
        public void When_collection_is_null_then_all_be_of_type_should_fail()
        {
            // Arrange
            IEnumerable<object> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().AllBeOfType(typeof(object), "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be \"*.Object\" *failure message*, but found collection is <null>.");
        }

        [Fact]
        public void When_all_of_the_types_in_a_collection_match_expected_type_exactly_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().AllBeOfType(typeof(int));
        }

        [Fact]
        public void When_all_of_the_types_in_a_collection_match_expected_generic_type_exactly_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().AllBeOfType<int>();
        }

        [Fact]
        public void When_matching_a_collection_against_an_exact_type_it_should_return_the_casted_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().AllBeOfType<int>()
                .Which.Should().Equal(1, 2, 3);
        }

        [Fact]
        public void When_one_of_the_types_does_not_match_exactly_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            var collection = new object[] { new Exception(), new ArgumentException("foo") };

            // Act
            Action act = () => collection.Should().AllBeOfType(typeof(Exception), "because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Exception\" because they are of different type, but found \"[System.Exception, System.ArgumentException]\".");
        }

        [Fact]
        public void When_one_of_the_types_does_not_match_exactly_the_generic_type_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            var collection = new object[] { new Exception(), new ArgumentException("foo") };

            // Act
            Action act = () => collection.Should().AllBeOfType<Exception>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Exception\" because they are of different type, but found \"[System.Exception, System.ArgumentException]\".");
        }

        [Fact]
        public void When_one_of_the_elements_is_null_for_an_exact_match_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            var collection = new object[] { 1, null, 3 };

            // Act
            Action act = () => collection.Should().AllBeOfType<int>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found a null element.");
        }

        [Fact]
        public void When_collection_of_types_match_expected_type_exactly_it_should_succeed()
        {
            // Arrange
            var collection = new[] { typeof(int), typeof(int), typeof(int) };

            // Act / Assert
            collection.Should().AllBeOfType<int>();
        }

        [Fact]
        public void When_collection_of_types_and_objects_match_type_exactly_it_should_succeed()
        {
            // Arrange
            var collection = new object[] { typeof(ArgumentException), new ArgumentException("foo") };

            // Act / Assert
            collection.Should().AllBeOfType<ArgumentException>();
        }

        [Fact]
        public void When_collection_of_types_and_objects_do_not_match_type_exactly_it_should_throw()
        {
            // Arrange
            var collection = new object[] { typeof(Exception), new ArgumentException("foo") };

            // Act
            Action act = () => collection.Should().AllBeOfType<ArgumentException>();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.ArgumentException\", but found \"[System.Exception, System.ArgumentException]\".");
        }

        [Fact]
        public void When_collection_is_null_then_all_be_of_typeOfT_should_fail()
        {
            // Arrange
            IEnumerable<object> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().AllBeOfType<object>("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be \"*.Object\" *failure message*, but found collection is <null>.");
        }
    }
}
