using System;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class TaskOfTAssertionSpecs
    {
        [Fact]
        public void When_subject_it_null_when_expecting_to_complete_it_should_throw()
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

        [Fact]
        public void When_subject_it_null_when_expecting_to_complete_async_it_should_throw()
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
            timer.CompletesBeforeTimeout();

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

            timer.RunsIntoTimeout();

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
