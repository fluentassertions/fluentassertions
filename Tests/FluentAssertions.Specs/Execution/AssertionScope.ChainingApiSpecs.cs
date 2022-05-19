using System;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Execution;

/// <content>
/// The chaining API specs.
/// </content>
public partial class AssertionScopeSpecs
{
    [Fact]
    public void When_the_previous_assertion_succeeded_it_should_not_affect_the_next_one()
    {
        bool succeeded = false;

        // Act
        try
        {
            Execute.Assertion
                .ForCondition(condition: true)
                .FailWith("First assertion")
                .Then
                .FailWith("Second assertion");
        }
        catch (Exception e)
        {
            // Assert
            succeeded = (e is XunitException xUnitException) && xUnitException.Message.Contains("Second");
        }

        if (!succeeded)
        {
            throw new XunitException("Expected the second assertion to fail");
        }
    }

    [Fact]
    public void When_the_previous_assertion_succeeded_it_should_not_affect_the_next_one_with_arguments()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .FailWith("First assertion")
            .Then
            .FailWith("Second {0}", "assertion");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Second \"assertion\"");
    }

    [Fact]
    public void When_the_previous_assertion_succeeded_it_should_not_affect_the_next_one_with_argument_providers()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .FailWith("First assertion")
            .Then
            .FailWith("Second {0}", () => "assertion");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Second \"assertion\"");
    }

    [Fact]
    public void When_the_previous_assertion_succeeded_it_should_not_affect_the_next_one_with_a_fail_reason_function()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .FailWith("First assertion")
            .Then
            .FailWith(() => new FailReason("Second {0}", "assertion"));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Second \"assertion\"");
    }

    [Fact]
    public void When_continuing_an_assertion_chain_the_reason_should_be_part_of_consecutive_failures()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .FailWith("First assertion")
            .Then
            .BecauseOf("because reasons")
            .FailWith("Expected{reason}");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected because reasons");
    }

    [Fact]
    public void When_continuing_an_assertion_chain_the_reason_with_arguments_should_be_part_of_consecutive_failures()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .FailWith("First assertion")
            .Then
            .BecauseOf("because {0}", "reasons")
            .FailWith("Expected{reason}");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected because reasons");
    }

    [Fact]
    public void When_a_given_is_used_before_an_assertion_then_the_result_should_be_available_for_evaluation()
    {
        // Act
        Action act = () => Execute.Assertion
            .Given(() => new[] { "a", "b" })
            .ForCondition(collection => collection.Length > 0)
            .FailWith("First assertion");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_evaluate_the_succeeding_given_statement()
    {
        // Arrange
        using var _ = new AssertionScope(new IgnoringFailuresAssertionStrategy());

        // Act / Assert
        Execute.Assertion
            .ForCondition(false)
            .FailWith("First assertion")
            .Then
            .Given<object>(() => throw new InvalidOperationException());
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_evaluate_the_succeeding_condition()
    {
        // Arrange
        bool secondConditionEvaluated = false;
        try
        {
            using var _ = new AssertionScope();

            // Act
            Execute.Assertion
                .Given(() => (string)null)
                .ForCondition(s => s is not null)
                .FailWith("but is was null")
                .Then
                .ForCondition(_ => secondConditionEvaluated = true)
                .FailWith("it should be 42");
        }
        catch
        {
            // Ignore
        }

        // Assert
        secondConditionEvaluated.Should().BeFalse("because the 2nd condition should not be invoked");
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_execute_the_succeeding_failure()
    {
        // Arrange
        using var scope = new AssertionScope();

        // Act
        Execute.Assertion
            .ForCondition(false)
            .FailWith("First assertion")
            .Then
            .ForCondition(false)
            .FailWith("Second assertion");

        string[] failures = scope.Discard();
        scope.Dispose();

        Assert.Single(failures);
        Assert.Contains("First assertion", failures);
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_execute_the_succeeding_failure_with_arguments()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            Execute.Assertion
                .ForCondition(false)
                .FailWith("First assertion")
                .Then
                .FailWith("Second {0}", "assertion");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("First assertion");
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_execute_the_succeeding_failure_with_argument_providers()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            Execute.Assertion
                .ForCondition(false)
                .FailWith("First assertion")
                .Then
                .FailWith("Second {0}", () => "assertion");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("First assertion");
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_execute_the_succeeding_failure_with_a_fail_reason_function()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            Execute.Assertion
                .ForCondition(false)
                .FailWith("First assertion")
                .Then
                .FailWith(() => new FailReason("Second {0}", "assertion"));
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("First assertion");
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_execute_the_succeeding_expectation()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            Execute.Assertion
                .WithExpectation("Expectations are the root ")
                .ForCondition(false)
                .FailWith("of disappointment")
                .Then
                .WithExpectation("Assumptions are the root ")
                .FailWith("of all evil");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expectations are the root of disappointment");
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_execute_the_succeeding_expectation_with_arguments()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            Execute.Assertion
                .WithExpectation("Expectations are the {0} ", "root")
                .ForCondition(false)
                .FailWith("of disappointment")
                .Then
                .WithExpectation("Assumptions are the {0} ", "root")
                .FailWith("of all evil");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expectations are the \"root\" of disappointment");
    }

    [Fact]
    public void When_the_previous_assertion_failed_it_should_not_execute_the_succeeding_default_identifier()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            Execute.Assertion
                .WithDefaultIdentifier("identifier")
                .ForCondition(false)
                .FailWith("Expected {context}")
                .Then
                .WithDefaultIdentifier("other")
                .FailWith("Expected {context}");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected identifier");
    }

    [Fact]
    public void When_continuing_a_failed_assertion_chain_consecutive_resons_are_ignored()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            Execute.Assertion
                .BecauseOf("because {0}", "whatever")
                .ForCondition(false)
                .FailWith("Expected{reason}")
                .Then
                .BecauseOf("because reasons")
                .FailWith("Expected{reason}");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected because whatever");
    }

    [Fact]
    public void When_continuing_a_failed_assertion_chain_consecutive_resons_with_arguments_are_ignored()
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            Execute.Assertion
                .BecauseOf("because {0}", "whatever")
                .ForCondition(false)
                .FailWith("Expected{reason}")
                .Then
                .BecauseOf("because {0}", "reasons")
                .FailWith("Expected{reason}");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected because whatever");
    }

    [Fact]
    public void When_the_previous_assertion_succeeded_it_should_evaluate_the_succeeding_given_statement()
    {
        // Act
        Action act = () => Execute.Assertion
            .ForCondition(true)
            .FailWith("First assertion")
            .Then
            .Given<object>(() => throw new InvalidOperationException());

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void When_the_previous_assertion_succeeded_it_should_not_affect_the_succeeding_expectation()
    {
        // Act
        Action act = () => Execute.Assertion
            .WithExpectation("Expectations are the root ")
            .ForCondition(true)
            .FailWith("of disappointment")
            .Then
            .WithExpectation("Assumptions are the root ")
            .FailWith("of all evil");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Assumptions are the root of all evil");
    }

    [Fact]
    public void When_the_previous_assertion_succeeded_it_should_not_affect_the_succeeding_expectation_with_arguments()
    {
        // Act
        Action act = () => Execute.Assertion
            .WithExpectation("Expectations are the {0} ", "root")
            .ForCondition(true)
            .FailWith("of disappointment")
            .Then
            .WithExpectation("Assumptions are the {0} ", "root")
            .FailWith("of all evil");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Assumptions are the \"root\" of all evil");
    }

    [Fact]
    public void When_the_previous_assertion_succeeded_it_should_not_affect_the_succeeding_default_identifier()
    {
        // Act
        Action act = () =>
        {
            Execute.Assertion
                .WithDefaultIdentifier("identifier")
                .ForCondition(true)
                .FailWith("Expected {context}")
                .Then
                .WithDefaultIdentifier("other")
                .FailWith("Expected {context}");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected other");
    }
}
