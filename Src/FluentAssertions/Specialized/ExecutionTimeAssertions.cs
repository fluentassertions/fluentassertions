using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentAssertions.Execution;

namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Provides methods for asserting that the execution time of an <see cref="Action"/> satisfies certain conditions.
    /// </summary>
    public class ExecutionTimeAssertions
    {
        private readonly TimeSpan executionTimeSpan;

        private readonly string actionDescription;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTime"/> class.
        /// </summary>
        /// <param name="executionTime">The execution on which time must be asserted.</param>
        public ExecutionTimeAssertions(ExecutionTime executionTime)
        {
            executionTimeSpan = executionTime.ExecutionTimeSpan;
            actionDescription = executionTime.ActionDescription;
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
            Execute.Assertion
                .ForCondition(executionTimeSpan.CompareTo(maxDuration) <= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + actionDescription + " should be less or equal to {0}{reason}, but it required {1}.",
                    maxDuration, executionTimeSpan);
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
            Execute.Assertion
                .ForCondition(executionTimeSpan.CompareTo(maxDuration) < 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + actionDescription + " should be less than {0}{reason}, but it required {1}.",
                    maxDuration, executionTimeSpan);
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
            Execute.Assertion
                .ForCondition(executionTimeSpan.CompareTo(minDuration) >= 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + actionDescription + " should be greater or equal to {0}{reason}, but it required {1}.",
                    minDuration, executionTimeSpan);
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
            Execute.Assertion
                .ForCondition(executionTimeSpan.CompareTo(minDuration) > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + actionDescription + " should be greater than {0}{reason}, but it required {1}.",
                    minDuration, executionTimeSpan);
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

            Execute.Assertion
                .ForCondition((executionTimeSpan >= minimumValue) && (executionTimeSpan <= maximumValue))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + actionDescription + " should be within {0} from {1}{reason}, but it required {2}.",
                    precision, expectedDuration, executionTimeSpan);
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

            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();

            ExecutionTimeSpan = stopwatch.Elapsed;
        }

        internal TimeSpan ExecutionTimeSpan { get; }

        internal string ActionDescription { get; }
    }

    public class MemberExecutionTime<T> : ExecutionTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberExecutionTime&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="subject">The object that exposes the method or property.</param>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        public MemberExecutionTime(T subject, Expression<Action<T>> action) :
            base(() => action.Compile()(subject), "(" + action.Body + ")")
        {
        }
    }
}

