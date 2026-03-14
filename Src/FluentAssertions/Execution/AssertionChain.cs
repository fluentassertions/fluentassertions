using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

/// <summary>
/// Provides a fluent API to build simple or composite assertions, and which can flow from one assertion to another.
/// </summary>
/// <remarks>
/// This is the core engine of many of the assertion APIs in this library. When combined with <see cref="AssertionScope"/>,
/// you can run multiple assertions which failure messages will be collected until the scope is disposed.
/// </remarks>
[System.Diagnostics.StackTraceHidden]
public sealed class AssertionChain
{
    private readonly Func<AssertionScope> getCurrentScope;
    private readonly ContextDataDictionary contextData = new();
    private readonly SubjectIdentificationBuilder identifierBuilder;
    private string fallbackIdentifier = "object";
    private Func<string> reason;
    private bool? succeeded;

    // We need to keep track of this because we don't want the second successful assertion hide the first unsuccessful assertion
    private Func<string> expectation;

    private static readonly AsyncLocal<AssertionChain> Instance = new();

    /// <summary>
    /// Ensures that the next call to <see cref="GetOrCreate"/> will reuse the current instance.
    /// </summary>
    public AssertionChain ReuseOnce()
    {
        Instance.Value = this;
        return this;
    }

    /// <summary>
    /// Indicates whether the previous assertion in the chain was successful.
    /// </summary>
    /// <remarks>
    /// This property is used internally to determine if subsequent assertions
    /// should be evaluated based on the result of the previous assertion.
    /// </remarks>
    internal bool PreviousAssertionSucceeded { get; private set; } = true;

    /// <summary>
    /// Indicates whether the caller identifier has been manually overridden.
    /// </summary>
    /// <remarks>
    /// This property is used to track if the caller identifier has been customized using the
    /// <see cref="OverrideCallerIdentifier"/> method or similar methods that modify the identifier.
    /// </remarks>
    public bool HasOverriddenCallerIdentifier => identifierBuilder.HasOverriddenIdentifier;

    /// <summary>
    /// Either starts a new assertion chain, or, when <see cref="ReuseOnce"/> was called, for once, will return
    /// an existing instance.
    /// </summary>
    public static AssertionChain GetOrCreate()
    {
        if (Instance.Value != null)
        {
            AssertionChain assertionChain = Instance.Value;
            Instance.Value = null;
            return assertionChain;
        }

        return new AssertionChain(() => AssertionScope.Current,
            () => FluentAssertions.CallerIdentifier.DetermineCallerIdentities());
    }

    private AssertionChain(Func<AssertionScope> getCurrentScope, Func<string[]> getCallerIdentifiers)
    {
        this.getCurrentScope = getCurrentScope;

        identifierBuilder = new SubjectIdentificationBuilder(getCallerIdentifiers, () => getCurrentScope().Name());
    }

    /// <summary>
    /// The effective caller identifier including any prefixes and postfixes configured through
    /// <see cref="WithCallerPostfix"/>.
    /// </summary>
    /// <remarks>
    /// Can be overridden with <see cref="OverrideCallerIdentifier"/>.
    /// </remarks>
    public string CallerIdentifier => identifierBuilder.Build();

    /// <summary>
    /// Adds an explanation of why the assertion is supposed to succeed to the scope.
    /// </summary>
    /// <param name="reason">
    /// An object containing a formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed, as well as zero or more objects to format the placeholders.
    /// If the phrase does not start with the word <i>because</i>, it is prepended automatically.explaining why the assertion is needed.
    /// </param>
    public AssertionChain BecauseOf(Reason reason)
    {
        return BecauseOf(reason.FormattedMessage, reason.Arguments);
    }

    /// <summary>
    /// Adds an explanation of why the assertion is supposed to succeed to the scope.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AssertionChain BecauseOf([StringSyntax("CompositeFormat")] string because, params object[] becauseArgs)
    {
        reason = () =>
        {
            try
            {
                string becauseOrEmpty = because ?? string.Empty;

                return becauseArgs?.Length > 0
                    ? string.Format(CultureInfo.InvariantCulture, becauseOrEmpty, becauseArgs)
                    : becauseOrEmpty;
            }
            catch (FormatException formatException)
            {
                return
                    $"**WARNING** because message '{because}' could not be formatted with string.Format{Environment.NewLine}{formatException.StackTrace}";
            }
        };

        return this;
    }

