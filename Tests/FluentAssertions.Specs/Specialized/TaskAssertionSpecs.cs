using System;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
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
        public void When_subject_is_null_when_expecting_to_complete_it_should_throw()
        {
            // Arrange
            var timer = new FakeClock();
            var timeSpan = 0.Milliseconds();
            Func<Task> action = null;

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
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Action action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithin(100.Milliseconds());
            taskFactory.SetResult(true);
            timer.CompletesBeforeTimeout();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_task_completes_slow_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Action action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithin(100.Milliseconds());
            timer.RunsIntoTimeout();

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_subject_is_null_when_expecting_to_complete_async_it_should_throw()
        {
            // Arrange
            var timer = new FakeClock();
            var timeSpan = 0.Milliseconds();
            Func<Task> action = null;

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
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());
            taskFactory.SetResult(true);
            timer.Complete();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_task_completes_slow_async_it_should_fail()
        {
            // Arrange
            var timer = new FakeClock();
            var taskFactory = new TaskCompletionSource<bool>();

            // Act
            Func<Task> action = () => taskFactory.Awaiting(t => (Task)t.Task).Should(timer).CompleteWithinAsync(100.Milliseconds());
            timer.Complete();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
