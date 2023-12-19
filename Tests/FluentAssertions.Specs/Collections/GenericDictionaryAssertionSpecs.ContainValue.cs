using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Collections;

public partial class GenericDictionaryAssertionSpecs
{
    public class ContainValue
    {
        [Fact]
        public void When_dictionary_contains_expected_value_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValue("One");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Null_dictionaries_do_not_contain_any_values()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().ContainValue("One", "because {0}", "we do");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dictionary to contain values {\"One\"} because we do, but found <null>.");
        }

        [Fact]
        public void When_dictionary_contains_expected_null_value_it_should_succeed()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = null
            };

            // Act
            Action act = () => dictionary.Should().ContainValue(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_specified_value_exists_it_should_allow_continuation_using_that_value()
        {
            // Arrange
            var myClass = new MyClass
            {
                SomeProperty = 0
            };

            var dictionary = new Dictionary<int, MyClass>
            {
                [1] = myClass
            };

            // Act
            Action act = () => dictionary.Should().ContainValue(myClass).Which.SomeProperty.Should().BeGreaterThan(0);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*greater*0*0*");
        }

        [Fact]
        public void When_multiple_matches_for_the_specified_value_exist_continuation_using_the_matched_value_should_fail()
        {
            // Arrange
            var myClass = new MyClass { SomeProperty = 0 };

            var dictionary = new Dictionary<int, MyClass>
            {
                [1] = myClass,
                [2] = new() { SomeProperty = 0 }
            };

            // Act
            Action act =
                () =>
                    dictionary.Should()
                        .ContainValue(new MyClass { SomeProperty = 0 })
                        .Which.Should()
                        .BeSameAs(myClass);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_dictionary_does_not_contain_single_value_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().ContainValue("Three", "because {0}", "we do");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} to contain value \"Three\" because we do.");
        }
    }

    public class NotContainValue
    {
        [Fact]
        public void When_dictionary_does_not_contain_a_value_that_is_not_in_the_dictionary_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValue("Three");

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void When_dictionary_contains_an_unexpected_value_it_should_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                [1] = "One",
                [2] = "Two"
            };

            // Act
            Action act = () => dictionary.Should().NotContainValue("One", "because we {0} like it", "don't");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary {[1] = \"One\", [2] = \"Two\"} not to contain value \"One\" because we don't like it, but found it anyhow.");
        }

        [Fact]
        public void When_asserting_dictionary_does_not_contain_value_against_null_dictionary_it_should_throw()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                dictionary.Should().NotContainValue("One", "because we want to test the behaviour with a null subject");
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected dictionary not to contain value \"One\" because we want to test the behaviour with a null subject, but found <null>.");
        }
    }
}
