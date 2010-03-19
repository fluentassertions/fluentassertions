using System;
using System.Diagnostics;

namespace FluentAssertions
{
    public class StringAssertions : Assertions<string, StringAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        internal StringAssertions(string value)
        {
            Subject = value;
        }

        public AndConstraint<StringAssertions> Be(string expected)
        {
            return Be(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> Be(string expected, string reason, params object[] reasonParameters)
        {
            VerifyStringsAgainstNulls(expected, reason, reasonParameters);
            VerifyStringLengthEquality(expected, reason, reasonParameters);

            int indexOfMismatch = Subject.IndexOfFirstMismatch(expected);

            if (indexOfMismatch != -1)
            {
                FailWith("Expected {0}{2}, but {1} differs near '" + Subject[indexOfMismatch] + "' (index " + indexOfMismatch + ").", 
                    expected, Subject, reason, reasonParameters);
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
            VerifyStringsAgainstNulls(expected, reason, reasonParameters);
            VerifyStringLengthEquality(expected, reason, reasonParameters);

            for (int index = 0; index < Subject.Length; index++)
            {
                if (char.ToLower(Subject[index]) != char.ToLower(expected[index]))
                {
                    FailWith("Expected {0}{2}, but {1} differs near '" + Subject[index] + "' (index " + index + ").",
                        expected, Subject, reason, reasonParameters);
                }
            }

            return new AndConstraint<StringAssertions>(this);
        }

        private void VerifyStringsAgainstNulls(string expected, string reason, object[] reasonParameters)
        {
            if ((expected == null) && (Subject != null))
            {
                FailWith("Expected string to be {0}, but found {1}.", expected, Subject, reason, reasonParameters);
            }

            if (Subject == null)
            {
                FailWith("Expected {0}{2}, but found {1}.",
                    expected, Subject, reason, reasonParameters);
            }
        }

        private void VerifyStringLengthEquality(string expected, string reason, object[] reasonParameters)
        {
            if (Subject.Length < expected.Length)
            {
                FailWith("Expected {0}{2}, but {1} is too short.",
                    expected, Subject, reason, reasonParameters);
            }
            
            if (Subject.Length > expected.Length)
            {
                FailWith("Expected {0}{2}, but {1} is too long.",
                    expected, Subject, reason, reasonParameters);
            }
        }

        public AndConstraint<StringAssertions> NotBe(string expected)
        {
            return NotBe(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> NotBe(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject != expected),
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

            VerifyThat(() => Subject.StartsWith(expected),
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

            VerifyThat(() => Subject.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase),
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

            VerifyThat(() => Subject.EndsWith(expected),
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

            VerifyThat(() => Subject.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase),
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

            VerifyThat(() => Subject.Contains(expected),
                "Expected string {1} to contain {0}{2}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        public virtual AndConstraint<StringAssertions> BeEmpty(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ((Subject != null) && (Subject.Length == 0)),
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
            VerifyThat(() => (Subject.Length > 0),
                "Did not expect empty string{2}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> HaveLength(int expected)
        {
            return HaveLength(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> HaveLength(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject.Length == expected),
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
            VerifyThat(() => (Subject == null),
                "Expected string to be <null>{2}, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);           
        }

        public AndConstraint<StringAssertions> NotBeNull()
        {
            return NotBeNull(string.Empty);
        }
        
        public virtual AndConstraint<StringAssertions> NotBeNull(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject != null),
                "Expected string not to be <null>{2}.", null, null, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);       
        }
    }
}