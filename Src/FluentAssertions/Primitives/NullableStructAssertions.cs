using System;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable struct is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableStructAssertions<TSource>
        where TSource : struct
    {
        public NullableStructAssertions(TSource? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public TSource? Subject { get; }

        /// <summary>
        /// Asserts that a nullable struct value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndWhichConstraint<NullableStructAssertions<TSource>, TSource> HaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a value{reason}.");

            return new AndWhichConstraint<NullableStructAssertions<TSource>, TSource>(this, Subject.Value);
        }

        /// <summary>
        /// Asserts that a nullable struct value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndWhichConstraint<NullableStructAssertions<TSource>, TSource> NotBeNull(string because = "", params object[] becauseArgs)
        {
            return HaveValue(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a nullable struct value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableStructAssertions<TSource>> NotHaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<NullableStructAssertions<TSource>>(this);
        }

        /// <summary>
        /// Asserts that a nullable struct value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<NullableStructAssertions<TSource>> BeNull(string because = "", params object[] becauseArgs)
        {
            return NotHaveValue(because, becauseArgs);
        }
    }
}
