using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertionsAsync;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Execution
{
    /// <summary>
    /// Type <see cref="AssertionScope"/> specs.
    /// </summary>
    public partial class AssertionScopeSpecs
    {
        [Fact]
        public void When_disposed_it_should_throw_any_failures()
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            // Act
            Action act = scope.Dispose;

            // Assert
            try
            {
                act();
            }
            catch (Exception exception)
            {
                exception.Message.Should().StartWith("Failure1");
            }
        }

        [Fact]
        public void When_disposed_it_should_throw_any_failures_and_properly_format_using_args()
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure{0}", 1);

            // Act
            Action act = scope.Dispose;

            // Assert
            try
            {
                act();
            }
            catch (Exception exception)
            {
                exception.Message.Should().StartWith("Failure1");
            }
        }

        [Fact]
        public void When_lazy_version_is_not_disposed_it_should_not_execute_fail_reason_function()
        {
            // Arrange
            var scope = new AssertionScope();
            bool failReasonCalled = false;

            AssertionScope.Current
                .ForCondition(true)
                .FailWith(() =>
                {
                    failReasonCalled = true;
                    return new FailReason("Failure");
                });

            // Act
            Action act = scope.Dispose;

            // Assert
            act();
            failReasonCalled.Should().BeFalse(" fail reason function cannot be called for scope that successful");
        }

        [Fact]
        public void When_lazy_version_is_disposed_it_should_throw_any_failures_and_properly_format_using_args()
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith(() => new FailReason("Failure{0}", 1));

            // Act
            Action act = scope.Dispose;

            // Assert
            try
            {
                act();
            }
            catch (Exception exception)
            {
                exception.Message.Should().StartWith("Failure1");
            }
        }

        [Fact]
        public void When_multiple_scopes_are_nested_it_should_throw_all_failures_from_the_outer_scope()
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure2");

                using var deeplyNestedScope = new AssertionScope();
                deeplyNestedScope.FailWith("Failure3");
            }

            // Act
            Action act = scope.Dispose;

            // Assert
            try
            {
                act();
            }
            catch (Exception exception)
            {
                exception.Message.Should().ContainAll("Failure1", "Failure2", "Failure3");
            }
        }

        [Fact]
        public void When_a_nested_scope_is_discarded_its_failures_should_also_be_discarded()
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure2");

                using var deeplyNestedScope = new AssertionScope();
                deeplyNestedScope.FailWith("Failure3");
                deeplyNestedScope.Discard();
            }

            // Act
            Action act = scope.Dispose;

            // Assert
            try
            {
                act();
            }
            catch (Exception exception)
            {
                exception.Message.Should().ContainAll("Failure1", "Failure2")
                    .And.NotContain("Failure3");
            }
        }

        [Fact]
        public async Task When_using_AssertionScope_across_thread_boundaries_it_should_work()
        {
            using var semaphore = new SemaphoreSlim(0, 1);
            await Task.WhenAll(SemaphoreYieldAndWait(semaphore), SemaphoreYieldAndRelease(semaphore));
        }

        private static async Task SemaphoreYieldAndWait(SemaphoreSlim semaphore)
        {
            await Task.Yield();
            var scope = new AssertionScope();
            await semaphore.WaitAsync();
            scope.Should().BeSameAs(AssertionScope.Current);
        }

        private static async Task SemaphoreYieldAndRelease(SemaphoreSlim semaphore)
        {
            await Task.Yield();
            var scope = new AssertionScope();
            semaphore.Release();
            scope.Should().BeSameAs(AssertionScope.Current);
        }

        [Fact]
        public void When_custom_strategy_used_respect_its_behavior()
        {
            // Arrange
            var scope = new AssertionScope(new FailWithStupidMessageAssertionStrategy());

            // Act
            Action act = () => scope.FailWith("Failure 1");

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("Good luck with understanding what's going on!");
        }

        [Fact]
        public void When_custom_strategy_is_null_it_should_throw()
        {
            // Arrange
            IAssertionStrategy strategy = null;

            // Arrange / Act
            Func<AssertionScope> act = () => new AssertionScope(strategy);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("assertionStrategy");
        }

        [Fact]
        public void When_using_a_custom_strategy_it_should_include_failure_messages_of_all_failing_assertions()
        {
            // Arrange
            var scope = new AssertionScope(new CustomAssertionStrategy());
            false.Should().BeTrue();
            true.Should().BeFalse();

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("*but found false*but found true*");
        }

        [Fact]
        public void When_nested_scope_is_disposed_it_passes_reports_to_parent_scope()
        {
            // Arrange/Act
            using var outerScope = new AssertionScope();
            outerScope.AddReportable("outerReportable", "foo");

            using (var innerScope = new AssertionScope())
            {
                innerScope.AddReportable("innerReportable", "bar");
            }

            // Assert
            outerScope.Get<string>("innerReportable").Should().Be("bar");
        }

        [Fact]
        public async Task Formatting_options_passed_to_inner_assertion_scopes()
        {
            // Arrange
            var subject = new[]
            {
                new
                {
                    Value = 42
                }
            };

            var expected = new[]
            {
                new
                {
                    Value = 42
                },
                new
                {
                    Value = 42
                }
            };

            // Act
            using var scope = new AssertionScope();
            scope.FormattingOptions.MaxDepth = 1;
            await subject.Should().BeEquivalentToAsync(expected);

            // Assert
            scope.Discard().Should().ContainSingle()
                .Which.Should().Contain("Maximum recursion depth of 1 was reached");
        }

        public class CustomAssertionStrategy : IAssertionStrategy
        {
            private readonly List<string> failureMessages = new();

            public IEnumerable<string> FailureMessages => failureMessages;

            public IEnumerable<string> DiscardFailures()
            {
                var discardedFailures = failureMessages.ToArray();
                failureMessages.Clear();
                return discardedFailures;
            }

            public void ThrowIfAny(IDictionary<string, object> context)
            {
                if (failureMessages.Count > 0)
                {
                    var builder = new StringBuilder();
                    builder.AppendJoin(Environment.NewLine, failureMessages).AppendLine();

                    if (context.Any())
                    {
                        foreach (KeyValuePair<string, object> pair in context)
                        {
                            builder.AppendFormat(CultureInfo.InvariantCulture, "\nWith {0}:\n{1}", pair.Key, pair.Value);
                        }
                    }

                    Services.ThrowException(builder.ToString());
                }
            }

            public void HandleFailure(string message)
            {
                failureMessages.Add(message);
            }
        }

        internal class FailWithStupidMessageAssertionStrategy : IAssertionStrategy
        {
            public IEnumerable<string> FailureMessages => new string[0];

            public void HandleFailure(string message) =>
                Services.ThrowException("Good luck with understanding what's going on!");

            public IEnumerable<string> DiscardFailures() => new string[0];

            public void ThrowIfAny(IDictionary<string, object> context)
            {
                // do nothing
            }
        }
    }
}

#pragma warning disable RCS1110, CA1050, S3903 // Declare type inside namespace.
public class AssertionScopeSpecsWithoutNamespace
#pragma warning restore RCS1110, CA1050, S3903 // Declare type inside namespace.
{
    [Fact]
    public void This_class_should_not_be_inside_a_namespace()
    {
        // Arrange
        Type type = typeof(AssertionScopeSpecsWithoutNamespace);

        // Act / Assert
        type.Assembly.Should().DefineType(null, type.Name, "this class should not be inside a namespace");
    }

    [Fact]
    public void When_the_test_method_is_not_inside_a_namespace_it_should_not_throw_a_NullReferenceException()
    {
        // Act
        Action act = () => 1.Should().Be(2, "we don't want a NullReferenceException");

        // Assert
        act.Should().ThrowExactly<XunitException>()
            .WithMessage("*we don't want a NullReferenceException*");
    }
}
