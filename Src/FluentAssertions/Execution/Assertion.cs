using System;
using System.Globalization;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

public class Assertion<TAssertion> : IAssertion
    where TAssertion : Assertion<TAssertion>
{
    private const string FallbackIdentifier = "object";
    private Func<string> getCallerIdentifier;
    private bool previousAssertionSucceeded;
    private Func<string> reason;
    private bool? succeeded;
    private Func<string> expectation;

    public Assertion(IAssertionScope currentScope, Func<string> getCallerIdentifier, bool previousAssertionSucceeded = true)
    {
        this.CurrentScope = currentScope;
        this.getCallerIdentifier = getCallerIdentifier;
        this.previousAssertionSucceeded = previousAssertionSucceeded;
    }

    internal Assertion(IAssertion previousAssertion)
        : this(previousAssertion.CurrentScope, previousAssertion.GetCallerIdentifier, previousAssertion.Succeeded)
    {
    }

    /// <summary>
    /// Gets or sets the context of the current assertion scope, e.g. the path of the object graph
    /// that is being asserted on. The context is provided by a <see cref="Lazy{String}"/> which
    /// only gets evaluated when its value is actually needed (in most cases during a failure).
    /// </summary>
    public Lazy<string> Context { get; set; }

    /// <summary>
    /// Adds an explanation of why the assertion is supposed to succeed to the scope.
    /// </summary>
    public TAssertion BecauseOf(Reason reason)
    {
        return BecauseOf(reason.FormattedMessage, reason.Arguments);
    }

    /// <inheritdoc cref="IAssertionScope.BecauseOf(string, object[])"/>
    public TAssertion BecauseOf(string because, params object[] becauseArgs)
    {
        if (previousAssertionSucceeded)
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
        }

        return (TAssertion)this;
    }

    /// <inheritdoc cref="IAssertionScope.ForCondition(bool)"/>
    public TAssertion ForCondition(bool condition)
    {
        if (previousAssertionSucceeded)
        {
            succeeded = condition;
        }

        return (TAssertion)this;
    }

    void IAssertion.ForCondition(bool predicate) => ForCondition(predicate);

    void IAssertion.FailWith(string message, object[] args) => FailWith(message, args);

    public TAssertion ForConstraint(OccurrenceConstraint constraint, int actualOccurrences)
    {
        if (previousAssertionSucceeded)
        {
            constraint.RegisterReportables(CurrentScope);
            succeeded = constraint.Assert(actualOccurrences);
        }

        return (TAssertion)this;
    }

    public TAssertion WithExpectation(string message, params object[] args)
    {
        if (previousAssertionSucceeded)
        {
            Func<string> localReason = reason;

            expectation = () =>
            {
                var messageBuilder = new MessageBuilder(CurrentScope.FormattingOptions);
                string actualReason = localReason?.Invoke() ?? string.Empty;
                string identifier = getCallerIdentifier();

                return messageBuilder.Build(
                    message,
                    args,
                    actualReason,
                    CurrentScope.ContextData,
                    identifier,
                    FallbackIdentifier);
            };
        }

        return (TAssertion)this;
    }

    public NewContinuation<ContinuedAssertion> FailWith(string message)
    {
        return FailWith(() => new FailReason(message));
    }

    public NewContinuation<ContinuedAssertion> FailWith(string message, params object[] args)
    {
        return FailWith(() => new FailReason(message, args));
    }

    public NewContinuation<ContinuedAssertion> FailWith(string message, params Func<object>[] argProviders)
    {
        return FailWith(
            () => new FailReason(
                message,
                argProviders.Select(a => a()).ToArray()));
    }

    public NewContinuation<ContinuedAssertion> FailWith(Func<FailReason> failReasonFunc)
    {
        return FailWith(
            () =>
            {
                string localReason = reason?.Invoke() ?? string.Empty;
                var messageBuilder = new MessageBuilder(CurrentScope.FormattingOptions);
                string identifier = getCallerIdentifier();
                FailReason failReason = failReasonFunc();

                string result = messageBuilder.Build(
                    failReason.Message,
                    failReason.Args,
                    localReason,
                    CurrentScope.ContextData,
                    identifier,
                    FallbackIdentifier);

                return result;
            });
    }

    private NewContinuation<ContinuedAssertion> FailWith(Func<string> failReasonFunc)
    {
        if (previousAssertionSucceeded)
        {
            previousAssertionSucceeded = succeeded is not null or true;
            if (!previousAssertionSucceeded)
            {
                string result = failReasonFunc();

                if (expectation is not null)
                {
                    result = expectation() + result;
                }

                CurrentScope.AddPreFormattedFailure(result.Capitalize());
            }
        }

        // Reset the state for successive assertions on this object
        succeeded = null;

        return new NewContinuation<ContinuedAssertion>(this);
    }

    public TAssertion WithCallerPostfix(string postfix)
    {
        var originalCallerIdentifier = getCallerIdentifier;
        getCallerIdentifier = () => originalCallerIdentifier() + postfix;
        return (TAssertion)this;
    }

    public IAssertionScope CurrentScope { get; }

    public Func<string> GetCallerIdentifier => getCallerIdentifier;

    public bool Succeeded => previousAssertionSucceeded && (succeeded is null or true);

    public TAssertion UsingLineBreaks
    {
        get
        {
            CurrentScope.FormattingOptions.UseLineBreaks = true;
            return (TAssertion)this;
        }
    }
}

public class Assertion : Assertion<Assertion>
{
    public Assertion(IAssertionScope currentScope, Func<string> getCallerIdentifier)
        : base(currentScope, getCallerIdentifier, previousAssertionSucceeded: true)
    {
    }
}
