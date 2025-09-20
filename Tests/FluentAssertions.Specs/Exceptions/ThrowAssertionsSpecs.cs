using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Exceptions;

public class ThrowAssertionsSpecs
{
    [Fact]
    public void Succeeds_for_any_exception_thrown_by_subject()
    {
        // Arrange
        Action testSubject = () => throw new InvalidOperationException();

        // Act / Assert
        testSubject.Should().Throw();
    }

    [Fact]
    public void Succeeds_for_expected_exception_thrown_by_subject()
    {
        // Arrange
        Action testSubject = () => throw new InvalidOperationException();

        // Act / Assert
        testSubject.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Succeeds_for_any_exception_thrown_by_func()
    {
        // Arrange
        Func<int> testSubject = () => throw new InvalidOperationException();

        // Act / Assert
        testSubject.Should().Throw();
    }

    [Fact]
    public void Succeeds_for_expected_exception_thrown_by_func()
    {
        // Arrange
        Func<int> testSubject = () => throw new InvalidOperationException();

        // Act / Assert
        testSubject.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Succeeds_for_any_exception_thrown_by_action()
    {
        // Arrange
        var act = new Action(() => throw new InvalidOperationException("Some exception"));

        // Act / Assert
        act.Should().Throw();
    }

    [Fact]
    public void Succeeds_for_expected_exception_thrown_by_action()
    {
        // Arrange
        var act = new Action(() => throw new InvalidOperationException("Some exception"));

        // Act / Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void When_subject_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
    {
        try
        {
            Action testSubject = () => { };

            testSubject.Should().Throw<Exception>();

            throw new XunitException("Should().Throw() did not throw");
        }
        catch (XunitException ex)
        {
            ex.Message.Should().Be(
                "Expected a <System.Exception> to be thrown, but no exception was thrown.");
        }
    }

    [Fact]
    public void When_func_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
    {
        try
        {
            Func<int> testSubject = () => 42;

            testSubject.Should().Throw<Exception>();

            throw new XunitException("Should().Throw() did not throw");
        }
        catch (XunitException ex)
        {
            ex.Message.Should().Be(
                "Expected a <System.Exception> to be thrown, but no exception was thrown.");
        }
    }

    [Fact]
    public void When_func_does_not_throw_it_should_be_chainable()
    {
        // Arrange
        Func<int> testSubject = () => 42;

        // Act / Assert
        testSubject.Should().NotThrow()
            .Which.Should().Be(42);
    }

    [Fact]
    public void When_action_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
    {
        try
        {
            var act = new Action(() => { });

            act.Should().Throw<Exception>();

            throw new XunitException("Should().Throw() did not throw");
        }
        catch (XunitException ex)
        {
            ex.Message.Should().Be(
                "Expected a <System.Exception> to be thrown, but no exception was thrown.");
        }
    }
}
