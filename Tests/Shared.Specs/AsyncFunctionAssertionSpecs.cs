using System;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class AsyncFunctionAssertionSpecs
    {
        [Fact]
        public void When_subject_throws_subclass_of_expected_exact_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentNullException>())
                .Should().ThrowExactly<ArgumentException>("because {0} should do that", "IFoo.Do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected type to be System.ArgumentException because IFoo.Do should do that, but found System.ArgumentNullException.");
        }

        [Fact]
        public void When_subject_throws_the_expected_exact_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            asyncObject
                .Awaiting(x => x.ThrowAsync<ArgumentNullException>())
                .Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void When_function_of_task_int_in_async_method_throws_the_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();
            Func<Task<int>> f = () => asyncObject.ThrowTaskIntAsync<ArgumentNullException>(true);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task When_function_of_task_int_returns_value_result_can_be_asserted()
        {
            // Arrange/Act
            Func<Task<int>> func = () => Task.FromResult(42);

            // Assert
            func.Should().NotThrow().Which.Should().Be(42);
            (await func.Should().NotThrowAsync()).Which.Should().Be(42);
        }

        [Fact]
        public void When_function_of_task_int_in_async_method_throws_not_excepted_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();
            Func<Task<int>> f = () => asyncObject.ThrowTaskIntAsync<InvalidOperationException>(true);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void When_async_function_completes_fast_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> func = () => asyncObject.ReturnAsync(42);
            Action action = () => func.Should().CompleteWithin(200.Milliseconds()).And.Result.Should().Be(42);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_async_function_completes_slow_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> func = () => asyncObject.SlowReturnAsync(200.Milliseconds(), 42);
            Action action = () => func.Should().CompleteWithin(10.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_async_function_completes_fast_async_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> func = () => asyncObject.ReturnAsync(42);
            Func<Task> action = async () =>
                (await func.Should().CompleteWithinAsync(200.Milliseconds()))
                .And.Result.Should().Be(42);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_async_function_completes_slow_async_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> func = () => asyncObject.SlowReturnAsync(200.Milliseconds(), 42);
            Func<Task> action = () => func.Should().CompleteWithinAsync(10.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_async_function_throws_fast_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> func = () => asyncObject.ThrowTaskIntAsync<InvalidOperationException>(true);
            Action action = () => func.Should().ThrowWithin<InvalidOperationException>(200.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_async_function_throws_slow_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> func = () => asyncObject.SlowThrowTaskIntAsync<InvalidOperationException>(200.Milliseconds(), true);
            Action action = () => func.Should().ThrowWithin<InvalidOperationException>(10.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_async_function_throws_fast_async_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> func = () => asyncObject.ThrowTaskIntAsync<InvalidOperationException>(true);
            Func<Task> action = async () =>
                await func.Should().ThrowWithinAsync<InvalidOperationException>(200.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_async_function_throws_slow_async_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> func = () => asyncObject.SlowThrowTaskIntAsync<InvalidOperationException>(200.Milliseconds(), true);
            Func<Task> action = () => func.Should().ThrowWithinAsync<InvalidOperationException>(10.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public async Task When_func_throws_subclass_of_expected_async_exact_exception_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task<int>> action = async () => await asyncObject.ThrowTaskIntAsync<ArgumentNullException>(true);
            Func<Task> testAction = async () => await action.Should().ThrowExactlyAsync<ArgumentException>("ABCDE");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            (await testAction.Should().ThrowAsync<XunitException>())
                .WithMessage("*ArgumentException*ABCDE*ArgumentNullException*");
        }
    }

    internal class AsyncClass
    {
        public async Task ThrowAsync<TException>()
            where TException : Exception, new()
        {
            await Task.Factory.StartNew(() => throw new TException());
        }

        public async Task SucceedAsync()
        {
            await Task.FromResult(42);
        }

        public Task DelayAsync(TimeSpan timeSpan)
        {
            return Task.Delay(timeSpan);
        }

        public async Task<int> ReturnAsync(int value)
        {
            return await Task.FromResult(value);
        }

        public async Task<int> SlowReturnAsync(TimeSpan timeSpan, int value)
        {
            await Task.Delay(timeSpan);
            return await Task.FromResult(value);
        }

        public async Task<int> SlowThrowAsync<TException>(TimeSpan timeSpan)
            where TException : Exception, new()
        {
            await Task.Delay(timeSpan);
            throw new TException();
        }

        public Task IncompleteTask()
        {
            return new TaskCompletionSource<bool>().Task;
        }

        public async Task<int> ThrowTaskIntAsync<TException>(bool throwException)
            where TException : Exception, new()
        {
            if (throwException)
            {
                throw new TException();
            }

            return await Task.FromResult(123);
        }

        public async Task<int> SlowThrowTaskIntAsync<TException>(TimeSpan timeSpan, bool throwException)
            where TException : Exception, new()
        {
            await Task.Delay(timeSpan);
            if (throwException)
            {
                throw new TException();
            }

            return await Task.FromResult(123);
        }
    }
}
