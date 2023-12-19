using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync;
using FluentAssertionsAsync.Equivalency;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

#pragma warning disable RCS1192, RCS1214, S4144 // verbatim string literals and interpolated strings

namespace FluentAssertionsAsync.Specs.Execution
{
    public class CallerIdentifierSpecs
    {
        [Fact]
        public void When_namespace_is_exactly_System_caller_should_be_unknown()
        {
            // Act
            Action act = () => SystemNamespaceClass.DetermineCallerIdentityInNamespace();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected function to be*");
        }

        [Fact]
        public void When_namespace_is_nested_under_System_caller_should_be_unknown()
        {
            // Act
            Action act = () => System.Data.NestedSystemNamespaceClass.DetermineCallerIdentityInNamespace();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected function to be*");
        }

        [Fact]
        public void When_namespace_is_prefixed_with_System_caller_should_be_known()
        {
            // Act
            Action act = () => SystemPrefixed.SystemPrefixedNamespaceClass.DetermineCallerIdentityInNamespace();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected actualCaller to be*");
        }

        [Fact]
        public void When_variable_name_contains_Should_it_should_identify_the_entire_variable_name_as_the_caller()
        {
            // Arrange
            string fooShould = "bar";

            // Act
            Action act = () => fooShould.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected fooShould to be <null>*");
        }

        [Fact]
        public async Task When_should_is_passed_argument_context_should_still_be_found()
        {
            // Arrange
            var bob = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => bob.Should(timer).NotCompleteWithinAsync(1.Seconds(), "test {0}", "testArg");
            bob.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Did not expect bob to complete within 1s because test testArg.");
        }

        [Fact]
        public void When_variable_is_captured_it_should_use_the_variable_name()
        {
            // Arrange
            string foo = "bar";

            // Act
            Action act = () => foo.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo to be <null>*");
        }

        [Fact]
        public void When_field_is_the_subject_it_should_use_the_field_name()
        {
            // Arrange & Act
            Action act = () =>
            {
                var foo = new Foo();
                foo.Field.Should().BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.Field to be <null>*");
        }

        [Fact]
        public void When_property_is_the_subject_it_should_use_the_property_name()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.Bar.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.Bar to be <null>*");
        }

        [Fact]
        public void When_method_name_is_the_subject_it_should_use_the_method_name()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod().Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod() to be <null>*");
        }

