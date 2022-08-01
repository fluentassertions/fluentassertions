using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Specialized;

public static class TaskAssertionSpecs
{
    public class DefaultContext
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

        [Fact]
        public async Task When_subject_is_null_when_expecting_to_complete_it_should_throw()
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
        public async Task When_task_completes_fast_when_expecting_to_complete_it_should_succeed()
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
        public async Task When_task_consumes_time_in_sync_portion_when_expecting_to_complete_it_should_fail()
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
        public async Task When_task_completes_late_when_expecting_to_complete_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }
    }

    [Collection("UIFacts")]
    public class UIThread
    {
        [UIFact]
        public async Task When_task_completes_fast_when_expecting_to_complete_it_should_succeed()
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
        public async Task When_task_completes_late_when_expecting_to_complete_it_should_fail()
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
