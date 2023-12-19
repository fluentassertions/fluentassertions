using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]BeDecoratedWithOrInherit specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class BeDecoratedWithOrInherit
    {
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
                    .BeDecoratedWithOrInherit<DummyClassAttribute>(a => a.Name == "Expected" && a.IsEnabled);

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
                    .BeDecoratedWithOrInherit<DummyClassAttribute>(a => a.Name == "Unexpected" && a.IsEnabled);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithInheritedAttribute to be decorated with or inherit *.DummyClassAttribute " +
                    "that matches (a.Name == \"Unexpected\")*a.IsEnabled, but no matching attribute was found.");
        }
    }

    public class NotBeDecoratedWithOrInherit
    {
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
                    .NotBeDecoratedWithOrInherit<DummyClassAttribute>(a => a.Name == "Unexpected" && a.IsEnabled);

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
                    .NotBeDecoratedWithOrInherit<DummyClassAttribute>(a => a.Name == "Expected" && a.IsEnabled);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type *.ClassWithInheritedAttribute to not be decorated with or inherit *.DummyClassAttribute " +
                    "that matches (a.Name == \"Expected\") * a.IsEnabled, but a matching attribute was found.");
        }
    }
}
