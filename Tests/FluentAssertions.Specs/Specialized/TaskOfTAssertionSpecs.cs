using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
#if NETFRAMEWORK
using FluentAssertions.Specs.Common;
#endif
using FluentAssertions.Specs.Exceptions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Specialized
{
    public class TaskOfTAssertionSpecs
    {
        #region CompleteWithinAsync

        [Fact]
        public async Task When_subject_is_null_when_expecting_to_complete_async_it_should_throw()
        {
            // Arrange
            var timeSpan = 0.Milliseconds();
            Func<Task<int>> action = null;

            // Act
            Func<Task> testAction = () => action.Should().CompleteWithinAsync(
                timeSpan, "because we want to test the failure {0}", "message");

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_task_completes_fast_async_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Func<Task> action = async () =>
            {
                Func<Task<int>> func = () => taskFactory.Task;

                (await func.Should(timer).CompleteWithinAsync(100.Milliseconds()))
                    .Which.Should().Be(42);
            };

            taskFactory.SetResult(42);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_task_completes_async_and_result_is_not_expected_it_should_throw()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Func<Task> action = async () =>
            {
                Func<Task<int>> funcSubject = () => taskFactory.Task;

                (await funcSubject.Should(timer).CompleteWithinAsync(100.Milliseconds()))
                    .Which.Should().Be(42);
            };

            taskFactory.SetResult(99);
            timer.Complete();

            // Assert
            // TODO message currently shows "Expected (await funcSubject to be...", but should be "Expected funcSubject to be...",
            // maybe with or without await.
            await action.Should().ThrowAsync<XunitException>().WithMessage("*to be 42, but found 99.");
        }

        [Fact]
        public async Task When_task_completes_async_and_async_result_is_not_expected_it_should_throw()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Func<Task> action = async () =>
            {
                Func<Task<int>> funcSubject = () => taskFactory.Task;

                await funcSubject.Should(timer).CompleteWithinAsync(100.Milliseconds()).WithResult(42);
            };

            taskFactory.SetResult(99);
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>().WithMessage("Expected await funcSubject to be 42, but found 99.");
        }

        [Fact]
        public async Task When_task_completes_slow_async_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Func<Task> action = () =>
            {
                Func<Task<int>> func = () => taskFactory.Task;

                return func.Should(timer).CompleteWithinAsync(100.Milliseconds());
            };

            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        #endregion

        #region NotThrowAfterAsync

        [Fact]
        public async Task When_subject_is_null_when_expecting_to_not_throw_async_it_should_throw()
        {
            // Arrange
            Func<Task<int>> action = null;

            // Act
            Func<Task> testAction = () => action.Should().NotThrowAsync(
                "because we want to test the failure {0}", "message");

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_task_does_not_throw_async_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Func<Task> action = async () =>
            {
                Func<Task<int>> func = () => taskFactory.Task;

                (await func.Should(timer).NotThrowAsync())
                    .Which.Should().Be(42);
            };

            taskFactory.SetResult(42);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [UIFact]
        public async Task When_task_does_not_throw_async_on_UI_thread_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Func<Task> action = async () =>
            {
                Func<Task<int>> func = () => taskFactory.Task;

                (await func.Should(timer).NotThrowAsync())
                    .Which.Should().Be(42);
            };

            taskFactory.SetResult(42);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_task_throws_async_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();

            // Act
            Func<Task> action = () =>
            {
                Func<Task<int>> func = () => throw new AggregateException();

                return func.Should(timer).NotThrowAsync();
            };

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [UIFact]
        public async Task When_task_throws_async_on_UI_thread_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();

            // Act
            Func<Task> action = () =>
            {
                Func<Task<int>> func = () => throw new AggregateException();

                return func.Should(timer).NotThrowAsync();
            };

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_subject_is_null_and_expecting_to_not_throw_async_it_should_throw()
        {
            // Arrange
            var waitTime = 0.Milliseconds();
            var pollInterval = 0.Milliseconds();
            Func<Task<int>> action = null;

            // Act
            Func<Task> testAction = () => action.Should().NotThrowAfterAsync(
                waitTime, pollInterval, "because we want to test the failure {0}", "message");

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_wait_time_is_negative_and_expecting_to_not_throw_async_it_should_throw()
        {
            // Arrange
            var waitTime = -1.Milliseconds();
            var pollInterval = 10.Milliseconds();

            var asyncObject = new AsyncClass();
            Func<Task<int>> someFunc = () => asyncObject.ReturnTaskInt();

            // Act
            Func<Task> act = () => someFunc.Should().NotThrowAfterAsync(waitTime, pollInterval);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("* value of waitTime must be non-negative*");
        }

        [Fact]
        public async Task When_poll_interval_is_negative_and_expecting_to_not_throw_async_it_should_throw()
        {
            // Arrange
            var waitTime = 10.Milliseconds();
            var pollInterval = -1.Milliseconds();

            var asyncObject = new AsyncClass();
            Func<Task<int>> someFunc = () => asyncObject.ReturnTaskInt();

            // Act
            Func<Task> act = () => someFunc.Should().NotThrowAfterAsync(waitTime, pollInterval);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("* value of pollInterval must be non-negative*");
        }

        [Fact]
        public async Task When_no_exception_should_be_thrown_async_for_null_after_wait_time_it_should_throw()
        {
            // Arrange
            var waitTime = 2.Seconds();
            var pollInterval = 10.Milliseconds();

            Func<Task<int>> func = null;

            // Act
            Func<Task> action = () => func.Should()
                .NotThrowAfterAsync(waitTime, pollInterval, "we passed valid arguments");

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("*but found <null>*");
        }

        [Fact]
        public async Task When_no_exception_should_be_thrown_async_after_wait_time_but_it_was_it_should_throw()
        {
            // Arrange
            var waitTime = 2.Seconds();
            var pollInterval = 10.Milliseconds();

            var clock = new FakeClock();
            var timer = clock.StartTimer();
            clock.CompleteAfter(waitTime);

            Func<Task<int>> throwLongerThanWaitTime = async () =>
            {
                if (timer.Elapsed <= waitTime.Multiply(1.5))
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Yield();
                return 42;
            };

            // Act
            Func<Task> action = () => throwLongerThanWaitTime.Should(clock)
                .NotThrowAfterAsync(waitTime, pollInterval, "we passed valid arguments");

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Did not expect any exceptions after 2s because we passed valid arguments*");
        }

        [UIFact]
        public async Task When_no_exception_should_be_thrown_async_on_UI_thread_after_wait_time_but_it_was_it_should_throw()
        {
            // Arrange
            var waitTime = 2.Seconds();
            var pollInterval = 10.Milliseconds();

            var clock = new FakeClock();
            var timer = clock.StartTimer();
            clock.CompleteAfter(waitTime);

            Func<Task<int>> throwLongerThanWaitTime = async () =>
            {
                if (timer.Elapsed <= waitTime.Multiply(1.5))
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Yield();
                return 42;
            };

            // Act
            Func<Task> action = () => throwLongerThanWaitTime.Should(clock)
                .NotThrowAfterAsync(waitTime, pollInterval, "we passed valid arguments");

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Did not expect any exceptions after 2s because we passed valid arguments*");
        }

        [Fact]
        public async Task When_no_exception_should_be_thrown_async_after_wait_time_and_none_was_it_should_not_throw()
        {
            // Arrange
            var waitTime = 6.Seconds();
            var pollInterval = 10.Milliseconds();

            var clock = new FakeClock();
            var timer = clock.StartTimer();
            clock.Delay(waitTime);

            Func<Task<int>> throwShorterThanWaitTime = async () =>
            {
                if (timer.Elapsed <= waitTime.Divide(12))
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Yield();
                return 42;
            };

            // Act
            Func<Task> act = async () =>
            {
                (await throwShorterThanWaitTime.Should(clock).NotThrowAfterAsync(waitTime, pollInterval))
                    .Which.Should().Be(42);
            };

            // Assert
            await act.Should().NotThrowAsync();
        }

        [UIFact]
        public async Task When_no_exception_should_be_thrown_async_on_UI_thread_after_wait_time_and_none_was_it_should_not_throw()
        {
            // Arrange
            var waitTime = 6.Seconds();
            var pollInterval = 10.Milliseconds();

            var clock = new FakeClock();
            var timer = clock.StartTimer();
            clock.Delay(waitTime);

            Func<Task<int>> throwShorterThanWaitTime = async () =>
            {
                if (timer.Elapsed <= waitTime.Divide(12))
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Yield();
                return 42;
            };

            // Act
            Func<Task> act = async () =>
            {
                (await throwShorterThanWaitTime.Should(clock).NotThrowAfterAsync(waitTime, pollInterval))
                    .Which.Should().Be(42);
            };

            // Assert
            await act.Should().NotThrowAsync();
        }

        #endregion
    }
}
