using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class TaskCompletionSourceAssertionSpecs
    {
        [Fact]
        public void When_TCS_completes_in_time_it_should_succeed()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds());
            subject.SetResult(true);
            timer.Complete();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_TCS_completes_in_time_and_result_is_expected_it_should_succeed()
        {
            // Arrange
            var subject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = async () => (await subject.Should(timer).CompleteWithinAsync(1.Seconds())).Which.Should().Be(42);
            subject.SetResult(42);
            timer.Complete();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_TCS_completes_in_time_and_result_is_not_expected_it_should_fail()
        {
            // Arrange
            var subject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = async () => (await subject.Should(timer).CompleteWithinAsync(1.Seconds())).Which.Should().Be(42);
            subject.SetResult(99);
            timer.Complete();

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected * to be 42, but found 99.");
        }

        [Fact]
        public void When_TCS_did_not_complete_in_time_it_should_fail()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds(), "test {0}", "testArg");
            timer.Complete();

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject to complete within 1s because test testArg.");
        }

        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void When_TCS_is_null_it_should_fail()
        {
            // Arrange
            TaskCompletionSource<bool> subject = null;

            // Act
            Func<Task> action = () => subject.Should().CompleteWithinAsync(1.Seconds());

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject to complete within 1s, but found <null>.");
        }
    }
}
