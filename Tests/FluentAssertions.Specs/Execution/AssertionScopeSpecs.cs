using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Execution
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

            AssertionChain.GetOrCreate().FailWith("Failure1");

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

            AssertionChain.GetOrCreate().FailWith("Failure{0}", 1);

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

            AssertionChain.GetOrCreate()
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

            AssertionChain
                .GetOrCreate()
                .FailWith(() => new FailReason("Failure{0}", 1));

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

            AssertionChain.GetOrCreate().FailWith("Failure1");

            using (new AssertionScope())
            {
                AssertionChain.GetOrCreate().FailWith("Failure2");

                using var deeplyNestedScope = new AssertionScope();
                AssertionChain.GetOrCreate().FailWith("Failure3");
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

            AssertionChain.GetOrCreate().FailWith("Failure1");

            using (new AssertionScope())
            {
                AssertionChain.GetOrCreate().FailWith("Failure2");

                using var deeplyNestedScope = new AssertionScope();
                AssertionChain.GetOrCreate().FailWith("Failure3");
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
        public async Task When_using_a_scope_across_thread_boundaries_it_should_work()
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
            using var _ = new AssertionScope(new FailWithStupidMessageAssertionStrategy());

            // Act
            Action act = () => AssertionChain.GetOrCreate().FailWith("Failure 1");

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
            var outerScope = new AssertionScope();
            outerScope.AddReportable("outerReportable", "foo");

            using (var innerScope = new AssertionScope())
            {
                innerScope.AddReportable("innerReportable", "bar");
            }

            AssertionChain.GetOrCreate().FailWith("whatever reason");

            Action act = () => outerScope.Dispose();

            // Assert
            act.Should().Throw<XunitException>()
                .Which.Message.Should().Match("Whatever reason*outerReportable*foo*innerReportable*bar*");
        }

        [Fact]
        public void Formatting_options_passed_to_inner_assertion_scopes()
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
            subject.Should().BeEquivalentTo(expected);

            // Assert
            scope.Discard().Should().ContainSingle()
                .Which.Should().Contain("Maximum recursion depth of 1 was reached");
        }

        [Fact]
        public void Multiple_named_scopes_will_prefix_the_caller_identifier()
        {
            // Arrange
            List<int> nonEmptyList = [1, 2];

            // Act
            Action act = () =>
            {
                using var scope1 = new AssertionScope("Test1");
                using var scope2 = new AssertionScope("Test2");
                nonEmptyList.Should().BeEmpty();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected Test1/Test2/nonEmptyList to be empty*");
        }

        public class CustomAssertionStrategy : IAssertionStrategy
        {
            private readonly List<string> failureMessages = [];

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

                    AssertionEngine.TestFramework.Throw(builder.ToString());
                }
            }

            public void HandleFailure(string message)
            {
                failureMessages.Add(message);
            }
        }

        internal class FailWithStupidMessageAssertionStrategy : IAssertionStrategy
        {
            public IEnumerable<string> FailureMessages => [];

            public void HandleFailure(string message) =>
                AssertionEngine.TestFramework.Throw("Good luck with understanding what's going on!");

            public IEnumerable<string> DiscardFailures() => [];

            public void ThrowIfAny(IDictionary<string, object> context)
            {
                // do nothing
            }
        }
    }
}

#pragma warning disable MA0047
public class AssertionScopeSpecsWithoutNamespace
#pragma warning restore MA0047
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
