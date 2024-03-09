using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

public sealed class AssertionChain
{
    private readonly StringBuilder tracing = new();
    private readonly Func<AssertionScope> getCurrentScope;
    private readonly ContextDataItems contextData = new();
    private string fallbackIdentifier = "object";
    private Func<string> getCallerIdentifier;
    private bool previousAssertionSucceeded;
    private Func<string> reason;
    private bool? succeeded;
    private Func<string> expectation;

    private static readonly AsyncLocal<AssertionChain> Instance = new();

    public static void ReuseOnce(AssertionChain assertionChain)
    {
        Instance.Value = assertionChain;
    }

    public static AssertionChain GetOrCreate()
    {
        if (Instance.Value != null)
        {
            AssertionChain assertionChain = Instance.Value;
            Instance.Value = null;
            return assertionChain;
        }

        return new AssertionChain(() => AssertionScope.Current, () => FluentAssertions.CallerIdentifier.DetermineCallerIdentity());
    }

    private AssertionChain(Func<AssertionScope> getCurrentScope, Func<string> getCallerIdentifier, bool previousAssertionSucceeded = true)
    {
        this.getCurrentScope = getCurrentScope;
        this.previousAssertionSucceeded = previousAssertionSucceeded;

        this.getCallerIdentifier = () =>
        {
            var identifier = getCurrentScope().Name?.Value;
            if (string.IsNullOrEmpty(identifier))
            {
                return getCallerIdentifier();
            }

            return identifier;
        };
    }

    public string CallerIdentifier => getCallerIdentifier();

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
    public AssertionChain BecauseOf(string because, params object[] becauseArgs)
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

    public AssertionChain ForCondition(bool condition)
    {
        if (previousAssertionSucceeded)
        {
            succeeded = condition;
        }

        return this;
    }

    public AssertionChain ForConstraint(OccurrenceConstraint constraint, int actualOccurrences)
    {
        if (previousAssertionSucceeded)
        {
            constraint.RegisterReportables(this);
            succeeded = constraint.Assert(actualOccurrences);
        }

        return this;
    }

    public AssertionChain WithExpectation(string message, params object[] args)
    {
        if (previousAssertionSucceeded)
        {
            expectation = () =>
            {
                var formatter = new FailureMessageFormatter(getCurrentScope().FormattingOptions)
                    .WithReason(reason?.Invoke() ?? string.Empty)
                    .WithContext(contextData)
                    .WithIdentifier(getCallerIdentifier())
                    .WithFallbackIdentifier(fallbackIdentifier);

                return formatter.Format(message, args);
            };
        }

        return this;
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
        return FailWith(
            () => new FailReason(
                message,
                argProviders.Select(a => a()).ToArray()));
    }

    public Continuation FailWith(Func<FailReason> failReasonFunc)
    {
        return FailWith(() =>
        {
            var formatter = new FailureMessageFormatter(getCurrentScope().FormattingOptions)
                .WithReason(reason?.Invoke() ?? string.Empty)
                .WithContext(contextData)
                .WithIdentifier(getCallerIdentifier())
                .WithFallbackIdentifier(fallbackIdentifier);

            FailReason failReason = failReasonFunc();

            return formatter.Format(failReason.Message, failReason.Args);
        });
    }

    private Continuation FailWith(Func<string> failReasonFunc)
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

                getCurrentScope().AddPreFormattedFailure(result.Capitalize());
            }
        }

        // Reset the state for successive assertions on this object
        succeeded = null;

        return new Continuation(this);
    }

    public void AddCallerPostfix(string postfix)
    {
        var originalCallerIdentifier = getCallerIdentifier;
        getCallerIdentifier = () => originalCallerIdentifier() + postfix;
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

    public void WithReportable(string name, Func<string> content)
    {
        getCurrentScope().AddReportable(name, content);
    }

    /// <summary>
    /// Adds a block of tracing to the scope for reporting when an assertion fails.
    /// </summary>
    public void AppendTracing(string tracingBlock)
    {
        tracing.Append(tracingBlock);
    }

    public void AddPreFormattedFailure(string failure)
    {
        getCurrentScope().AddPreFormattedFailure(failure);
    }

    internal void TrackComparands(object subject, object expectation)
    {
        contextData.Add(new ContextDataItems.DataItem("subject", subject, reportable: false, requiresFormatting: true));
        contextData.Add(new ContextDataItems.DataItem("expectation", expectation, reportable: false, requiresFormatting: true));
    }

    public bool Succeeded => previousAssertionSucceeded && (succeeded is null or true);

    public AssertionChain UsingLineBreaks
    {
        get
        {
            getCurrentScope().FormattingOptions.UseLineBreaks = true;
            return this;
        }
    }

    public void OverrideCallerIdentifier(Func<string> getCallerIdentifier)
    {
        this.getCallerIdentifier = getCallerIdentifier;
    }
}