        [Fact]
        public void When_method_contains_arguments_it_should_add_them_to_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod("test").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(\"test\") to be <null>*");
        }

        [Fact]
        public void When_the_caller_contains_multiple_members_it_should_include_them_all()
        {
            // Arrange
            string test1 = "test1";
            var foo = new Foo
            {
                Field = "test3"
            };

            // Act
            Action act = () => foo.GetFoo(test1).GetFooStatic("test" + 2).GetFoo(foo.Field).Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.GetFoo(test1).GetFooStatic(\"test\" + 2).GetFoo(foo.Field) to be <null>*");
        }

        [Fact]
        public void When_the_caller_contains_multiple_members_across_multiple_lines_it_should_include_them_all()
        {
            // Arrange
            string test1 = "test1";
            var foo = new Foo
            {
                Field = "test3"
            };

            // Act
            Action act = () => foo
                .GetFoo(test1)
                .GetFooStatic("test" + 2)
                .GetFoo(foo.Field)
                .Should()
                .BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.GetFoo(test1).GetFooStatic(\"test\" + 2).GetFoo(foo.Field) to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_Should_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod(".Should()").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(\".Should()\") to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_semicolon_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod("test;").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(\"test;\") to be <null>*");
        }

        [Fact]
        [SuppressMessage("Code should not contain multiple statements on one line", "SA1107")]
        [SuppressMessage("Code should not contain multiple statements on one line", "IDE0055")]
        public void When_there_are_several_statements_on_the_line_it_should_use_the_correct_statement()
        {
            // Arrange
            var foo = new Foo();

            // Act
#pragma warning disable format
            Action act = () =>
            {
                var foo2 = foo; foo2.Should().BeNull();
            };
#pragma warning restore format

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo2 to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_escaped_quote_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod("test\";").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(\"test\\\";\") to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_verbatim_string_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod(@"test", argument2: $@"test2", argument3: @$"test3").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(@\"test\", argument2: $@\"test2\", argument3: @$\"test3\") to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_multi_line_verbatim_string_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod(@"
            bob dole
            "
                )
                .Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(@\"            bob dole            \") to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_verbatim_string_interpolation_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod(@$"test"";").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(@$\"test\"\";\") to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_concatenations_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod("1" + 2).Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(\"1\" + 2) to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_multi_line_concatenations_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod("abc"
                    + "def"
                )
                .Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(\"abc\"+ \"def\") to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_string_with_comment_like_contents_inside_it_should_include_them_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod("test//test2/*test3*/").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(\"test//test2/*test3*/\") to be <null>*");
        }

        [Fact]
        public void When_arguments_contain_verbatim_string_with_verbatim_like_string_inside_it_should_include_them_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.BarMethod(@"test @"" @$"" bob").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.BarMethod(@\"test @\"\" @$\"\" bob\") to be <null>*");
        }

        [Fact]
        [SuppressMessage("Single - line comment should be preceded by blank line", "SA1515")]
        [SuppressMessage("Single - line comment should be preceded by blank line", "IDE0055")]
        public void When_the_caller_contains_single_line_comment_it_should_ignore_that()
        {
            // Arrange
            string foo = "bar";

            // Act
            Action act = () =>
            {
                foo
                    // some important comment
                    .Should().BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo to be <null>*");
        }

        [Fact]
        [SuppressMessage("Single - line comment should be preceded by blank line", "SA1515")]
        [SuppressMessage("Single - line comment should be preceded by blank line", "IDE0055")]
        public void When_the_caller_contains_multi_line_comment_it_should_ignore_that()
        {
            // Arrange
            string foo = "bar";

            // Act
            Action act = () =>
            {
                foo
                    /*
                     * some important comment
                     */
                    .Should().BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo to be <null>*");
        }

        [Fact]
        [SuppressMessage("Single - line comment should be preceded by blank line", "SA1515")]
        [SuppressMessage("Single - line comment should be preceded by blank line", "IDE0055")]
        public void When_the_caller_contains_several_comments_it_should_ignore_them()
        {
            // Arrange
            string foo = "bar";

            // Act
            Action act = () =>
            {
                foo
                    // some important comment
                    /* another one */.Should().BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo to be <null>*");
        }

        [Fact]
        [SuppressMessage("Code should not contain multiple statements on one line", "IDE0055")]
        [SuppressMessage("Single-line comment should be preceded by blank line", "SA1515")]
        public void When_the_caller_with_methods_has_comments_between_it_should_ignore_them()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () =>
            {
                foo
                    // some important comment
                    .GetFoo("bob")
                    /* another one */
                    .BarMethod()
                    .Should().BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.GetFoo(\"bob\").BarMethod() to be <null>*");
        }

        [Fact]
        public void When_the_method_has_Should_prefix_it_should_read_whole_method()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.ShouldReturnSomeBool().Should().BeFalse();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.ShouldReturnSomeBool() to be false*");
        }

        [Collection("UIFacts")]
        public class UIFacts
        {
            [UIFact]
            public async Task Caller_identification_should_also_work_for_statements_following_async_code()
            {
                // Arrange
                const string someText = "Hello";
                Func<Task> task = async () => await Task.Yield();

                // Act
                await task.Should().NotThrowAsync();
                Action act = () => someText.Should().Be("Hi");

                // Assert
                act.Should().Throw<XunitException>()
                    .WithMessage("*someText*", "it should capture the variable name");
            }
        }

        [Fact]
        public void A_method_taking_an_array_initializer_is_an_identifier()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.GetFoo(new[] { 1, 2, 3 }.Sum() + "")
                .Should()
                .BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.GetFoo(new[] { 1, 2, 3 }.Sum() + \"\") to be <null>*");
        }

        [Fact]
        public void A_method_taking_a_target_typed_new_expression_is_an_identifier()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.GetFoo(new('a', 10))
                .Should()
                .BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.GetFoo(new('a', 10)) to be <null>*");
        }

        [Fact]
        public void A_method_taking_a_list_is_an_identifier()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.GetFoo(new List<int> { 1, 2, 3 }.Sum() + "")
                .Should()
                .BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo.GetFoo(new List<int> { 1, 2, 3 }*");
        }

        [Fact]
        public void An_array_initializer_preceding_an_assertion_is_not_an_identifier()
        {
            // Act
            Action act = () => new[] { 1, 2, 3 }.Should().BeEmpty();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected collection to be empty*");
        }

        [Fact]
        public void An_object_initializer_preceding_an_assertion_is_not_an_identifier()
        {
            // Act
            Action act = () => new { Property = "blah" }.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected object to be*");
        }

        [Fact]
        public void All_core_code_anywhere_in_the_stack_trace_is_ignored()
        {
            /*
             We want to test this specific scenario.

                1. CallerIdentifier.DetermineCallerIdentity
                2. FluentAssertions code
                3. Custom extension <--- pointed to by lastUserStackFrameBeforeFluentAssertionsCodeIndex
                4. FluentAssertions code  <--- this is where DetermineCallerIdentity tried to get the variable name from before the fix
                5. Test
             */

            var node = Node.From<Foo>(GetSubjectId);

            // Assert
            node.Description.Should().StartWith("node.Description");
        }

        [CustomAssertion]
        private string GetSubjectId() => AssertionScope.Current.CallerIdentity;
    }

#pragma warning disable IDE0060, RCS1163 // Remove unused parameter
    [SuppressMessage("The name of a C# element does not begin with an upper-case letter", "SA1300")]
    [SuppressMessage("Parameter is never used", "CA1801")]
    public class Foo
    {
        public string Field = "bar";

        public string Bar => "bar";

        public string BarMethod() => Bar;

        public string BarMethod(string argument) => Bar;

        public string BarMethod(string argument, string argument2, string argument3) => Bar;

        public bool ShouldReturnSomeBool() => true;

        public Foo GetFoo(string argument) => this;
    }

    [SuppressMessage("Parameter is never used", "CA1801")]
    public static class Extensions
    {
        public static Foo GetFooStatic(this Foo foo, string prm) => foo;
    }
#pragma warning restore IDE0060, RCS1163 // Remove unused parameter
}

namespace System
{
    public static class SystemNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => AssertionScope.Current.CallerIdentity;
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}

namespace SystemPrefixed
{
    public static class SystemPrefixedNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => AssertionScope.Current.CallerIdentity;
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}

namespace System.Data
{
    public static class NestedSystemNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => AssertionScope.Current.CallerIdentity;
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}
