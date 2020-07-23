using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableBooleanAssertions : NullableBooleanAssertions<NullableBooleanAssertions>
    {
        public NullableBooleanAssertions(bool? value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableBooleanAssertions<TAssertions> : BooleanAssertions<TAssertions>
        where TAssertions : NullableBooleanAssertions<TAssertions>
    {
        public NullableBooleanAssertions(bool? value)
            : base(value)
        {
        }

        /// <summary>
        /// Asserts that a nullable boolean value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a value{reason}.");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that a nullable boolean value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeNull(string because = "", params object[] becauseArgs)
        {
            return HaveValue(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a nullable boolean value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that a nullable boolean value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeNull(string because = "", params object[] becauseArgs)
        {
            return NotHaveValue(because, becauseArgs);
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> Be(bool? expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the value is not <c>false</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeFalse(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || Subject.Value)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:nullable boolean} not to be {0}{reason}, but found {1}.", false, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the value is not <c>true</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeTrue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || !Subject.Value)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:nullable boolean} not to be {0}{reason}, but found {1}.", true, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }
    }
}
