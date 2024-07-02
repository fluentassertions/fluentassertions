using System;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Execution;

public partial class AssertionScopeSpecs
{
    [Fact]
    public void A_scoped_formatter_will_be_used()
    {
        // Arrange
        Action act = () =>
        {
            // Act
            using var outerScope = new AssertionScope();

            var outerFormatter = new OuterFormatter();
            outerScope.FormattingOptions.AddFormatter(outerFormatter);
            1.Should().Be(2);
        };

        // Assert
        act.Should().Throw<XunitException>().Which.Message.Should().Match("*I am a test formatter*");
    }

    [Fact]
    public void A_scoped_formatter_is_not_available_after_disposal()
    {
        // Arrange
        Action act = () =>
        {
            // Act
            using var outerScope = new AssertionScope();

            var outerFormatter = new OuterFormatter();
            outerScope.FormattingOptions.AddFormatter(outerFormatter);

            // ReSharper disable once DisposeOnUsingVariable
            outerScope.Dispose();

            1.Should().Be(2);
        };

        // Assert
        act.Should().Throw<XunitException>().Which.Message.Should().NotMatch("*I am a test formatter*");
    }

    [Fact]
    public void Removing_a_formatter_from_scope_works()
    {
        // Arrange
        using var outerScope = new AssertionScope();
        var outerFormatter = new OuterFormatter();

        // Act
        outerScope.FormattingOptions.AddFormatter(outerFormatter);
        outerScope.FormattingOptions.RemoveFormatter(outerFormatter);

        // Assert
        outerScope.FormattingOptions.ScopedFormatters.Should().BeEmpty();
    }

    [Fact]
    public void Add_a_formatter_to_nested_scope_adds_and_removes_correctly_on_dispose()
    {
        // Arrange
        using var outerScope = new AssertionScope("outside");

        var outerFormatter = new OuterFormatter();
        var innerFormatter = new InnerFormatter();

        // Act
        outerScope.FormattingOptions.AddFormatter(outerFormatter);

        using (var innerScope = new AssertionScope("inside"))
        {
            innerScope.FormattingOptions.AddFormatter(innerFormatter);

            // Assert
            innerScope.FormattingOptions.ScopedFormatters.Should()
                .HaveCount(2).And
                .Contain(outerFormatter).And.Contain(innerFormatter);
        }

        outerScope.FormattingOptions.ScopedFormatters.Should()
            .ContainSingle().Which.Should().Be(outerFormatter);

        // ReSharper disable once DisposeOnUsingVariable
        outerScope.Dispose();
        outerScope.FormattingOptions.ScopedFormatters.Should().BeEmpty();
    }

    [Fact]
    public void Removing_a_formatter_from_outer_scope_inside_nested_scope_leaves_outer_scope_untouched()
    {
        // Arrange
        using var outerScope = new AssertionScope();
        var outerFormatter = new OuterFormatter();
        var innerFormatter = new InnerFormatter();

        // Act
        outerScope.FormattingOptions.AddFormatter(outerFormatter);

        using var innerScope = new AssertionScope();
        innerScope.FormattingOptions.AddFormatter(innerFormatter);
        innerScope.FormattingOptions.RemoveFormatter(outerFormatter);

        // Assert
        innerScope.FormattingOptions.ScopedFormatters.Should().ContainSingle()
            .Which.Should().Be(innerFormatter);

        outerScope.FormattingOptions.ScopedFormatters.Should().ContainSingle()
            .Which.Should().Be(outerFormatter);
    }

    private class OuterFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => true;

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            formattedGraph.AddFragment("I am a test formatter");
        }
    }

    private class InnerFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => true;

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            formattedGraph.AddFragment("I am another test formatter");
        }
    }
}
