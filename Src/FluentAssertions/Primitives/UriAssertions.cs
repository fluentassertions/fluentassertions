using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

/// <summary>
/// Contains a number of methods to assert that a <see cref="Uri"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class UriAssertions : UriAssertions<UriAssertions>
{
    public UriAssertions(Uri value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }
}

/// <summary>
/// Contains a number of methods to assert that a <see cref="Uri"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class UriAssertions<TAssertions> : ObjectAssertions<Uri, TAssertions>
    where TAssertions : UriAssertions<TAssertions>
{
    public UriAssertions(Uri value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }

    /// <summary>
    /// Asserts that the URI equals <paramref name="expected"/>, comparing like <see cref="Uri.Equals(object)"/>.
    /// The scheme and host are therefore compared case-insensitively.
    /// </summary>
    /// <param name="expected">The expected URI as a string.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> Be(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        // ReSharper disable once SuspiciousTypeConversion.Global (Uri.Equals parses a string argument)
        bool isEqual = Subject is not null && Subject.Equals(expected);

        CurrentAssertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(isEqual)
            .FailWith("Expected {context:URI} to be {0}{reason}, but found {1}.", expected, Subject?.OriginalString);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI does not equal <paramref name="unexpected"/>, comparing like <see cref="Uri.Equals(object)"/>.
    /// </summary>
    /// <param name="unexpected">The URI string that is not expected.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotBe(string unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        // ReSharper disable once SuspiciousTypeConversion.Global (Uri.Equals parses a string argument)
        bool isEqual = Subject is not null && Subject.Equals(unexpected);

        CurrentAssertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(!isEqual)
            .FailWith("Did not expect {context:URI} to be {0}{reason}.", unexpected);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI is absolute.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeAbsolute(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is { IsAbsoluteUri: true })
            .FailWith("Expected {context:URI} to be absolute{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI is relative.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeRelative(
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        CurrentAssertionChain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is { IsAbsoluteUri: false })
            .FailWith("Expected {context:URI} to be relative{reason}, but found {0}.", Subject);

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI has the expected scheme, compared case-insensitively.
    /// </summary>
    /// <param name="expected">The expected scheme, such as <c>https</c>.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveScheme(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("scheme", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(string.Equals(Subject.Scheme, expected, StringComparison.OrdinalIgnoreCase))
                .FailWith("Expected {context:URI} {0} to have scheme {1}{reason}, but found {2}.", Subject, expected,
                    Subject.Scheme);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI does not have the unexpected scheme, compared case-insensitively.
    /// </summary>
    /// <param name="unexpected">The scheme that is not expected.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveScheme(string unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("scheme", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.Equals(Subject.Scheme, unexpected, StringComparison.OrdinalIgnoreCase))
                .FailWith("Did not expect {context:URI} {0} to have scheme {1}{reason}.", Subject, unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI has the expected host, compared case-insensitively.
    /// </summary>
    /// <param name="expected">The expected host.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveHost(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("host", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(string.Equals(Subject.Host, expected, StringComparison.OrdinalIgnoreCase))
                .FailWith("Expected {context:URI} {0} to have host {1}{reason}, but found {2}.", Subject, expected, Subject.Host);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI does not have the unexpected host, compared case-insensitively.
    /// </summary>
    /// <param name="unexpected">The host that is not expected.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveHost(string unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("host", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.Equals(Subject.Host, unexpected, StringComparison.OrdinalIgnoreCase))
                .FailWith("Did not expect {context:URI} {0} to have host {1}{reason}.", Subject, unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI has the expected port. The default port of a well-known scheme is taken into account.
    /// </summary>
    /// <param name="expected">The expected port.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HavePort(int expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("port", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Port == expected)
                .FailWith("Expected {context:URI} {0} to have port {1}{reason}, but found {2}.", Subject, expected, Subject.Port);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI does not have the unexpected port.
    /// </summary>
    /// <param name="unexpected">The port that is not expected.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHavePort(int unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("port", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Port != unexpected)
                .FailWith("Did not expect {context:URI} {0} to have port {1}{reason}.", Subject, unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI has the expected absolute path, compared case-sensitively and excluding the query string.
    /// </summary>
    /// <param name="expected">The expected path, such as <c>/orders/42</c>.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HavePath(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("path", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Uri.UnescapeDataString(Subject.AbsolutePath) == expected)
                .FailWith("Expected {context:URI} {0} to have path {1}{reason}, but found {2}.", Subject, expected,
                    Uri.UnescapeDataString(Subject.AbsolutePath));
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI does not have the unexpected absolute path.
    /// </summary>
    /// <param name="unexpected">The path that is not expected.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHavePath(string unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("path", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Uri.UnescapeDataString(Subject.AbsolutePath) != unexpected)
                .FailWith("Did not expect {context:URI} {0} to have path {1}{reason}.", Subject, unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the query string contains a parameter with the specified name.
    /// </summary>
    /// <param name="name">The name of the query parameter.</param>
    /// <returns>An object that exposes the (unescaped) value of the first matching parameter through <c>Which</c>.</returns>
    public AndWhichConstraint<TAssertions, string> HaveQueryParameter(string name)
    {
        string parameterValue = null;

        if (AssertIsAbsolute("query parameter", string.Empty))
        {
            parameterValue = GetQueryParameters(name).FirstOrDefault();

            CurrentAssertionChain
                .ForCondition(parameterValue is not null)
                .FailWith("Expected {context:URI} {0} to have query parameter {1}, but it was not found.", Subject, name);
        }

        return new AndWhichConstraint<TAssertions, string>((TAssertions)this, parameterValue, CurrentAssertionChain);
    }

    /// <summary>
    /// Asserts that the query string contains a parameter with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the query parameter.</param>
    /// <param name="expectedValue">The expected (unescaped) value of the query parameter.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveQueryParameter(string name, string expectedValue,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("query parameter", because, becauseArgs))
        {
            string[] values = GetQueryParameters(name).ToArray();

            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(values.Contains(expectedValue))
                .FailWith(
                    "Expected {context:URI} {0} to have query parameter {1} with value {2}{reason}, but " +
                    (values.Length == 0 ? "the parameter was not found." : "found {3}."),
                    Subject, name, expectedValue, values);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the query string does not contain a parameter with the specified name.
    /// </summary>
    /// <param name="name">The name of the query parameter.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveQueryParameter(string name,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("query parameter", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!GetQueryParameters(name).Any())
                .FailWith("Did not expect {context:URI} {0} to have query parameter {1}{reason}.", Subject, name);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI has the expected fragment, excluding the leading <c>#</c>.
    /// </summary>
    /// <param name="expected">The expected fragment.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveFragment(string expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("fragment", because, becauseArgs))
        {
            string actual = Uri.UnescapeDataString(Subject.Fragment.TrimStart('#'));

            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(actual == expected)
                .FailWith("Expected {context:URI} {0} to have fragment {1}{reason}, but found {2}.", Subject, expected, actual);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI does not have the unexpected fragment, excluding the leading <c>#</c>.
    /// </summary>
    /// <param name="unexpected">The fragment that is not expected.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> NotHaveFragment(string unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        if (AssertIsAbsolute("fragment", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Uri.UnescapeDataString(Subject.Fragment.TrimStart('#')) != unexpected)
                .FailWith("Did not expect {context:URI} {0} to have fragment {1}{reason}.", Subject, unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI is a base of <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The URI that is expected to start with the subject.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="expected"/> is <c>null</c>.</exception>
    public AndConstraint<TAssertions> BeBaseOf(Uri expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expected);

        if (AssertIsAbsolute("base", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.IsBaseOf(expected))
                .FailWith("Expected {context:URI} {0} to be a base of {1}{reason}.", Subject, expected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the URI is not a base of <paramref name="unexpected"/>.
    /// </summary>
    /// <param name="unexpected">The URI that is not expected to start with the subject.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="unexpected"/> is <c>null</c>.</exception>
    public AndConstraint<TAssertions> NotBeBaseOf(Uri unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(unexpected);

        if (AssertIsAbsolute("base", because, becauseArgs))
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!Subject.IsBaseOf(unexpected))
                .FailWith("Did not expect {context:URI} {0} to be a base of {1}{reason}.", Subject, unexpected);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    [StackTraceHidden]
    private bool AssertIsAbsolute(string part, string because, params object[] becauseArgs)
    {
        bool isAbsolute = Subject is { IsAbsoluteUri: true };

        CurrentAssertionChain
            .ForCondition(isAbsolute)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:URI} to be an absolute URI with a " + part + "{reason}, but found {0}.", Subject);

        return isAbsolute;
    }

    [StackTraceHidden]
    private IEnumerable<string> GetQueryParameters(string name)
    {
        foreach (string pair in Subject.Query.TrimStart('?').Split('&'))
        {
            if (pair.Length == 0)
            {
                continue;
            }

            int separator = pair.IndexOf('=', 0);
            string key = separator < 0 ? pair : pair.Substring(0, separator);

            if (Unescape(key) == name)
            {
                yield return separator < 0 ? string.Empty : Unescape(pair.Substring(separator + 1));
            }
        }
    }

    [StackTraceHidden]
    private static string Unescape(string value) => Uri.UnescapeDataString(value.Replace('+', ' '));

    /// <inheritdoc />
    protected override string Identifier => "URI";
}
