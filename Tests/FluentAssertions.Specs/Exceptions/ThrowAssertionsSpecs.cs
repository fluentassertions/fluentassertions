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
        Does testSubject = Does.Throw<InvalidOperationException>();

        // Act / Assert
        testSubject.Invoking(x => x.Do()).Should().Throw();
    }

    [Fact]
    public void Succeeds_for_expected_exception_thrown_by_subject()
    {
        // Arrange
        Does testSubject = Does.Throw<InvalidOperationException>();

        // Act / Assert
        testSubject.Invoking(x => x.Do()).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Succeeds_for_any_exception_thrown_by_func()
    {
        // Arrange
        Does testSubject = Does.Throw<InvalidOperationException>();

        // Act / Assert
        testSubject.Invoking(x => x.Return()).Should().Throw();
    }

    [Fact]
    public void Succeeds_for_expected_exception_thrown_by_func()
    {
        // Arrange
        Does testSubject = Does.Throw<InvalidOperationException>();

        // Act / Assert
        testSubject.Invoking(x => x.Return()).Should().Throw<InvalidOperationException>();
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
            Does testSubject = Does.NotThrow();

            testSubject.Invoking(x => x.Do()).Should().Throw<Exception>();

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
            Does testSubject = Does.NotThrow();

            testSubject.Invoking(x => x.Return()).Should().Throw<Exception>();

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
        Does testSubject = Does.NotThrow();

        // Act / Assert
        testSubject.Invoking(x => x.Return()).Should().NotThrow()
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
