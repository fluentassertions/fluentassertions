using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

/// <content>
/// The AllBeAssignableTo specs.
/// </content>
public partial class CollectionAssertionSpecs
{
    public class AllBeAssignableTo
    {
        [Fact]
        public void When_the_types_in_a_collection_is_matched_against_a_null_type_it_should_throw()
        {
            // Arrange
            var collection = Array.Empty<int>();

            // Act
            Action act = () => collection.Should().AllBeAssignableTo(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("expectedType");
        }

        [Fact]
        public void All_items_in_an_empty_collection_are_assignable_to_a_generic_type()
        {
            // Arrange
            var collection = Array.Empty<int>();

            // Act / Assert
            collection.Should().AllBeAssignableTo<int>();
        }

        [Fact]
        public void When_collection_is_null_then_all_be_assignable_to_should_fail()
        {
            // Arrange
            IEnumerable<object> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().AllBeAssignableTo(typeof(object), "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be \"*.Object\" *failure message*, but found collection is <null>.");
        }

        [Fact]
        public void All_items_in_an_empty_collection_are_assignable_to_a_type()
        {
            // Arrange
            var collection = Array.Empty<int>();

            // Act / Assert
            collection.Should().AllBeAssignableTo(typeof(int));
        }

        [Fact]
        public void When_all_of_the_types_in_a_collection_match_expected_type_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().AllBeAssignableTo(typeof(int));
        }

        [Fact]
        public void When_all_of_the_types_in_a_collection_match_expected_generic_type_it_should_succeed()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().AllBeAssignableTo<int>();
        }

        [Fact]
        public void When_matching_a_collection_against_a_type_it_should_return_the_casted_items()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act / Assert
            collection.Should().AllBeAssignableTo<int>()
                .Which.Should().Equal(1, 2, 3);
        }

        [Fact]
        public void When_all_of_the_types_in_a_collection_match_the_type_or_subtype_it_should_succeed()
        {
            // Arrange
            var collection = new object[] { new Exception(), new ArgumentException("foo") };

            // Act / Assert
            collection.Should().AllBeAssignableTo<Exception>();
        }

        [Fact]
        public void When_one_of_the_types_does_not_match_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            var collection = new object[] { 1, "2", 3 };

            // Act
            Action act = () => collection.Should().AllBeAssignableTo(typeof(int), "because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found \"[System.Int32, System.String, System.Int32]\".");
        }

        [Fact]
        public void When_one_of_the_types_does_not_match_the_generic_type_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            var collection = new object[] { 1, "2", 3 };

            // Act
            Action act = () => collection.Should().AllBeAssignableTo<int>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found \"[System.Int32, System.String, System.Int32]\".");
        }

        [Fact]
        public void When_one_of_the_elements_is_null_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            var collection = new object[] { 1, null, 3 };

            // Act
            Action act = () => collection.Should().AllBeAssignableTo<int>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found a null element.");
        }

        [Fact]
        public void When_collection_is_of_matching_types_it_should_succeed()
        {
            // Arrange
            var collection = new[] { typeof(Exception), typeof(ArgumentException) };

            // Act / Assert
            collection.Should().AllBeAssignableTo<Exception>();
        }

        [Fact]
        public void When_collection_of_types_contains_one_type_that_does_not_match_it_should_throw_with_a_clear_explanation()
        {
            // Arrange
            var collection = new[] { typeof(int), typeof(string), typeof(int) };

            // Act
            Action act = () => collection.Should().AllBeAssignableTo<int>("because they are of different type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be \"System.Int32\" because they are of different type, but found \"[System.Int32, System.String, System.Int32]\".");
        }

        [Fact]
        public void When_collection_of_types_and_objects_are_all_of_matching_types_it_should_succeed()
        {
            // Arrange
            var collection = new object[] { typeof(int), 2, typeof(int) };

            // Act / Assert
            collection.Should().AllBeAssignableTo<int>();
        }

        [Fact]
        public void When_collection_of_different_types_and_objects_are_all_assignable_to_type_it_should_succeed()
        {
            // Arrange
            var collection = new object[] { typeof(Exception), new ArgumentException("foo") };

            // Act / Assert
            collection.Should().AllBeAssignableTo<Exception>();
        }

        [Fact]
        public void When_collection_is_null_then_all_be_assignable_toOfT_should_fail()
        {
            // Arrange
            IEnumerable<object> collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().AllBeAssignableTo<object>("we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be \"*.Object\" *failure message*, but found collection is <null>.");
        }
    }
}
