using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using FluentAssertions.Execution;
using FluentAssertions.Localization;
using JetBrains.Annotations;

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
        public StringAssertions(string value) : base(value)
        {
        }

        /// <summary>
        /// Asserts that a string is exactly the same as another string, including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> Be(string expected, string because = "", params object[] becauseArgs)
        {
            var stringEqualityValidator = new StringEqualityValidator(Subject, expected, StringComparison.CurrentCulture, because, becauseArgs);
            stringEqualityValidator.Validate();

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
            return BeOneOf(validValues, string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="string"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> BeOneOf(IEnumerable<string> validValues, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(validValues.Contains(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringToBeOneOfXFormat + Resources.Common_CommaButFoundYFormat,
                    validValues, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is exactly the same as another string, including any leading or trailing whitespace, with
        /// the exception of the casing.
        /// </summary>
        /// <param name="expected">
        /// The string that the subject is expected to be equivalent to.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> BeEquivalentTo(string expected, string because = "",
            params object[] becauseArgs)
        {
            var expectation = new StringEqualityValidator(
                Subject, expected, StringComparison.CurrentCultureIgnoreCase, because, becauseArgs);

            expectation.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is not exactly the same as the specified <paramref name="unexpected"/>,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to be equivalent to.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotBe(string unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringNotToBeXFormat, unexpected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string matches a wildcard pattern.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> Match(string wildcardPattern, string because = "", params object[] becauseArgs)
        {
            var stringWildcardMatchingValidator = new StringWildcardMatchingValidator(Subject, wildcardPattern, because, becauseArgs);
            stringWildcardMatchingValidator.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not match a wildcard pattern.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotMatch(string wildcardPattern, string because = "", params object[] becauseArgs)
        {
            new StringWildcardMatchingValidator(Subject, wildcardPattern, because, becauseArgs)
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> MatchEquivalentOf(string wildcardPattern, string because = "",
            params object[] becauseArgs)
        {
            var validator = new StringWildcardMatchingValidator(Subject, wildcardPattern, because, becauseArgs)
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotMatchEquivalentOf(string wildcardPattern, string because = "",
            params object[] becauseArgs)
        {
            var validator = new StringWildcardMatchingValidator(Subject, wildcardPattern, because, becauseArgs)
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> MatchRegex([RegexPattern] string regularExpression, string because = "", params object[] becauseArgs)
        {
            if (regularExpression is null)
            {
                throw new ArgumentNullException(nameof(regularExpression), Resources.String_CannotMatchStringAgainstNull);
            }

            Execute.Assertion
                .ForCondition(!(Subject is null))
                .UsingLineBreaks
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringToMatchRegexXFormat + Resources.Common_CommaButItWasNull,
                    regularExpression);

            bool isMatch = false;
            try
            {
                isMatch = Regex.IsMatch(Subject, regularExpression);
            }
            catch (ArgumentException)
            {
                Execute.Assertion
                    .FailWith(Resources.String_CannotMatchStringAgainstXBecauseItIsNotValidRegexFormat, regularExpression);
            }

            Execute.Assertion
                .ForCondition(isMatch)
                .BecauseOf(because, becauseArgs)
                .UsingLineBreaks
                .FailWith(Resources.String_ExpectedStringToMatchRegexXFormat + Resources.String_CommaButYDoesNotMatchFormat,
                    regularExpression, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not match a regular expression.
        /// </summary>
        /// <param name="regularExpression">
        /// The regular expression with which the subject is matched.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotMatchRegex([RegexPattern] string regularExpression, string because = "", params object[] becauseArgs)
        {
            if (regularExpression is null)
            {
                throw new ArgumentNullException(nameof(regularExpression), Resources.String_CannotMatchStringAgainstNull);
            }

            Execute.Assertion
                .ForCondition(!(Subject is null))
                .UsingLineBreaks
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringToNotMatchRegexXFormat + Resources.Common_CommaButItWasNull,
                    regularExpression);

            bool isMatch = false;
            try
            {
                isMatch = Regex.IsMatch(Subject, regularExpression);
            }
            catch (ArgumentException)
            {
                Execute.Assertion.FailWith(Resources.String_CannotMatchStringAgainstXBecauseItIsNotValidRegexFormat,
                    regularExpression);
            }

            Execute.Assertion
                .ForCondition(!isMatch)
                .BecauseOf(because, becauseArgs)
                .UsingLineBreaks
                .FailWith(Resources.String_DidNotExpectStringToMatchRegexXButYMatchesFormat,
                    regularExpression, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string starts exactly with the specified <paramref name="expected"/> value,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to start with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> StartWith(string expected, string because = "", params object[] becauseArgs)
        {
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected), Resources.String_CannotCompareStartOfStringWithNull);
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotCompareStartOfStringWithEmptyString, nameof(expected));
            }

            var stringStartValidator = new StringStartValidator(Subject, expected, StringComparison.CurrentCulture, because, becauseArgs);
            stringStartValidator.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not start with the specified <paramref name="unexpected"/> value,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to start with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotStartWith(string unexpected, string because = "", params object[] becauseArgs)
        {
            if (unexpected is null)
            {
                throw new ArgumentNullException(nameof(unexpected), Resources.String_CannotCompareStartOfStringWithNull);
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotCompareStartOfStringWithEmptyString, nameof(unexpected));
            }

            var negatedStringStartValidator = new NegatedStringStartValidator(Subject, unexpected, StringComparison.CurrentCulture, because, becauseArgs);
            negatedStringStartValidator.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string starts with the specified <paramref name="expected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to start with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> StartWithEquivalent(string expected, string because = "",
            params object[] becauseArgs)
        {
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected), Resources.String_CannotCompareStringStartEquivalenceWithNull);
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotCompareStringStartEquivalenceWithEmptyString, nameof(expected));
            }

            var stringStartValidator = new StringStartValidator(Subject, expected, StringComparison.CurrentCultureIgnoreCase, because, becauseArgs);
            stringStartValidator.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not start with the specified <paramref name="unexpected"/> value,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to start with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotStartWithEquivalentOf(string unexpected, string because = "", params object[] becauseArgs)
        {
            if (unexpected is null)
            {
                throw new ArgumentNullException(nameof(unexpected), Resources.String_CannotCompareStartOfStringWithNull);
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotCompareStartOfStringWithEmptyString, nameof(unexpected));
            }

            var negatedStringStartValidator = new NegatedStringStartValidator(Subject, unexpected, StringComparison.CurrentCultureIgnoreCase, because, becauseArgs);
            negatedStringStartValidator.Validate();

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string ends exactly with the specified <paramref name="expected"/>,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to end with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> EndWith(string expected, string because = "", params object[] becauseArgs)
        {
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected), Resources.String_CannotCompareStringEndWithNull);
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotCompareStringEndWithEmptyString, nameof(expected));
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.String_ExpectedStringXToEndWithYFormat,
                        Subject, expected);
            }

            if (Subject.Length < expected.Length)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.String_ExpectedStringToEndWithXButYIsTooShortFormat,
                        expected, Subject);
            }

            Execute.Assertion
                .ForCondition(Subject.EndsWith(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringXToEndWithYFormat,
                    Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not end exactly with the specified <paramref name="unexpected"/>,
        /// including the casing and any leading or trailing whitespace.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to end with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotEndWith(string unexpected, string because = "", params object[] becauseArgs)
        {
            if (unexpected is null)
            {
                throw new ArgumentNullException(nameof(unexpected), Resources.String_CannotCompareEndOfStringWithNull);
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotCompareEndOfStringWithEmptyString, nameof(unexpected));
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.String_ExpectedStringThatDoesNotEndWithXFormat + Resources.Common_CommaButFoundYFormat,
                        unexpected, Subject);
            }

            Execute.Assertion
                .ForCondition(!Subject.EndsWith(unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringXNotToEndWithYFormat,
                    Subject, unexpected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string ends with the specified <paramref name="expected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to end with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> EndWithEquivalent(string expected, string because = "", params object[] becauseArgs)
        {
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected), Resources.String_CannotCompareStringEndEquivalenceWithNull);
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotCompareStringEndEquivalenceWithEmptyString, nameof(expected));
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.String_ExpectedStringThatEndsWithEquivalentOfXFormat + Resources.Common_CommaButFoundYFormat,
                        expected, Subject);
            }

            if (Subject.Length < expected.Length)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.String_ExpectedStringToEndWithEquivalentOfXButYIsTooShortFormat,
                        expected, Subject);
            }

            Execute.Assertion
                .ForCondition(Subject.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringThatEndsWithEquivalentOfXFormat + Resources.Common_CommaButFoundYFormat,
                    expected, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not end with the specified <paramref name="unexpected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to end with.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotEndWithEquivalentOf(string unexpected, string because = "", params object[] becauseArgs)
        {
            if (unexpected is null)
            {
                throw new ArgumentNullException(nameof(unexpected), Resources.String_CannotCompareEndOfStringWithNull);
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotCompareEndOfStringWithEmptyString, nameof(unexpected));
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.String_ExpectedStringThatDoesNotEndWithEquivalentOfXNoReasonFormat + Resources.Common_CommaButFoundYFormat,
                        unexpected, Subject);
            }

            Execute.Assertion
                .ForCondition(!Subject.EndsWith(unexpected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringThatDoesNotEndWithEquivalentOfXFormat + Resources.Common_CommaButFoundYFormat,
                    unexpected, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains another (fragment of a) string.
        /// </summary>
        /// <param name="expected">
        /// The (fragment of a) string that the current string should contain.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> Contain(string expected, string because = "", params object[] becauseArgs)
        {
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected), Resources.String_CannotAssertStringContainmentAgainstNull);
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotAssertStringContainmentAgainstAnEmptyString, nameof(expected));
            }

            Execute.Assertion
                .ForCondition(Contains(Subject, expected, StringComparison.Ordinal))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringXToContainYFormat,
                    Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains the specified <paramref name="expected"/>,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="expected">The string that the subject is expected to contain.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> ContainEquivalentOf(string expected, string because = "", params object[] becauseArgs)
        {
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected), Resources.String_CannotAssertStringContainmentAgainstNull);
            }

            if (expected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotAssertStringContainmentAgainstAnEmptyString, nameof(expected));
            }

            Execute.Assertion
                .ForCondition(Contains(Subject, expected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringToContainEquivalentOfXButFoundYFormat,
                    expected, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains all values present in <paramref name="values"/>.
        /// </summary>
        /// <param name="values">
        /// The values that should all be present in the string
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> ContainAll(IEnumerable<string> values, string because = "", params object[] becauseArgs)
        {
            ThrowIfValuesNullOrEmpty(values);

            var missing = values.Where(v => !Contains(Subject, v, StringComparison.Ordinal)).ToArray();
            Execute.Assertion
                .ForCondition(values.All(v => Contains(Subject, v, StringComparison.Ordinal)))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringXToContainTheStringsYFormat, Subject, missing);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains all values present in <paramref name="values"/>.
        /// </summary>
        /// <param name="values">
        /// The values that should all be present in the string
        /// </param>
        public AndConstraint<StringAssertions> ContainAll(params string[] values)
        {
            return ContainAll(values, because: string.Empty);
        }

        /// <summary>
        /// Asserts that a string contains at least one value present in <paramref name="values"/>,.
        /// </summary>
        /// <param name="values">
        /// The values that should will be tested against the string
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> ContainAny(IEnumerable<string> values, string because = "", params object[] becauseArgs)
        {
            ThrowIfValuesNullOrEmpty(values);

            Execute.Assertion
                .ForCondition(values.Any(v => Contains(Subject, v, StringComparison.Ordinal)))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringXToContainAtLeastOneOfTheStringsYFormat,
                    Subject, values.ToArray());

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains at least one value present in <paramref name="values"/>,.
        /// </summary>
        /// <param name="values">
        /// The values that should will be tested against the string
        /// </param>
        public AndConstraint<StringAssertions> ContainAny(params string[] values)
        {
            return ContainAny(values, because: string.Empty);
        }

        /// <summary>
        /// Asserts that a string does not contain another (fragment of a) string.
        /// </summary>
        /// <param name="unexpected">
        /// The (fragment of a) string that the current string should not contain.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotContain(string unexpected, string because = "",
            params object[] becauseArgs)
        {
            if (unexpected is null)
            {
                throw new ArgumentNullException(nameof(unexpected), Resources.String_CannotAssertStringContainmentAgainstNull);
            }

            if (unexpected.Length == 0)
            {
                throw new ArgumentException(Resources.String_CannotAssertStringContainmentAgainstAnEmptyString, nameof(unexpected));
            }

            Execute.Assertion
                .ForCondition(!Contains(Subject, unexpected, StringComparison.Ordinal))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_DidNotExpectStringXToContainYFormat,
                    Subject, unexpected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not contain all of the strings provided in <paramref name="values"/>. The string
        /// may contain some subset of the provided values.
        /// </summary>
        /// <param name="values">
        /// The values that should not be present in the string
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotContainAll(IEnumerable<string> values, string because = "",
            params object[] becauseArgs)
        {
            ThrowIfValuesNullOrEmpty(values);

            var matches = values.Count(v => Contains(Subject, v, StringComparison.Ordinal));

            Execute.Assertion
                .ForCondition(matches != values.Count())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_DidNotExpectStringXToContainAllOfTheStringsYFormat,
                    Subject, values);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not contain all of the strings provided in <paramref name="values"/>. The string
        /// may contain some subset of the provided values.
        /// </summary>
        /// <param name="values">
        /// The values that should not be present in the string
        /// </param>
        public AndConstraint<StringAssertions> NotContainAll(params string[] values)
        {
            return NotContainAll(values, because: string.Empty);
        }

        /// <summary>
        /// Asserts that a string does not contain any of the strings provided in <paramref name="values"/>.
        /// </summary>
        /// <param name="values">
        /// The values that should not be present in the string
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotContainAny(IEnumerable<string> values, string because = "",
            params object[] becauseArgs)
        {
            ThrowIfValuesNullOrEmpty(values);

            var matches = values.Where(v => Contains(Subject, v, StringComparison.Ordinal));

            Execute.Assertion
                .ForCondition(!matches.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_DidNotExpectStringXToContainAnyOfTheStringsYFormat, Subject, matches);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string does not contain any of the strings provided in <paramref name="values"/>.
        /// </summary>
        /// <param name="values">
        /// The values that should not be present in the string
        /// </param>
        public AndConstraint<StringAssertions> NotContainAny(params string[] values)
        {
            return NotContainAny(values, because: string.Empty);
        }

        /// <summary>
        /// Asserts that a string does not contain the specified <paramref name="unexpected"/> string,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="unexpected">The string that the subject is not expected to contain.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotContainEquivalentOf(string unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Contains(Subject, unexpected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_DidNotExpectStringToContainEquivalentOfYButFoundZFormat,
                    unexpected, Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        private static bool Contains(string actual, string expected, StringComparison comparison)
        {
            return (actual ?? "").IndexOf(expected ?? "", comparison) >= 0;
        }

        /// <summary>
        /// Asserts that a string is <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> BeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject?.Length == 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringToBeEmpty + Resources.Common_CommaButFoundXFormat,
                    Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is not <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotBeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Length > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_DidNotExpectStringToBeEmpty);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string has the specified <paramref name="expected"/> length.
        /// </summary>
        /// <param name="expected">The expected length of the string</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> HaveLength(int expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Length == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringWithLengthXButFoundStringYWithLengthZFormat,
                    expected, Subject, Subject.Length);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is neither <c>null</c> nor <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> NotBeNullOrEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!string.IsNullOrEmpty(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringNotToBeNullOrEmpty + Resources.Common_CommaButFoundXFormat,
                    Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is either <c>null</c> or <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> BeNullOrEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(string.IsNullOrEmpty(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringToBeNullOrEmpty + Resources.Common_CommaButFoundXFormat,
                    Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is neither <c>null</c> nor <see cref="string.Empty"/> nor white space
        /// </summary>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> NotBeNullOrWhiteSpace(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!string.IsNullOrWhiteSpace(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringNotToBeNullOrWhitespace + Resources.Common_CommaButFoundXFormat,
                    Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string is either <c>null</c> or <see cref="string.Empty"/> or white space
        /// </summary>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<StringAssertions> BeNullOrWhiteSpace(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(string.IsNullOrWhiteSpace(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.String_ExpectedStringToBeNullOrWhitespace + Resources.Common_CommaButFoundXFormat,
                    Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        private static void ThrowIfValuesNullOrEmpty(IEnumerable<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values), Resources.String_CannotAssertStringContainmentOfValuesInNullCollection);
            }

            if (!values.Any())
            {
                throw new ArgumentException(Resources.String_CannotAssertStringContainmentOfValuesInEmptyCollection, nameof(values));
            }
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "string";
    }
}
