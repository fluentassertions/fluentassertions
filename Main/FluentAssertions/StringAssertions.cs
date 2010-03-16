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
            ActualValue = value;
        }

        public AndConstraint<StringAssertions> Equal(string expected)
        {
            return Equal(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> Equal(string expected, string reason, params object[] reasonParameters)
        {
            if ((expected == null) && (ActualValue != null))
            {
                FailWith("Expected string to be {0}, but found {1}.", expected, ActualValue, reason, reasonParameters);
            }

            if (ActualValue == null)
            {
                FailWith("Expected {0}{2}, but found {1}.",
                    expected, ActualValue, reason, reasonParameters);
            }

            if (ActualValue.Length < expected.Length)
            {
                FailWith("Expected {0}{2}, but {1} is too short.",
                    expected, ActualValue, reason, reasonParameters);
            }
            
            if (ActualValue.Length > expected.Length)
            {
                FailWith("Expected {0}{2}, but {1} is too long.",
                    expected, ActualValue, reason, reasonParameters);
            }

            for (int index = 0; index < ActualValue.Length; index++)
            {
                if (ActualValue[index] != expected[index])
                {
                    FailWith("Expected {0}{2}, but {1} differs near '" + ActualValue[index] + "' (index " + index + ").", 
                        expected, ActualValue, reason, reasonParameters);
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
            VerifyThat(() => (String.Compare(ActualValue, expected, StringComparison.CurrentCultureIgnoreCase) == 0),
                "Expected string equivalent to {0}{2}, but found {1}.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> NotEqual(string expected)
        {
            return NotEqual(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> NotEqual(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue != expected),
                "Expected string not to be equal to {0}{2}.", expected, ActualValue, reason, reasonParameters);

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

            VerifyThat(() => ActualValue.StartsWith(expected),
                "Expected string {1} to start with {0}{2}.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> StartWithEquivalent(string expected)
        {
            return StartWithEquivalent(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> StartWithEquivalent(string expected, string reason,
            params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.StartsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "Expected string starting with equivalent of {0}{2}, but found {1}.", expected, ActualValue, reason,
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

            VerifyThat(() => ActualValue.EndsWith(expected),
                "Expected string {1} to end with {0}{2}.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> EndWithEquivalent(string expected)
        {
            return EndWithEquivalent(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> EndWithEquivalent(string expected, string reason,
            params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "Expected string ending with equivalent of {0}{2}, but found {1}.", expected, ActualValue, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> Contain(string expected)
        {
            return Contain(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> Contain(string expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ActualValue.Contains(expected),
                "Expected string containing {0}{2}, but found {1}.", expected, ActualValue, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> BeEmpty()
        {
            return BeEmpty(String.Empty);
        }

        public virtual AndConstraint<StringAssertions> BeEmpty(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ((ActualValue != null) && (ActualValue.Length == 0)),
                "Expected empty string{2}, but found {1}.", null, ActualValue, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> NotBeEmpty()
        {
            return NotBeEmpty(String.Empty);
        }

        public virtual AndConstraint<StringAssertions> NotBeEmpty(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Length > 0),
                "Did not expect empty string{2}.", null, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> HaveLength(int expected)
        {
            return HaveLength(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> HaveLength(int expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue.Length == expected),
                "Expected string with length {0}{2}, but found string {1}.", expected, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> BeNull()
        {
            return BeNull(string.Empty);
        }

        public virtual AndConstraint<StringAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue == null),
                "Expected string to be <null>{2}, but found {1}.", null, ActualValue, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);           
        }

        public AndConstraint<StringAssertions> NotBeNull()
        {
            return NotBeNull(string.Empty);
        }
        
        public virtual AndConstraint<StringAssertions> NotBeNull(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (ActualValue != null),
                "Expected string not to be <null>{2}.", null, null, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);       
        }
    }
}