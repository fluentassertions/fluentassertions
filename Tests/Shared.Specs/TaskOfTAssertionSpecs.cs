using System;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class TaskOfTAssertionSpecs
    {
        [Fact]
        public void When_task_completes_fast_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange/Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                this.DelayedReturn(10.Milliseconds(), 42)
                .Should().CompleteWithin(200.Milliseconds())
                .And.Result.Should().Be(42);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_task_completes_slow_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange/Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () =>
                this.DelayedReturn(200.Milliseconds(), 42)
                .Should().CompleteWithin(10.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_task_completes_fast_async_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange/Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = async () =>
                (await this.DelayedReturn(10.Milliseconds(), 42)
                .Should().CompleteWithinAsync(200.Milliseconds()))
                .And.Result.Should().Be(42);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().NotThrow();
        }

        [Fact]
        public void When_task_completes_slow_async_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange/Act
            //-----------------------------------------------------------------------------------------------------------
            Func<Task> action = () =>
                this.DelayedReturn(200.Milliseconds(), 42)
                .Should().CompleteWithinAsync(10.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.Should().Throw<XunitException>();
        }

        private async Task<T> DelayedReturn<T>(TimeSpan delay, T result)
        {
            await Task.Delay(delay);
            return result;
        }
    }
}
