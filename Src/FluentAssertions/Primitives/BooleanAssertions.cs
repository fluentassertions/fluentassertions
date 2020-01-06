using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class BooleanAssertions
    {
        public BooleanAssertions(bool? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public bool? Subject { get; private set; }

        /// <summary>
        /// Asserts that the value is <c>false</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<BooleanAssertions> BeFalse(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == false)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:boolean} to be false{reason}, but found {0}.", Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the value is <c>true</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<BooleanAssertions> BeTrue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == true)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:boolean} to be true{reason}, but found {0}.", Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<BooleanAssertions> Be(bool expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:boolean} to be {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }
    }
}
