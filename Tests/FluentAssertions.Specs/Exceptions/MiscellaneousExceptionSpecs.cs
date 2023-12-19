using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Exceptions;

public class MiscellaneousExceptionSpecs
{
    [Fact]
    public void When_getting_value_of_property_of_thrown_exception_it_should_return_value_of_property()
    {
        // Arrange
        const string SomeParamNameValue = "param";
        Does target = Does.Throw(new ExceptionWithProperties(SomeParamNameValue));

        // Act
        Action act = target.Do;

        // Assert
        act.Should().Throw<ExceptionWithProperties>().And.Property.Should().Be(SomeParamNameValue);
    }

    [Fact]
    public void When_validating_a_subject_against_multiple_conditions_it_should_support_chaining()
    {
        // Arrange
        Does testSubject = Does.Throw(new InvalidOperationException("message", new ArgumentException("inner message")));

        // Act / Assert
        testSubject
            .Invoking(x => x.Do())
            .Should().Throw<InvalidOperationException>()
            .WithInnerException<ArgumentException>()
            .WithMessage("inner message");
    }

    [Fact]
    public void When_a_yielding_enumerable_throws_an_expected_exception_it_should_not_throw()
    {
        // Act
        Func<IEnumerable<char>> act = () => MethodThatUsesYield("aaa!aaa");

        // Assert
        act.Enumerating().Should().Throw<Exception>();
    }

    private static IEnumerable<char> MethodThatUsesYield(string bar)
    {
        foreach (var character in bar)
        {
            if (character.Equals('!'))
            {
                throw new Exception("No exclamation marks allowed.");
            }

            yield return char.ToUpperInvariant(character);
        }
    }

    [Fact]
    public void When_custom_condition_is_not_met_it_should_throw()
    {
        // Arrange
        Action act = () => throw new ArgumentException("");

        try
        {
            // Act
            act
                .Should().Throw<ArgumentException>()
                .Where(e => e.Message.Length > 0, "an exception must have a message");

            throw new XunitException("This point should not be reached");
        }
        catch (XunitException exc)
        {
            // Assert
            exc.Message.Should().StartWith(
                "Expected exception where (e.Message.Length > 0) because an exception must have a message, but the condition was not met");
        }
    }

    [Fact]
    public void When_a_2nd_condition_is_not_met_it_should_throw()
    {
        // Arrange
        Action act = () => throw new ArgumentException("Fail");

        try
        {
            // Act
            act
                .Should().Throw<ArgumentException>()
                .Where(e => e.Message.Length > 0)
                .Where(e => e.Message == "Error");

            throw new XunitException("This point should not be reached");
        }
        catch (XunitException exc)
        {
            // Assert
            exc.Message.Should().StartWith(
                "Expected exception where (e.Message == \"Error\"), but the condition was not met");
        }
        catch (Exception exc)
        {
            exc.Message.Should().StartWith(
                "Expected exception where (e.Message == \"Error\"), but the condition was not met");
        }
    }

    [Fact]
    public void When_custom_condition_is_met_it_should_not_throw()
    {
        // Arrange / Act
        Action act = () => throw new ArgumentException("");

        // Assert
        act
            .Should().Throw<ArgumentException>()
            .Where(e => e.Message.Length == 0);
    }

    [Fact]
    public void When_two_exceptions_are_thrown_and_the_assertion_assumes_there_can_only_be_one_it_should_fail()
    {
        // Arrange
        Does testSubject = Does.Throw(new AggregateException(new Exception(), new Exception()));
        Action throwingMethod = testSubject.Do;

        // Act
        Action action = () => throwingMethod.Should().Throw<Exception>().And.Message.Should();

        // Assert
        action.Should().Throw<Exception>();
    }

    [Fact]
    public void When_an_exception_of_a_different_type_is_thrown_it_should_include_the_type_of_the_thrown_exception()
    {
        // Arrange
        Action throwException = () => throw new ExceptionWithEmptyToString();

        // Act
        Action act =
            () => throwException.Should().Throw<ArgumentNullException>();

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage($"*System.ArgumentNullException*{typeof(ExceptionWithEmptyToString)}*");
    }

    [Fact]
    public void When_a_method_throws_with_a_matching_parameter_name_it_should_succeed()
    {
        // Arrange
        Action throwException = () => throw new ArgumentNullException("someParameter");

        // Act
        Action act = () =>
            throwException.Should().Throw<ArgumentException>()
                .WithParameterName("someParameter");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_a_method_throws_with_a_non_matching_parameter_name_it_should_fail_with_a_descriptive_message()
    {
        // Arrange
        Action throwException = () => throw new ArgumentNullException("someOtherParameter");

        // Act
        Action act = () =>
            throwException.Should().Throw<ArgumentException>()
                .WithParameterName("someParameter", "we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*with parameter name \"someParameter\"*we want to test the failure message*\"someOtherParameter\"*");
    }
}
