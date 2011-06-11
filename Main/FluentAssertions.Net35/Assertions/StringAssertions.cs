using System;

namespace FluentAssertions.Assertions
{
    public class StringAssertions
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:System.Object" /> class.
        /// </summary>
        protected internal StringAssertions(string value)
        {
            Subject = value;
        }

        /// <summary>
        ///   Gets the object which value is being asserted.
        /// </summary>
        public string Subject { get; private set; }

        /// <summary>
        ///   Asserts that a string is equal to another string.
        /// </summary>
        /// <param name = "expected">
        ///   The expected string.
        /// </param>
        public AndConstraint<StringAssertions> Be(string expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that a string is exactly the same as another string, including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name = "expected">
        ///   The expected string.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public virtual AndConstraint<StringAssertions> Be(string expected, string reason, params object[] reasonArgs)
        {
            new StringEqualityValidator(Subject, expected, reason, reasonArgs).Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        ///   Case insensitive comparison
        /// </summary>
        public AndConstraint<StringAssertions> BeEquivalentTo(string expected)
        {
            return BeEquivalentTo(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that a string is exactly the same as another string, including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name = "expected">
        ///   The expected string.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public virtual AndConstraint<StringAssertions> BeEquivalentTo(string expected, string reason,
            params object[] reasonArgs)
        {
            var expectation = new StringEqualityValidator(Subject, expected, reason, reasonArgs)
            {
                CaseSensitive = false
            };

            expectation.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> NotBe(string expected)
        {
            return NotBe(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> NotBe(string expected, string reason,
            params object[] reasonParameters)
        {
            Execute.Verify(() => (Subject != expected),
                "Expected string not to be {0}{2}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> StartWith(string expected)
        {
            return StartWith(expected, String.Empty);
        }

        public virtual AndConstraint<StringAssertions> StartWith(string expected, string reason,
            params object[] reasonParameters)
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
                Execute.Fail("Expected string {1} to start with equivalent of {0}{2}.", expected, Subject, reason,
                    reasonParameters);
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

        public virtual AndConstraint<StringAssertions> EndWith(string expected, string reason,
            params object[] reasonParameters)
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
                Execute.Fail("Expected string {1} to end with equivalent of {0}{2}.", expected, Subject, reason,
                    reasonParameters);
            }

            Execute.Verify(() => Subject.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase),
                "Expected string {1} to end with equivalent of {0}{2}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        ///   Asserts that a string contains another (fragment of a) string.
        /// </summary>
        /// <param name = "expected">
        ///   The (fragement of a) string that the current string should contain.
        /// </param>
        public AndConstraint<StringAssertions> Contain(string expected)
        {
            return Contain(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that a string contains another (fragment of a) string.
        /// </summary>
        /// <param name = "expected">
        ///   The (fragement of a) string that the current string should contain.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public virtual AndConstraint<StringAssertions> Contain(string expected, string reason,
            params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new ArgumentException("Cannot assert string containment against <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.");
            }

            Execute.Verification
                .ForCondition(Contains(Subject, expected, StringComparison.Ordinal))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string {1} to contain {2}{0}.", Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        public AndConstraint<StringAssertions> ContainEquivalentOf(string expectedValue)
        {
            return ContainEquivalentOf(expectedValue, null);
        }

        public AndConstraint<StringAssertions> ContainEquivalentOf(string expectedValue, string reason,
            params object[] reasonParameters)
        {
            if (string.IsNullOrEmpty(expectedValue))
            {
                throw new ArgumentNullException("expectedValue",
                    "Null and empty strings are considered to be contained in all strings.");
            }

            Execute.Verify(() => Contains(Subject, expectedValue, StringComparison.CurrentCultureIgnoreCase),
                "Expected string to contain equivalent of {0}{2} but found {1}", expectedValue, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        ///   Asserts that a string does not contain another (fragment of a) string.
        /// </summary>
        /// <param name = "expected">
        ///   The (fragement of a) string that the current string should not contain.
        /// </param>
        public AndConstraint<StringAssertions> NotContain(string expected)
        {
            return NotContain(expected, null);
        }

        /// <summary>
        ///   Asserts that a string does not contain another (fragment of a) string.
        /// </summary>
        /// <param name = "expected">
        ///   The (fragement of a) string that the current string should not contain.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public virtual AndConstraint<StringAssertions> NotContain(string expected, string reason,
            params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new ArgumentException("Cannot assert string containment against <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.");
            }

            Execute.Verification
                .ForCondition(!Contains(Subject, expected, StringComparison.Ordinal))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect string {1} to contain {2}{0}.", Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        public virtual AndConstraint<StringAssertions> NotContainEquivalentOf(string expectedValue)
        {
            return NotContainEquivalentOf(expectedValue, null);
        }

        public virtual AndConstraint<StringAssertions> NotContainEquivalentOf(string expectedValue, string reason,
            params object[] reasonParamenters)
        {
            Execute.Verify(() => !Contains(Subject, expectedValue, StringComparison.CurrentCultureIgnoreCase),
                "Did not expect string to contain equivalent of {0}{2} but found {1}", expectedValue, Subject, reason,
                reasonParamenters);

            return new AndConstraint<StringAssertions>(this);
        }

        static bool Contains(string actual, string expected, StringComparison comparison)
        {
            return (actual ?? "").IndexOf(expected ?? "", comparison) >= 0;
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

        public virtual AndConstraint<StringAssertions> HaveLength(int expected, string reason,
            params object[] reasonParameters)
        {
            Execute.Verify(() => (Subject.Length == expected),
                "Expected string with length {0}{2}, but found string {1} with length " + Subject.Length + ".",
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
        ///   Ensures that a string is neither <c>null</c> or empty.
        /// </summary>
        public AndConstraint<StringAssertions> NotBeNullOrEmpty()
        {
            return NotBeNullOrEmpty(string.Empty);
        }

        /// <summary>
        ///   Ensures that a string is neither <c>null</c> or empty.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonParameters">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public virtual AndConstraint<StringAssertions> NotBeNullOrEmpty(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => !string.IsNullOrEmpty(Subject),
                "Expected string not to be <null> or empty{2}, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        ///   Ensures that a string is neither <c>null</c> or empty.
        /// </summary>
        public AndConstraint<StringAssertions> BeNullOrEmpty()
        {
            return BeNullOrEmpty(string.Empty);
        }

        /// <summary>
        ///   Ensures that a string is either <c>null</c> or empty.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonParameters">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public virtual AndConstraint<StringAssertions> BeNullOrEmpty(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => string.IsNullOrEmpty(Subject),
                "Expected string to be <null> or empty{2}, but found {1}.", null, Subject, reason, reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        ///   Ensures that a string is neither <c>null</c> nor empty nor white space
        /// </summary>
        public AndConstraint<StringAssertions> NotBeBlank()
        {
            return NotBeBlank(string.Empty);
        }

        /// <summary>
        ///   Ensures that a string is neither <c>null</c> nor empty nor white space
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonParameters">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> NotBeBlank(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => !IsBlank(Subject), "Expected non-blank string, but found {1}.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        ///   Ensures that a string is either <c>null</c> or empty or white space
        /// </summary>
        public AndConstraint<StringAssertions> BeBlank()
        {
            return BeBlank(string.Empty);
        }

        /// <summary>
        ///   Ensures that a string is either <c>null</c> or empty or white space
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonParameters">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> BeBlank(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => IsBlank(Subject), "Expected blank string, but found {1}.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<StringAssertions>(this);
        }

        private static bool IsBlank(string str)
        {
            return (str == null) || string.IsNullOrEmpty(str.Trim());
        }
    }
}