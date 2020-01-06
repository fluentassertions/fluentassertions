using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
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
        /// Initializes a new instance of the <see cref="System.Object" /> class.
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
                .FailWith("Expected {context:string} to be one of {0}{reason}, but found {1}.", validValues, Subject);

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
        /// Asserts that a string is not exactly the same as another string, including any leading or trailing whitespace, with
        /// the exception of the casing.
        /// </summary>
        /// <param name="unexpected">
        /// The string that the subject is not expected to be equivalent to.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> NotBeEquivalentTo(string unexpected, string because = "",
            params object[] becauseArgs)
        {
            bool notEquivalent;
            using (var scope = new AssertionScope())
            {
                Subject.Should().BeEquivalentTo(unexpected);
                notEquivalent = scope.Discard().Any();
            }

            Execute.Assertion
                .ForCondition(notEquivalent)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} not to be equivalent to {0}{reason}, but they are.", unexpected);

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
                .FailWith("Expected {context:string} not to be {0}{reason}.", unexpected);

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
            Guard.ThrowIfArgumentIsNull(regularExpression, nameof(regularExpression), "Cannot match string against <null>.");

            Execute.Assertion
                .ForCondition(!(Subject is null))
                .UsingLineBreaks
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} to match regex {0}{reason}, but it was <null>.", regularExpression);

            try
            {
                Execute.Assertion
                .ForCondition(Regex.IsMatch(Subject, regularExpression))
                .BecauseOf(because, becauseArgs)
                .UsingLineBreaks
                .FailWith("Expected {context:string} to match regex {0}{reason}, but {1} does not match.", regularExpression, Subject);
            }
            catch (ArgumentException)
            {
                Execute.Assertion
                    .FailWith("Cannot match {context:string} against {0} because it is not a valid regular expression.", regularExpression);
            }

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
            Guard.ThrowIfArgumentIsNull(regularExpression, nameof(regularExpression), "Cannot match string against <null>.");

            Execute.Assertion
                .ForCondition(!(Subject is null))
                .UsingLineBreaks
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} to not match regex {0}{reason}, but it was <null>.", regularExpression);

            try
            {
                Execute.Assertion
                    .ForCondition(!Regex.IsMatch(Subject, regularExpression))
                    .BecauseOf(because, becauseArgs)
                    .UsingLineBreaks
                    .FailWith("Did not expect {context:string} to match regex {0}{reason}, but {1} matches.", regularExpression, Subject);
            }
            catch (ArgumentException)
            {
                Execute.Assertion.FailWith("Cannot match {context:string} against {0} because it is not a valid regular expression.",
                    regularExpression);
            }

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
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare start of string with <null>.");

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare start of string with empty string.", nameof(expected));
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
            Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare start of string with <null>.");

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot compare start of string with empty string.", nameof(unexpected));
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
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare string start equivalence with <null>.");

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare string start equivalence with empty string.", nameof(expected));
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
            Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare start of string with <null>.");

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot compare start of string with empty string.", nameof(unexpected));
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
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare string end with <null>.");

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare string end with empty string.", nameof(expected));
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} {0} to end with {1}{reason}.", Subject, expected);
            }

            if (Subject.Length < expected.Length)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} to end with {0}{reason}, but {1} is too short.", expected, Subject);
            }

            Execute.Assertion
                .ForCondition(Subject.EndsWith(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} {0} to end with {1}{reason}.", Subject, expected);

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
            Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare end of string with <null>.");

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot compare end of string with empty string.", nameof(unexpected));
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} that does not end with {1}, but found {0}.", Subject, unexpected);
            }

            Execute.Assertion
                .ForCondition(!Subject.EndsWith(unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} {0} not to end with {1}{reason}.", Subject, unexpected);

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
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare string end equivalence with <null>.");

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot compare string end equivalence with empty string.", nameof(expected));
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} that ends with equivalent of {0}{reason}, but found {1}.", expected, Subject);
            }

            if (Subject.Length < expected.Length)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} to end with equivalent of {0}{reason}, but {1} is too short.", expected, Subject);
            }

            Execute.Assertion
                .ForCondition(Subject.EndsWith(expected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} that ends with equivalent of {0}{reason}, but found {1}.", expected, Subject);

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
            Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare end of string with <null>.");

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot compare end of string with empty string.", nameof(unexpected));
            }

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} that does not end with equivalent of {0}, but found {1}.", unexpected, Subject);
            }

            Execute.Assertion
                .ForCondition(!Subject.EndsWith(unexpected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} that does not end with equivalent of {0}{reason}, but found {1}.", unexpected, Subject);

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
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot assert string containment against <null>.");

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.", nameof(expected));
            }

            Execute.Assertion
                .ForCondition(Contains(Subject, expected, StringComparison.Ordinal))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} {0} to contain {1}{reason}.", Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains another (fragment of a) string a set amount of times.
        /// </summary>
        /// <param name="expected">
        /// The (fragment of a) string that the current string should contain.
        /// </param>
        /// <param name="occurrenceConstraint">
        /// A constraint specifying the amount of times a substring should be present within the test subject.
        /// It can be created by invoking static methods Once, Twice, Thrice, or Times(int)
        /// on the classes <see cref="Exactly"/>, <see cref="AtLeast"/>, <see cref="MoreThan"/>, <see cref="AtMost"/>, and <see cref="LessThan"/>.
        /// For example, <see cref="Exactly.Times(int)"/> or <see cref="LessThan.Twice()"/>.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> Contain(string expected, OccurrenceConstraint occurrenceConstraint, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot assert string containment against <null>.");

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.", nameof(expected));
            }

            int actual = Subject.CountSubstring(expected, StringComparison.Ordinal);

            Execute.Assertion
                .ForCondition(occurrenceConstraint.Assert(actual))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    $"Expected {{context:string}} {{0}} to contain {{1}} {occurrenceConstraint.Mode} {occurrenceConstraint.ExpectedCount.Times()}{{reason}}, but found it {actual.Times()}.",
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
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot assert string containment against <null>.");

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.", nameof(expected));
            }

            Execute.Assertion
                .ForCondition(Contains(Subject, expected, StringComparison.CurrentCultureIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} {0} to contain the equivalent of {1}{reason}.", Subject, expected);

            return new AndConstraint<StringAssertions>(this);
        }

        /// <summary>
        /// Asserts that a string contains the specified <paramref name="expected"/> a set amount of times,
        /// including any leading or trailing whitespace, with the exception of the casing.
        /// </summary>
        /// <param name="expected">
        /// The (fragment of a) string that the current string should contain.
        /// </param>
        /// <param name="occurrenceConstraint">
        /// A constraint specifying the amount of times a substring should be present within the test subject.
        /// It can be created by invoking static methods Once, Twice, Thrice, or Times(int)
        /// on the classes <see cref="Exactly"/>, <see cref="AtLeast"/>, <see cref="MoreThan"/>, <see cref="AtMost"/>, and <see cref="LessThan"/>.
        /// For example, <see cref="Exactly.Times(int)"/> or <see cref="LessThan.Twice()"/>.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringAssertions> ContainEquivalentOf(string expected, OccurrenceConstraint occurrenceConstraint, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot assert string containment against <null>.");

            if (expected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.", nameof(expected));
            }

            int actual = Subject.CountSubstring(expected, StringComparison.OrdinalIgnoreCase);

            Execute.Assertion
                .ForCondition(occurrenceConstraint.Assert(actual))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    $"Expected {{context:string}} {{0}} to contain equivalent of {{1}} {occurrenceConstraint.Mode} {occurrenceConstraint.ExpectedCount.Times()}{{reason}}, but found it {actual.Times()}.",
                    Subject, expected);

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
                .FailWith("Expected {context:string} {0} to contain the strings: {1}{reason}.", Subject, missing);

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
                .FailWith("Expected {context:string} {0} to contain at least one of the strings: {1}{reason}.", Subject, values.ToArray());

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
            Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot assert string containment against <null>.");

            if (unexpected.Length == 0)
            {
                throw new ArgumentException("Cannot assert string containment against an empty string.", nameof(unexpected));
            }

            Execute.Assertion
                .ForCondition(!Contains(Subject, unexpected, StringComparison.Ordinal))
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:string} {0} to contain {1}{reason}.", Subject, unexpected);

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
                .FailWith("Did not expect {context:string} {0} to contain all of the strings: {1}{reason}.", Subject, values);

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

            IEnumerable<string> matches = values.Where(v => Contains(Subject, v, StringComparison.Ordinal));

            Execute.Assertion
                .ForCondition(!matches.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:string} {0} to contain any of the strings: {1}{reason}.", Subject, matches);

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
                .FailWith("Did not expect {context:string} to contain equivalent of {0}{reason} but found {1}.", unexpected, Subject);

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
                .FailWith("Expected {context:string} to be empty{reason}, but found {0}.", Subject);

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
                .FailWith("Did not expect {context:string} to be empty{reason}.");

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
                .FailWith("Expected {context:string} with length {0}{reason}, but found string {1} with length {2}.",
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
                .FailWith("Expected {context:string} not to be <null> or empty{reason}, but found {0}.", Subject);

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
                .FailWith("Expected {context:string} to be <null> or empty{reason}, but found {0}.", Subject);

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
                .FailWith("Expected {context:string} not to be <null> or whitespace{reason}, but found {0}.", Subject);

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
                .FailWith("Expected {context:string} to be <null> or whitespace{reason}, but found {0}.", Subject);

            return new AndConstraint<StringAssertions>(this);
        }

        private static void ThrowIfValuesNullOrEmpty(IEnumerable<string> values)
        {
            Guard.ThrowIfArgumentIsNull(values, nameof(values), "Cannot assert string containment of values in null collection");

            if (!values.Any())
            {
                throw new ArgumentException("Cannot assert string containment of values in empty collection", nameof(values));
            }
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "string";
    }
}
