using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric
{
    [DebuggerNonUserCode]
    public class NullableNumericAssertions<T> : NumericAssertions<T> where T : struct
    {
        public NullableNumericAssertions(T? value) : base(value)
        {
        }

        /// <summary>
        /// Asserts that a nullable numeric value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>      
        public AndConstraint<NullableNumericAssertions<T>> HaveValue(string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected a value{reason}.");

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>  
        public AndConstraint<NullableNumericAssertions<T>> NotHaveValue(string because = "", params object[] reasonArgs) 
        {
            Execute.Assertion
                .ForCondition(ReferenceEquals(Subject, null))
                .BecauseOf(because, reasonArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<NullableNumericAssertions<T>>(this);
        }
    }
}