using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that a task yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class TaskOfTAssertions<T>
    {
        private readonly ITimer timer;

        public TaskOfTAssertions(Task<T> subject, ITimer timer)
        {
            Subject = subject;
            this.timer = timer;
        }

        /// <summary>
        /// Gets the <see cref="Task{T}"/> that is being asserted.
        /// </summary>
        public Task<T> Subject { get; private set; }

        /// <summary>
        /// Gets the result of the <see cref="Task{T}"/>.
        /// </summary>
        public T Result => this.Subject.Result;

        /// <summary>
        /// Asserts that the current <see cref="Task{T}"/> will complete within specified time.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<TaskOfTAssertions<T>, T> CompleteWithin(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            var completed = this.timer.Wait(Subject, timeSpan);
            Execute.Assertion
                .ForCondition(completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            return new AndWhichConstraint<TaskOfTAssertions<T>, T>(this, Subject.Result);
        }

        /// <summary>
        /// Asserts that the current <see cref="Task{T}"/> will complete within specified time.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public async Task<AndWhichConstraint<TaskOfTAssertions<T>, T>> CompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            var delayTask = this.timer.DelayAsync(timeSpan);
            var completedTask = await Task.WhenAny(Subject, delayTask);
            Execute.Assertion
                .ForCondition(completedTask.Equals(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);

            return new AndWhichConstraint<TaskOfTAssertions<T>, T>(this, Subject.Result);
        }
    }
}
