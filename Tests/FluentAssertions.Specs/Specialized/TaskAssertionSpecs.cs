using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;
using static FluentAssertions.FluentActions;

namespace FluentAssertions.Specs.Specialized;

public static class TaskAssertionSpecs
{
    public class Extension
    {
        [Fact]
        public void When_getting_the_subject_it_should_remain_unchanged()
        {
            // Arrange
            Func<Task<int>> subject = () => Task.FromResult(42);

            // Act
            Action action = () => subject.Should().Subject.As<object>().Should().BeSameAs(subject);

            // Assert
            action.Should().NotThrow("the Subject should remain the same");
        }
    }

    public class NotThrow
    {
        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            Func<Task<int>> subject = () => Task.FromResult(42);

            // Act
            Action action = () => subject.Should().Subject.As<object>().Should().BeSameAs(subject);

            // Assert
            action.Should().NotThrow("the Subject should remain the same").And.NotBeNull();
        }
    }

    public class NotThrowAfter
    {
        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            Func<Task<int>> subject = () => Task.FromResult(42);

            // Act
            Action action = () => subject.Should().Subject.As<object>().Should().BeSameAs(subject);

            // Assert
            action.Should().NotThrowAfter(1.Seconds(), 1.Seconds()).And.NotBeNull();
        }
    }

    public class CompleteWithinAsync
    {
        [Fact]
        public async Task When_subject_is_null_it_should_throw()
        {
            // Arrange
            var timeSpan = 0.Milliseconds();
            Func<Task> action = null;

            // Act
            Func<Task> testAction = () => action.Should().CompleteWithinAsync(
                timeSpan, "because we want to test the failure {0}", "message");

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_task_completes_fast_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
                taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());

            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_task_consumes_time_in_sync_portion_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t =>
                {
                    // simulate sync work longer than accepted time
                    timer.Delay(101.Milliseconds());
                    return (Task)t.Task;
                })
                .Should(timer)
                .CompleteWithinAsync(100.Milliseconds());

            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_task_completes_late_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
                taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());

            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task Canceled_tasks_are_also_completed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t => (Task)t.Task)
                .Should(timer)
                .CompleteWithinAsync(100.Milliseconds());

            taskFactory.SetCanceled();
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Excepted_tasks_unexpectedly_completed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t => (Task)t.Task)
                .Should(timer)
                .CompleteWithinAsync(100.Milliseconds());

            taskFactory.SetException(new OperationCanceledException());
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<OperationCanceledException>();
        }
    }

    public class NotCompleteWithinAsync
    {
        [Fact]
        public async Task When_subject_is_null_it_should_throw()
        {
            // Arrange
            var timeSpan = 0.Milliseconds();
            Func<Task> action = null;

            // Act
            Func<Task> testAction = () => action.Should().NotCompleteWithinAsync(
                timeSpan, "because we want to test the failure {0}", "message");

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_task_completes_fast_it_should_throw()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t => (Task)t.Task).Should(timer)
                .NotCompleteWithinAsync(100.Milliseconds());

            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_task_consumes_time_in_sync_portion_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t =>
                {
                    // simulate sync work longer than accepted time
                    timer.Delay(101.Milliseconds());
                    return (Task)t.Task;
                })
                .Should(timer)
                .NotCompleteWithinAsync(100.Milliseconds());

            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_task_completes_late_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t => (Task)t.Task).Should(timer)
                .NotCompleteWithinAsync(100.Milliseconds());

            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Canceled_tasks_are_also_completed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t => (Task)t.Task).Should(timer)
                .NotCompleteWithinAsync(100.Milliseconds());

            taskFactory.SetCanceled();
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task Excepted_tasks_unexpectedly_completed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t => (Task)t.Task).Should(timer)
                .NotCompleteWithinAsync(100.Milliseconds());

            taskFactory.SetException(new OperationCanceledException());
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<OperationCanceledException>();
        }
    }

    public class ThrowAsync
    {
        [Fact]
        public async Task When_subject_is_null_it_should_throw()
        {
            // Arrange
            Func<Task> action = null;

            // Act
            Func<Task> testAction = () => action.Should().ThrowAsync<InvalidOperationException>(
                "because we want to test the failure {0}", "message");

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_subject_is_null_in_assertion_scope_it_should_throw()
        {
            // Arrange
            Func<Task> action = null;

            // Act
            Func<Task> testAction = async () =>
            {
                using var _ = new AssertionScope();

                await action.Should().ThrowAsync<InvalidOperationException>(
                    "because we want to test the failure {0}", "message");
            };

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_task_throws_it_should_succeed()
        {
            // Act
            Func<Task> action = () =>
            {
                return
                    Awaiting(() => Task.FromException(new InvalidOperationException("foo")))
                        .Should().ThrowAsync<InvalidOperationException>();
            };

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_task_throws_unexpected_exception_it_should_fail()
        {
            // Act
            Func<Task> action = () =>
            {
                return
                    Awaiting(() => Task.FromException(new NotSupportedException("foo")))
                        .Should().ThrowAsync<InvalidOperationException>();
            };

            // Assert
            await action.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown,"
                + " but found <System.NotSupportedException>:*foo*");
        }

        [Fact]
        public async Task When_task_completes_without_exception_it_should_fail()
        {
            // Act
            Func<Task> action = () =>
            {
                return
                    Awaiting(() => Task.CompletedTask)
                        .Should().ThrowAsync<InvalidOperationException>();
            };

            // Assert
            await action.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown, but no exception was thrown.");
        }
    }

    public class ThrowWithinAsync
    {
        [Fact]
        public async Task When_subject_is_null_it_should_throw()
        {
            // Arrange
            Func<Task> action = null;

            // Act
            Func<Task> testAction = () => action.Should().ThrowWithinAsync<InvalidOperationException>(
                100.Milliseconds(), "because we want to test the failure {0}", "message");

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_subject_is_null_in_assertion_scope_it_should_throw()
        {
            // Arrange
            Func<Task> action = null;

            // Act
            Func<Task> testAction = async () =>
            {
                using var _ = new AssertionScope();

                await action.Should().ThrowWithinAsync<InvalidOperationException>(
                    100.Milliseconds(), "because we want to test the failure {0}", "message");
            };

            // Assert
            await testAction.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Theory]
        [InlineData(99)]
        [InlineData(100)]
        public async Task When_task_throws_fast_it_should_succeed(int delay)
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return taskFactory
                    .Awaiting(t => (Task)t.Task)
                    .Should(timer).ThrowWithinAsync<InvalidOperationException>(100.Ticks());
            };

            timer.Delay(delay.Ticks());
            taskFactory.SetException(new InvalidOperationException("foo"));

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_task_throws_slow_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return taskFactory
                    .Awaiting(t => (Task)t.Task)
                    .Should(timer).ThrowWithinAsync<InvalidOperationException>(100.Ticks());
            };

            timer.Delay(101.Ticks());
            taskFactory.SetException(new InvalidOperationException("foo"));
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_task_throws_asynchronous_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return Awaiting(() => (Task)taskFactory.Task)
                    .Should(timer).ThrowWithinAsync<InvalidOperationException>(1.Seconds());
            };

            _ = action.Invoke();
            taskFactory.SetException(new InvalidOperationException("foo"));

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_task_not_completes_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return taskFactory
                    .Awaiting(t => (Task)t.Task)
                    .Should(timer).ThrowWithinAsync<InvalidOperationException>(
                        100.Ticks(), "because we want to test the failure {0}", "message");
            };

            timer.Delay(101.Ticks());

            // Assert
            await action.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown within 10.0µs"
                + " because we want to test the failure message,"
                + " but no exception was thrown.");
        }

        [Fact]
        public async Task When_task_completes_without_exception_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return taskFactory
                    .Awaiting(t => (Task)t.Task)
                    .Should(timer).ThrowWithinAsync<InvalidOperationException>(100.Milliseconds());
            };

            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown within 100ms, but no exception was thrown.");
        }

        [Fact]
        public async Task When_task_throws_unexpected_exception_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return taskFactory
                    .Awaiting(t => (Task)t.Task)
                    .Should(timer).ThrowWithinAsync<InvalidOperationException>(100.Milliseconds());
            };

            taskFactory.SetException(new NotSupportedException("foo"));

            // Assert
            await action.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown within 100ms,"
                + " but found <System.NotSupportedException>:*foo*");
        }

        [Fact]
        public async Task When_task_throws_unexpected_exception_asynchronous_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return Awaiting(() => (Task)taskFactory.Task)
                    .Should(timer).ThrowWithinAsync<InvalidOperationException>(1.Seconds());
            };

            _ = action.Invoke();
            taskFactory.SetException(new NotSupportedException("foo"));

            // Assert
            await action.Should().ThrowAsync<XunitException>().WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown within 1s,"
                + " but found <System.NotSupportedException>:*foo*");
        }

        [Fact]
        public async Task When_task_is_canceled_before_timeout_it_succeeds()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return Awaiting(() => (Task)taskFactory.Task)
                    .Should(timer).ThrowWithinAsync<TaskCanceledException>(1.Seconds());
            };

            _ = action.Invoke();

            taskFactory.SetCanceled();
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_task_is_canceled_after_timeout_it_fails()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
            {
                return Awaiting(() => (Task)taskFactory.Task)
                    .Should(timer).ThrowWithinAsync<TaskCanceledException>(1.Seconds());
            };

            _ = action.Invoke();

            timer.Delay(1.Seconds());
            taskFactory.SetCanceled();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }
    }

    public class ThrowExactlyAsync
    {
        [Fact]
        public async Task Does_not_continue_assertion_on_exact_exception_type()
        {
            // Arrange
            var a = () => Task.Delay(1);

            // Act
            using var scope = new AssertionScope();
            await a.Should().ThrowExactlyAsync<InvalidOperationException>();

            // Assert
            scope.Discard().Should().ContainSingle()
                .Which.Should().Match("*InvalidOperationException*no exception*");
        }
    }

    [Collection("UIFacts")]
    public class CompleteWithinAsyncUIFacts
    {
        [UIFact]
        public async Task When_task_completes_fast_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
                taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());

            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [UIFact]
        public async Task When_task_completes_late_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () =>
                taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());

            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [UIFact]
        public async Task When_task_is_checking_synchronization_context_it_should_succeed()
        {
            // Arrange
            Func<Task> task = CheckContextAsync;

            // Act
            Func<Task> action = () => this.Awaiting(_ => task()).Should().CompleteWithinAsync(1.Seconds());

            // Assert
            await action.Should().NotThrowAsync();

            async Task CheckContextAsync()
            {
                await Task.Delay(1);
                SynchronizationContext.Current.Should().NotBeNull();
            }
        }
    }
}
