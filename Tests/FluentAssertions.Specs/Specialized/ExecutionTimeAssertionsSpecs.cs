using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertionsAsync.Extensions;
using FluentAssertionsAsync.Specialized;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Specialized;

public class ExecutionTimeAssertionsSpecs
{
    public class BeLessThanOrEqualTo
    {
        [Fact]
        public void When_the_execution_time_of_a_member_is_not_less_than_or_equal_to_a_limit_it_should_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(610)).Should().BeLessThanOrEqualTo(500.Milliseconds(),
                "we like speed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*(s.Sleep(610)) should be less than or equal to 500ms because we like speed, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_less_than_or_equal_to_a_limit_it_should_not_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(0)).Should().BeLessThanOrEqualTo(500.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_not_less_than_or_equal_to_a_limit_it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(510);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThanOrEqualTo(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less than or equal to 100ms, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_less_than_or_equal_to_a_limit_it_should_not_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(100);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThanOrEqualTo(1.Seconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_action_runs_indefinitely_it_should_be_stopped_and_throw_if_there_is_less_than_or_equal_condition()
        {
            // Arrange
            Action someAction = () =>
            {
                // lets cause a deadlock
                var semaphore = new Semaphore(0, 1); // my weapon of choice is a semaphore
                semaphore.WaitOne(); // oops
            };

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThanOrEqualTo(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less than or equal to 100ms, but it required more than*");
        }

        [Fact]
        public void Actions_with_brackets_fail_with_correctly_formatted_message()
        {
            // Arrange
            var subject = new List<object>();

            // Act
            Action act = () =>
                subject.ExecutionTimeOf(s => s.AddRange(new object[] { })).Should().BeLessThanOrEqualTo(1.Nanoseconds());

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .Which.Message.Should().Contain("{}").And.NotContain("{0}");
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act / Assert
            subject.ExecutionTimeOf(s => s.Sleep(0))
                .Should().BeLessThanOrEqualTo(500.Milliseconds())
                .And.BeCloseTo(0.Seconds(), 500.Milliseconds());
        }
    }

    public class BeLessThan
    {
        [Fact]
        public void When_the_execution_time_of_a_member_is_not_less_than_a_limit_it_should_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(610)).Should().BeLessThan(500.Milliseconds(),
                "we like speed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*(s.Sleep(610)) should be less than 500ms because we like speed, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_less_than_a_limit_it_should_not_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(0)).Should().BeLessThan(500.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_not_less_than_a_limit_it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(510);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less than 100ms, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_async_action_is_not_less_than_a_limit_it_should_throw()
        {
            // Arrange
            Func<Task> someAction = () => Task.Delay(TimeSpan.FromMilliseconds(150));

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less than 100ms, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_less_than_a_limit_it_should_not_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(100);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(2.Seconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_async_action_is_less_than_a_limit_it_should_not_throw()
        {
            // Arrange
            Func<Task> someAction = () => Task.Delay(TimeSpan.FromMilliseconds(100));

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(20.Seconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_action_runs_indefinitely_it_should_be_stopped_and_throw_if_there_is_less_than_condition()
        {
            // Arrange
            Action someAction = () =>
            {
                // lets cause a deadlock
                var semaphore = new Semaphore(0, 1); // my weapon of choice is a semaphore
                semaphore.WaitOne(); // oops
            };

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less than 100ms, but it required more than*");
        }

        [Fact]
        public void Actions_with_brackets_fail_with_correctly_formatted_message()
        {
            // Arrange
            var subject = new List<object>();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.AddRange(new object[] { })).Should().BeLessThan(1.Nanoseconds());

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .Which.Message.Should().Contain("{}").And.NotContain("{0}");
        }
    }

    public class BeGreaterThanOrEqualTo
    {
        [Fact]
        public void When_the_execution_time_of_a_member_is_not_greater_than_or_equal_to_a_limit_it_should_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(100)).Should().BeGreaterThanOrEqualTo(1.Seconds(),
                "we like speed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*(s.Sleep(100)) should be greater than or equal to 1s because we like speed, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_greater_than_or_equal_to_a_limit_it_should_not_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(100)).Should().BeGreaterThanOrEqualTo(50.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_not_greater_than_or_equal_to_a_limit_it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(100);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterThanOrEqualTo(1.Seconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be greater than or equal to 1s, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_greater_than_or_equal_to_a_limit_it_should_not_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(100);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterThanOrEqualTo(50.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_action_runs_indefinitely_it_should_be_stopped_and_not_throw_if_there_is_greater_than_or_equal_condition()
        {
            // Arrange
            Action someAction = () =>
            {
                // lets cause a deadlock
                var semaphore = new Semaphore(0, 1); // my weapon of choice is a semaphore
                semaphore.WaitOne(); // oops
            };

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterThanOrEqualTo(100.Milliseconds());

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void Actions_with_brackets_fail_with_correctly_formatted_message()
        {
            // Arrange
            var subject = new List<object>();

            // Act
            Action act = () =>
                subject.ExecutionTimeOf(s => s.AddRange(new object[] { })).Should().BeGreaterThanOrEqualTo(1.Days());

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .Which.Message.Should().Contain("{}").And.NotContain("{0}");
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act / Assert
            subject.ExecutionTimeOf(s => s.Sleep(100))
                .Should().BeGreaterThanOrEqualTo(50.Milliseconds())
                .And.BeCloseTo(0.Seconds(), 500.Milliseconds());
        }
    }

    public class BeGreaterThan
    {
        [Fact]
        public void When_the_execution_time_of_a_member_is_not_greater_than_a_limit_it_should_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(100)).Should().BeGreaterThan(1.Seconds(),
                "we like speed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*(s.Sleep(100)) should be greater than 1s because we like speed, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_greater_than_a_limit_it_should_not_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(200)).Should().BeGreaterThan(100.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_not_greater_than_a_limit_it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(100);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterThan(1.Seconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be greater than 1s, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_greater_than_a_limit_it_should_not_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(200);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterThan(100.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_action_runs_indefinitely_it_should_be_stopped_and_not_throw_if_there_is_greater_than_condition()
        {
            // Arrange
            Action someAction = () =>
            {
                // lets cause a deadlock
                var semaphore = new Semaphore(0, 1); // my weapon of choice is a semaphore
                semaphore.WaitOne(); // oops
            };

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterThan(100.Milliseconds());

            // Assert
            act.Should().NotThrow<XunitException>();
        }

        [Fact]
        public void Actions_with_brackets_fail_with_correctly_formatted_message()
        {
            // Arrange
            var subject = new List<object>();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.AddRange(new object[] { })).Should().BeGreaterThan(1.Days());

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .Which.Message.Should().Contain("{}").And.NotContain("{0}");
        }
    }

    public class BeCloseTo
    {
        [Fact]
        public void When_asserting_that_execution_time_is_close_to_a_negative_precision_it_should_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(200)).Should().BeCloseTo(100.Milliseconds(),
                -1.Ticks());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_not_close_to_a_limit_it_should_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(200)).Should().BeCloseTo(100.Milliseconds(),
                50.Milliseconds(),
                "we like speed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*(s.Sleep(200)) should be within 50ms from 100ms because we like speed, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_close_to_a_limit_it_should_not_throw()
        {
            // Arrange
            var subject = new SleepingClass();
            var timer = new TestTimer(() => 210.Milliseconds());

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(0), () => timer).Should().BeCloseTo(200.Milliseconds(),
                150.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_not_close_to_a_limit_it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(200);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeCloseTo(100.Milliseconds(), 50.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be within 50ms from 100ms, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_close_to_a_limit_it_should_not_throw()
        {
            // Arrange
            Action someAction = () => { };
            var timer = new TestTimer(() => 210.Milliseconds());

            // Act
            Action act = () => someAction.ExecutionTime(() => timer).Should().BeCloseTo(200.Milliseconds(), 15.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_action_runs_indefinitely_it_should_be_stopped_and_throw_if_there_is_be_close_to_condition()
        {
            // Arrange
            Action someAction = () =>
            {
                // lets cause a deadlock
                var semaphore = new Semaphore(0, 1); // my weapon of choice is a semaphore
                semaphore.WaitOne(); // oops
            };

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeCloseTo(100.Milliseconds(), 50.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be within 50ms from 100ms, but it required*");
        }

        [Fact]
        public void Actions_with_brackets_fail_with_correctly_formatted_message()
        {
            // Arrange
            var subject = new List<object>();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.AddRange(new object[] { }))
                .Should().BeCloseTo(1.Days(), 50.Milliseconds());

            // Assert
            act.Should().ThrowExactly<XunitException>()
                .Which.Message.Should().Contain("{}").And.NotContain("{0}");
        }
    }

    public class ExecutingTime
    {
        [Fact]
        public void When_action_runs_inside_execution_time_exceptions_are_captured_and_rethrown()
        {
            // Arrange
            Action someAction = () => throw new ArgumentException("Let's say somebody called the wrong method.");

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(200.Milliseconds());

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Let's say somebody called the wrong method.");
        }

        [Fact]
        public void Stopwatch_is_not_stopped_after_first_execution_time_assertion()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(300);

            // Act
            Action act = () =>
            {
                // I know it's not meant to be used like this,
                // but since you can, it should still give consistent results
                ExecutionTime time = someAction.ExecutionTime();
                time.Should().BeGreaterThan(100.Milliseconds());
                time.Should().BeGreaterThan(200.Milliseconds());
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_on_null_execution_it_should_throw()
        {
            // Arrange
            ExecutionTime executionTime = null;

            // Act
            Func<ExecutionTimeAssertions> act = () => new ExecutionTimeAssertions(executionTime);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("executionTime");
        }

        [Fact]
        public void When_asserting_on_null_action_it_should_throw()
        {
            // Arrange
            Action someAction = null;

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(1.Days());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("action");
        }

        [Fact]
        public void When_asserting_on_null_func_it_should_throw()
        {
            // Arrange
            Func<Task> someFunc = null;

            // Act
            Action act = () => someFunc.ExecutionTime().Should().BeLessThan(1.Days());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("action");
        }

        [Fact]
        public void When_asserting_execution_time_of_null_action_it_should_throw()
        {
            // Arrange
            object subject = null;

            // Act
            var act = () => subject.ExecutionTimeOf(s => s.ToString()).Should().BeLessThan(1.Days());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("subject");
        }

        [Fact]
        public void When_asserting_execution_time_of_null_it_should_throw()
        {
            // Arrange
            var subject = new object();

            // Act
            Action act = () => subject.ExecutionTimeOf(null).Should().BeLessThan(1.Days());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("action");
        }

        [Fact]
        public void When_accidentally_using_equals_it_should_throw_a_helpful_error()
        {
            // Arrange
            var subject = new object();

            // Act
            var act = () => subject.ExecutionTimeOf(s => s.ToString()).Should().Equals(1.Seconds());

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage(
                "Equals is not part of Fluent Assertions. Did you mean BeLessThanOrEqualTo() or BeGreaterThanOrEqualTo() instead?");
        }
    }

    internal class SleepingClass
    {
        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}
