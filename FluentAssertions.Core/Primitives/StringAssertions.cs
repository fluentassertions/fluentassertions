using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using FluentAssertions.Execution;

using System.Linq;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="string"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class StringAssertions : ReferenceTypeAssertions<string, StringAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public StringAssertions(string value)
        {
            Subject = value;
        }

        /// <summary>
        /// Asserts that a string is exactly the same as another string, including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> Be(string expected, string reason = "", params object[] reasonArgs)
        {
            new StringEqualityValidator(Subject, expected, StringComparison.CurrentCulture, reason, reasonArgs).Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="string"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<StringAssertions> BeOneOf(params string[] validValues)
        {
            return BeOneOf(validValues, String.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="string"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> BeOneOf(IEnumerable<string> validValues, string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(validValues.Contains(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value to be one of {0}{reason}, but found {1}.", validValues, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is exactly the same as another string, including any leading or trailing whitespace, with 
        /// the exception of the casing.
        /// </summary>
        /// <param name="expected">
        /// The string that the subject is expected to be equivalent to.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> BeEquivalentTo(string expected, string reason = "",
            params object[] reasonArgs)
        {
            var expectation = new StringEqualityValidator(
                Subject, expected, StringComparison.CurrentCultureIgnoreCase, reason, reasonArgs);

            expectation.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is not exactly the same as the specified <paramref name="unexpected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to be equivalent to.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotBe(string unexpected, string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != unexpected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string not to be {0}{reason}.", unexpected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string matches a wildcard pattern.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> Match(string wildcardPattern, string reason = "", params object[] reasonArgs)
        {
            new StringWildcardMatchingValidator(Subject, wildcardPattern, reason, reasonArgs).Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not match a wildcard pattern.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotMatch(string wildcardPattern, string reason = "", params object[] reasonArgs)
        {
            new StringWildcardMatchingValidator(Subject, wildcardPattern, reason, reasonArgs)
            {
                Negate = true
            }.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string matches a wildcard pattern.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> MatchEquivalentOf(string wildcardPattern, string reason = "",
            params object[] reasonArgs)
        {
            var validator = new StringWildcardMatchingValidator(Subject, wildcardPattern, reason, reasonArgs)
            {
                IgnoreCase = true,
                IgnoreNewLineDifferences = true
            };

            validator.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not match a wildcard pattern.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotMatchEquivalentOf(string wildcardPattern, string reason = "",
            params object[] reasonArgs)
        {
            var validator = new StringWildcardMatchingValidator(Subject, wildcardPattern, reason, reasonArgs)
            {
                IgnoreCase = true,
                IgnoreNewLineDifferences = true,
                Negate = true
            };

            validator.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string matches a regular expression.
        /// </summary>
        /// <param name="regularExpression">
        /// The regular expression with which the subject is matched.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> MatchRegex(string regularExpression, string reason = "", params object[] reasonArgs)
        {
          if (regularExpression == null)
          {
            throw new NullReferenceException("Cannot match string against <null>.");
          }

          Execute.Assertion
              .ForCondition(!ReferenceEquals(Subject, null))
              .UsingLineBreaks
              .BecauseOf(reason, reasonArgs)
              .FailWith("Expected string to match regex {0}{reason}, but it was <null>.", regularExpression);

          bool isMatch;
          try
          {
            isMatch = Regex.IsMatch(Subject, regularExpression);
          }
          catch (ArgumentException)
          {
            throw new ArgumentException(
              string.Format(
                "Cannot match string against \"{0}\" because it is not a valid regular expression.",
                regularExpression));
          }

          Execute.Assertion
              .ForCondition(isMatch)
              .BecauseOf(reason, reasonArgs)
              .UsingLineBreaks
              .FailWith("Expected string to match regex {0}{reason}, but {1} does not match.", regularExpression, Subject);

          return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not match a regular expression.
        /// </summary>
        /// <param name="regularExpression">
        /// The regular expression with which the subject is matched.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotMatchRegex(string regularExpression, string reason = "", params object[] reasonArgs)
        {
          if (regularExpression == null)
          {
            throw new NullReferenceException("Cannot match string against <null>.");
          }

          Execute.Assertion
              .ForCondition(!ReferenceEquals(Subject, null))
              .UsingLineBreaks
              .BecauseOf(reason, reasonArgs)
              .FailWith("Expected string to not match regex {0}{reason}, but it was <null>.", regularExpression);
          
          bool isMatch;
          try
          {
            isMatch = Regex.IsMatch(Subject, regularExpression);
          }
          catch (ArgumentException)
          {
            throw new ArgumentException(
              string.Format(
                "Cannot match string against \"{0}\" because it is not a valid regular expression.",
                regularExpression));
          }

          Execute.Assertion
              .ForCondition(!isMatch)
              .BecauseOf(reason, reasonArgs)
              .UsingLineBreaks
              .FailWith("Did not expect string to match regex {0}{reason}, but {1} matches.", regularExpression, Subject);

          return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string starts exactly with the specified <paramref name="expected"/> value,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to start with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> StartWith(string expected, string reason = "", params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot compare start of string with <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare start of string with empty string.");
            }

            new StringStartValidator(Subject, expected, StringComparison.CurrentCulture, reason, reasonArgs).Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not start with the specified <paramref name="unexpected"/> value,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to start with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotStartWith(string unexpected, string reason = "", params object[] reasonArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot compare start of string with <null>.");
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot compare start of string with empty string.");
            }

            new NegatedStringStartValidator(Subject, unexpected, StringComparison.CurrentCulture, reason, reasonArgs).Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string starts with the specified <paramref name="expected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to start with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> StartWithEquivalent(string expected, string reason = "",
            params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new NullReferenceException("Cannot compare string start equivalence with <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare string start equivalence with empty string.");
            }

            new StringStartValidator(Subject, expected, StringComparison.CurrentCultureIgnoreCase, reason, reasonArgs).Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not start with the specified <paramref name="unexpected"/> value,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to start with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotStartWithEquivalentOf(string unexpected, string reason = "", params object[] reasonArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot compare start of string with <null>.");
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot compare start of string with empty string.");
            }

            new NegatedStringStartValidator(Subject, unexpected, StringComparison.CurrentCultureIgnoreCase, reason, reasonArgs).Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string ends exactly with the specified <paramref name="expected"/>,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to end with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> EndWith(string expected, string reason = "", params object[] reasonArgs)
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
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected string {0} to end with {1}{reason}.", Subject, expected);
            }

            if (Subject.Length < expected.Length)
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected string to end with {0}{reason}, but {1} is too short.", expected, Subject);
            }

            Execute.Assertion
                .ForCondition(Subject.EndsWith(expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string {0} to end with {1}{reason}.", Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not end exactly with the specified <paramref name="unexpected"/>,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to end with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotEndWith(string unexpected, string reason = "", params object[] reasonArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot compare end of string with <null>.");
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot compare end of string with empty string.");
            }

            if (Subject == null)
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected string that does not end with {1}, but found {0}.", Subject, unexpected);
            }

            Execute.Assertion
                .ForCondition(!Subject.EndsWith(unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string {0} not to end with {1}{reason}.", Subject, unexpected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string ends with the specified <paramref name="expected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to end with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> EndWithEquivalent(string expected, string reason = "", params object[] reasonArgs)
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
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected string that ends with equivalent of {0}{reason}, but found {1}.", expected, Subject);
            }

            if (Subject.Length < expected.Length)
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected string to end with equivalent of {0}{reason}, but {1} is too short.", expected, Subject);
            }

            Execute.Assertion
                .ForCondition(Subject.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string that ends with equivalent of {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not end with the specified <paramref name="unexpected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to end with.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotEndWithEquivalentOf(string unexpected, string reason = "", params object[] reasonArgs)
        {
            if (unexpected == null)
            {
                throw new NullReferenceException("Cannot compare end of string with <null>.");
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot compare end of string with empty string.");
            }

            if (Subject == null)
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected string that does not end with equivalent of {0}, but found {1}.", unexpected, Subject);
            }

            Execute.Assertion
                .ForCondition(!Subject.EndsWith(unexpected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string that does not end with equivalent of {0}{reason}, but found {1}.", unexpected, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains another (fragment of a) string.
        /// </summary>
        /// <param name="expected">
        /// The (fragement of a) string that the current string should contain.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> Contain(string expected, string reason = "", params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new ArgumentException("Cannot assert string containment against <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.");
            }

            Execute.Assertion
                .ForCondition(Contains(Subject, expected, StringComparison.Ordinal))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string {0} to contain {1}{reason}.", Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains the specified <paramref name="expected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to contain.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> ContainEquivalentOf(string expected, string reason = "", params object[] reasonArgs)
        {
            if (expected == null)
            {
                throw new ArgumentException("Cannot assert string containment against <null>.");
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.");
            }

            Execute.Assertion
                .ForCondition(Contains(Subject, expected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string to contain equivalent of {0}{reason} but found {1}", expected, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not contain another (fragment of a) string.
        /// </summary>
        /// <param name="expected">
        /// The (fragement of a) string that the current string should not contain.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotContain(string expected, string reason = "",
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

            Execute.Assertion
                .ForCondition(!Contains(Subject, expected, StringComparison.Ordinal))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect string {0} to contain {1}{reason}.", Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not contain the specified <paramref name="unexpected"/> string,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to contain.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotContainEquivalentOf(string unexpected, string reason = "",
            params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!Contains(Subject, unexpected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect string to contain equivalent of {0}{reason} but found {1}", unexpected, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        static bool Contains(string actual, string expected, StringComparison comparison)
        {
            return (actual ?? "").IndexOf(expected ?? "", comparison) >= 0;
        }

        /// <summary>
        /// Asserts that a string is <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> BeEmpty(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition((Subject != null) && (Subject.Length == 0))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected empty string{reason}, but found {0}.", Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is not <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> NotBeEmpty(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Length > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect empty string{reason}.");

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string has the specified <paramref name="expected"/> length.
        /// </summary>
        /// <param name="expected">The expected length of the string</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringAssertions> HaveLength(int expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Length == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string with length {0}{reason}, but found string {1} with length {2}.",
                    expected, Subject, Subject.Length);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is neither <c>null</c> nor <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> NotBeNullOrEmpty(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!string.IsNullOrEmpty(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string not to be <null> or empty{reason}, but found {0}.", Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is either <c>null</c> or <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> BeNullOrEmpty(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(string.IsNullOrEmpty(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string to be <null> or empty{reason}, but found {0}.", Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is neither <c>null</c> nor <see cref="string.Empty"/> nor white space
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> NotBeNullOrWhiteSpace(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!IsBlank(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string not to be <null> or whitespace{reason}, but found {0}.", Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is either <c>null</c> or <see cref="string.Empty"/> or white space
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> BeNullOrWhiteSpace(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(IsBlank(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected string to be <null> or whitespace{reason}, but found {0}.", Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        private static bool IsBlank(string value)
        {
            return (value == null) || string.IsNullOrEmpty(value.Trim());
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "string"; }
        }
    }
}