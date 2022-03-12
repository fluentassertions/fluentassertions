using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Execution
{
    /// <content>
    /// The message formatting specs.
    /// </content>
    public partial class AssertionScopeSpecs
    {
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
        public void The_failure_message_should_use_the_name_of_the_scope_as_context()
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
        public void The_failure_message_should_use_the_lazy_name_of_the_scope_as_context()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope(new Lazy<string>(() => "lazy foo"));
                new[] { 1, 2, 3 }.Should().Equal(3, 2, 1);
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected lazy foo to be equal to*");
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
                default(int[]).Should().Equal(3, 2, 1);
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
                .WithMessage("MyValue*");
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
                .WithMessage("SomeValueAnotherValue*");
        }

        [Fact]
        public void When_adding_reportable_values_they_should_be_reported_after_the_message()
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
                .WithMessage("*With SomeKey:\nSomeValue\nWith AnotherKey:\nAnotherValue");
        }

        [Fact]
        public void When_adding_non_reportable_value_it_should_not_be_reported_after_the_message()
        {
            // Arrange
            var scope = new AssertionScope();
            scope.AddNonReportable("SomeKey", "SomeValue");

            AssertionScope.Current.FailWith("{SomeKey}");

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .Which.Message.Should().NotContain("With SomeKey:\nSomeValue");
        }

        [Fact]
        public void When_adding_non_reportable_value_it_should_be_retrievable_from_context()
        {
            // Arrange
            var scope = new AssertionScope();
            scope.AddNonReportable("SomeKey", "SomeValue");

            // Act
            var value = scope.Get<string>("SomeKey");

            // Assert
            value.Should().Be("SomeValue");
        }

        [Fact]
        public void When_using_a_deferred_reportable_value_it_is_not_calculated_if_there_are_no_failures()
        {
            // Arrange
            var scope = new AssertionScope();
            var deferredValueInvoked = false;

            scope.AddReportable("MyKey", () =>
            {
                deferredValueInvoked = true;

                return "MyValue";
            });

            // Act
            scope.Dispose();

            // Assert
            deferredValueInvoked.Should().BeFalse();
        }

        [Fact]
        public void When_using_a_deferred_reportable_value_it_is_calculated_if_there_is_a_failure()
        {
            // Arrange
            var scope = new AssertionScope();
            var deferredValueInvoked = false;

            scope.AddReportable("MyKey", () =>
            {
                deferredValueInvoked = true;

                return "MyValue";
            });

            AssertionScope.Current.FailWith("{MyKey}");

            // Act
            Action act = scope.Dispose;

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .WithMessage("*MyValue*");
            deferredValueInvoked.Should().BeTrue();
        }

        [Fact]
        public void When_an_expectation_is_defined_it_should_be_preceeding_the_failure_message()
        {
            // Act
            Action act = () => Execute.Assertion
                .WithExpectation("Expectations are the root ")
                .ForCondition(false)
                .FailWith("of disappointment");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expectations are the root of disappointment");
        }

        [Fact]
        public void When_an_expectation_with_arguments_is_defined_it_should_be_preceeding_the_failure_message()
        {
            // Act
            Action act = () => Execute.Assertion
                .WithExpectation("Expectations are the {0} ", "root")
                .ForCondition(false)
                .FailWith("of disappointment");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expectations are the \"root\" of disappointment");
        }

        [Fact]
        public void When_no_identifier_can_be_resolved_replace_context_with_object()
        {
            // Act
            Action act = () => Execute.Assertion
                .ForCondition(false)
                .FailWith("Expected {context}");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected object");
        }

        [Fact]
        public void When_no_identifier_can_be_resolved_replace_context_with_inline_declared_fallback_identifier()
        {
            // Act
            Action act = () => Execute.Assertion
                .ForCondition(false)
                .FailWith("Expected {context:fallback}");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected fallback");
        }

        [Fact]
        public void When_no_identifier_can_be_resolved_replace_context_with_defined_default_identifier()
        {
            // Act
            Action act = () => Execute.Assertion
                .WithDefaultIdentifier("identifier")
                .ForCondition(false)
                .FailWith("Expected {context}");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected identifier");
        }

        [Fact]
        public void The_failure_message_should_contain_the_reason()
        {
            // Act
            Action act = () => Execute.Assertion
                .BecauseOf("because reasons")
                .FailWith("Expected{reason}");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected because reasons");
        }

        [Fact]
        public void The_failure_message_should_contain_the_reason_with_arguments()
        {
            // Act
            Action act = () => Execute.Assertion
                .BecauseOf("because {0}", "reasons")
                .FailWith("Expected{reason}");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected because reasons");
        }
    }
}
