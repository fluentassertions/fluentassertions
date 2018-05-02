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

        /// <summary>
        /// Checks the executing action if it satisfies a condition.
        /// If the execution runs into an exeption, then this will rethrow it.
        /// </summary>
        /// <param name="condition">Condition to check un the current elapsed time.</param>
        /// <param name="result">The expected result when polling is stopped.</param>
        /// <param name="stopExecution">If polling is stopped then task execution is also stopped.</param>
        /// <param name="rate">The rate at which the conition is re-checked.</param>
        private void PollUntil(Func<TimeSpan, bool> condition, bool result, TimeSpan rate)
        {
            var forcedStop = false;
            while (execution.IsRunning)
            {
                if (condition(execution.ElapsedTime) == result)
                {
                    break;
                }
                Task.Delay(rate).Wait();
            }
            if (forcedStop == false && execution.Exception != null)
            {
                // rethrow captured exception
                throw execution.Exception;
            }
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
            Func<TimeSpan, bool> condition = duration => duration.CompareTo(maxDuration) <= 0;
            PollUntil(condition, result: false, rate: maxDuration);
            Execute.Assertion
                .ForCondition(condition(execution.ElapsedTime))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + execution.ActionDescription + " should be less or equal to {0}{reason}, but it required {1}.",
                    maxDuration, execution.ElapsedTime);
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
            Func<TimeSpan, bool> condition = duration => duration.CompareTo(maxDuration) < 0;
            PollUntil(condition, result: false, rate: maxDuration);
            Execute.Assertion
                .ForCondition(condition(execution.ElapsedTime))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + execution.ActionDescription + " should be less than {0}{reason}, but it required {1}.",
                    maxDuration, execution.ElapsedTime);
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
            Func<TimeSpan, bool> condition = duration => duration.CompareTo(minDuration) >= 0;
            PollUntil(condition, result: true, rate: minDuration);
            Execute.Assertion
                .ForCondition(condition(execution.ElapsedTime))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + execution.ActionDescription + " should be greater or equal to {0}{reason}, but it required {1}.",
                    minDuration, execution.ElapsedTime);
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
            Func<TimeSpan, bool> condition = duration => duration.CompareTo(minDuration) > 0;
            PollUntil(condition, result: true, rate: minDuration);
            Execute.Assertion
                .ForCondition(condition(execution.ElapsedTime))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + execution.ActionDescription + " should be greater than {0}{reason}, but it required {1}.",
                    minDuration, execution.ElapsedTime);
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
            
            PollUntil(condition: elapsed => elapsed.CompareTo(maximumValue) < 0, result: false, rate: maximumValue);

            Execute.Assertion
                .ForCondition((execution.ElapsedTime >= minimumValue) && (execution.ElapsedTime <= maximumValue))
                .BecauseOf(because, becauseArgs)
                .FailWith("Execution of " + execution.ActionDescription + " should be within {0} from {1}{reason}, but it required {2}.",
                    precision, expectedDuration, execution.ElapsedTime);
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
            Stopwatch = new Stopwatch();
            IsRunning = true;
            Task = Task.Run(() => {
                // move stopwatch as close to action start as possible
                // so that we have to get correct time readings
                try
                {
                    Stopwatch.Start();
                    action();
                }
                catch (Exception exception)
                {
                    Exception = exception;
                }
                finally
                {
                    // ensures that we stop the stopwatch even on exceptions
                    Stopwatch.Stop();
                    IsRunning = false;
                }
            });
        }

        internal TimeSpan ElapsedTime => Stopwatch.Elapsed;

        internal bool IsRunning { get; private set; }
        
        internal string ActionDescription { get; }

        internal Task Task { get; }

        internal Exception Exception { get; private set; }

        internal readonly Stopwatch Stopwatch;
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
