using System;
using System.Globalization;
using System.Linq.Expressions;
using FluentAssertions.Extensions;
using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs.Formatting
{
    public class PredicateLambdaExpressionValueFormatterSpecs
    {
        private PredicateLambdaExpressionValueFormatter formatter = new();

        [Fact]
        public void When_first_level_properties_are_tested_for_equality_against_constants_then_output_should_be_readable()
        {
            // Act
            string result = Format(a => a.Text == "foo" && a.Number == 123);

            // Assert
            result.Should().Be("(a.Text == \"foo\") AndAlso (a.Number == 123)");
        }

        [Fact]
        public void When_first_level_properties_are_tested_for_equality_against_constant_expressions_then_output_should_contain_values_of_constant_expressions()
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

        private string Format(Expression<Func<SomeClass, bool>> expression) => formatter.Format(expression, new FormattingContext(), null);

        private class SomeClass
        {
            public string Text { get; set; }

            public int Number { get; set; }
        }
    }
}
