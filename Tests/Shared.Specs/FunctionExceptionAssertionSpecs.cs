using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using FluentAssertions.Specialized;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class FunctionExceptionAssertionSpecs
    {
        [Fact]
        public void When_method_throws_an_empty_AggregateException_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> act = () => throw new AggregateException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act2 = () => act.Should().NotThrow();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act2.Should().Throw<XunitException>();
        }

#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        [Theory]
        [MemberData(nameof(AggregateExceptionTestData))]
        public void When_the_expected_exception_is_wrapped_it_should_succeed<T>(Func<int> action, T _)
            where T : Exception
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act/Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<T>();
        }

        [Theory]
        [MemberData(nameof(AggregateExceptionTestData))]
        public void When_the_expected_exception_is_not_wrapped_it_should_fail<T>(Func<int> action, T _)
            where T : Exception
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act2 = () => action.Should().NotThrow<T>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act2.Should().Throw<XunitException>();
        }
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters

        public static IEnumerable<object[]> AggregateExceptionTestData()
        {
            var tasks = new Func<int>[]
            {
                AggregateExceptionWithLeftNestedException,
                AggregateExceptionWithRightNestedException
            };

            var types = new Exception[]
            {
                new AggregateException(),
                new ArgumentNullException(),
                new InvalidOperationException()
            };

            foreach (var task in tasks)
            {
                foreach (var type in types)
                {
                    yield return new object[] { task, type };
                }
            }

            yield return new object[] { (Func<int>)EmptyAggregateException, new AggregateException() };
        }

        private static int AggregateExceptionWithLeftNestedException()
        {
            var ex1 = new AggregateException(new InvalidOperationException());
            var ex2 = new ArgumentNullException();
            var wrapped = new AggregateException(ex1, ex2);

            throw wrapped;
        }

        private static int AggregateExceptionWithRightNestedException()
        {
            var ex1 = new ArgumentNullException();
            var ex2 = new AggregateException(new InvalidOperationException());
            var wrapped = new AggregateException(ex1, ex2);

            throw wrapped;
        }

        private static int EmptyAggregateException()
        {
            throw new AggregateException();
        }

        [Fact]
        public void Function_Assertions_should_expose_subject()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<int> subject = f.Should().Subject;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.Should().BeSameAs(f);
        }

        #region Throw
        [Fact]
        public void When_function_throws_the_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_function_throws_subclass_of_the_expected_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void When_function_does_not_throw_expected_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().Throw<InvalidCastException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*InvalidCastException*but*ArgumentNullException*");
        }

        [Fact]
        public void When_function_does_not_throw_expected_exception_but_throws_aggregate_it_should_fail_with_inner_exception()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new AggregateException(new ArgumentNullException());

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().Throw<InvalidCastException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*InvalidCastException*but*ArgumentNullException*");
        }

        [Fact]
        public void When_function_does_throw_expected_exception_but_in_aggregate_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new AggregateException(new ArgumentNullException());

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().Throw<ArgumentNullException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_function_does_not_throw_expected_exception_but_throws_aggregate_in_aggregate_it_should_fail_with_inner_exception_one_level_deep()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new AggregateException(new AggregateException(new ArgumentNullException()));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().Throw<InvalidCastException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*InvalidCastException*but*ArgumentNullException*");
        }

        [Fact]
        public void When_function_does_throw_expected_exception_but_in_aggregate_in_aggregate_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new AggregateException(new AggregateException(new ArgumentNullException()));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().Throw<ArgumentNullException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_function_does_not_throw_any_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => 12;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().Throw<InvalidCastException>("that's what I {0}", "said");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*InvalidCastException*that's what I said*but*no exception*");
        }

        #endregion

        #region ThrowExactly
        [Fact]
        public void When_function_throws_the_expected_exact_exception_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void When_function_throws_subclass_of_the_expected_exact_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().ThrowExactly<ArgumentException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*ArgumentException*but*ArgumentNullException*");
        }

        [Fact]
        public void When_function_does_not_throw_expected_exact_exception_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().ThrowExactly<InvalidCastException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*InvalidCastException*but*ArgumentNullException*");
        }

        [Fact]
        public void When_function_does_not_throw_any_exception_when_expected_exact_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => 12;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().ThrowExactly<InvalidCastException>("that's what I {0}", "said");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("Expected*InvalidCastException*that's what I said*but*no exception*");
        }

        #endregion

        #region NotThrow
        [Fact]
        public void When_function_does_not_throw_exception_and_that_was_expected_it_should_succeed_then_continue_assertion()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => 12;

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().NotThrow().Which.Should().Be(12);
        }

        [Fact]
        public void When_function_throw_exception_and_that_was_not_expected_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().NotThrow("that's what he {0}", "told me");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("*no*exception*that's what he told me*but*ArgumentNullException*");
        }

        [Fact]
        public void When_function_throw_aggregate_exception_and_that_was_not_expected_it_should_fail_with_inner_exception_in_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new AggregateException(new ArgumentNullException());

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().NotThrow("that's what he {0}", "told me");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("*no*exception*that's what he told me*but*ArgumentNullException*");
        }

        [Fact]
        public void When_function_throw_aggregate_in_aggregate_exception_and_that_was_not_expected_it_should_fail_with_most_inner_exception_in_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new AggregateException(new AggregateException(new ArgumentNullException()));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().NotThrow("that's what he {0}", "told me");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("*no*exception*that's what he told me*but*ArgumentNullException*");
        }

        [Fact]
        public void When_an_assertion_fails_on_NotThrow_succeeding_message_should_be_included()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> throwingFunction = () => throw new Exception();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
            {
                using (new AssertionScope())
                {
                    throwingFunction.Should().NotThrow()
                        .And.BeNull();
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "*Did not expect any exception*" +
                    "*to be <null>*"
                );
        }

        #endregion

        #region NotThrowAfter
        [Fact]
        public void When_wait_time_is_negative_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var waitTime = -1.Milliseconds();
            var pollInterval = 10.Milliseconds();
            Func<int> someFunc = () => 0;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                someFunc.Should().NotThrowAfter(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of waitTime must be non-negative*");
        }

        [Fact]
        public void When_poll_interval_is_negative_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var waitTime = 10.Milliseconds();
            var pollInterval = -1.Milliseconds();
            Func<int> someAction = () => 0;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                someAction.Should().NotThrowAfter(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("* value of pollInterval must be non-negative*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_after_wait_time_but_it_was_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var watch = Stopwatch.StartNew();
            var waitTime = 100.Milliseconds();
            var pollInterval = 10.Milliseconds();

            Func<int> throwLongerThanWaitTime = () =>
            {
                if (watch.Elapsed <= waitTime.Multiply(1.5))
                {
                    throw new ArgumentException("An exception was forced");
                }

                return 0;
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                throwLongerThanWaitTime.Should().NotThrowAfter(waitTime, pollInterval, "we passed valid arguments");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                         .WithMessage("Did not expect any exceptions after 0.100s because we passed valid arguments*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_after_wait_time_and_none_was_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var watch = Stopwatch.StartNew();
            var waitTime = 100.Milliseconds();
            var pollInterval = 10.Milliseconds();

            Func<int> throwShorterThanWaitTime = () =>
            {
                if (watch.Elapsed <= waitTime.Divide(2))
                {
                    throw new ArgumentException("An exception was forced");
                }

                return 0;
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => throwShorterThanWaitTime.Should().NotThrowAfter(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().NotThrow();
        }

        [Fact]
        public void When_no_exception_should_be_thrown_after_wait_time_the_func_result_should_be_returned()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var watch = Stopwatch.StartNew();
            var waitTime = 100.Milliseconds();
            var pollInterval = 10.Milliseconds();

            Func<int> throwShorterThanWaitTime = () =>
            {
                if (watch.Elapsed <= waitTime.Divide(2))
                {
                    throw new ArgumentException("An exception was forced");
                }

                return 42;
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            AndWhichConstraint<FunctionAssertions<int>, int> act =
                throwShorterThanWaitTime.Should().NotThrowAfter(waitTime, pollInterval);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Subject.Should().Be(42);
        }

        [Fact]
        public void When_an_assertion_fails_on_NotThrowAfter_succeeding_message_should_be_included()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var waitTime = TimeSpan.Zero;
            var pollInterval = TimeSpan.Zero;
            Func<int> throwingFunction = () => throw new Exception();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
            {
                using (new AssertionScope())
                {
                    throwingFunction.Should().NotThrowAfter(waitTime, pollInterval)
                        .And.BeNull();
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "*Did not expect any exceptions after*" +
                    "*to be <null>*"
                );
        }
        #endregion
        #region NotThrow<T>
        [Fact]
        public void When_function_does_not_throw_at_all_when_some_particular_exception_was_not_expected_it_should_succeed_but_then_cannot_continue_assertion()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => 12;

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            f.Should().NotThrow<ArgumentException>();
            //.Which.Should().Be(12); <- this is invalid, because NotThrow<T> does not guarantee that no exception was thrown.
        }

        [Fact]
        public void When_function_does_throw_exception_and_that_exception_was_not_expected_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new InvalidOperationException("custom message");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().NotThrow<InvalidOperationException>("it was so {0}", "fast");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>()
                .WithMessage("*Did not expect System.InvalidOperationException because it was so fast, but found one with message*custom message*");
        }

        [Fact]
        public void When_function_throw_one_exception_but_other_was_not_expected_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<int> f = () => throw new ArgumentNullException();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => f.Should().NotThrow<InvalidOperationException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_no_exception_should_be_thrown_by_sync_over_async_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Func<bool> func = () => Task.Delay(0).Wait(0);

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            func.Should().NotThrow();
        }

        #endregion
    }
}
