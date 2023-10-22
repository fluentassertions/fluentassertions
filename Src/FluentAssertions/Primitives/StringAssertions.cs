using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using JetBrains.Annotations;

namespace FluentAssertions.Primitives;

/// <summary>
/// Contains a number of methods to assert that a <see cref="string"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class StringAssertions : StringAssertions<StringAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StringAssertions"/> class.
    /// </summary>
    public StringAssertions(string value)
        : base(value)
    {
    }
}

/// <summary>
/// Contains a number of methods to assert that a <see cref="string"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class StringAssertions<TAssertions> : ReferenceTypeAssertions<string, TAssertions>
    where TAssertions : StringAssertions<TAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StringAssertions{TAssertions}"/> class.
    /// </summary>
    public StringAssertions(string value)
        : base(value)
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(string expected, string because = "", params object[] becauseArgs)
    {
        var stringEqualityValidator = new StringValidator(
            new StringEqualityStrategy(StringComparison.Ordinal),
            because, becauseArgs);

        stringEqualityValidator.Validate(Subject, expected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="string"/> is one of the specified <paramref name="validValues"/>.
    /// </summary>
    /// <param name="validValues">
    /// The values that are valid.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(params string[] validValues)
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeOneOf(IEnumerable<string> validValues, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(validValues.Contains(Subject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} to be one of {0}{reason}, but found {1}.", validValues, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeEquivalentTo(string expected, string because = "",
        params object[] becauseArgs)
    {
        var expectation = new StringValidator(
            new StringEqualityStrategy(StringComparison.OrdinalIgnoreCase),
            because, becauseArgs);

        expectation.Validate(Subject, expected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeEquivalentTo(string unexpected, string because = "",
        params object[] becauseArgs)
    {
        bool notEquivalent;

        using (var scope = new AssertionScope())
        {
            BeEquivalentTo(unexpected);
            notEquivalent = scope.Discard().Length > 0;
        }

        Execute.Assertion
            .ForCondition(notEquivalent)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} not to be equivalent to {0}{reason}, but they are.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBe(string unexpected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != unexpected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} not to be {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string matches the <paramref name="wildcardPattern"/>.
    /// </summary>
    /// <param name="wildcardPattern">
    /// The pattern to match against the subject. This parameter can contain a combination of literal text and wildcard
    /// (* and ?) characters, but it doesn't support regular expressions.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// <paramref name="wildcardPattern"/> can be a combination of literal and wildcard characters,
    /// but it doesn't support regular expressions. The following wildcard specifiers are permitted in
    /// <paramref name="wildcardPattern"/>.
    /// <list type="table">
    /// <listheader>
    /// <term>Wildcard character</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>* (asterisk)</term>
    /// <description>Zero or more characters in that position.</description>
    /// </item>
    /// <item>
    /// <term>? (question mark)</term>
    /// <description>Exactly one character in that position.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="wildcardPattern"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="wildcardPattern"/> is empty.</exception>
    public AndConstraint<TAssertions> Match(string wildcardPattern, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(wildcardPattern, nameof(wildcardPattern),
            "Cannot match string against <null>. Provide a wildcard pattern or use the BeNull method.");

        Guard.ThrowIfArgumentIsEmpty(wildcardPattern, nameof(wildcardPattern),
            "Cannot match string against an empty string. Provide a wildcard pattern or use the BeEmpty method.");

        var stringWildcardMatchingValidator = new StringValidator(
            new StringWildcardMatchingStrategy(),
            because, becauseArgs);

        stringWildcardMatchingValidator.Validate(Subject, wildcardPattern);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string does not match the <paramref name="wildcardPattern"/>.
    /// </summary>
    /// <param name="wildcardPattern">
    /// The pattern to match against the subject. This parameter can contain a combination literal text and wildcard of
    /// (* and ?) characters, but it doesn't support regular expressions.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// <paramref name="wildcardPattern"/> can be a combination of literal and wildcard characters,
    /// but it doesn't support regular expressions. The following wildcard specifiers are permitted in
    /// <paramref name="wildcardPattern"/>.
    /// <list type="table">
    /// <listheader>
    /// <term>Wildcard character</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>* (asterisk)</term>
    /// <description>Zero or more characters in that position.</description>
    /// </item>
    /// <item>
    /// <term>? (question mark)</term>
    /// <description>Exactly one character in that position.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="wildcardPattern"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="wildcardPattern"/> is empty.</exception>
    public AndConstraint<TAssertions> NotMatch(string wildcardPattern, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(wildcardPattern, nameof(wildcardPattern),
            "Cannot match string against <null>. Provide a wildcard pattern or use the NotBeNull method.");

        Guard.ThrowIfArgumentIsEmpty(wildcardPattern, nameof(wildcardPattern),
            "Cannot match string against an empty string. Provide a wildcard pattern or use the NotBeEmpty method.");

        var stringWildcardMatchingValidator = new StringValidator(
            new StringWildcardMatchingStrategy
            {
                Negate = true
            },
            because, becauseArgs);

        stringWildcardMatchingValidator.Validate(Subject, wildcardPattern);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string matches the <paramref name="wildcardPattern"/>.
    /// </summary>
    /// <param name="wildcardPattern">
    /// The pattern to match against the subject. This parameter can contain a combination of literal text and wildcard
    /// (* and ?) characters, but it doesn't support regular expressions.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// <paramref name="wildcardPattern"/> can be a combination of literal and wildcard characters,
    /// but it doesn't support regular expressions. The following wildcard specifiers are permitted in
    /// <paramref name="wildcardPattern"/>.
    /// <list type="table">
    /// <listheader>
    /// <term>Wildcard character</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>* (asterisk)</term>
    /// <description>Zero or more characters in that position.</description>
    /// </item>
    /// <item>
    /// <term>? (question mark)</term>
    /// <description>Exactly one character in that position.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="wildcardPattern"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="wildcardPattern"/> is empty.</exception>
    public AndConstraint<TAssertions> MatchEquivalentOf(string wildcardPattern, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(wildcardPattern, nameof(wildcardPattern),
            "Cannot match string against <null>. Provide a wildcard pattern or use the BeNull method.");

        Guard.ThrowIfArgumentIsEmpty(wildcardPattern, nameof(wildcardPattern),
            "Cannot match string against an empty string. Provide a wildcard pattern or use the BeEmpty method.");

        var stringWildcardMatchingValidator = new StringValidator(
            new StringWildcardMatchingStrategy
            {
                IgnoreCase = true,
                IgnoreNewLineDifferences = true
            },
            because, becauseArgs);

        stringWildcardMatchingValidator.Validate(Subject, wildcardPattern);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string does not match the <paramref name="wildcardPattern"/>.
    /// </summary>
    /// <param name="wildcardPattern">
    /// The pattern to match against the subject. This parameter can contain a combination of literal text and wildcard
    /// (* and ?) characters, but it doesn't support regular expressions.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <remarks>
    /// <paramref name="wildcardPattern"/> can be a combination of literal and wildcard characters,
    /// but it doesn't support regular expressions. The following wildcard specifiers are permitted in
    /// <paramref name="wildcardPattern"/>.
    /// <list type="table">
    /// <listheader>
    /// <term>Wildcard character</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>* (asterisk)</term>
    /// <description>Zero or more characters in that position.</description>
    /// </item>
    /// <item>
    /// <term>? (question mark)</term>
    /// <description>Exactly one character in that position.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="wildcardPattern"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="wildcardPattern"/> is empty.</exception>
    public AndConstraint<TAssertions> NotMatchEquivalentOf(string wildcardPattern, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(wildcardPattern, nameof(wildcardPattern),
            "Cannot match string against <null>. Provide a wildcard pattern or use the NotBeNull method.");

        Guard.ThrowIfArgumentIsEmpty(wildcardPattern, nameof(wildcardPattern),
            "Cannot match string against an empty string. Provide a wildcard pattern or use the NotBeEmpty method.");

        var stringWildcardMatchingValidator = new StringValidator(
            new StringWildcardMatchingStrategy
            {
                IgnoreCase = true,
                IgnoreNewLineDifferences = true,
                Negate = true
            },
            because, becauseArgs);

        stringWildcardMatchingValidator.Validate(Subject, wildcardPattern);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string matches a regular expression with expected occurrence
    /// </summary>
    /// <param name="regularExpression">
    /// The regular expression with which the subject is matched.
    /// </param>
    /// <param name="occurrenceConstraint">
    /// A constraint specifying the expected amount of times a regex should match a string.
    /// It can be created by invoking static methods Once, Twice, Thrice, or Times(int)
    /// on the classes <see cref="Exactly"/>, <see cref="AtLeast"/>, <see cref="MoreThan"/>, <see cref="AtMost"/>, and <see cref="LessThan"/>.
    /// For example, <see cref="Exactly.Times(int)"/> or <see cref="LessThan.Twice()"/>.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="regularExpression"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> MatchRegex([RegexPattern][StringSyntax("Regex")] string regularExpression,
        OccurrenceConstraint occurrenceConstraint, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(regularExpression, nameof(regularExpression),
            "Cannot match string against <null>. Provide a regex pattern or use the BeNull method.");

        Regex regex;

        try
        {
            regex = new Regex(regularExpression);
        }
        catch (ArgumentException)
        {
            Execute.Assertion.FailWith("Cannot match {context:string} against {0} because it is not a valid regular expression.",
                regularExpression);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        return MatchRegex(regex, occurrenceConstraint, because, becauseArgs);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="regularExpression"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> MatchRegex([RegexPattern][StringSyntax("Regex")] string regularExpression,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(regularExpression, nameof(regularExpression),
            "Cannot match string against <null>. Provide a regex pattern or use the BeNull method.");

        Regex regex;

        try
        {
            regex = new Regex(regularExpression);
        }
        catch (ArgumentException)
        {
            Execute.Assertion.FailWith("Cannot match {context:string} against {0} because it is not a valid regular expression.",
                regularExpression);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        return MatchRegex(regex, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that a string matches a regular expression with expected occurrence
    /// </summary>
    /// <param name="regularExpression">
    /// The regular expression with which the subject is matched.
    /// </param>
    /// <param name="occurrenceConstraint">
    /// A constraint specifying the expected amount of times a regex should match a string.
    /// It can be created by invoking static methods Once, Twice, Thrice, or Times(int)
    /// on the classes <see cref="Exactly"/>, <see cref="AtLeast"/>, <see cref="MoreThan"/>, <see cref="AtMost"/>, and <see cref="LessThan"/>.
    /// For example, <see cref="Exactly.Times(int)"/> or <see cref="LessThan.Twice()"/>.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="regularExpression"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="regularExpression"/> is empty.</exception>
    public AndConstraint<TAssertions> MatchRegex(Regex regularExpression,
        OccurrenceConstraint occurrenceConstraint, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(regularExpression, nameof(regularExpression),
            "Cannot match string against <null>. Provide a regex pattern or use the BeNull method.");

        var regexStr = regularExpression.ToString();

        Guard.ThrowIfArgumentIsEmpty(regexStr, nameof(regularExpression),
            "Cannot match string against an empty string. Provide a regex pattern or use the BeEmpty method.");

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .UsingLineBreaks
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} to match regex {0}{reason}, but it was <null>.", regexStr);

        if (success)
        {
            int actual = regularExpression.Matches(Subject).Count;

            Execute.Assertion
                .ForConstraint(occurrenceConstraint, actual)
                .UsingLineBreaks
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} {0} to match regex {1} {expectedOccurrence}{reason}, " +
                    $"but found it {actual.Times()}.",
                    Subject, regexStr);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="regularExpression"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="regularExpression"/> is empty.</exception>
    public AndConstraint<TAssertions> MatchRegex(Regex regularExpression,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(regularExpression, nameof(regularExpression),
            "Cannot match string against <null>. Provide a regex pattern or use the BeNull method.");

        var regexStr = regularExpression.ToString();

        Guard.ThrowIfArgumentIsEmpty(regexStr, nameof(regularExpression),
            "Cannot match string against an empty string. Provide a regex pattern or use the BeEmpty method.");

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .UsingLineBreaks
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} to match regex {0}{reason}, but it was <null>.", regexStr);

        if (success)
        {
            Execute.Assertion
                .ForCondition(regularExpression.IsMatch(Subject))
                .BecauseOf(because, becauseArgs)
                .UsingLineBreaks
                .FailWith("Expected {context:string} to match regex {0}{reason}, but {1} does not match.", regexStr, Subject);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="regularExpression"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotMatchRegex([RegexPattern][StringSyntax("Regex")] string regularExpression,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(regularExpression, nameof(regularExpression),
            "Cannot match string against <null>. Provide a regex pattern or use the NotBeNull method.");

        Regex regex;

        try
        {
            regex = new Regex(regularExpression);
        }
        catch (ArgumentException)
        {
            Execute.Assertion.FailWith("Cannot match {context:string} against {0} because it is not a valid regular expression.",
                regularExpression);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        return NotMatchRegex(regex, because, becauseArgs);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="regularExpression"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="regularExpression"/> is empty.</exception>
    public AndConstraint<TAssertions> NotMatchRegex(Regex regularExpression, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(regularExpression, nameof(regularExpression),
            "Cannot match string against <null>. Provide a regex pattern or use the NotBeNull method.");

        var regexStr = regularExpression.ToString();

        Guard.ThrowIfArgumentIsEmpty(regexStr, nameof(regularExpression),
            "Cannot match string against an empty regex pattern. Provide a regex pattern or use the NotBeEmpty method.");

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .UsingLineBreaks
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} to not match regex {0}{reason}, but it was <null>.", regexStr);

        if (success)
        {
            Execute.Assertion
                .ForCondition(!regularExpression.IsMatch(Subject))
                .BecauseOf(because, becauseArgs)
                .UsingLineBreaks
                .FailWith("Did not expect {context:string} to match regex {0}{reason}, but {1} matches.", regexStr, Subject);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> StartWith(string expected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare start of string with <null>.");

        var stringStartValidator = new StringValidator(
            new StringStartStrategy(StringComparison.Ordinal),
            because, becauseArgs);

        stringStartValidator.Validate(Subject, expected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotStartWith(string unexpected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare start of string with <null>.");

        var negatedStringStartValidator = new StringValidator(
            new NegatedStringStartStrategy(StringComparison.Ordinal),
            because, becauseArgs);

        negatedStringStartValidator.Validate(Subject, unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> StartWithEquivalentOf(string expected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare string start equivalence with <null>.");

        var stringStartValidator = new StringValidator(
            new StringStartStrategy(StringComparison.OrdinalIgnoreCase),
            because, becauseArgs);

        stringStartValidator.Validate(Subject, expected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotStartWithEquivalentOf(string unexpected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare start of string with <null>.");

        var negatedStringStartValidator = new StringValidator(
            new NegatedStringStartStrategy(StringComparison.OrdinalIgnoreCase),
            because, becauseArgs);

        negatedStringStartValidator.Validate(Subject, unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> EndWith(string expected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare string end with <null>.");

        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:string} {0} to end with {1}{reason}.", Subject, expected);

        if (success)
        {
            success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Length >= expected.Length)
                .FailWith("Expected {context:string} to end with {0}{reason}, but {1} is too short.", expected, Subject);

            if (success)
            {
                Execute.Assertion
                    .ForCondition(Subject.EndsWith(expected, StringComparison.Ordinal))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:string} {0} to end with {1}{reason}.", Subject, expected);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotEndWith(string unexpected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare end of string with <null>.");

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} that does not end with {1}{reason}, but found {0}.", Subject, unexpected);

        if (success)
        {
            Execute.Assertion
                .ForCondition(!Subject.EndsWith(unexpected, StringComparison.Ordinal))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:string} {0} not to end with {1}{reason}.", Subject, unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> EndWithEquivalentOf(string expected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot compare string end equivalence with <null>.");

        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                "Expected {context:string} that ends with equivalent of {0}{reason}, but found {1}.", expected, Subject);

        if (success)
        {
            success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Length >= expected.Length)
                .FailWith(
                    "Expected {context:string} to end with equivalent of {0}{reason}, but {1} is too short.",
                    expected, Subject);

            if (success)
            {
                Execute.Assertion
                    .ForCondition(Subject.EndsWith(expected, StringComparison.OrdinalIgnoreCase))
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:string} that ends with equivalent of {0}{reason}, but found {1}.",
                        expected, Subject);
            }
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    public AndConstraint<TAssertions> NotEndWithEquivalentOf(string unexpected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare end of string with <null>.");

        var success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith(
                "Expected {context:string} that does not end with equivalent of {0}{reason}, but found {1}.",
                unexpected, Subject);

        if (success)
        {
            Execute.Assertion
                .ForCondition(!Subject.EndsWith(unexpected, StringComparison.OrdinalIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:string} that does not end with equivalent of {0}{reason}, but found {1}.",
                    unexpected, Subject);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndConstraint<TAssertions> Contain(string expected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot assert string containment against <null>.");
        Guard.ThrowIfArgumentIsEmpty(expected, nameof(expected), "Cannot assert string containment against an empty string.");

        Execute.Assertion
            .ForCondition(Contains(Subject, expected, StringComparison.Ordinal))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} {0} to contain {1}{reason}.", Subject, expected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndConstraint<TAssertions> Contain(string expected, OccurrenceConstraint occurrenceConstraint, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot assert string containment against <null>.");
        Guard.ThrowIfArgumentIsEmpty(expected, nameof(expected), "Cannot assert string containment against an empty string.");

        int actual = Subject.CountSubstring(expected, StringComparison.Ordinal);

        Execute.Assertion
            .ForConstraint(occurrenceConstraint, actual)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                $"Expected {{context:string}} {{0}} to contain {{1}} {{expectedOccurrence}}{{reason}}, but found it {actual.Times()}.",
                Subject, expected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndConstraint<TAssertions> ContainEquivalentOf(string expected, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot assert string containment against <null>.");
        Guard.ThrowIfArgumentIsEmpty(expected, nameof(expected), "Cannot assert string containment against an empty string.");

        Execute.Assertion
            .ForCondition(Contains(Subject, expected, StringComparison.OrdinalIgnoreCase))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} {0} to contain the equivalent of {1}{reason}.", Subject, expected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="expected"/> is empty.</exception>
    public AndConstraint<TAssertions> ContainEquivalentOf(string expected, OccurrenceConstraint occurrenceConstraint,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot assert string containment against <null>.");
        Guard.ThrowIfArgumentIsEmpty(expected, nameof(expected), "Cannot assert string containment against an empty string.");

        int actual = Subject.CountSubstring(expected, StringComparison.OrdinalIgnoreCase);

        Execute.Assertion
            .ForConstraint(occurrenceConstraint, actual)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                $"Expected {{context:string}} {{0}} to contain equivalent of {{1}} {{expectedOccurrence}}{{reason}}, but found it {actual.Times()}.",
                Subject, expected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> ContainAll(IEnumerable<string> values, string because = "", params object[] becauseArgs)
    {
        ThrowIfValuesNullOrEmpty(values);

        IEnumerable<string> missing = values.Where(v => !Contains(Subject, v, StringComparison.Ordinal));

        Execute.Assertion
            .ForCondition(values.All(v => Contains(Subject, v, StringComparison.Ordinal)))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} {0} to contain the strings: {1}{reason}.", Subject, missing);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string contains all values present in <paramref name="values"/>.
    /// </summary>
    /// <param name="values">
    /// The values that should all be present in the string
    /// </param>
    public AndConstraint<TAssertions> ContainAll(params string[] values)
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> ContainAny(IEnumerable<string> values, string because = "", params object[] becauseArgs)
    {
        ThrowIfValuesNullOrEmpty(values);

        Execute.Assertion
            .ForCondition(values.Any(v => Contains(Subject, v, StringComparison.Ordinal)))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} {0} to contain at least one of the strings: {1}{reason}.", Subject, values);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string contains at least one value present in <paramref name="values"/>,.
    /// </summary>
    /// <param name="values">
    /// The values that should will be tested against the string
    /// </param>
    public AndConstraint<TAssertions> ContainAny(params string[] values)
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="unexpected"/> is empty.</exception>
    public AndConstraint<TAssertions> NotContain(string unexpected, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot assert string containment against <null>.");
        Guard.ThrowIfArgumentIsEmpty(unexpected, nameof(unexpected), "Cannot assert string containment against an empty string.");

        Execute.Assertion
            .ForCondition(!Contains(Subject, unexpected, StringComparison.Ordinal))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:string} {0} to contain {1}{reason}.", Subject, unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotContainAll(IEnumerable<string> values, string because = "",
        params object[] becauseArgs)
    {
        ThrowIfValuesNullOrEmpty(values);

        var matches = values.Count(v => Contains(Subject, v, StringComparison.Ordinal));

        Execute.Assertion
            .ForCondition(matches != values.Count())
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:string} {0} to contain all of the strings: {1}{reason}.", Subject, values);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string does not contain all of the strings provided in <paramref name="values"/>. The string
    /// may contain some subset of the provided values.
    /// </summary>
    /// <param name="values">
    /// The values that should not be present in the string
    /// </param>
    public AndConstraint<TAssertions> NotContainAll(params string[] values)
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotContainAny(IEnumerable<string> values, string because = "",
        params object[] becauseArgs)
    {
        ThrowIfValuesNullOrEmpty(values);

        IEnumerable<string> matches = values.Where(v => Contains(Subject, v, StringComparison.Ordinal));

        Execute.Assertion
            .ForCondition(!matches.Any())
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:string} {0} to contain any of the strings: {1}{reason}.", Subject, matches);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string does not contain any of the strings provided in <paramref name="values"/>.
    /// </summary>
    /// <param name="values">
    /// The values that should not be present in the string
    /// </param>
    public AndConstraint<TAssertions> NotContainAny(params string[] values)
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotContainEquivalentOf(string unexpected, string because = "",
        params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Contains(Subject, unexpected, StringComparison.OrdinalIgnoreCase))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:string} to contain equivalent of {0}{reason} but found {1}.", unexpected, Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    private static bool Contains(string actual, string expected, StringComparison comparison)
    {
        return (actual ?? string.Empty).Contains(expected ?? string.Empty, comparison);
    }

    /// <summary>
    /// Asserts that a string is <see cref="string.Empty"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeEmpty(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject?.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} to be empty{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string is not <see cref="string.Empty"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeEmpty(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is null || Subject.Length > 0)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:string} to be empty{reason}.");

        return new AndConstraint<TAssertions>((TAssertions)this);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveLength(int expected, string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected {context:string} with length {0}{reason}, but found <null>.", expected);

        if (success)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Length == expected)
                .FailWith("Expected {context:string} with length {0}{reason}, but found string {1} with length {2}.",
                    expected, Subject, Subject.Length);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string is neither <see langword="null"/> nor <see cref="string.Empty"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotBeNullOrEmpty(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!string.IsNullOrEmpty(Subject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} not to be <null> or empty{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string is either <see langword="null"/> or <see cref="string.Empty"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> BeNullOrEmpty(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(string.IsNullOrEmpty(Subject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} to be <null> or empty{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string is neither <see langword="null"/> nor <see cref="string.Empty"/> nor white space
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotBeNullOrWhiteSpace(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!string.IsNullOrWhiteSpace(Subject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} not to be <null> or whitespace{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that a string is either <see langword="null"/> or <see cref="string.Empty"/> or white space
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> BeNullOrWhiteSpace(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(string.IsNullOrWhiteSpace(Subject))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:string} to be <null> or whitespace{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that all characters in a string are in upper casing.
    /// </summary>
    /// <remarks>
    /// Be careful that numbers and special characters don't have casing, so  <see cref="BeUpperCased"/>
    /// will always fail on a string that contains anything but alphabetic characters.
    /// In those cases, we recommend using <see cref="NotBeLowerCased"/>.
    /// </remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeUpperCased(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject?.All(char.IsUpper) == true)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected all characters in {context:string} to be upper cased{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that all characters in a string are not in upper casing.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeUpperCased(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is null || Subject.Any(ch => !char.IsUpper(ch)))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect any characters in {context:string} to be upper cased{reason}.");

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that all characters in a string are in lower casing.
    /// </summary>
    /// <remarks>
    /// Be careful that numbers and special characters don't have casing, so <see cref="BeLowerCased"/> will always fail on
    /// a string that contains anything but alphabetic characters.
    /// In those cases, we recommend using <see cref="NotBeUpperCased"/>.
    /// </remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeLowerCased(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject?.All(char.IsLower) == true)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected all characters in {context:string} to be lower cased{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that all characters in a string are not in lower casing.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBeLowerCased(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject is null || Subject.Any(ch => !char.IsLower(ch)))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect any characters in {context:string} to be lower cased{reason}.");

        return new AndConstraint<TAssertions>((TAssertions)this);
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
