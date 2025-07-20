using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class InlineAssertionsSpecs
{
    [Fact]
    public void The_inline_condition_must_be_met()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatMatches<int>(age => age < 30)
        };

        // Act
        var act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*actual.Age*meet condition*(age < 30)*");
    }

    [Fact]
    public void The_type_of_the_condition_must_be_met_too()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatMatches<string>(s => s.Length > 0)
        };

        // Act
        var act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*actual.Age*type*String*found*Int32*");
    }

    [Fact]
    public void Succeeds_for_a_matching_condition()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatMatches<int>(age => age < 40)
        };

        // Act / Assert
        actual.Should().BeEquivalentTo(expectation);
    }

    [Fact]
    public void A_condition_expression_is_required()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatMatches<int>(null)
        };

        // Act
        var act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*condition*required*");
    }

    [Fact]
    public void The_inline_assertion_must_be_met()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatSatisfies<int>(age => age.Should().BeLessThan(30))
        };

        // Act
        var act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*age to be less than 30*found 30*");
    }

    [Fact]
    public void Succeeds_for_a_matching_assertion()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatSatisfies<int>(age => age.Should().BeGreaterThan(20))
        };

        // Act / Assert
        actual.Should().BeEquivalentTo(expectation);
    }

    [Fact]
    public void An_assertion_action_is_required()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatSatisfies<int>(null)
        };

        // Act
        var act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*assertion*required*");
    }

    [Fact]
    public void The_type_expected_by_the_assertion_must_be_met_too()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatSatisfies<string>(s => s.Should().BeNullOrEmpty())
        };

        // Act
        var act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*actual.Age*type*String*found*Int32*");
    }
}
