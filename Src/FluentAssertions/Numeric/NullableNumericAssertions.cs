using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric
{
    [DebuggerNonUserCode]
    public class NullableNumericAssertions<T> : NumericAssertions<T>
        where T : struct
    {
        public NullableNumericAssertions(T? value)
            : base(value)
        {
        }

        /// <summary>
        /// Asserts that a nullable numeric value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableNumericAssertions<T>> HaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a value{reason}.");

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableNumericAssertions<T>> NotBeNull(string because = "", params object[] becauseArgs)
        {
            return HaveValue(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableNumericAssertions<T>> NotHaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableNumericAssertions<T>> BeNull(string because = "", params object[] becauseArgs)
        {
            return NotHaveValue(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">
        /// The predicate which must be satisfied
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableNumericAssertions<T>> Match(Expression<Func<T?, bool>> predicate,
            string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            Execute.Assertion
                .ForCondition(predicate.Compile()((T?)Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected value to match {0}{reason}, but found {1}.", predicate.Body, Subject);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }
    }
}
