using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentAssertions.Execution;
using System.Threading.Tasks;
using System.Threading;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Provides methods for asserting that the execution time of an <see cref="Action"/> satisfies certain conditions.
    /// </summary>
    public class ExecutionTimeAssertions
    {
        private readonly ExecutionTime execution;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTime"/> class.
        /// </summary>
        /// <param name="executionTime">The execution on which time must be asserted.</param>
        public ExecutionTimeAssertions(ExecutionTime executionTime)
        {
            this.execution = executionTime;
        }

        private class PollResult
        {
            /// <summary>
            /// Elapsed time of the action. (not between start of PollUntil and end of PollUntil)
            /// </summary>
            public TimeSpan Elapsed;

            /// <summary>
            /// True if the action is still running.
            /// </summary>
            public Boolean IsRunning;
        }

        /// <summary>
        /// Checks the executing action if it satisfies a condition.
        /// If the execution runs into an exeption, then this will rethrow it.
        /// </summary>
        /// <param name="condition">Condition to check on the current elapsed time.</param>
        /// <param name="expectedResult">Polling stops when condition returns the expected result.</param>
        /// <param name="rate">The rate at which the condition is re-checked.</param>
        /// <return>The elapsed time. (use this, don't measure twice)</return>
        private PollResult PollUntil(Func<TimeSpan, bool> condition, bool expectedResult, TimeSpan rate)
        {
            var poll = new PollResult
            {
                Elapsed = execution.ElapsedTime,
                IsRunning = execution.IsRunning
            };
            while (poll.IsRunning)
            {
                if (condition(poll.Elapsed) == expectedResult)
                {
                    break;
                }
                poll.IsRunning = !execution.Task.Wait(rate);
                poll.Elapsed = execution.ElapsedTime;
            }
            if (execution.Exception != null)
            {
                // rethrow captured exception
                throw execution.Exception;
            }
            return poll;
        }

        /// <summary>
        /// Asserts that the execution time of the operation is less or equal to a specified amount of time.
        /// </summary>
        /// <param name="maxDuration">
        /// The maximum allowed duration.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public void BeLessOrEqualTo(TimeSpan maxDuration, string because = "", params object[] becauseArgs)
        {
            bool Condition(TimeSpan duration) => duration.CompareTo(maxDuration) <= 0;
            var pollResult = PollUntil(Condition, expectedResult: false, rate: maxDuration);
            Execute.Assertion
                .ForCondition(Condition(pollResult.Elapsed))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " +
                          execution.ActionDescription + " should be less or equal to {0}{reason}, but it required " +
                          (pollResult.IsRunning ? "more than " : "exactly ") + "{1}.",
                    maxDuration,
                    pollResult.Elapsed);
        }

        /// <summary>
        /// Asserts that the execution time of the operation is less than a specified amount of time.
        /// </summary>
        /// <param name="maxDuration">
        /// The maximum allowed duration.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public void BeLessThan(TimeSpan maxDuration, string because = "", params object[] becauseArgs)
        {
            bool Condition(TimeSpan duration) => duration.CompareTo(maxDuration) < 0;
            var pollResult = PollUntil(Condition, expectedResult: false, rate: maxDuration);
            Execute.Assertion
                .ForCondition(Condition(execution.ElapsedTime))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " +
                          execution.ActionDescription + " should be less than {0}{reason}, but it required " +
                          (pollResult.IsRunning ? "more than " : "exactly ") + "{1}.",
                    maxDuration,
                    pollResult.Elapsed);
        }

        /// <summary>
        /// Asserts that the execution time of the operation is greater or equal to a specified amount of time.
        /// </summary>
        /// <param name="minDuration">
        /// The minimum allowed duration.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public void BeGreaterOrEqualTo(TimeSpan minDuration, string because = "", params object[] becauseArgs)
        {
            bool Condition(TimeSpan duration) => duration.CompareTo(minDuration) >= 0;
            var pollResult = PollUntil(Condition, expectedResult: true, rate: minDuration);
            Execute.Assertion
                .ForCondition(Condition(pollResult.Elapsed))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " +
                          execution.ActionDescription + " should be greater or equal to {0}{reason}, but it required " +
                          (pollResult.IsRunning ? "more than " : "exactly ") + "{1}.",
                    minDuration,
                    pollResult.Elapsed);
        }

        /// <summary>
        /// Asserts that the execution time of the operation is greater than a specified amount of time.
        /// </summary>
        /// <param name="minDuration">
        /// The minimum allowed duration.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public void BeGreaterThan(TimeSpan minDuration, string because = "", params object[] becauseArgs)
        {
            bool Condition(TimeSpan duration) => duration.CompareTo(minDuration) > 0;
            var pollResult = PollUntil(Condition, expectedResult: true, rate: minDuration);
            Execute.Assertion
                .ForCondition(Condition(pollResult.Elapsed))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " +
                          execution.ActionDescription + " should be greater than {0}{reason}, but it required " +
                          (pollResult.IsRunning ? "more than " : "exactly ") + "{1}.",
                    minDuration,
                    pollResult.Elapsed);
        }

        /// <summary>
        /// Asserts that the execution time of the operation is within the expected duration.
        /// by a specified precision.
        /// </summary>
        /// <param name="expectedDuration">
        /// The expected duration.
        /// </param>
        /// <param name="precision">
        /// The maximum amount of time which the execution time may differ from the expected duration.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public void BeCloseTo(TimeSpan expectedDuration, TimeSpan precision, string because = "", params object[] becauseArgs)
        {
            var minimumValue = expectedDuration - precision;
            var maximumValue = expectedDuration + precision;

            bool MaxCondition(TimeSpan duration) => duration <= maximumValue;
            bool MinCondition(TimeSpan duration) => duration >= minimumValue;

            // for polling we only use max condition, we don't want poll to stop if
            // elapsed time didn't even get to the acceptable range
            var pollResult = PollUntil(MaxCondition, expectedResult: false, rate: maximumValue);

            Execute.Assertion
                .ForCondition(MinCondition(pollResult.Elapsed) && MaxCondition(pollResult.Elapsed))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + execution.ActionDescription +
                          " should be within {0} from {1}{reason}, but it required " +
                          (pollResult.IsRunning ? "more than " : "exactly ") + "{2}.",
                    precision,
                    expectedDuration,
                    pollResult.Elapsed);
        }
    }

    public class ExecutionTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTime"/> class.
        /// </summary>
        /// <param name="action">The action of which the execution time must be asserted.</param>
        public ExecutionTime(Action action)
            : this(action, "the action")
        {
        }
        
        protected ExecutionTime(Action action, string actionDescription)
        {
            ActionDescription = actionDescription;
            stopwatch = new Stopwatch();
            IsRunning = true;
            Task = Task.Run(() => {
                // move stopwatch as close to action start as possible
                // so that we have to get correct time readings
                try
                {
                    stopwatch.Start();
                    action();
                }
                catch (Exception exception)
                {
                    Exception = exception;
                }
                finally
                {
                    // ensures that we stop the stopwatch even on exceptions
                    stopwatch.Stop();
                    IsRunning = false;
                }
            });
        }

        internal TimeSpan ElapsedTime => stopwatch.Elapsed;

        internal bool IsRunning { get; private set; }
        
        internal string ActionDescription { get; }

        internal Task Task { get; }

        internal Exception Exception { get; private set; }

        private readonly Stopwatch stopwatch;
    }

    public class MemberExecutionTime<T> : ExecutionTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberExecutionTime&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="subject">The object that exposes the method or property.</param>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        public MemberExecutionTime(T subject, Expression<Action<T>> action)
            : base(() => action.Compile()(subject), "(" + action.Body + ")")
        {
        }
    }
}
