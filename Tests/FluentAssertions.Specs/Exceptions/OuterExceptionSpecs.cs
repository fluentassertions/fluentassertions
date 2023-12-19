using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Exceptions;

public class OuterExceptionSpecs
{
    [Fact]
    public void When_subject_throws_expected_exception_with_an_expected_message_it_should_not_do_anything()
    {
        // Arrange
        Does testSubject = Does.Throw(new InvalidOperationException("some message"));

        // Act / Assert
        testSubject.Invoking(x => x.Do()).Should().Throw<InvalidOperationException>().WithMessage("some message");
    }

    [Fact]
    public void When_subject_throws_expected_exception_but_with_unexpected_message_it_should_throw()
    {
        // Arrange
        Does testSubject = Does.Throw(new InvalidOperationException("some"));

        try
        {
            // Act
            testSubject
                .Invoking(x => x.Do())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("some message");

            throw new XunitException("This point should not be reached");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should().Match(
                "Expected exception message to match the equivalent of*\"some message\", but*\"some\" does not*");
        }
    }

    [Fact]
    public void When_subject_throws_expected_exception_with_message_starting_with_expected_message_it_should_not_throw()
    {
        // Arrange
        Does testSubject = Does.Throw(new InvalidOperationException("expected message"));

        // Act
        Action action = testSubject.Do;

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("expected mes*");
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_subject_throws_expected_exception_with_message_that_does_not_start_with_expected_message_it_should_throw()
    {
        // Arrange
        Does testSubject = Does.Throw(new InvalidOperationException("OxpectOd message"));

        // Act
        Action action = () => testSubject
            .Invoking(s => s.Do())
            .Should().Throw<InvalidOperationException>()
            .WithMessage("Expected mes");

        // Assert
        action.Should().Throw<Exception>()
            .WithMessage(
                "Expected exception message to match the equivalent of*\"Expected mes*\", but*\"OxpectOd message\" does not*");
    }

    [Fact]
    public void
        When_subject_throws_expected_exception_with_message_starting_with_expected_equivalent_message_it_should_not_throw()
    {
        // Arrange
        Does testSubject = Does.Throw(new InvalidOperationException("Expected Message"));

        // Act
        Action action = testSubject.Do;

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("expected mes*");
    }

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_subject_throws_expected_exception_with_message_that_does_not_start_with_equivalent_message_it_should_throw()
    {
        // Arrange
        Does testSubject = Does.Throw(new InvalidOperationException("OxpectOd message"));

        // Act
        Action action = () => testSubject
            .Invoking(s => s.Do())
            .Should().Throw<InvalidOperationException>()
            .WithMessage("expected mes");

        // Assert
        action.Should().Throw<Exception>()
            .WithMessage(
                "Expected exception message to match the equivalent of*\"expected mes*\", but*\"OxpectOd message\" does not*");
    }

    [Fact]
    public void When_subject_throws_some_exception_with_unexpected_message_it_should_throw_with_clear_description()
    {
        // Arrange
        Does subjectThatThrows = Does.Throw(new InvalidOperationException("message1"));

        try
        {
            // Act
            subjectThatThrows
                .Invoking(x => x.Do())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("message2", "because we want to test the failure {0}", "message");

            throw new XunitException("This point should not be reached");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should().Match(
                "Expected exception message to match the equivalent of \"message2\" because we want to test the failure message, but \"message1\" does not*");
        }
    }

    [Fact]
    public void When_subject_throws_some_exception_with_an_empty_message_it_should_throw_with_clear_description()
    {
        // Arrange
        Does subjectThatThrows = Does.Throw(new InvalidOperationException(""));

        try
        {
            // Act
            subjectThatThrows
                .Invoking(x => x.Do())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("message2");

            throw new XunitException("This point should not be reached");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should().Match(
                "Expected exception message to match the equivalent of \"message2\"*, but \"\"*");
        }
    }

    [Fact]
    public void
        When_subject_throws_some_exception_with_message_which_contains_complete_expected_exception_and_more_it_should_throw()
    {
        // Arrange
        Does subjectThatThrows = Does.Throw(new ArgumentNullException("someParam", "message2"));

        try
        {
            // Act
            subjectThatThrows
                .Invoking(x => x.Do("something"))
                .Should().Throw<ArgumentNullException>()
                .WithMessage("message2");

            throw new XunitException("This point should not be reached");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should().Match(
                "Expected exception message to match the equivalent of*\"message2\", but*message2*someParam*");
        }
    }

    [Fact]
    public void When_no_exception_was_thrown_but_one_was_expected_it_should_clearly_report_that()
    {
        try
        {
            // Arrange
            Does testSubject = Does.NotThrow();

            // Act
            testSubject.Invoking(x => x.Do()).Should().Throw<Exception>("because {0} should do that", "Does.Do");

            throw new XunitException("This point should not be reached");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should().Be(
                "Expected a <System.Exception> to be thrown because Does.Do should do that, but no exception was thrown.");
        }
    }

    [Fact]
    public void When_subject_throws_another_exception_than_expected_it_should_include_details_of_that_exception()
    {
        // Arrange
        var actualException = new ArgumentException();

        Does testSubject = Does.Throw(actualException);

        try
        {
            // Act
            testSubject
                .Invoking(x => x.Do())
                .Should().Throw<InvalidOperationException>("because {0} should throw that one", "Does.Do");

            throw new XunitException("This point should not be reached");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should().StartWith(
                "Expected a <System.InvalidOperationException> to be thrown because Does.Do should throw that one, but found <System.ArgumentException>:");

            ex.Message.Should().Contain(actualException.Message);
        }
    }

    [Fact]
    public void When_subject_throws_exception_with_message_with_braces_but_a_different_message_is_expected_it_should_report_that()
    {
        // Arrange
        Does subjectThatThrows = Does.Throw(new Exception("message with {}"));

        try
        {
            // Act
            subjectThatThrows
                .Invoking(x => x.Do("something"))
                .Should().Throw<Exception>()
                .WithMessage("message without");

            throw new XunitException("this point should not be reached");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should().Match(
                "Expected exception message to match the equivalent of*\"message without\"*, but*\"message with {}*");
        }
    }

    [Fact]
    public void When_asserting_with_an_aggregate_exception_type_the_asserts_should_occur_against_the_aggregate_exception()
    {
        // Arrange
        Does testSubject = Does.Throw(new AggregateException("Outer Message", new Exception("Inner Message")));

        // Act
        Action act = testSubject.Do;

        // Assert
        act.Should().Throw<AggregateException>()
            .WithMessage("Outer Message*")
            .WithInnerException<Exception>()
            .WithMessage("Inner Message");
    }

    [Fact]
    public void
        When_asserting_with_an_aggregate_exception_and_inner_exception_type_from_argument_the_asserts_should_occur_against_the_aggregate_exception()
    {
        // Arrange
        Does testSubject = Does.Throw(new AggregateException("Outer Message", new Exception("Inner Message")));

        // Act
        Action act = testSubject.Do;

        // Assert
        act.Should().Throw<AggregateException>()
            .WithMessage("Outer Message*")
            .WithInnerException(typeof(Exception))
            .WithMessage("Inner Message");
    }
}
