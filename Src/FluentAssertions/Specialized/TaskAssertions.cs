using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Contains a number of methods to assert that a task yields the expected result.
    /// </summary>
    [DebuggerNonUserCode]
    public class TaskAssertions
    {
        public TaskAssertions(Task subject)
        {
            Subject = subject;
        }

        /// <summary>
        /// Gets the <see cref="Task"/> that is being asserted.
        /// </summary>
        public Task Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="Task"/> will complete within specified time range.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public void CompleteWithin(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            var completed = Subject.Wait(timeSpan);
            Execute.Assertion
                .ForCondition(completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);
        }

        /// <summary>
        /// Asserts that the current <see cref="Task"/> will complete within specified time range.
        /// </summary>
        /// <param name="timeSpan">The allowed time span for the operation.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public async Task CompleteWithinAsync(
            TimeSpan timeSpan, string because = "", params object[] becauseArgs)
        {
            var delayTask = Task.Delay(timeSpan);
            var completedTask = await Task.WhenAny(Subject, delayTask);
            Execute.Assertion
                .ForCondition(completedTask.Equals(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:task} to complete within {0}{reason}.", timeSpan);
        }
    }
}
