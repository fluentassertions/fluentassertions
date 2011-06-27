using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class BooleanAssertions
    {
        protected internal BooleanAssertions(bool? value)
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
        public AndConstraint<BooleanAssertions> BeFalse()
        {
            return BeFalse(String.Empty);
        }

        /// <summary>
        /// Asserts that the value is <c>false</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<BooleanAssertions> BeFalse(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.Value)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", false, Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the value is <c>true</c>.
        /// </summary>
        public AndConstraint<BooleanAssertions> BeTrue()
        {
            return BeTrue(String.Empty);
        }

        /// <summary>
        /// Asserts that the value is <c>true</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<BooleanAssertions> BeTrue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", true, Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }

        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        public AndConstraint<BooleanAssertions> Be(bool expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the value is equal to the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<BooleanAssertions> Be(bool expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value.Equals(expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<BooleanAssertions>(this);
        }
    }
}