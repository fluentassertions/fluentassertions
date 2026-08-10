using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Specialized;

public static class ValueTaskAssertionSpecs
{
    public class ThrowAsync
    {
        [Fact]
        public async Task Succeeds_for_a_value_task_that_throws_the_expected_exception()
        {
            // Arrange
            Func<ValueTask> subject = async () =>
            {
                await Task.Yield();
                throw new ArgumentException();
            };

            // Act / Assert
            await subject.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task The_value_task_must_throw_the_expected_exception()
        {
            // Arrange
            Func<ValueTask> subject = () => default;

            // Act
            Func<Task> act = () => subject.Should().ThrowAsync<ArgumentException>(
                "because we want to test the failure {0}", "message");

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public async Task Succeeds_for_a_generic_value_task_that_throws_the_expected_exception()
        {
            // Arrange
            Func<ValueTask<int>> subject = async () =>
            {
                await Task.Yield();
                throw new ArgumentException();
            };

            // Act / Assert
            await subject.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Fails_for_a_null_delegate()
        {
            // Arrange
            Func<ValueTask> subject = null;

            // Act
            Func<Task> act = () => subject.Should().ThrowAsync<ArgumentException>();

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*found <null>*");
        }
    }

    public class NotThrowAsync
    {
        [Fact]
        public async Task Succeeds_for_a_value_task_that_does_not_throw()
        {
            // Arrange
            Func<ValueTask> subject = () => default;

            // Act / Assert
            await subject.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Succeeds_for_a_generic_value_task_that_does_not_throw()
        {
            // Arrange
            Func<ValueTask<int>> subject = () => new ValueTask<int>(42);

            // Act / Assert
            (await subject.Should().NotThrowAsync()).Which.Should().Be(42);
        }

        [Fact]
        public async Task The_value_task_must_not_throw()
        {
            // Arrange
            Func<ValueTask> subject = async () =>
            {
                await Task.Yield();
                throw new ArgumentException("some message");
            };

            // Act
            Func<Task> act = () => subject.Should().NotThrowAsync(
                "because we want to test the failure {0}", "message");

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*some message*");
        }

        [Fact]
        public async Task Fails_for_a_null_generic_delegate()
        {
            // Arrange
            Func<ValueTask<int>> subject = null;

            // Act
            Func<Task> act = () => subject.Should().NotThrowAsync();

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*found <null>*");
        }

        [Fact]
        public async Task The_value_task_is_invoked_only_once()
        {
            // Arrange
            var invocations = 0;

            Func<ValueTask> subject = () =>
            {
                invocations++;
                return default;
            };

            // Act
            await subject.Should().NotThrowAsync();

            // Assert
            invocations.Should().Be(1);
        }
    }

    public class CompleteWithinAsync
    {
        [Fact]
        public async Task Succeeds_for_a_value_task_that_completes_in_time()
        {
            // Arrange
            Func<ValueTask> subject = () => default;

            // Act / Assert
            await subject.Should().CompleteWithinAsync(1.Minutes());
        }

        [Fact]
        public async Task Succeeds_for_a_generic_value_task_that_completes_in_time()
        {
            // Arrange
            Func<ValueTask<int>> subject = () => new ValueTask<int>(42);

            // Act / Assert
            (await subject.Should().CompleteWithinAsync(1.Minutes())).Which.Should().Be(42);
        }

        [Fact]
        public async Task The_value_task_must_complete_within_the_time_limit()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();

            // ReSharper disable once AccessToDisposedClosure
            Func<ValueTask> subject = () => new ValueTask(Task.Delay(Timeout.Infinite, cancellationTokenSource.Token));

            // Act
            Func<Task> act = () => subject.Should().CompleteWithinAsync(
                10.Milliseconds(), "because we want to test the failure {0}", "message");

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public async Task Fails_for_a_null_delegate()
        {
            // Arrange
            Func<ValueTask> subject = null;

            // Act
            Func<Task> act = () => subject.Should().CompleteWithinAsync(1.Minutes());

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*found <null>*");
        }
    }

    public class Subject
    {
        [Fact]
        public void Exposes_the_task_based_adapter_rather_than_the_original_delegate()
        {
            // Arrange
            Func<ValueTask> subject = () => default;

            // Act
            Func<Task> exposedSubject = subject.Should().Subject;

            // Assert
            exposedSubject.Should().NotBeNull();
            exposedSubject.As<object>().Should().NotBeSameAs(subject);
        }

        [Fact]
        public async Task Invokes_the_original_delegate_when_the_adapter_is_invoked()
        {
            // Arrange
            var invocations = 0;

            Func<ValueTask> subject = () =>
            {
                invocations++;
                return default;
            };

            // Act
            await subject.Should().Subject();

            // Assert
            invocations.Should().Be(1);
        }
    }
}
