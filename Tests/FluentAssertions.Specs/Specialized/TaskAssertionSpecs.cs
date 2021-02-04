using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Specialized
{
    public class TaskAssertionSpecs
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
        public async Task When_subject_is_null_when_expecting_to_complete_async_it_should_throw()
        {
            // Arrange
            var timer = new FakeClock();
            var timeSpan = 0.Milliseconds();
            Func<Task> action = null;

            // Act
            Func<Task> testAction = () => action.Should().CompleteWithin(
                timeSpan, "because we want to test the failure {0}", "message");

            // Assert
            await testAction.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*found <null>*");
        }

        [Fact]
        public async Task When_task_completes_fast_async_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithin(100.Milliseconds());
            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().NotThrow();
        }

        [UIFact]
        public async Task When_task_completes_on_UI_thread_fast_async_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithin(100.Milliseconds());
            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().NotThrow();
        }

        [Fact]
        public async Task When_task_completes_slow_async_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithin(100.Milliseconds());
            timer.Complete();

            // Assert
            await action.Should().Throw<XunitException>();
        }

        [UIFact]
        public async Task When_task_completes_on_UI_thread_slow_async_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithin(100.Milliseconds());
            timer.Complete();

            // Assert
            await action.Should().Throw<XunitException>();
        }

        [UIFact]
        public async Task When_task_is_checking_synchronization_context_on_UI_thread_it_should_succeed()
        {
            // Arrange
            Func<Task> task = CheckContextAsync;

            // Act
            Func<Task> action = () => this.Awaiting(x => task()).Should().CompleteWithin(1.Seconds());

            // Assert
            await action.Should().NotThrow();

            async Task CheckContextAsync()
            {
                await Task.Delay(1);
                SynchronizationContext.Current.Should().NotBeNull();
            }
        }
    }
}
