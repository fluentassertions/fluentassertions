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
        act.Should().Throw<XunitException>().WithMessage($"*{nameof(OuterFormatter)}*");
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
        act.Should().Throw<XunitException>().Which.Message.Should().NotMatch($"*{nameof(OuterFormatter)}*");
    }

    [Fact]
    public void Removing_a_formatter_from_scope_works()
    {
        // Arrange
        using var outerScope = new AssertionScope();
        var outerFormatter = new OuterFormatter();

        // Act 1
        outerScope.FormattingOptions.AddFormatter(outerFormatter);
        1.Should().Be(2);

        // Assert 1
        outerScope.Discard().Should().ContainSingle().Which.Should().Match($"*{nameof(OuterFormatter)}*");

        // Act 2
        outerScope.FormattingOptions.RemoveFormatter(outerFormatter);
        1.Should().Be(2);

        // Assert2
        outerScope.Discard().Should().ContainSingle().Which.Should().NotMatch($"*{nameof(OuterFormatter)}*");
        outerScope.FormattingOptions.ScopedFormatters.Should().BeEmpty();
    }

    [Fact]
    public void Add_a_formatter_to_nested_scope_adds_and_removes_correctly_on_dispose()
    {
        // Arrange
        using var outerScope = new AssertionScope("outside");

        var outerFormatter = new OuterFormatter();
        var innerFormatter = new InnerFormatter();

        // Act 1
        outerScope.FormattingOptions.AddFormatter(outerFormatter);
        1.Should().Be(2);

        // Assert 1 / Test if outer scope contains OuterFormatter
        outerScope.Discard().Should().ContainSingle().Which.Should().Match($"*{nameof(OuterFormatter)}*");

        using (var innerScope = new AssertionScope("inside"))
        {
            // Act 2
            innerScope.FormattingOptions.AddFormatter(innerFormatter);
            "1".Should().Be("2"); // InnerFormatter
            1.Should().Be(2); // OuterFormatter

            // Assert 2
            innerScope.Discard().Should()
                .SatisfyRespectively(
                    failure1 => failure1.Should().Match($"*{nameof(InnerFormatter)}*"),
                    failure2 => failure2.Should().Match($"*{nameof(OuterFormatter)}*"));
        }

        // Act 3
        1.Should().Be(2);

        // Assert 3
        outerScope.Discard().Should().ContainSingle().Which.Should().Match($"*{nameof(OuterFormatter)}*");
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
        1.Should().Be(2);
        "1".Should().Be("2");

        // Assert
        innerScope.Discard().Should().SatisfyRespectively(
            failure1 => failure1.Should().Match("*2, but found 1*"),
            failure2 => failure2.Should().NotMatch($"*{nameof(OuterFormatter)}*")
                .And.Match($"*{nameof(InnerFormatter)}*"));

        outerScope.FormattingOptions.ScopedFormatters.Should().ContainSingle()
            .Which.Should().Be(outerFormatter);
    }

    private class OuterFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => value is int;

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            formattedGraph.AddFragment(nameof(OuterFormatter));
        }
    }

    private class InnerFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => value is string;

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            formattedGraph.AddFragment(nameof(InnerFormatter));
        }
    }
}
