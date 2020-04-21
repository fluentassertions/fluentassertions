using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using FluentAssertions.Specialized;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class ExecutionTimeAssertionsSpecs
    {
        #region BeLessOrEqualTo
        [Fact]
        public void When_the_execution_time_of_a_member_is_not_less_or_equal_to_a_limit_it_should_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(610)).Should().BeLessOrEqualTo(500.Milliseconds(),
                "we like speed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*(s.Sleep(610)) should be less or equal to 0.500s because we like speed, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_less_or_equal_to_a_limit_it_should_not_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(0)).Should().BeLessOrEqualTo(500.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_not_less_or_equal_to_a_limit_it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(510);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessOrEqualTo(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less or equal to 0.100s, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_less_or_equal_to_a_limit_it_should_not_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(100);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessOrEqualTo(1.Seconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_action_runs_indefinitely_it_should_be_stopped_and_throw_if_there_is_less_or_equal_condition()
        {
            // Arrange
            Action someAction = () =>
            {
                // lets cause a deadlock
                var semaphore = new Semaphore(0, 1); // my weapon of choice is a semaphore
                semaphore.WaitOne(); // oops
            };

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessOrEqualTo(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less or equal to 0.100s, but it required more than*");
        }
        #endregion

        #region BeLessThan
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
                "*(s.Sleep(610)) should be less than 0.500s because we like speed, but it required*");
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
        public void When_the_execution_time_of_an_action_is_not_less_than_a_limit__it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(510);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less than 0.100s, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_async_action_is_not_less_than_a_limit__it_should_throw()
        {
            // Arrange
            Func<Task> someAction = () => Task.Delay(TimeSpan.FromMilliseconds(150));

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(100.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be less than 0.100s, but it required*");
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
            Action act = () => someAction.ExecutionTime().Should().BeLessThan(2.Seconds());

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
                "*action should be less than 0.100s, but it required more than*");
        }
        #endregion

        #region BeGreaterOrEqualTo
        [Fact]
        public void When_the_execution_time_of_a_member_is_not_greater_or_equal_to_a_limit_it_should_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(100)).Should().BeGreaterOrEqualTo(1.Seconds(),
                "we like speed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*(s.Sleep(100)) should be greater or equal to 1s because we like speed, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_greater_or_equal_to_a_limit_it_should_not_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(100)).Should().BeGreaterOrEqualTo(50.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_not_greater_or_equal_to_a_limit_it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(100);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterOrEqualTo(1.Seconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be greater or equal to 1s, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_greater_or_equal_to_a_limit_it_should_not_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(100);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterOrEqualTo(50.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_action_runs_indefinitely_it_should_be_stopped_and_not_throw_if_there_is_greater_or_equal_condition()
        {
            // Arrange
            Action someAction = () =>
            {
                // lets cause a deadlock
                var semaphore = new Semaphore(0, 1); // my weapon of choice is a semaphore
                semaphore.WaitOne(); // oops
            };

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeGreaterOrEqualTo(100.Milliseconds());

            // Assert
            act.Should().NotThrow<XunitException>();
        }
        #endregion

        #region BeGreaterThan
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
        public void When_the_execution_time_of_an_action_is_not_greater_than_a_limit__it_should_throw()
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
        #endregion

        #region BeCloseTo
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
                "*(s.Sleep(200)) should be within 0.050s from 0.100s because we like speed, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_a_member_is_close_to_a_limit_it_should_not_throw()
        {
            // Arrange
            var subject = new SleepingClass();

            // Act
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(210)).Should().BeCloseTo(200.Milliseconds(),
                150.Milliseconds());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_not_close_to_a_limit__it_should_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(200);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeCloseTo(100.Milliseconds(), 50.Milliseconds());

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "*action should be within 0.050s from 0.100s, but it required*");
        }

        [Fact]
        public void When_the_execution_time_of_an_action_is_close_to_a_limit_it_should_not_throw()
        {
            // Arrange
            Action someAction = () => Thread.Sleep(210);

            // Act
            Action act = () => someAction.ExecutionTime().Should().BeCloseTo(200.Milliseconds(), 150.Milliseconds());

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
                "*action should be within 0.050s from 0.100s, but it required*");
        }
        #endregion

        #region ExecutionTime
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
            Action act = () => new ExecutionTimeAssertions(executionTime);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("executionTime");
        }
        #endregion

        internal class SleepingClass
        {
            public void Sleep(int milliseconds)
            {
                Thread.Sleep(milliseconds);
            }
        }
    }
}
