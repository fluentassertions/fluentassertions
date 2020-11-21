using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class CallerIdentifierSpecs
    {
        [Fact]
        public void When_namespace_is_exactly_System_caller_should_be_unknown()
        {
            // Act
            Action act = () => System.SystemNamespaceClass.DetermineCallerIdentityInNamespace();

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
                .WithMessage("*Expected fooShould to be <null>*");
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
                .WithMessage("*Expected foo to be <null>*");
        }

        [Fact]
        public void When_variable_is_not_captured_it_should_use_the_variable_name()
        {
            // Arrange & Act
            Action act = () =>
            {
                string foo = "bar";
                foo.Should().BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo to be <null>*");
        }

        [Fact]
        public void When_field_is_the_caller_it_should_use_the_field_name()
        {
            // Arrange & Act
            Action act = () =>
            {
                var foo = new Foo();
                foo.Field.Should().BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.Field to be <null>*");
        }

        [Fact]
        public void When_property_is_the_caller_it_should_use_the_property_name()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.Bar.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.Bar to be <null>*");
        }

        [Fact]
        public void When_method_name_contains_get_it_should_not_remove_the_prefix()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.get_BarMethod().Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.get_BarMethod() to be <null>*");
        }

        [Fact]
        public void When_method_contains_parameters_it_should_add_them_to_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.get_BarMethod("test").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.get_BarMethod(\"test\") to be <null>*");
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
                .WithMessage("*Expected foo.GetFoo(test1).GetFooStatic(\"test\"+2).GetFoo(foo.Field) to be <null>*");
        }

        [Fact]
        public void When_parameters_contain_Should_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.get_BarMethod(".Should()").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.get_BarMethod(\".Should()\") to be <null>*");
        }

        [Fact]
        public void When_parameters_contain_semicolon_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.get_BarMethod("test;").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.get_BarMethod(\"test;\") to be <null>*");
        }

        [Fact]
        [SuppressMessage("Code should not contain multiple statements on one line", "SA1107")]
        [SuppressMessage("Code should not contain multiple statements on one line", "IDE0055")]
        public void When_there_are_several_statements_on_the_line_it_should_use_the_correct_statement()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () =>
            {
                var foo2 = foo; foo2.Should().BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo2 to be <null>*");
        }

        [Fact]
        public void When_parameters_contain_escaped_quote_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.get_BarMethod("test\";").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.get_BarMethod(\"test\\\";\") to be <null>*");
        }

        [Fact]
        public void When_parameters_contain_at_escaped_quote_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.get_BarMethod(@"test"";").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.get_BarMethod(@\"test\"\";\") to be <null>*");
        }

        [Fact]
        public void When_parameters_contain_at_and_dollar_escaped_quote_it_should_include_that_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.get_BarMethod(@$"test"";").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.get_BarMethod(@$\"test\"\";\") to be <null>*");
        }

        [Fact]
        public void When_parameters_contain_comments_it_should_include_them_to_the_caller()
        {
            // Arrange
            var foo = new Foo();

            // Act
            Action act = () => foo.get_BarMethod("test//test2/*test3*/").Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected foo.get_BarMethod(\"test//test2/*test3*/\") to be <null>*");
        }

        [Fact]
        [SuppressMessage("Single - line comment should be preceded by blank line", "SA1515")]
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
                .WithMessage("*Expected foo to be <null>*");
        }

        [Fact]
        [SuppressMessage("Single - line comment should be preceded by blank line", "SA1515")]
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
                .WithMessage("*Expected foo to be <null>*");
        }

        [Fact]
        [SuppressMessage("Single - line comment should be preceded by blank line", "SA1515")]
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
                .WithMessage("*Expected foo to be <null>*");
        }
    }

    [SuppressMessage("The name of a C# element does not begin with an upper-case letter", "SA1300")]
    [SuppressMessage("Parameter is never used", "CA1801")]
    public class Foo
    {
        public string Field = "bar";

        public string Bar { get; } = "bar";

        public string get_BarMethod() => Bar;

        public string get_BarMethod(string prm) => Bar;

        public Foo GetFoo(string prm) => this;
    }

    [SuppressMessage("Parameter is never used", "CA1801")]
    public static class Extensions
    {
        public static Foo GetFooStatic(this Foo foo, string prm) => foo;
    }
}

namespace System
{
    public static class SystemNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
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
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
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
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}
