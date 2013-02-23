using System;
using System.Threading;
using System.Threading.Tasks;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Net45.Specs
{
    [TestClass]
    public class AsyncFunctionExceptionAssertionSpecs
    {
        [TestMethod]
        public void When_async_method_throws_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var slowObject = new SlowClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
            {
                Func<Task> slowFunction = async () => { await slowObject.ThrowAsync<ArgumentException>(); };
                slowFunction.ShouldThrow<ArgumentException>();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_async_method_does_not_throw_expected_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var slowObject = new SlowClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
            {
                Func<Task> slowFunction = async () => { await slowObject.SucceedAsync(); };
                slowFunction.ShouldThrow<InvalidOperationException>();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected System.InvalidOperationException, but no exception was thrown",
                    ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_async_method_throws_unexpected_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var slowObject = new SlowClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
            {
                Func<Task> slowFunction = async () => { await slowObject.ThrowAsync<ArgumentException>(); };
                slowFunction.ShouldThrow<InvalidOperationException>();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected System.InvalidOperationException, but found System.ArgumentException",
                    ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_async_method_does_not_throw_exception_and_expecting_not_to_throw_anything_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var slowObject = new SlowClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
            {
                Func<Task> slowFunction = async () => { await slowObject.SucceedAsync(); };
                slowFunction.ShouldNotThrow();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_async_method_throws_exception_and_expecting_not_to_throw_anything_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var slowObject = new SlowClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
            {
                Func<Task> slowFunction = async () => { await slowObject.ThrowAsync<ArgumentException>(); };
                slowFunction.ShouldNotThrow();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect any exception, but found a System.ArgumentException",
                    ComparisonMode.StartWith);
        }

        [TestMethod]
        public void When_async_method_throws_exception_and_expected_not_to_throw_another_one_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var slowObject = new SlowClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
            {
                Func<Task> slowFunction = async () => { await slowObject.ThrowAsync<ArgumentException>(); };
                slowFunction.ShouldNotThrow<InvalidOperationException>();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_async_method_succeeds_and_expected_not_to_throw_particular_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var slowObject = new SlowClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
            {
                Func<Task> slowFunction = async () => { await slowObject.SucceedAsync(); };
                slowFunction.ShouldNotThrow<InvalidOperationException>();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_async_method_throws_exception_expected_not_to_be_thrown_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var slowObject = new SlowClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
            {
                Func<Task> slowFunction = async () => { await slowObject.ThrowAsync<ArgumentException>(); };
                slowFunction.ShouldNotThrow<ArgumentException>();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect System.ArgumentException, but found one",
                    ComparisonMode.StartWith);
        }
    }

    internal class SlowClass
    {
        public async Task ThrowAsync<TException>()
            where TException : Exception, new()
        {
            await Task.Factory.StartNew(() =>
            {
                Sleep(500);
                throw new TException();
            });
        }

        public async Task SucceedAsync()
        {
            await Task.Factory.StartNew(() => Sleep(500));
        }

        private static void Sleep(int timeout)
        {
#if WINRT
            new ManualResetEvent(false).WaitOne(timeout);
#else
            Thread.Sleep(timeout);
#endif
        }
    }
}