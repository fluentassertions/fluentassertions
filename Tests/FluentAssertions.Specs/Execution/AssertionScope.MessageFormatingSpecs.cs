using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Execution;

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
        act.Should().Throw<XunitException>()
            .Which.Message.Should().Contain("Failure", Exactly.Times(4));
    }

    [InlineData("foo")]
    [InlineData("{}")]
    [Theory]
    public void Message_should_use_the_name_of_the_scope_as_context(string context)
    {
        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope(context);
            new[] { 1, 2, 3 }.Should().Equal(3, 2, 1);
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage($"Expected {context} to be equal to*");
    }

    [Fact]
    public void Message_should_use_the_lazy_name_of_the_scope_as_context()
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
    public void Message_should_contain_each_unique_failed_assertion_seperately()
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
            .WithMessage(
                "Expected * to contain key 0.\n" +
                "Expected * to contain key 1.\n");
    }

    [Fact]
    public void Message_should_contain_the_same_failed_assertion_seperately_if_called_multiple_times()
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
            .WithMessage(
                "Expected * to contain a single item, but the collection is empty.\n" +
                "Expected * to contain a single item, but the collection is empty.\n");
    }

    [Fact]
    public void Because_reason_should_keep_parentheses_in_arguments_as_literals()
    {
        // Act
        Action act = () => 1.Should().Be(2, "can't use these in becauseArgs: {0} {1}", "{", "}");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*because can't use these in becauseArgs: { }*");
    }

    [Fact]
    public void Because_reason_should_ignore_undefined_arguments()
    {
        // Act
        object[] becauseArgs = null;
        Action act = () => 1.Should().Be(2, "it should still work", becauseArgs);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*because it should still work*");
    }

    [Fact]
    public void Because_reason_should_threat_parentheses_as_literals_if_no_arguments_are_defined()
    {
        // Act
        Action act = () => 1.Should().Be(2, "use of {} is okay if there are no because arguments");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*because use of {} is okay if there are no because arguments*");
    }

    [Fact]
    public void Because_reason_should_inform_about_invalid_parentheses_with_a_default_message()
    {
        // Act
        Action act = () => 1.Should().Be(2, "use of {} is considered invalid in because parameter with becauseArgs",
            "additional becauseArgs argument");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "*because message 'use of {} is considered invalid in because parameter with becauseArgs' could not be formatted with string.Format*");
    }

    [Fact]
    public void Message_should_keep_parentheses_in_literal_values()
    {
        // Act
        Action act = () => "{foo}".Should().Be("{bar}");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected string to be \"{bar}\", but \"{foo}\" differs near*");
    }

    [Fact]
    public void Message_should_contain_literal_value_if_marked_with_double_parentheses()
    {
        // Arrange
        var scope = new AssertionScope("context");

        AssertionScope.Current.FailWith("{{empty}}");

        // Act
        Action act = scope.Dispose;

        // Assert
        act.Should().ThrowExactly<XunitException>()
            .WithMessage("{empty}*");
    }

    [InlineData("\r")]
    [InlineData("\\r")]
    [InlineData("\\\r")]
    [InlineData("\\\\r")]
    [InlineData("\\\\\r")]
    [InlineData("\n")]
    [InlineData("\\n")]
    [InlineData("\\\n")]
    [InlineData("\\\\n")]
    [InlineData("\\\\\n")]
    [Theory]
    public void Message_should_not_have_modified_carriage_return_or_line_feed_control_characters(string str)
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
    [InlineData(@"\\r")]
    [InlineData("\\\\\r")]
    [InlineData("\n")]
    [InlineData("\\n")]
    [InlineData("\\\n")]
    [InlineData(@"\\n")]
    [InlineData("\\\\\n")]
    [Theory]
    public void Message_should_not_have_modified_carriage_return_or_line_feed_control_characters_in_supplied_arguments(string str)
    {
        // Arrange
        var scope = new AssertionScope();

        AssertionScope.Current.FailWith(@"\{0}\A", str);

        // Act
        Action act = scope.Dispose;

        // Assert
        act.Should().ThrowExactly<XunitException>()
            .WithMessage("\\\"" + str + "\"\\A*");
    }

    [Fact]
    public void Message_should_not_have_trailing_backslashes_removed_from_subject()
    {
        // Arrange / Act
        Action act = () => "A\\".Should().Be("A");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""* near "\" *""");
    }

    [Fact]
    public void Message_should_not_have_trailing_backslashes_removed_from_expectation()
    {
        // Arrange / Act
        Action act = () => "A".Should().Be("A\\");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""* to be "A\" *""");
    }

    [Fact]
    public void Message_should_have_named_placeholder_be_replaced_by_reportable_value()
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
    public void Message_should_have_named_placeholders_be_replaced_by_reportable_values()
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
    public void Message_should_have_reportable_values_appended_at_the_end()
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
    public void Message_should_not_have_nonreportable_values_appended_at_the_end()
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
    public void Message_should_have_named_placeholder_be_replaced_by_nonreportable_value()
    {
        // Arrange
        var scope = new AssertionScope();
        scope.AddNonReportable("SomeKey", "SomeValue");

        AssertionScope.Current.FailWith("{SomeKey}");

        // Act
        Action act = scope.Dispose;

        // Assert
        act.Should().ThrowExactly<XunitException>()
            .WithMessage("SomeValue");
    }

    [Fact]
    public void Deferred_reportable_values_should_not_be_calculated_in_absence_of_failures()
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
    public void Message_should_have_named_placeholder_be_replaced_by_defered_reportable_value()
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
            .WithMessage("MyValue*\n\nWith MyKey:\nMyValue\n");

        deferredValueInvoked.Should().BeTrue();
    }

    [Fact]
    public void Message_should_start_with_the_defined_expectation()
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
    public void Message_should_start_with_the_defined_expectation_and_arguments()
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
    public void Message_should_contain_object_as_context_if_identifier_can_not_be_resolved()
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
    public void Message_should_contain_the_fallback_value_as_context_if_identifier_can_not_be_resolved()
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
    public void Message_should_contain_the_default_identifier_as_context_if_identifier_can_not_be_resolved()
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
    public void Message_should_contain_the_reason_as_defined()
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
    public void Message_should_contain_the_reason_as_defined_with_arguments()
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
