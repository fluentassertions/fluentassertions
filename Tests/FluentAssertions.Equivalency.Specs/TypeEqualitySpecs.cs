using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

[Collection("ConfigurationSpecs")]
public class TypeEqualitySpecs
{
    [Fact]
    public void Throws_when_top_level_types_are_expected_to_match()
    {
        // Arrange
        var subject = new FooWithNestedClass();

        var expectation = new BarWithNested();

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation, x => x.WithStrictTyping());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject*type*TypeEqualitySpecs+BarWithNested*but*TypeEqualitySpecs+FooWithNestedClass*")
            .WithMessage("*The types of the fields and properties must be the same*");
    }

    [Fact]
    public void By_default_the_runtime_type_is_ignored()
    {
        // Arrange
        Nested subject = new();

        Nested expectation = new NestedSubtype();

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, x => x.WithStrictTyping());
    }

    [Fact]
    public void Can_check_the_runtime_type_if_that_was_requested()
    {
        // Arrange
        Nested subject = new();

        Nested expectation = new NestedSubtype();

        // Act / Assert
        Action act = () => subject.Should().BeEquivalentTo(expectation, x => x
            .PreferringRuntimeMemberTypes()
            .WithStrictTyping());

        act.Should().Throw<XunitException>().WithMessage(
            "Expected subject to be of type *TypeEqualitySpecs+NestedSubtype*, but found *TypeEqualitySpecs+Nested*");
    }

    [Fact]
    public void Uses_the_declared_type_for_members_of_root_objects()
    {
        // Arrange
        var subject = new FooWithNestedClass
        {
            Nested = new Nested()
        };

        var expectation = new BarWithNested
        {
            Nested = new NestedSubtype()
        };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, x => x
            .WithStrictTypingFor(info => info.Path.EndsWith("Nested")));
    }

    [Fact]
    public void The_collection_type_is_ignored()
    {
        // Arrange
        var subject = new[]
        {
            new FooWithNestedClass
            {
                Nested = new Nested()
            }
        };

        var expectation = new List<BarWithNested>
        {
            new()
            {
                Nested = new NestedSubtype()
            }
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation, x => x
            .WithStrictTyping());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject[0]*BarWithNested*but*FooWithNestedClass*");
    }

    [Fact]
    public void Can_use_the_runtime_type_for_members_of_root_objects()
    {
        // Arrange
        var subject = new FooWithNestedClass
        {
            Nested = new Nested()
        };

        var expectation = new BarWithNested
        {
            Nested = new NestedSubtype()
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation, x => x
            .PreferringRuntimeMemberTypes()
            .WithStrictTypingFor(info => info.Path.EndsWith("Nested")));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected property subject.Nested*NestedSubtype*but*Nested*")
            .WithMessage("*Use strict typing when info.Path.EndsWith(\"Nested\")*");
    }

    [Fact]
    public void Requesting_strict_typing_for_nested_members_ignores_root_objects_of_mismatching_types()
    {
        // Arrange
        var subject = new
        {
            Nested = new Nested(),
            OtherProperty = "value"
        };

        var expectation = new
        {
            Nested = new Nested()
        };

        // Act / Assert
        subject.Should().BeEquivalentTo(expectation, x =>
            x.WithStrictTypingFor(info => info.Path.EndsWith("Nested")));
    }

    [Fact]
    public void Can_request_strict_typing_for_nested_members_for_mismatching_roots()
    {
        // Arrange
        var subject = new
        {
            Nested = new Nested(),
            OtherProperty = "value"
        };

        var expectation = new
        {
            Nested = new NestedSubtype()
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation, x =>
            x.WithStrictTypingFor(info => info.Path.EndsWith("Nested")));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*subject.Nested*NestedSubtype*but*Nested*");
    }

    [Fact]
    public void Can_request_strict_typing_for_the_root_only()
    {
        // Arrange
        var subject = new
        {
            Nested = new Nested(),
            OtherProperty = "value"
        };

        var expectation = new
        {
            Nested = new NestedSubtype()
        };

        // Act / Assert
        Action act = () => subject.Should().BeEquivalentTo(expectation, x =>
            x.WithStrictTypingFor(info => info.Path.Length == 0));

        act.Should().Throw<XunitException>()
            .WithMessage("Expected*AnonymousType*NestedSubtype*but*AnonymousType*Nested*String*");
    }

    [Fact]
    public void Can_override_globally_applied_strict_typing_for_individual_assertions()
    {
        try
        {
            // Arrange
            AssertionConfiguration.Current.Equivalency.Modify(x => x.WithStrictTyping());

            var subject = new FooWithNestedClass
            {
                Nested = new Nested()
            };

            var expectation = new BarWithNested
            {
                Nested = new Nested()
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, x => x.WithoutStrictTyping());
        }
        finally
        {
            AssertionEngine.ResetToDefaults();
        }
    }

    private class FooWithNestedClass
    {
        [UsedImplicitly]
        public Nested Nested { get; set; }
    }

    private class BarWithNested
    {
        [UsedImplicitly]
        public Nested Nested { get; set; }
    }

    private class Nested
    {
        public string Name { get; set; }
    }

    private class NestedSubtype : Nested;
}
