using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

public class Assertion<TAssertion> : IAssertion
    where TAssertion : Assertion<TAssertion>
{
    private readonly StringBuilder tracing = new();
    private readonly ContextDataItems contextData = new();
    private string fallbackIdentifier = "object";
    private Func<string> getCallerIdentifier;
    private bool previousAssertionSucceeded;
    private Func<string> reason;
    private bool? succeeded;
    private Func<string> expectation;

    protected Assertion(Func<IAssertionScope> getCurrentScope, Func<string> getCallerIdentifier, bool previousAssertionSucceeded = true)
    {
        this.GetCurrentScope = getCurrentScope;
        this.getCallerIdentifier = getCallerIdentifier;
        this.previousAssertionSucceeded = previousAssertionSucceeded;
    }

    protected Assertion(IAssertion previousAssertion)
        : this(previousAssertion.GetCurrentScope, previousAssertion.GetCallerIdentifier, previousAssertion.Succeeded)
    {
        reason = previousAssertion.Reason;
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
            constraint.RegisterReportables(GetCurrentScope());
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
                var messageBuilder = new MessageBuilder(GetCurrentScope().FormattingOptions);
                string actualReason = localReason?.Invoke() ?? string.Empty;
                string identifier = getCallerIdentifier();

                return messageBuilder.Build(
                    message,
                    args,
                    actualReason,
                    GetCurrentScope().ContextData,
                    identifier,
                    fallbackIdentifier);
            };
        }

        return (TAssertion)this;
    }

    public void WithReportable(string name, Func<string> content)
    {
        GetCurrentScope().AddReportable(name, content);
    }

    /// <inheritdoc cref="IAssertionScope.WithDefaultIdentifier(string)"/>
    public TAssertion WithDefaultIdentifier(string identifier)
    {
        fallbackIdentifier = identifier;
        return (TAssertion)this;
    }

    public NewGivenSelector<T> Given<T>(Func<T> selector)
    {
        return new NewGivenSelector<T>(selector, this);
    }

    internal NewContinuation<ContinuedAssertion> FailWithPreFormatted(string formattedFailReason)
    {
        return FailWith(() => formattedFailReason);
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
                var messageBuilder = new MessageBuilder(GetCurrentScope().FormattingOptions);
                string identifier = getCallerIdentifier();
                FailReason failReason = failReasonFunc();

                string result = messageBuilder.Build(
                    failReason.Message,
                    failReason.Args,
                    localReason,
                    contextData,
                    identifier,
                    fallbackIdentifier);

                return result;
            });
    }

    private NewContinuation<ContinuedAssertion> FailWith(Func<string> failReasonFunc)
    {
        if (previousAssertionSucceeded)
        {
            previousAssertionSucceeded = succeeded is true;
            if (!previousAssertionSucceeded)
            {
                string result = failReasonFunc();

                if (expectation is not null)
                {
                    result = expectation() + result;
                }

                GetCurrentScope().AddPreFormattedFailure(result.Capitalize());
            }
        }

        // Reset the state for successive assertions on this object
        succeeded = null;

        return new NewContinuation<ContinuedAssertion>(this);
    }

    public void AddCallerPostfix(string postfix)
    {
        var originalCallerIdentifier = getCallerIdentifier;
        getCallerIdentifier = () => originalCallerIdentifier() + postfix;
    }

    /// <summary>
    /// Adds a block of tracing to the scope for reporting when an assertion fails.
    /// </summary>
    public void AppendTracing(string tracingBlock)
    {
        tracing.Append(tracingBlock);
    }

    internal void TrackComparands(object subject, object expectation)
    {
        contextData.Add(new ContextDataItems.DataItem("subject", subject, reportable: false, requiresFormatting: true));
        contextData.Add(new ContextDataItems.DataItem("expectation", expectation, reportable: false, requiresFormatting: true));
    }

    public Func<IAssertionScope> GetCurrentScope { get; }

    public Func<string> GetCallerIdentifier => getCallerIdentifier;

    public bool Succeeded => previousAssertionSucceeded && (succeeded is null or true);

    public TAssertion UsingLineBreaks
    {
        get
        {
            GetCurrentScope().FormattingOptions.UseLineBreaks = true;
            return (TAssertion)this;
        }
    }

    public Func<string> Reason => reason;
}

public sealed class Assertion : Assertion<Assertion>
{
    // REFACTOR: Do we really need to pass in the scope and identifier?
    private Assertion(Func<IAssertionScope> currentScope, Func<string> getCallerIdentifier)
        : base(currentScope, getCallerIdentifier, previousAssertionSucceeded: true)
    {
    }

    private static readonly AsyncLocal<Assertion> Instance = new();

    public static void ReuseOnce(Assertion assertion)
    {
        Instance.Value = assertion;
    }

    public static Assertion GetOrCreate()
    {
        return GetOrCreate(() => AssertionScope.Current, () => AssertionScope.Current.GetIdentifier());
    }

    public static Assertion GetOrCreate(Func<AssertionScope> getCurrent, Func<string> getCallerIdentifier)
    {
        if (Instance.Value != null)
        {
            Assertion assertion = Instance.Value;
            Instance.Value = null;
            return assertion;
        }

        return new Assertion(getCurrent, getCallerIdentifier);
    }
}
