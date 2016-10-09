using System;
using System.Threading.Tasks;

#if WINRT || CORE_CLR
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
                .Awaiting(async x => await x.ThrowAsync<ArgumentNullException>())
                .ShouldThrowExactly<ArgumentException>("because {0} should do that", "IFoo.Do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected type to be System.ArgumentException because IFoo.Do should do that, but found System.ArgumentNullException.");
        }

        [TestMethod]
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
                .Awaiting(async x => await x.ThrowAsync<ArgumentNullException>())
                .ShouldThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void When_async_method_throws_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(async x => await x.ThrowAsync<ArgumentException>())
                .ShouldThrow<ArgumentException>();

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
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(async x => await x.SucceedAsync())
                .ShouldThrow<InvalidOperationException>("because {0} should do that", "IFoo.Do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected System.InvalidOperationException because IFoo.Do should do that, but no exception was thrown*");
        }

        [TestMethod]
        public void When_async_method_throws_unexpected_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(async x => await x.ThrowAsync<ArgumentException>())
                .ShouldThrow<InvalidOperationException>("because {0} should do that", "IFoo.Do");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected System.InvalidOperationException because IFoo.Do should do that, but found*System.ArgumentException*");
        }

        [TestMethod]
        public void When_async_method_does_not_throw_exception_and_that_was_expected_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(async x => await x.SucceedAsync())
                .ShouldNotThrow();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_async_method_throws_exception_and_no_exception_was_expected_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(async x => await x.ThrowAsync<ArgumentException>())
                .ShouldNotThrow();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect any exception, but found a System.ArgumentException*");
        }

        [TestMethod]
        public void When_async_method_throws_exception_and_expected_not_to_throw_another_one_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(async x => await x.ThrowAsync<ArgumentException>())
                .ShouldNotThrow<InvalidOperationException>();

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
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(async x => await asyncObject.SucceedAsync())
                .ShouldNotThrow<InvalidOperationException>();

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
            var asyncObject = new AsyncClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncObject
                .Awaiting(async x => await x.ThrowAsync<ArgumentException>())
                .ShouldNotThrow<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect System.ArgumentException, but found one*");
        }

        [TestMethod]
        public void When_async_method_throws_exception_without_inner_exception_it_should_throw_AssertFailedException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> asyncAction = () =>
            {
                throw new InvalidOperationException();
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => asyncAction.ShouldNotThrow();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect any exception, but found*");
        }
    }

    internal class AsyncClass
    {
        public async Task ThrowAsync<TException>()
            where TException : Exception, new()
        {
            await Task.Factory.StartNew(() =>
            {
                throw new TException();
            });
        }

        public async Task SucceedAsync()
        {
            await Task.FromResult(0);
        }
    }
}