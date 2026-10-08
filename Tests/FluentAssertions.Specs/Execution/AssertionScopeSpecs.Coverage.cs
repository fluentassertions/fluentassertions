using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Execution
{
    /// <summary>
    /// Specs for <see cref="AssertionScope"/> and <see cref="AssertionChain"/> public APIs
    /// that previously lacked direct test coverage.
    /// See https://github.com/fluentassertions/fluentassertions/issues/1823.
    /// </summary>
    public partial class AssertionScopeSpecs
    {
        [Fact]
        public void When_adding_a_preformatted_failure_it_should_be_thrown_on_dispose()
        {
            // Arrange
            var scope = new AssertionScope();
            scope.AddPreFormattedFailure("This is a preformatted failure");

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("This is a preformatted failure*");
        }

        [Fact]
        public void HasFailures_should_return_false_when_no_failures_were_added()
        {
            // Arrange
            using var scope = new AssertionScope();

            // Act
            bool result = scope.HasFailures();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HasFailures_should_return_true_after_adding_a_preformatted_failure()
        {
            // Arrange
            var scope = new AssertionScope();
            scope.AddPreFormattedFailure("failure");

            // Act
            bool result = scope.HasFailures();

            // Assert
            result.Should().BeTrue();

            // Cleanup: prevent Dispose from throwing
            scope.Discard();
            scope.Dispose();
        }

        [Fact]
        public void When_appending_tracing_it_should_be_included_in_the_failure_message()
        {
            // Arrange
            var scope = new AssertionScope();
            scope.AppendTracing("trace block content");

            AssertionChain.GetOrCreate().FailWith("Failure");

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("*trace block content*");
        }

        [Fact]
        public void WithReportable_should_append_the_deferred_value_to_the_failure_message()
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionChain.GetOrCreate()
                .WithReportable("DeferredKey", () => "DeferredValue")
                .FailWith("Failure");

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("*With DeferredKey:*DeferredValue*");
        }

        [Fact]
        public void WithReportable_should_not_evaluate_the_value_when_there_are_no_failures()
        {
            // Arrange
            using var scope = new AssertionScope();
            bool valueEvaluated = false;

            // Act
            AssertionChain.GetOrCreate()
                .WithReportable("DeferredKey", () =>
                {
                    valueEvaluated = true;
                    return "DeferredValue";
                });

            scope.Dispose();

            // Assert
            valueEvaluated.Should().BeFalse();
        }
    }
}
