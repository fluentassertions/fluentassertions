using System;
using FluentAssertionsAsync.Execution;
#if NET47
using FluentAssertionsAsync.Specs.Common;
#endif
using Xunit;
using Xunit.Sdk;
using static FluentAssertionsAsync.Extensions.FluentTimeSpanExtensions;

namespace FluentAssertionsAsync.Specs.Exceptions;

public class NotThrowSpecs
{
    [Fact]
    public void When_subject_is_null_when_an_exception_should_not_be_thrown_it_should_throw()
    {
        // Arrange
        Action act = null;

        // Act
        Action action = () =>
        {
            using var _ = new AssertionScope();
            act.Should().NotThrow("because we want to test the failure {0}", "message");
        };

        // Assert
        action.Should().Throw<XunitException>()
            .WithMessage("*because we want to test the failure message*found <null>*")
            .Where(e => !e.Message.Contains("NullReferenceException"));
    }

    [Fact]
    public void When_a_specific_exception_should_not_be_thrown_but_it_was_it_should_throw()
    {
        // Arrange
        Does foo = Does.Throw(new ArgumentException("An exception was forced"));

        // Act
        Action action =
            () => foo.Invoking(f => f.Do()).Should().NotThrow<ArgumentException>("we passed valid arguments");

        // Assert
        action
            .Should().Throw<XunitException>().WithMessage(
                "Did not expect System.ArgumentException because we passed valid arguments, " +
                "but found System.ArgumentException: An exception was forced*");
    }

    [Fact]
    public void When_a_specific_exception_should_not_be_thrown_but_another_was_it_should_succeed()
    {
        // Arrange
        Does foo = Does.Throw<ArgumentException>();

        // Act / Assert
        foo.Invoking(f => f.Do()).Should().NotThrow<InvalidOperationException>();
    }

    [Fact]
    public void When_no_exception_should_be_thrown_but_it_was_it_should_throw()
    {
        // Arrange
        Does foo = Does.Throw(new ArgumentException("An exception was forced"));

        // Act
        Action action = () => foo.Invoking(f => f.Do()).Should().NotThrow("we passed valid arguments");

        // Assert
        action
            .Should().Throw<XunitException>().WithMessage(
                "Did not expect any exception because we passed valid arguments, " +
                "but found System.ArgumentException: An exception was forced*");
    }

    [Fact]
    public void When_no_exception_should_be_thrown_and_none_was_it_should_not_throw()
    {
        // Arrange
        Does foo = Does.NotThrow();

        // Act / Assert
        foo.Invoking(f => f.Do()).Should().NotThrow();
    }

    [Fact]
    public void When_subject_is_null_when_it_should_not_throw_it_should_throw()
    {
        // Arrange
        Action act = null;

        // Act
        Action action = () =>
        {
            using var _ = new AssertionScope();

            act.Should().NotThrowAfter(0.Milliseconds(), 0.Milliseconds(),
                "because we want to test the failure {0}", "message");
        };

        // Assert
        action.Should().Throw<XunitException>()
            .WithMessage("*because we want to test the failure message*found <null>*")
            .Where(e => !e.Message.Contains("NullReferenceException"));
    }

#pragma warning disable CS1998
    [Fact]
    public void When_subject_is_async_it_should_throw()
    {
        // Arrange
        Action someAsyncAction = async () => { };

        // Act
        Action action = () =>
            someAsyncAction.Should().NotThrowAfter(1.Milliseconds(), 1.Milliseconds());

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot use action assertions on an async void method.*");
    }
#pragma warning restore CS1998

    [Fact]
    public void When_wait_time_is_negative_it_should_throw()
    {
        // Arrange
        var waitTime = -1.Milliseconds();
        var pollInterval = 10.Milliseconds();
        Action someAction = () => { };

        // Act
        Action action = () =>
            someAction.Should().NotThrowAfter(waitTime, pollInterval);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("waitTime")
            .WithMessage("*must be non-negative*");
    }

    [Fact]
    public void When_poll_interval_is_negative_it_should_throw()
    {
        // Arrange
        var waitTime = 10.Milliseconds();
        var pollInterval = -1.Milliseconds();
        Action someAction = () => { };

        // Act
        Action action = () =>
            someAction.Should().NotThrowAfter(waitTime, pollInterval);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("pollInterval")
            .WithMessage("*must be non-negative*");
    }

    [Fact]
    public void When_no_exception_should_be_thrown_after_wait_time_but_it_was_it_should_throw()
    {
        // Arrange
        var waitTime = 100.Milliseconds();
        var pollInterval = 10.Milliseconds();

        var clock = new FakeClock();
        var timer = clock.StartTimer();

        Action throwLongerThanWaitTime = () =>
        {
            if (timer.Elapsed < waitTime.Multiply(1.5))
            {
                throw new ArgumentException("An exception was forced");
            }
        };

        // Act
        Action action = () =>
            throwLongerThanWaitTime.Should(clock).NotThrowAfter(waitTime, pollInterval, "we passed valid arguments");

        // Assert
        action.Should().Throw<XunitException>()
            .WithMessage("Did not expect any exceptions after 100ms because we passed valid arguments*");
    }

    [Fact]
    public void When_no_exception_should_be_thrown_after_wait_time_and_none_was_it_should_not_throw()
    {
        // Arrange
        var clock = new FakeClock();
        var timer = clock.StartTimer();
        var waitTime = 100.Milliseconds();
        var pollInterval = 10.Milliseconds();

        Action throwShorterThanWaitTime = () =>
        {
            if (timer.Elapsed <= waitTime.Divide(2))
            {
                throw new ArgumentException("An exception was forced");
            }
        };

        // Act
        Action act = () => throwShorterThanWaitTime.Should(clock).NotThrowAfter(waitTime, pollInterval);

        // Assert
        act.Should().NotThrow();
    }
}
