using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

#pragma warning disable RCS1110 // Declare type inside namespace.
public class AssertionScopeSpecsWithoutNamespace
#pragma warning restore RCS1110 // Declare type inside namespace.
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

namespace FluentAssertions.Specs
{
    public class AssertionScopeSpecs
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
                exception.Message.Should().Contain("Failure1");
                exception.Message.Should().Contain("Failure2");
                exception.Message.Should().Contain("Failure3");
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
                exception.Message.Should().Contain("Failure1");
                exception.Message.Should().Contain("Failure2");
                exception.Message.Should().NotContain("Failure3");
            }
        }

        [Fact]
        public void When_the_same_failure_is_handled_twice_or_more_it_should_still_report_it_once()
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure");
            AssertionScope.Current.FailWith("Failure");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure");
                nestedScope.FailWith("Failure");
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
                int matches = new Regex(".*Failure.*").Matches(exception.Message).Count;

                matches.Should().Be(4);
            }
        }

        [Fact]
        public void When_an_assertion_fails_in_a_named_scope_it_should_use_the_name_as_the_assertion_context()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope("foo");
                new[] { 1, 2, 3 }.Should().Equal(3, 2, 1);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo to be equal to*");
        }

        [Fact]
        public void When_an_assertion_fails_on_ContainKey_succeeding_message_should_be_included()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                var values = new Dictionary<int, int>();
                values.Should().ContainKey(0);
                values.Should().ContainKey(1);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*to contain key 0*Expected*to contain key 1*");
        }

        [Fact]
        public void When_an_assertion_fails_on_ContainSingle_succeeding_message_should_be_included()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                var values = new List<int>();
                values.Should().ContainSingle();
                values.Should().ContainSingle();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*to contain a single item, but the collection is empty*" +
                "Expected*to contain a single item, but the collection is empty*");
        }

        [Fact]
        public void When_an_assertion_fails_on_BeOfType_succeeding_message_should_be_included()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                var item = string.Empty;
                item.Should().BeOfType<int>();
                item.Should().BeOfType<long>();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                "Expected type to be System.Int32, but found System.String.*" +
                "Expected type to be System.Int64, but found System.String.");
        }

        [Fact]
        public void When_an_assertion_fails_on_BeAssignableTo_succeeding_message_should_be_included()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                var item = string.Empty;
                item.Should().BeAssignableTo<int>();
                item.Should().BeAssignableTo<long>();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                "Expected * to be assignable to System.Int32, but System.String is not.*" +
                "Expected * to be assignable to System.Int64, but System.String is not.");
        }

        [Fact]
        public void When_parentheses_are_used_in_the_because_arguments_it_should_render_them_correctly()
        {
            // Act
            Action act = () => 1.Should().Be(2, "can't use these in becauseArgs: {0} {1}", "{", "}");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because can't use these in becauseArgs: { }*");
        }

        [Fact]
        public void When_becauseArgs_is_null_it_should_render_reason_correctly()
        {
            // Act
            object[] becauseArgs = null;
            Action act = () => 1.Should().Be(2, "it should still work", becauseArgs);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*it should still work*");
        }

        [Fact]
        public void When_invalid_format_is_used_in_because_parameter_without_becauseArgs_it_should_still_render_reason_correctly()
        {
            // Act
            Action act = () => 1.Should().Be(2, "use of {} is okay if there are no because parameters");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because use of {} is okay if there are no because parameters*");
        }

        [Fact]
        public void When_invalid_format_is_used_in_because_parameter_along_with_becauseArgs_it_should_render_default_text()
        {
            // Act
            Action act = () => 1.Should().Be(2, "use of {} is considered invalid in because parameter with becauseArgs", "additional becauseArgs parameter");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because message 'use of {} is considered invalid in because parameter with becauseArgs' could not be formatted with string.Format*");
        }

        [Fact]
        public void When_an_assertion_fails_in_a_scope_with_braces_it_should_use_the_name_as_the_assertion_context()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope("{}");
                default(Array).Should().Equal(3, 2, 1);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected {} to be equal to*");
        }

        [Fact]
        public void When_parentheses_are_used_in_literal_values_it_should_render_them_correctly()
        {
            // Act
            Action act = () => "{foo}".Should().Be("{bar}");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected string to be \"{bar}\", but \"{foo}\" differs near*");
        }

        [Fact]
        public void When_message_contains_double_braces_they_should_not_be_replaced_with_context()
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("{{empty}}");

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("*empty*");
        }

        [InlineData("\r")]
        [InlineData("\\r")]
        [InlineData("\\\r")]
        [InlineData("\\\\r")]
        [InlineData("\\\\\r")]
        [Theory]
        public void When_message_contains_backslash_followed_by_r_is_should_format_correctly(string str)
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith(str);

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage(str);
        }

        [InlineData("\r")]
        [InlineData("\\r")]
        [InlineData("\\\r")]
        [InlineData("\\\\r")]
        [InlineData("\\\\\r")]
        [Theory]
        public void When_message_argument_contains_backslash_followed_by_r_is_should_format_correctly(string str)
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("\\{0}\\A", str);

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("\\\"" + str + "\"\\A*");
        }

        [InlineData("\n")]
        [InlineData("\\n")]
        [InlineData("\\\n")]
        [InlineData("\\\\n")]
        [InlineData("\\\\\n")]
        [Theory]
        public void When_message_contains_backslash_followed_by_n_is_should_format_correctly(string str)
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith(str);

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage(str);
        }

        [InlineData("\n")]
        [InlineData("\\n")]
        [InlineData("\\\n")]
        [InlineData("\\\\n")]
        [InlineData("\\\\\n")]
        [Theory]
        public void When_message_argument_contains_backslash_followed_by_n_is_should_format_correctly(string str)
        {
            // Arrange
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("\\{0}\\A", str);

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("\\\"" + str + "\"\\A*");
        }

        [Fact]
        public void When_subject_has_trailing_backslash_the_failure_message_should_contain_the_trailing_backslash()
        {
            // Arrange / Act
            Action act = () => "A\\".Should().Be("A");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"* near ""\"" *", "trailing backslashes should not be removed from failure message");
        }

        [Fact]
        public void When_expectation_has_trailing_backslash_the_failure_message_should_contain_the_trailing_backslash()
        {
            // Arrange / Act
            Action act = () => "A".Should().Be("A\\");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"* to be ""A\"" *", "trailing backslashes should not be removed from failure message");
        }

        [Fact]
        public void When_message_starts_with_single_braces_they_should_be_replaced_with_context()
        {
            // Arrange
            var scope = new AssertionScope();
            scope.AddReportable("MyKey", "MyValue");

            AssertionScope.Current.FailWith("{MyKey}");

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("*MyValue*");
        }

        [Fact]
        public void When_message_starts_with_two_single_braces_they_should_be_replaced_with_context()
        {
            // Arrange
            var scope = new AssertionScope();
            scope.AddReportable("SomeKey", "SomeValue");
            scope.AddReportable("AnotherKey", "AnotherValue");

            AssertionScope.Current.FailWith("{SomeKey}{AnotherKey}");

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("*SomeValue*AnotherValue*");
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
            Action act = () => new AssertionScope(strategy);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("assertionStrategy");
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

        public class CustomAssertionStrategy : IAssertionStrategy
        {
            private readonly List<string> failureMessages = new List<string>();

            /// <summary>
            /// Returns the messages for the assertion failures that happened until now.
            /// </summary>
            public IEnumerable<string> FailureMessages => failureMessages;

            /// <summary>
            /// Discards and returns the failure messages that happened up to now.
            /// </summary>
            public IEnumerable<string> DiscardFailures()
            {
                var discardedFailures = failureMessages.ToArray();
                failureMessages.Clear();
                return discardedFailures;
            }

            /// <summary>
            /// Will throw a combined exception for any failures have been collected since <see cref="StartCollecting"/> was called.
            /// </summary>
            public void ThrowIfAny(IDictionary<string, object> context)
            {
                if (failureMessages.Any())
                {
                    var builder = new StringBuilder();
                    builder.AppendLine(string.Join(Environment.NewLine, failureMessages));

                    if (context.Any())
                    {
                        foreach (KeyValuePair<string, object> pair in context)
                        {
                            builder.AppendFormat("\nWith {0}:\n{1}", pair.Key, pair.Value);
                        }
                    }

                    Services.ThrowException(builder.ToString());
                }
            }

            /// <summary>
            /// Instructs the strategy to handle a assertion failure.
            /// </summary>
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
