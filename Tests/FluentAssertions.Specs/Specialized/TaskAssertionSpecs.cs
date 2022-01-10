using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Common;
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
        public async Task When_task_completes_fast_async_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());
            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Sync_work_in_async_method_is_taken_into_account()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory
                .Awaiting(t =>
                {
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

        [UIFact]
        public async Task When_task_completes_on_UI_thread_fast_async_it_should_succeed()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());
            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_task_completes_slow_async_it_should_fail()
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

        [UIFact]
        public async Task When_task_completes_on_UI_thread_slow_async_it_should_fail()
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

        [UIFact]
        public async Task When_task_is_checking_synchronization_context_on_UI_thread_it_should_succeed()
        {
            // Arrange
            Func<Task> task = CheckContextAsync;

            // Act
            Func<Task> action = () => this.Awaiting(x => task()).Should().CompleteWithinAsync(1.Seconds());

            // Assert
            await action.Should().NotThrowAsync();

            async Task CheckContextAsync()
            {
                await Task.Delay(1);
                SynchronizationContext.Current.Should().NotBeNull();
            }
        }

        [UIFact]
        public async Task When_resolving_async_to_main_thread_variable_name_used_for_assertion_should_be_detected()
        {
            // Arrange
            const string someText = "Hello";
            Func<Task> task = async () => await Task.Yield();

            // Act
            await task.Should().NotThrowAsync();
            Action act = () => someText.Should().Be("Hi");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*someText*", "it should capture the variable name");
        }
    }
}