    [SuppressMessage("Maintainability", "AV1564:Parameter in public or internal member is of type bool or bool?")]
    public AssertionChain ForCondition(bool condition)
    {
        if (PreviousAssertionSucceeded)
        {
            succeeded = condition;
        }

        return this;
    }

    public AssertionChain ForConstraint(OccurrenceConstraint constraint, int actualOccurrences)
    {
        if (PreviousAssertionSucceeded)
        {
            constraint.RegisterContextData((key, value) => contextData.Add(new ContextDataDictionary.DataItem(key, value)));

            succeeded = constraint.Assert(actualOccurrences);
        }

        return this;
    }

    public Continuation WithExpectation(string message, object arg1, Action<AssertionChain> chain)
    {
        return WithExpectation(message, chain, arg1);
    }

    public Continuation WithExpectation(string message, object arg1, object arg2, Action<AssertionChain> chain)
    {
        return WithExpectation(message, chain, arg1, arg2);
    }

    public Continuation WithExpectation(string message, Action<AssertionChain> chain)
    {
        return WithExpectation(message, chain, []);
    }

    private Continuation WithExpectation(string message, Action<AssertionChain> chain, params object[] args)
    {
        if (PreviousAssertionSucceeded)
        {
            // Capture scope-dependent values eagerly so they remain correct
            // even when the expectation message is rendered later (deferred rendering).
            var capturedFormattingOptions = getCurrentScope().FormattingOptions;
            var capturedReason = reason;
            var capturedIdentifier = CallerIdentifier;
            var capturedFallbackIdentifier = fallbackIdentifier;

            expectation = () =>
            {
                var formatter = new FailureMessageFormatter(capturedFormattingOptions)
                    .WithReason(capturedReason?.Invoke() ?? string.Empty)
                    .WithContext(contextData)
                    .WithIdentifier(capturedIdentifier)
                    .WithFallbackIdentifier(capturedFallbackIdentifier);

                return formatter.Format(message, args);
            };

            chain(this);

            expectation = null;
        }

        return new Continuation(this);
    }

    public AssertionChain WithDefaultIdentifier(string identifier)
    {
        fallbackIdentifier = identifier;
        return this;
    }

    public GivenSelector<T> Given<T>(Func<T> selector)
    {
        return new GivenSelector<T>(selector, this);
    }

    internal Continuation FailWithPreFormatted(string formattedFailReason)
    {
        return FailWith(() => formattedFailReason);
    }

    public Continuation FailWith(string message)
    {
        return FailWith(() => new FailReason(message));
    }

    public Continuation FailWith(string message, params object[] args)
    {
        return FailWith(() => new FailReason(message, args));
    }

    public Continuation FailWith(string message, params Func<object>[] argProviders)
    {
        return FailWith(() => new FailReason(
            message,
            argProviders.Select(a => a()).ToArray()));
    }

    public Continuation FailWith(Func<FailReason> getFailureReason)
    {
        if (PreviousAssertionSucceeded)
        {
            PreviousAssertionSucceeded = succeeded == true;

            if (succeeded != true)
            {
                // Capture scope-dependent values at failure time so they remain correct
                // even when the failure message is rendered later (deferred rendering).
                var capturedFormattingOptions = getCurrentScope().FormattingOptions;
                var capturedReason = reason;
                var capturedIdentifier = CallerIdentifier;
                var capturedFallbackIdentifier = fallbackIdentifier;
                Func<string> capturedExpectation = expectation;

                var failure = new AssertionFailure(() =>
                {
                    var formatter = new FailureMessageFormatter(capturedFormattingOptions)
                        .WithReason(capturedReason?.Invoke() ?? string.Empty)
                        .WithContext(contextData)
                        .WithIdentifier(capturedIdentifier)
                        .WithFallbackIdentifier(capturedFallbackIdentifier);

                    FailReason failReason = getFailureReason();
                    string message = formatter.Format(failReason.Message, failReason.Args);

                    if (capturedExpectation is not null)
                    {
                        message = capturedExpectation() + message;
                    }

                    return message.Capitalize().RemoveTrailingWhitespaceFromLines();
                });

                getCurrentScope().AddFailure(failure);
            }
        }

        // Reset the state for successive assertions on this object
        succeeded = null;

        return new Continuation(this);
    }

