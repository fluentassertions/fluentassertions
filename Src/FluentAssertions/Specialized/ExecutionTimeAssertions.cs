using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentAssertions.Execution;


#if !PORTABLE && !SILVERLIGHT
namespace FluentAssertions.Specialized
{
    /// <summary>
    /// Provides methods for asserting that the execution time of an <see cref="Action"/> satisfies certain conditions.
    /// </summary>
    public class ExecutionTimeAssertions
    {
        private readonly TimeSpan executionTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTimeAssertions"/> class.
        /// </summary>
        /// <param name="action">The action of which the execution time must be asserted.</param>
        public ExecutionTimeAssertions(Action action)
        {
            ActionDescription = "the action";

            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();

            executionTime = stopwatch.Elapsed;
        }

        protected string ActionDescription { get; set; }

        /// <summary>
        /// Asserts that the execution time of the operation does not exceed a specified amount of time.
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
        public void ShouldNotExceed(TimeSpan maxDuration, string because = "", params object[] becauseArgs)
        {
            if (executionTime > maxDuration)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Execution of " + ActionDescription + " should not exceed {0}{reason}, but it required {1}",
                        maxDuration, executionTime);
            }
        }
    }

    /// <summary>
    /// Provides methods for asserting that the execution time of an object member satisfies certain conditions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MemberExecutionTimeAssertions<T> : ExecutionTimeAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberExecutionTimeAssertions&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="subject">The object that exposes the method or property.</param>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        public MemberExecutionTimeAssertions(T subject, Expression<Action<T>> action) :
            base(() => action.Compile()(subject))
        {
            ActionDescription = "(" + action.Body + ")";
        }
    }
}
#endif