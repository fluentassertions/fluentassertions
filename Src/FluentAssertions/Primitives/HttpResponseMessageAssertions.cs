using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

/// <summary>
/// Contains a number of methods to assert that a <see cref="HttpResponseMessage"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class HttpResponseMessageAssertions : HttpResponseMessageAssertions<HttpResponseMessageAssertions>
{
    public HttpResponseMessageAssertions(HttpResponseMessage value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }
}

/// <summary>
/// Contains a number of methods to assert that a <see cref="HttpResponseMessage" /> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class HttpResponseMessageAssertions<TAssertions> : ObjectAssertions<HttpResponseMessage, TAssertions>
    where TAssertions : HttpResponseMessageAssertions<TAssertions>
{
    private readonly AssertionChain assertionChain;

    protected HttpResponseMessageAssertions(HttpResponseMessage value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the <see cref="HttpStatusCode"/> is successful (2xx).
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeSuccessful([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected HttpStatusCode to be successful (2xx){reason}, but HttpResponseMessage was <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(Subject!.IsSuccessStatusCode)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected HttpStatusCode to be successful (2xx){reason}, but found {0}.", Subject.StatusCode);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="HttpStatusCode"/> is redirection (3xx).
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeRedirection([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected HttpStatusCode to be redirection (3xx){reason}, but HttpResponseMessage was <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition((int)Subject!.StatusCode is >= 300 and <= 399)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected HttpStatusCode to be redirection (3xx){reason}, but found {0}.", Subject.StatusCode);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="HttpStatusCode"/> is either client (4xx) or server error (5xx).
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveError([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected HttpStatusCode to be an error{reason}, but HttpResponseMessage was <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(IsClientError() || IsServerError())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected HttpStatusCode to be an error{reason}, but found {0}.", Subject.StatusCode);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="HttpStatusCode"/> is client error (4xx).
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveClientError([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected HttpStatusCode to be client error (4xx){reason}, but HttpResponseMessage was <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(IsClientError())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected HttpStatusCode to be client error (4xx){reason}, but found {0}.", Subject.StatusCode);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="HttpStatusCode"/> is server error (5xx).
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveServerError([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected HttpStatusCode to be server error (5xx){reason}, but HttpResponseMessage was <null>.");

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(IsServerError())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected HttpStatusCode to be server error (5xx){reason}, but found {0}.", Subject.StatusCode);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="HttpStatusCode"/> is equal to the specified <paramref name="expected"/> value.
    /// </summary>
    /// <param name="expected">The expected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> HaveStatusCode(HttpStatusCode expected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected HttpStatusCode to be {0}{reason}, but HttpResponseMessage was <null>.", expected);

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(Subject!.StatusCode == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected HttpStatusCode to be {0}{reason}, but found {1}.", expected, Subject.StatusCode);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    /// <summary>
    /// Asserts that the <see cref="HttpStatusCode"/> is not equal to the specified <paramref name="unexpected"/> value.
    /// </summary>
    /// <param name="unexpected">The unexpected value</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<TAssertions> NotHaveStatusCode(HttpStatusCode unexpected,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected HttpStatusCode not to be {0}{reason}, but HttpResponseMessage was <null>.", unexpected);

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(Subject!.StatusCode != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected HttpStatusCode not to be {0}{reason}, but found {1}.", unexpected, Subject.StatusCode);
        }

        return new AndConstraint<TAssertions>((TAssertions)this);
    }

    private bool IsServerError() => (int)Subject.StatusCode is >= 500 and <= 599;

    private bool IsClientError() => (int)Subject.StatusCode is >= 400 and <= 499;

    protected override string Identifier => "HTTP response message";
}
