using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertionsAsync.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Formatting;

public class PredicateLambdaExpressionValueFormatterSpecs
{
    private readonly PredicateLambdaExpressionValueFormatter formatter = new();

    [Fact]
    public void Constructor_expression_with_argument_can_be_formatted()
    {
        // Arrange
        Expression expression = (string arg) => new TestItem { Value = arg };

        // Act
        string result = Formatter.ToString(expression);

        // Assert
        result.Should().Be("new TestItem() {Value = arg}");
    }

    [Fact]
    public void Constructor_expression_can_be_simplified()
    {
        // Arrange
        string value = "foo";
        Expression expression = () => new TestItem { Value = value };

        // Act
        string result = Formatter.ToString(expression);

        // Assert
        result.Should().Be("new TestItem() {Value = \"foo\"}");
    }

    private sealed class TestItem
    {
        public string Value { get; set; }
    }

    [Fact]
    public void When_first_level_properties_are_tested_for_equality_against_constants_then_output_should_be_readable()
    {
        // Act
        string result = Format(a => a.Text == "foo" && a.Number == 123);

        // Assert
        result.Should().Be("(a.Text == \"foo\") AndAlso (a.Number == 123)");
    }

    [Fact]
    public void
        When_first_level_properties_are_tested_for_equality_against_constant_expressions_then_output_should_contain_values_of_constant_expressions()
    {
        // Arrange
        var expectedText = "foo";
        var expectedNumber = 123;

        // Act
        string result = Format(a => a.Text == expectedText && a.Number == expectedNumber);

        // Assert
        result.Should().Be("(a.Text == \"foo\") AndAlso (a.Number == 123)");
    }

    [Fact]
    public void When_more_than_two_conditions_are_joined_with_and_operator_then_output_should_not_have_nested_parenthesis()
    {
        // Act
        string result = Format(a => a.Text == "123" && a.Number >= 0 && a.Number <= 1000);

        // Assert
        result.Should().Be("(a.Text == \"123\") AndAlso (a.Number >= 0) AndAlso (a.Number <= 1000)");
    }

    [Fact]
    public void When_condition_contains_extension_method_then_extension_method_must_be_formatted()
    {
        // Act
        string result = Format(a => a.TextIsNotBlank() && a.Number >= 0 && a.Number <= 1000);

        // Assert
        result.Should().Be("a.TextIsNotBlank() AndAlso (a.Number >= 0) AndAlso (a.Number <= 1000)");
    }

    [Fact]
    public void When_condition_contains_linq_extension_method_then_extension_method_must_be_formatted()
    {
        // Arrange
        var allowed = new[] { 1, 2, 3 };

        // Act
        string result = Format<int>(a => allowed.Contains(a));

        // Assert
        result.Should().Be("value(System.Int32[]).Contains(a)");
    }

    [Fact]
    public void Formatting_a_lifted_binary_operator()
    {
        // Arrange
        var subject = new ClassWithNullables { Number = 42 };

        // Act
        Action act = () => subject.Should().Match<ClassWithNullables>(e => e.Number > 43);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*e.Number > *43*");
    }

    private string Format(Expression<Func<SomeClass, bool>> expression) => Format<SomeClass>(expression);

    private string Format<T>(Expression<Func<T, bool>> expression)
    {
        var graph = new FormattedObjectGraph(maxLines: 100);

        formatter.Format(expression, graph, new FormattingContext(), null);

        return graph.ToString();
    }
}

internal class ClassWithNullables
{
    public int? Number { get; set; }
}

internal class SomeClass
{
    public string Text { get; set; }

    public int Number { get; set; }
}

internal static class SomeClassExtensions
{
    public static bool TextIsNotBlank(this SomeClass someObject) => !string.IsNullOrWhiteSpace(someObject.Text);
}
