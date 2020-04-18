using System;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class TaskOfTAssertionSpecs
    {
        #region CompleteWithin
        [Fact]
        public void When_subject_is_null_when_expecting_to_complete_it_should_throw()
        {
            // Arrange
            var timer = new FakeClock();
            var timeSpan = 0.Milliseconds();
            Func<Task<int>> action = null;

            // Act
            Action testAction = () => action.Should().CompleteWithin(
                timeSpan, "because we want to test the failure {0}", "message");

            // Assert
            testAction.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public void When_task_completes_fast_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Action action = () =>
            {
                Func<Task<int>> func = () => taskFactory.Task;

                func.Should(timer).CompleteWithin(100.Milliseconds())
                    .Which.Should().Be(42);
            };

            taskFactory.SetResult(42);
            timer.CompletesBeforeTimeout();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_task_completes_slow_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Action action = () =>
            {
                Func<Task<int>> func = () => taskFactory.Task;

                func.Should(timer).CompleteWithin(100.Milliseconds());
            };

            timer.RunsIntoTimeout();

            // Assert
            action.Should().Throw<XunitException>();
        }
        #endregion

        #region CompleteWithinAsync
        [Fact]
        public void When_subject_is_null_when_expecting_to_complete_async_it_should_throw()
        {
            // Arrange
            var timer = new FakeClock();
            var timeSpan = 0.Milliseconds();
            Func<Task<int>> action = null;

            // Act
            Func<Task> testAction = () => action.Should().CompleteWithinAsync(
                timeSpan, "because we want to test the failure {0}", "message");

            // Assert
            testAction.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public void When_task_completes_fast_async_it_should_succeed()
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
            action.Should().NotThrow();
        }

        [Fact]
        public void When_task_completes_slow_async_it_should_fail()
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
            action.Should().Throw<XunitException>();
        }
        #endregion

        #region NotThrow
        [Fact]
        public void When_subject_is_null_when_not_expecting_an_exception_it_should_throw()
        {
            // Arrange
            Func<Task<int>> action = null;

            // Act
            Action testAction = () => action.Should().NotThrow("because we want to test the failure {0}", "message");

            // Assert
            testAction.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public void When_method_throws_an_empty_AggregateException_it_should_fail()
        {
            // Arrange
            Func<Task<int>> act = () => throw new AggregateException();

            // Act
            Action act2 = () => act.Should().NotThrow("because we want to test the failure {0}", "message");

            // Assert
            act2.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_task_does_not_throw_when_not_expecting_an_exception_it_should_succeed()
        {
            // Arrange
            Func<Task<int>> action = () => Task.FromResult(42);

            // Act
            Action testAction = () => action.Should().NotThrow()
                .Which.Should().Be(42);

            // Assert
            testAction.Should().NotThrow();
        }
        #endregion

        #region NotThrowAsync
        [Fact]
        public void When_subject_is_null_when_expecting_to_not_throw_async_it_should_throw()
        {
            // Arrange
            Func<Task<int>> action = null;

            // Act
            Func<Task> testAction = () => action.Should().NotThrowAsync(
                "because we want to test the failure {0}", "message");

            // Assert
            testAction.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public void When_task_does_not_throw_async_it_should_succeed()
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
            action.Should().NotThrow();
        }

        [Fact]
        public void When_task_throws_async_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<int>();

            // Act
            Func<Task> action = () =>
            {
                Func<Task<int>> func = () => throw new AggregateException();

                return func.Should(timer).NotThrowAsync();
            };

            // Assert
            action.Should().Throw<XunitException>();
        }
        #endregion

        #region NotThrowAfter
        [Fact]
        public void When_subject_is_null_it_should_throw()
        {
            // Arrange
            var waitTime = 0.Milliseconds();
            var pollInterval = 0.Milliseconds();
            Func<Task<int>> action = null;

            // Act
            Action testAction = () => action.Should().NotThrowAfter(
                waitTime, pollInterval, "because we want to test the failure {0}", "message");

            // Assert
            testAction.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public void When_wait_time_is_negative_it_should_throw()
        {
            // Arrange
            var waitTime = -1.Milliseconds();
            var pollInterval = 10.Milliseconds();
            Func<Task<int>> someFunc = () => Task.FromResult(42);

            // Act
            Action action = () =>
                someFunc.Should().NotThrowAfter(waitTime, pollInterval);

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of waitTime must be non-negative*");
        }

        [Fact]
        public void When_poll_interval_is_negative_it_should_throw()
        {
            // Arrange
            var waitTime = 10.Milliseconds();
            var pollInterval = -1.Milliseconds();
            Func<Task<int>> someAction = () => Task.FromResult(42);

            // Act
            Action action = () =>
                someAction.Should().NotThrowAfter(waitTime, pollInterval);

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of pollInterval must be non-negative*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_after_wait_time_but_it_was_it_should_throw()
        {
            // Arrange
            var clock = new FakeClock();
            var timer = clock.StartTimer();
            var waitTime = 100.Milliseconds();
            var pollInterval = 10.Milliseconds();

            Func<Task<int>> throwLongerThanWaitTime = () =>
            {
                if (timer.Elapsed <= waitTime.Multiply(1.5))
                {
                    throw new ArgumentException("An exception was forced");
                }

                return Task.FromResult(42);
            };

            // Act
            Action action = () =>
                throwLongerThanWaitTime.Should(clock).NotThrowAfter(waitTime, pollInterval, "we passed valid arguments");

            // Assert
            action.Should().Throw<XunitException>()
                         .WithMessage("Did not expect any exceptions after 0.100s because we passed valid arguments*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_after_wait_time_and_none_was_it_should_not_throw()
        {
            // Arrange
            var clock = new FakeClock();
            var timer = clock.StartTimer();
            var waitTime = 100.Milliseconds();
            var pollInterval = 10.Milliseconds();

            Func<Task<int>> throwShorterThanWaitTime = () =>
            {
                if (timer.Elapsed <= waitTime.Divide(2))
                {
                    throw new ArgumentException("An exception was forced");
                }

                return Task.FromResult(42);
            };

            // Act
            Action act = () => throwShorterThanWaitTime.Should(clock).NotThrowAfter(waitTime, pollInterval);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_no_exception_should_be_thrown_after_wait_time_the_func_result_should_be_returned()
        {
            // Arrange
            var clock = new FakeClock();
            var timer = clock.StartTimer();
            var waitTime = 100.Milliseconds();
            var pollInterval = 10.Milliseconds();

            Func<Task<int>> throwShorterThanWaitTime = () =>
            {
                if (timer.Elapsed <= waitTime.Divide(2))
                {
                    throw new ArgumentException("An exception was forced");
                }

                return Task.FromResult(42);
            };

            // Act
            Action act = () => throwShorterThanWaitTime.Should(clock).NotThrowAfter(waitTime, pollInterval)
                .Which.Should().Be(42);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assertion_fails_on_NotThrowAfter_succeeding_message_should_be_included()
        {
            // Arrange
            var waitTime = TimeSpan.Zero;
            var pollInterval = TimeSpan.Zero;
            Func<Task<int>> throwingFunction = () => throw new Exception();

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                throwingFunction.Should().NotThrowAfter(waitTime, pollInterval)
                    .And.BeNull();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "*Did not expect any exceptions after*" +
                    "*to be <null>*"
                );
        }
        #endregion

        #region NotThrowAfterAsync
        [Fact]
        public void When_subject_is_null_and_expecting_to_not_throw_async_it_should_throw()
        {
            // Arrange
            var waitTime = 0.Milliseconds();
            var pollInterval = 0.Milliseconds();
            Func<Task<int>> action = null;

            // Act
            Func<Task> testAction = () => action.Should().NotThrowAfterAsync(
                waitTime, pollInterval, "because we want to test the failure {0}", "message");

            // Assert
            testAction.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public void When_wait_time_is_negative_and_expecting_to_not_throw_async_it_should_throw()
        {
            // Arrange
            var waitTime = -1.Milliseconds();
            var pollInterval = 10.Milliseconds();

            var asyncObject = new AsyncClass();
            Func<Task<int>> someFunc = () => asyncObject.ReturnTaskInt();

            // Act
            Func<Task> act = () => someFunc.Should().NotThrowAfterAsync(waitTime, pollInterval);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of waitTime must be non-negative*");
        }

        [Fact]
        public void When_poll_interval_is_negative_and_expecting_to_not_throw_async_it_should_throw()
        {
            // Arrange
            var waitTime = 10.Milliseconds();
            var pollInterval = -1.Milliseconds();

            var asyncObject = new AsyncClass();
            Func<Task<int>> someFunc = () => asyncObject.ReturnTaskInt();

            // Act
            Func<Task> act = () => someFunc.Should().NotThrowAfterAsync(waitTime, pollInterval);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of pollInterval must be non-negative*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_async_for_null_after_wait_time_it_should_throw()
        {
            // Arrange
            var waitTime = 2.Seconds();
            var pollInterval = 10.Milliseconds();

            Func<Task<int>> func = null;

            // Act
            Func<Task> action = () => func.Should()
                .NotThrowAfterAsync(waitTime, pollInterval, "we passed valid arguments");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("*but found <null>*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_async_after_wait_time_but_it_was_it_should_throw()
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
            action.Should().Throw<XunitException>()
                .WithMessage("Did not expect any exceptions after 2s because we passed valid arguments*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_async_after_wait_time_and_none_was_it_should_not_throw()
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
            act.Should().NotThrow();
        }
        #endregion
    }
}
