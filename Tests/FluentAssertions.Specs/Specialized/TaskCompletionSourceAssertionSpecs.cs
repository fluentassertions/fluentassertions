using System;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Specialized;

public class TaskCompletionSourceAssertionSpecs
{
#if NET6_0_OR_GREATER
    public class NonGeneric
    {
        [Fact]
        public async Task When_it_completes_in_time_it_should_succeed()
        {
            // Arrange
            var subject = new TaskCompletionSource();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds());
            subject.SetResult();
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_it_did_not_complete_in_time_it_should_fail()
        {
            // Arrange
            var subject = new TaskCompletionSource();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds(), "test {0}", "testArg");
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected subject to complete within 1s because test testArg.");
        }

        [Fact]
        public async Task When_it_is_null_it_should_fail()
        {
            // Arrange
            TaskCompletionSource subject = null;

            // Act
            Func<Task> action = () => subject.Should().CompleteWithinAsync(1.Seconds());

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected subject to complete within 1s, but found <null>.");
        }

        [Fact]
        public async Task When_it_completes_in_time_and_it_is_not_expected_it_should_fail()
        {
            // Arrange
            var subject = new TaskCompletionSource();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).NotCompleteWithinAsync(1.Seconds(), "test {0}", "testArg");
            subject.SetResult();
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>().WithMessage("*to not complete within*because test testArg*");
        }

        [Fact]
        public async Task When_it_is_canceled_before_completion_and_it_is_not_expected_it_should_fail()
        {
            // Arrange
            var subject = new TaskCompletionSource();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).NotCompleteWithinAsync(1.Seconds());
            subject.SetCanceled();
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_it_throws_before_completion_and_it_is_not_expected_it_should_fail()
        {
            // Arrange
            var subject = new TaskCompletionSource();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).NotCompleteWithinAsync(1.Seconds());
            subject.SetException(new OperationCanceledException());
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_it_did_not_complete_in_time_and_it_is_not_expected_it_should_succeed()
        {
            // Arrange
            var subject = new TaskCompletionSource();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).NotCompleteWithinAsync(1.Seconds());
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_it_is_null_and_we_validate_to_not_complete_it_should_fail()
        {
            // Arrange
            TaskCompletionSource subject = null;

            // Act
            Func<Task> action = () => subject.Should().NotCompleteWithinAsync(1.Seconds(), "test {0}", "testArg");

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected subject to not complete within 1s because test testArg, but found <null>.");
        }

        [Fact]
        public async Task When_accidentally_using_equals_it_should_throw_a_helpful_error()
        {
            // Arrange
            var subject = new TaskCompletionSource();

            // Act
            Func<Task> action = () => Task.FromResult(subject.Should().Equals(subject));

            // Assert
            await action.Should().ThrowAsync<NotSupportedException>()
                .WithMessage("Equals is not part of Fluent Assertions. Did you mean CompleteWithinAsync() instead?");
        }

        [Fact]
        public async Task Canceled_tasks_are_also_completed()
        {
            // Arrange
            var subject = new TaskCompletionSource();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds());
            subject.SetCanceled();
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Excepted_tasks_unexpectedly_completed()
        {
            // Arrange
            var subject = new TaskCompletionSource();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds());
            subject.SetException(new OperationCanceledException());
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }
    }
#endif

    public class Generic
    {
        [Fact]
        public async Task When_it_completes_in_time_it_should_succeed()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds());
            subject.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Canceled_tasks_do_not_return_default_value()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds()).WithResult(false);
            subject.SetCanceled();
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<OperationCanceledException>();
        }

        [Fact]
        public async Task Exception_throwing_tasks_do_not_cause_a_default_value_to_be_returned()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds()).WithResult(false);
            subject.SetException(new OperationCanceledException());
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<OperationCanceledException>();
        }

        [Fact]
        public async Task When_it_completes_in_time_and_result_is_expected_it_should_succeed()
        {
            // Arrange
            var subject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = async () => (await subject.Should(timer).CompleteWithinAsync(1.Seconds())).Which.Should().Be(42);
            subject.SetResult(42);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_it_completes_in_time_and_async_result_is_expected_it_should_succeed()
        {
            // Arrange
            var subject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds()).WithResult(42);
            subject.SetResult(42);
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_it_completes_in_time_and_result_is_not_expected_it_should_fail()
        {
            // Arrange
            var testSubject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = async () =>
                (await testSubject.Should(timer).CompleteWithinAsync(1.Seconds())).Which.Should().Be(42);

            testSubject.SetResult(99);
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected *testSubject* to be 42, but found 99 (difference of 57).");
        }

        [Fact]
        public async Task When_it_completes_in_time_and_async_result_is_not_expected_it_should_fail()
        {
            // Arrange
            var testSubject = new TaskCompletionSource<int>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => testSubject.Should(timer).CompleteWithinAsync(1.Seconds()).WithResult(42);
            testSubject.SetResult(99);
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected testSubject.Result to be 42, but found 99.");
        }

        [Fact]
        public async Task When_it_did_not_complete_in_time_it_should_fail()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).CompleteWithinAsync(1.Seconds(), "test {0}", "testArg");
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected subject to complete within 1s because test testArg.");
        }

        [Fact]
        public async Task When_it_is_null_it_should_fail()
        {
            // Arrange
            TaskCompletionSource<bool> subject = null;

            // Act
            Func<Task> action = () => subject.Should().CompleteWithinAsync(1.Seconds());

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Expected subject to complete within 1s, but found <null>.");
        }

        [Fact]
        public async Task When_it_completes_in_time_and_it_is_not_expected_it_should_fail()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).NotCompleteWithinAsync(1.Seconds(), "test {0}", "testArg");
            subject.SetResult(true);
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Did not expect*to complete within*because test testArg*");
        }

        [Fact]
        public async Task Canceled_tasks_are_also_completed()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).NotCompleteWithinAsync(1.Seconds());
            subject.SetCanceled();
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task Excepted_tasks_unexpectedly_completed()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).NotCompleteWithinAsync(1.Seconds());
            subject.SetException(new OperationCanceledException());
            timer.Complete();

            // Assert
            await action.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        public async Task When_it_did_not_complete_in_time_and_it_is_not_expected_it_should_succeed()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();
            var timer = new FakeClock();

            // Act
            Func<Task> action = () => subject.Should(timer).NotCompleteWithinAsync(1.Seconds());
            timer.Complete();

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_it_is_null_and_we_validate_to_not_complete_it_should_fail()
        {
            // Arrange
            TaskCompletionSource<bool> subject = null;

            // Act
            Func<Task> action = () => subject.Should().NotCompleteWithinAsync(1.Seconds(), "test {0}", "testArg");

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Did not expect subject to complete within 1s because test testArg, but found <null>.");
        }

        [Fact]
        public async Task When_accidentally_using_equals_with_generic_it_should_throw_a_helpful_error()
        {
            // Arrange
            var subject = new TaskCompletionSource<bool>();

            // Act
            Func<Task<bool>> action = () => Task.FromResult(subject.Should().Equals(subject));

            // Assert
            await action.Should().ThrowAsync<NotSupportedException>()
                .WithMessage("Equals is not part of Fluent Assertions. Did you mean CompleteWithinAsync() instead?");
        }
    }
}
