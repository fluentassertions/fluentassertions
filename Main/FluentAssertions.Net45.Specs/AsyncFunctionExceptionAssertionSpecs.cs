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
    }

    internal class SlowClass
    {
        public async Task ThrowAsync<TException>()
            where TException : Exception, new()
        {
            await Task.Factory.StartNew(() =>
            {
                Sleep(5500);
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
            Thread.Sleep(5500);
#endif
        }
    }
}