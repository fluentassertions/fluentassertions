using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="Guid"/> is in the correct state.
    /// </summary>
    [DebuggerNonUserCode]
    public class GuidAssertions
    {
        protected internal GuidAssertions(Guid? value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public Guid? Subject { get; private set; }

        #region BeEmpty / NotBeEmpty

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is <see cref="Guid.Empty"/>.
        /// </summary>
        public AndConstraint<GuidAssertions> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GuidAssertions> BeEmpty(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition((Subject.HasValue) && (Subject.Value == Guid.Empty))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected empty GUID{reason}, but found {0}.", Subject);

            return new AndConstraint<GuidAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is not <see cref="Guid.Empty"/>.
        /// </summary>
        public AndConstraint<GuidAssertions> NotBeEmpty()
        {
            return NotBeEmpty(String.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is not <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GuidAssertions> NotBeEmpty(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition((Subject.HasValue) && (Subject.Value != Guid.Empty))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect empty GUID{reason}.");

            return new AndConstraint<GuidAssertions>(this);
        }

        #endregion

        #region Be / NotBe

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is equal to the <paramref name="expected"/> GUID.
        /// </summary>
        /// <param name="expected">The expected value to compare the actual value with.</param>
        public AndConstraint<GuidAssertions> Be(Guid expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is equal to the <paramref name="expected"/> GUID.
        /// </summary>
        /// <param name="expected">The expected <see cref="string"/> value to compare the actual value with.</param>
        public AndConstraint<GuidAssertions> Be(string expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is equal to the <paramref name="expected"/> GUID.
        /// </summary>
        /// <param name="expected">The expected <see cref="string"/> value to compare the actual value with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GuidAssertions> Be(string expected, string reason, params object[] reasonArgs)
        {
            var expectedGuid = new Guid(expected);
            return Be(expectedGuid, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is equal to the <paramref name="expected"/> GUID.
        /// </summary>
        /// <param name="expected">The expected value to compare the actual value with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GuidAssertions> Be(Guid expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Equals(expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected GUID to be {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<GuidAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is not equal to the <paramref name="unexpected"/> GUID.
        /// </summary>
        /// <param name="unexpected">The unexpected value to compare the actual value with.</param>
        public AndConstraint<GuidAssertions> NotBe(Guid unexpected)
        {
            return NotBe(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="Guid"/> is not equal to the <paramref name="unexpected"/> GUID.
        /// </summary>
        /// <param name="unexpected">The unexpected value to compare the actual value with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<GuidAssertions> NotBe(Guid unexpected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.Equals(unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect GUID to be {0}{reason}.", Subject);

            return new AndConstraint<GuidAssertions>(this);
        }

        #endregion
    }
}