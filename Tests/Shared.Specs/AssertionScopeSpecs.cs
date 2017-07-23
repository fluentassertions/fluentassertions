using System;
using System.Text.RegularExpressions;

using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class AssertionScopeSpecs
    {
        [Fact]
        public void When_disposed_it_should_throw_any_failures()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = scope.Dispose;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
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
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure2");

                using (var deeplyNestedScope = new AssertionScope())
                {
                    deeplyNestedScope.FailWith("Failure3");
                }
            }

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = scope.Dispose;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
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
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure2");

                using (var deeplyNestedScope = new AssertionScope())
                {
                    deeplyNestedScope.FailWith("Failure3");
                    deeplyNestedScope.Discard();
                }
            }

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = scope.Dispose;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
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
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure");
            AssertionScope.Current.FailWith("Failure");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure");
                nestedScope.FailWith("Failure");
            }

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = scope.Dispose;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
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
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
            {
                using (new AssertionScope("foo"))
                {
                    new[] { 1, 2, 3 }.Should().Equal(3, 2, 1);
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected foo to be equal to*");
        }

        [Fact]
        public void When_parentheses_are_used_in_the_because_arguments_it_should_render_them_correctly()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => 1.Should().Be(2, "can't use these in becauseArgs: {0} {1}", "{", "}");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("*because can't use these in becauseArgs: { }*");
        }

        [Fact]
        public void When_parentheses_are_used_in_literal_values_it_should_render_them_correctly()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => "{foo}".Should().Be("{bar}");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected string to be \"{bar}\", but \"{foo}\" differs near*");
        }
    }
}