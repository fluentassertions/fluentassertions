using System;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    public class StringAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected internal StringAssertions(string value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public string Subject { get; private set; }

        public AndConstraint<StringAssertions> Be(string expected)
        {
            return Be(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> Be(string expected, string reason, params object[] reasonParameters)
        {
            if ((Subject != null) || (expected != null))
            {
                VerifyStringsAgainstNulls(expected, reason, reasonParameters);
                VerifyExpectedStringOnlyDiffersOnTrailingSpaces(Subject, expected, reason, reasonParameters);
                VerifyActualStringOnlyDiffersOnTrailingSpaces(Subject, expected, reason, reasonParameters);
                VerifyStringLengthEquality(expected, reason, reasonParameters);

                int indexOfMismatch = Subject.IndexOfFirstMismatch(expected);

                if (indexOfMismatch != -1)
                {
                    Execute.Fail("Expected {0}{2}, but {1} differs near " + Subject.Mismatch(indexOfMismatch) + ".",
                        expected, Subject, reason, reasonParameters);
                }
            }

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Case insensitive comparison
        /// </summary>
        public AndConstraint<StringAssertions> BeEquivalentTo(string expected)
        {
            return BeEquivalentTo(expected, String.Empty);
        }

        /// <summary>
        /// Case insensitive comparison
        /// </summary>
        public virtual AndConstraint<StringAssertions> BeEquivalentTo(string expected, string reason, params object[] reasonParameters)
        {
            if ((Subject != null) || (expected != null))
            {
                VerifyStringsAgainstNulls(expected, reason, reasonParameters);
                VerifyExpectedStringOnlyDiffersOnTrailingSpaces(Subject.ToLower(), expected.ToLower(), reason, reasonParameters);
                VerifyActualStringOnlyDiffersOnTrailingSpaces(Subject.ToLower(), expected.ToLower(), reason, reasonParameters);
                VerifyStringLengthEquality(expected, reason, reasonParameters);

                for (int index = 0; index < Subject.Length; index++)
                {
                    if (char.ToLower(Subject[index]) != char.ToLower(expected[index]))
                    {
                        Execute.Fail("Expected {0}{2}, but {1} differs near " + Subject.Mismatch(index) + ".",
                            expected, Subject, reason, reasonParameters);
                    }
                }
            }

            return new AndConstraint<StringAssertions>(this);
        }

        private void VerifyExpectedStringOnlyDiffersOnTrailingSpaces(string subject, string expected, string reason, params object[] reasonParameters)
        {
            if ((expected.Length > subject.Length) && (expected.TrimEnd() == subject))
            {
                Execute.Fail("Expected {0}{2}, but the expected string has trailing spaces compared to the actual string.",
                         expected, subject, reason, reasonParameters);
            }
        }

        private void VerifyActualStringOnlyDiffersOnTrailingSpaces(string subject, string expected, string reason, params object[] reasonParameters)
        {
            if ((subject.Length > expected.Length) && (subject.TrimEnd() == expected))
            {
                Execute.Fail("Expected {0}{2}, but the actual string has trailing spaces compared to the expected string.",
                         expected, subject, reason, reasonParameters);
            }
        }

        private void VerifyStringsAgainstNulls(string expected, string reason, object[] reasonParameters)
        {
            if ((expected == null) && (Subject != null))
            {
                Execute.Fail("Expected string to be {0}, but found {1}.", expected, Subject, reason, reasonParameters);
            }

            if (Subject == null)
            {
                Execute.Fail("Expected {0}{2}, but found {1}.",
                    expected, Subject, reason, reasonParameters);
            }
        }

        private void VerifyStringLengthEquality(string expected, string reason, object[] reasonParameters)
        {
            if (Subject.Length < expected.Length)
            {
                Execute.Fail("Expected {0}{2}, but {1} is too short.",
                    expected, Subject, reason, reasonParameters);
            }

            if (Subject.Length > expected.Length)
            {
                Execute.Fail("Expected {0}{2}, but {1} is too long.",
                    expected, Subject, reason, reasonParameters);
            }
        }

        public AndConstraint<StringAssertions> NotBe(string expected)
        {
            return NotBe(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> NotBe(string expected, string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => (Subject != expected),
                "Expected string not to be equal to {0}{2}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> StartWith(string expected)
        {
            return StartWith(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> StartWith(string expected, string reason, params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot compare start of string with <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare start of string with empty string.");
            }

            if (Subject == null)
            {
                Execute.Fail("Expected string {1} to start with {0}{2}.", expected, Subject, reason, reasonParameters);
            }

            Execute.Verify(() => Subject.StartsWith(expected),
                "Expected string {1} to start with {0}{2}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> StartWithEquivalent(string expected)
        {
            return StartWithEquivalent(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> StartWithEquivalent(string expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot compare string start equivalence with <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare string start equivalence with empty string.");
            }

            if (Subject == null)
            {
                Execute.Fail("Expected string {1} to start with equivalent of {0}{2}.", expected, Subject, reason, reasonParameters);
            }

            Execute.Verify(() => Subject.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "Expected string {1} to start with equivalent of {0}{2}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> EndWith(string expected)
        {
            return EndWith(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> EndWith(string expected, string reason, params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot compare string end with <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare string end with empty string.");
            }

            if (Subject == null)
            {
                Execute.Fail("Expected string {1} to end with {0}{2}.", expected, Subject, reason, reasonParameters);
            }

            Execute.Verify(() => Subject.EndsWith(expected),
                "Expected string {1} to end with {0}{2}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> EndWithEquivalent(string expected)
        {
            return EndWithEquivalent(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> EndWithEquivalent(string expected, string reason,
            params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot compare string end equivalence with <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare string end equivalence with empty string.");
            }

            if (Subject == null)
            {
                Execute.Fail("Expected string {1} to end with equivalent of {0}{2}.", expected, Subject, reason, reasonParameters);
            }

            Execute.Verify(() => Subject.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "Expected string {1} to end with equivalent of {0}{2}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> Contain(string expected)
        {
            return Contain(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> Contain(string expected, string reason, params object[] reasonParameters)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot check containment against <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot check containment against an empty string.");
            }

            if (Subject == null)
            {
                Execute.Fail("Expected string {1} to contain {0}{2}.", expected, Subject, reason, reasonParameters);
            }

            Execute.Verify(() => Subject.Contains(expected),
                "Expected string {1} to contain {0}{2}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> NotContain(string expectedValue)
        {
            return NotContain(expectedValue, null);
        }

        public virtual AndConstraint<StringAssertions> NotContain(string expectedValue, string reason, params object[] reasonParamenters)
        {
            if (Subject == null)
                throw new ArgumentNullException("Subject", "Null can not contain anything.");

            if (string.IsNullOrEmpty(expectedValue))
                throw new ArgumentNullException("expectedValue", "Null and empty strings are considered to be contained in all strings.");

            Execute.Verify(() => !Subject.Contains(expectedValue), "Expected string not containing {0} but found {1}", expectedValue, Subject, reason, reasonParamenters);

            return new AndConstraint<StringAssertions>(this);
        }

        public virtual AndConstraint<StringAssertions> NotContainEquivalentOf(string expectedValue)
        {
            return NotContainEquivalentOf(expectedValue, null);
        }

        public virtual AndConstraint<StringAssertions> NotContainEquivalentOf(string expectedValue, string reason, params object[] reasonParamenters)
        {
            if (Subject == null)
                throw new ArgumentNullException("Subject", "Null can not contain anything.");

            if (string.IsNullOrEmpty(expectedValue))
                throw new ArgumentNullException("expectedValue", "Null and empty strings are considered to be contained in all strings.");

            Execute.Verify(() => Subject.IndexOf(expectedValue, StringComparison.CurrentCultureIgnoreCase) < 0,
                "Expected string not containing equivalent of {0}{2} but found {1}", expectedValue, Subject, reason, reasonParamenters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        public virtual AndConstraint<StringAssertions> BeEmpty(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => ((Subject != null) && (Subject.Length == 0)),
                "Expected empty string{2}, but found {1}.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> NotBeEmpty()
        {
            return NotBeEmpty(String.Empty);
        }

        public virtual AndConstraint<StringAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => (Subject.Length > 0),
                "Did not expect empty string{2}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> HaveLength(int expected)
        {
            return HaveLength(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> HaveLength(int expected, string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => (Subject.Length == expected),
                "Expected string with length {0}{2}, but found string {1} with length <" + Subject.Length + ">.",
                expected, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> BeNull()
        {
            return BeNull(string.Empty);
        }

        public virtual AndConstraint<StringAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => (Subject == null),
                "Expected string to be <null>{2}, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> NotBeNull()
        {
            return NotBeNull(string.Empty);
        }

        public virtual AndConstraint<StringAssertions> NotBeNull(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => (Subject != null),
                "Expected string not to be <null>{2}.", null, null, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Ensures that a string is neither <c>null</c> or empty.
        /// </summary>
        public AndConstraint<StringAssertions> NotBeNullOrEmpty()
        {
            return NotBeNullOrEmpty(string.Empty);
        }

        /// <summary>
        /// Ensures that a string is neither <c>null</c> or empty.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public virtual AndConstraint<StringAssertions> NotBeNullOrEmpty(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => !string.IsNullOrEmpty(Subject),
                "Expected string not to be <null> or empty{2}, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Ensures that a string is neither <c>null</c> or empty.
        /// </summary>
        public AndConstraint<StringAssertions> BeNullOrEmpty()
        {
            return BeNullOrEmpty(string.Empty);
        }

        /// <summary>
        /// Ensures that a string is either <c>null</c> or empty.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public virtual AndConstraint<StringAssertions> BeNullOrEmpty(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => string.IsNullOrEmpty(Subject),
                "Expected string to be <null> or empty{2}, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Ensures that a string is neither <c>null</c> nor empty nor white space
        /// </summary>
        public AndConstraint<StringAssertions> NotBeBlank()
        {
            return NotBeBlank(string.Empty);
        }

        /// <summary>
        /// Ensures that a string is neither <c>null</c> nor empty nor white space
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> NotBeBlank(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => !IsBlank(Subject), "Expected non-blank string, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Ensures that a string is either <c>null</c> or empty or white space
        /// </summary>
        public AndConstraint<StringAssertions> BeBlank()
        {
            return BeBlank(string.Empty);
        }

        /// <summary>
        /// Ensures that a string is either <c>null</c> or empty or white space
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> BeBlank(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => IsBlank(Subject), "Expected blank string, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        static bool IsBlank(string str)
        {
            return str == null || string.IsNullOrEmpty(str.Trim());
        }
    }
}