    private Continuation FailWith(Func<string> getFailureReason)
    {
        if (PreviousAssertionSucceeded)
        {
            PreviousAssertionSucceeded = succeeded == true;

            if (succeeded != true)
            {
                Func<string> capturedExpectation = expectation;

                var failure = new AssertionFailure(() =>
                {
                    string message = getFailureReason();

                    if (capturedExpectation is not null)
                    {
                        message = capturedExpectation() + message;
                    }

                    return message.Capitalize().RemoveTrailingWhitespaceFromLines();
                });

                getCurrentScope().AddFailure(failure);
            }
        }

        // Reset the state for successive assertions on this object
        succeeded = null;

        return new Continuation(this);
    }

    /// <summary>
    /// Allows overriding the caller identifier for the next call to one of the `FailWith` overloads instead
    /// of relying on the automatic behavior that extracts the variable names from the C# code.
    /// </summary>
    public void OverrideCallerIdentifier(Func<string> getCallerIdentifier)
    {
        identifierBuilder.OverrideSubjectIdentifier(getCallerIdentifier);
    }

    /// <summary>
    /// Adds a postfix such as <c>[0]</c> to the caller identifier detected by the library.
    /// </summary>
    /// <remarks>
    /// Can be used by an assertion that uses <see cref="AndWhichConstraint{TParent,TSubject}"/> to return an object or
    /// collection on which another assertion is executed, and which wants to amend the automatically detected caller
    /// identifier with a postfix.
    /// </remarks>
    public AssertionChain WithCallerPostfix(string postfix)
    {
        identifierBuilder.UsePostfix(postfix);
        return this;
    }

    /// <summary>
    /// Adds a specified prefix to the caller identifier used in the current assertion chain.
    /// </summary>
    /// <param name="prefix">The prefix to prepend to the caller identifier.</param>
    /// <returns>An updated instance of <see cref="AssertionChain"/> with the specified prefix applied.</returns>
    public AssertionChain WithCallerPrefix(string prefix)
    {
        identifierBuilder.UsePrefix(prefix);
        return this;
    }

    /// <summary>
    /// Marks the next assertion as being part of a chained call to Should where it needs to find the next
    /// caller identifier.
    /// </summary>
    internal AssertionChain AdvanceToNextIdentifier()
    {
        identifierBuilder.AdvanceToNextSubject();
        return this;
    }

    /// <summary>
    /// Adds some information to the assertion that will be included in the message
    /// that is emitted if an assertion fails.
    /// </summary>
    public void AddReportable(string key, string value)
    {
        getCurrentScope().AddReportable(key, value);
    }

    /// <summary>
    /// Adds some information to the assertion that will be included in the message
    /// that is emitted if an assertion fails. The value is only calculated on failure.
    /// </summary>
    public void AddReportable(string key, Func<string> getValue)
    {
        getCurrentScope().AddReportable(key, getValue);
    }

    /// <summary>
    /// Fluent alternative for <see cref="AddReportable(string,string)"/>
    /// </summary>
    public AssertionChain WithReportable(string name, Func<string> content)
    {
        getCurrentScope().AddReportable(name, content);
        return this;
    }

    /// <summary>
    /// Registers a failure in the chain that doesn't need any parsing or formatting anymore.
    /// </summary>
    internal void AddPreFormattedFailure(string failure)
    {
        getCurrentScope().AddFailure(new AssertionFailure(failure));
    }

    /// <summary>
    /// Gets a value indicating whether all assertions in the <see cref="AssertionChain"/> have succeeded.
    /// </summary>
    public bool Succeeded => PreviousAssertionSucceeded && succeeded is null or true;

    public AssertionChain UsingLineBreaks
    {
        get
        {
            getCurrentScope().FormattingOptions.UseLineBreaks = true;
            return this;
        }
    }
}
