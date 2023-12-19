using System;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Execution;

public class GivenSelectorSpecs
{
    [Fact]
    public void A_consecutive_subject_should_be_selected()
    {
        // Arrange
        string value = string.Empty;

        // Act
        Execute.Assertion
            .ForCondition(true)
            .Given(() => "First selector")
            .Given(_ => value = "Second selector");

        // Assert
        value.Should().Be("Second selector");
    }

    [Fact]
    public void After_a_failed_condition_a_consecutive_subject_should_be_ignored()
    {
        // Arrange
        string value = string.Empty;

        // Act
        Execute.Assertion
            .ForCondition(false)
            .Given(() => "First selector")
            .Given(_ => value = "Second selector");

        // Assert
        value.Should().BeEmpty();
    }

    [Fact]
    public void A_consecutive_condition_should_be_evaluated()
    {
        // Act / Assert
        Execute.Assertion
            .ForCondition(true)
            .Given(() => "Subject")
            .ForCondition(_ => true)
            .FailWith("Failed");
    }

    [Fact]
    public void After_a_failed_condition_a_consecutive_condition_should_be_ignored()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(false)
            .Given(() => "Subject")
            .ForCondition(_ => throw new ApplicationException())
            .FailWith("Failed");

        // Assert
        act.Should().NotThrow<ApplicationException>();
    }

    [Fact]
    public void When_continuing_an_assertion_chain_it_fails_with_a_message_after_selecting_the_subject()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .Given(() => "First")
            .FailWith("First selector")
            .Then
            .Given(_ => "Second")
            .FailWith("Second selector");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Second selector");
    }

    [Fact]
    public void When_continuing_an_assertion_chain_it_fails_with_a_message_with_arguments_after_selecting_the_subject()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .Given(() => "First")
            .FailWith("{0} selector", "First")
            .Then
            .Given(_ => "Second")
            .FailWith("{0} selector", "Second");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("\"Second\" selector");
    }

    [Fact]
    public void When_continuing_an_assertion_chain_it_fails_with_a_message_with_argument_selectors_after_selecting_the_subject()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .Given(() => "First")
            .FailWith("{0} selector", _ => "First")
            .Then
            .Given(_ => "Second")
            .FailWith("{0} selector", _ => "Second");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("\"Second\" selector");
    }

    [Fact]
    public void When_continuing_a_failed_assertion_chain_consecutive_failure_messages_are_ignored()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();

            Execute.Assertion
                .Given(() => "First")
                .FailWith("First selector")
                .Then
                .FailWith("Second selector");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("First selector");
    }

    [Fact]
    public void When_continuing_a_failed_assertion_chain_consecutive_failure_messages_with_arguments_are_ignored()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();

            Execute.Assertion
                .Given(() => "First")
                .FailWith("{0} selector", "First")
                .Then
                .FailWith("{0} selector", "Second");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("\"First\" selector");
    }

    [Fact]
    public void When_continuing_a_failed_assertion_chain_consecutive_failure_messages_with_argument_selectors_are_ignored()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();

            Execute.Assertion
                .Given(() => "First")
                .FailWith("{0} selector", _ => "First")
                .Then
                .FailWith("{0} selector", _ => "Second");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("\"First\" selector");
    }

    [Fact]
    public void The_failure_message_should_be_preceded_by_the_expectation_after_selecting_a_subject()
    {
        // Act
        Action act = () => Execute.Assertion
            .WithExpectation("Expectation ")
            .Given(() => "Subject")
            .FailWith("Failure");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expectation Failure");
    }

    [Fact]
    public void
        The_failure_message_should_not_be_preceded_by_the_expectation_after_selecting_a_subject_and_clearing_the_expectation()
    {
        // Act
        Action act = () => Execute.Assertion
            .WithExpectation("Expectation ")
            .Given(() => "Subject")
            .ClearExpectation()
            .Then
            .FailWith("Failure");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Failure");
    }

    [Fact]
    public void Clearing_the_expectation_does_not_affect_a_successful_assertion()
    {
        // Act
        bool result = Execute.Assertion
            .WithExpectation("Expectation ")
            .Given(() => "Don't care")
            .ForCondition(_ => true)
            .FailWith("Should not fail")
            .Then
            .ClearExpectation();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Clearing_the_expectation_does_not_affect_a_failed_assertion()
    {
        // Act
        using var scope = new AssertionScope();

        bool result = Execute.Assertion
            .WithExpectation("Expectation ")
            .Given(() => "Don't care")
            .ForCondition(_ => false)
            .FailWith("Should fail")
            .Then
            .ClearExpectation();

        scope.Discard();

        // Assert
        if (result)
        {
            throw new XunitException("the assertion failed and should return false");
        }
    }
}
