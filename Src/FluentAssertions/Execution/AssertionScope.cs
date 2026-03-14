using System;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions.Execution;

/// <summary>
/// Represents an implicit or explicit scope within which multiple assertions can be collected.
/// </summary>
/// <remarks>
/// This class is supposed to have a very short lifetime and is not safe to be used in assertion that cross thread-boundaries
/// such as when using <see langword="async"/> or <see langword="await"/>.
/// </remarks>
// Remove all assertion logic from this class since it is superseded by the Assertion class
[System.Diagnostics.StackTraceHidden]
public sealed class AssertionScope : IDisposable
{
    private readonly IAssertionStrategy assertionStrategy;
    private static readonly AsyncLocal<AssertionScope> CurrentScope = new();
    private readonly Func<string> callerIdentityProvider = () => CallerIdentifier.DetermineCallerIdentity();
    private readonly ContextDataDictionary reportableData = new();
    private readonly StringBuilder tracing = new();

    private AssertionScope parent;

    /// <summary>
    /// Starts an unnamed scope within which multiple assertions can be executed
    /// and which will not throw until the scope is disposed.
    /// </summary>
    public AssertionScope()
        : this(() => null, new CollectingAssertionStrategy())
    {
    }

    /// <summary>
    /// Starts a named scope within which multiple assertions can be executed
    /// and which will not throw until the scope is disposed.
    /// </summary>
    public AssertionScope(string name)
        : this(() => name, new CollectingAssertionStrategy())
    {
    }

    /// <summary>
    /// Starts a new scope based on the given assertion strategy.
    /// </summary>
    /// <param name="assertionStrategy">The assertion strategy for this scope.</param>
    /// <exception cref="ArgumentNullException"><paramref name="assertionStrategy"/> is <see langword="null"/>.</exception>
    public AssertionScope(IAssertionStrategy assertionStrategy)
        : this(() => null, assertionStrategy)
    {
    }

    /// <summary>
    /// Starts a named scope within which multiple assertions can be executed
    /// and which will not throw until the scope is disposed.
    /// </summary>
    public AssertionScope(Func<string> name)
        : this(name, new CollectingAssertionStrategy())
    {
    }

    /// <summary>
    /// Starts a new scope based on the given assertion strategy and parent assertion scope
    /// </summary>
    /// <param name="assertionStrategy">The assertion strategy for this scope.</param>
    /// <exception cref="ArgumentNullException"><paramref name="assertionStrategy"/> is <see langword="null"/>.</exception>
    private AssertionScope(Func<string> name, IAssertionStrategy assertionStrategy)
    {
        Guard.ThrowIfArgumentIsNull(assertionStrategy);
        parent = CurrentScope.Value;
        CurrentScope.Value = this;

        this.assertionStrategy = assertionStrategy;

        if (parent is not null)
        {
            // Combine the existing Name with the parent.Name if it exists.
            Name = () =>
            {
                var parentName = parent.Name();
                if (parentName.IsNullOrEmpty())
                {
                    return name();
                }

                if (name().IsNullOrEmpty())
                {
                    return parentName;
                }

                return parentName + "/" + name();
            };

            callerIdentityProvider = parent.callerIdentityProvider;
            FormattingOptions = parent.FormattingOptions.Clone();
        }
        else
        {
            Name = name;
        }
    }

    /// <summary>
    /// Gets or sets the name of the current assertion scope, e.g. the path of the object graph
    /// that is being asserted on.
    /// </summary>
    /// <remarks>
    /// The context is provided by a <see cref="Lazy{String}"/> which
    /// only gets evaluated when its value is actually needed (in most cases during a failure).
    /// </remarks>
    public Func<string> Name { get; }

    /// <summary>
    /// Gets the current thread-specific assertion scope.
    /// </summary>
    public static AssertionScope Current
    {
#pragma warning disable CA2000 // AssertionScope should not be disposed here
        get
        {
            return CurrentScope.Value ?? new AssertionScope(() => null, new DefaultAssertionStrategy());
        }
#pragma warning restore CA2000
        private set => CurrentScope.Value = value;
    }

    /// <summary>
    /// Exposes the options the scope will use for formatting objects in case an assertion fails.
    /// </summary>
    public FormattingOptions FormattingOptions { get; } = AssertionConfiguration.Current.Formatting.Clone();

    /// <summary>
    /// Adds a pre-formatted failure message to the current scope.
    /// </summary>
    public void AddPreFormattedFailure(string formattedFailureMessage)
    {
        AddFailure(new AssertionFailure(formattedFailureMessage));
    }

    /// <summary>
    /// Adds an <see cref="AssertionFailure"/> to the current scope with deferred rendering.
    /// </summary>
    internal void AddFailure(AssertionFailure failure)
    {
        if (assertionStrategy is IAssertionStrategy2 strategy2)
        {
            strategy2.HandleFailure(failure);
        }
        else
        {
            assertionStrategy.HandleFailure(failure.ToString());
        }
    }

    /// <summary>
    /// Adds some information to the assertion scope that will be included in the message
    /// that is emitted if an assertion fails.
    /// </summary>
    internal void AddReportable(string key, string value)
    {
        reportableData.Add(new ContextDataDictionary.DataItem(key, value)
        {
            Reportable = true,
        });
    }

    /// <summary>
    /// Adds some information to the assertion scope that will be included in the message
    /// that is emitted if an assertion fails. The value is only calculated on failure.
    /// </summary>
    internal void AddReportable(string key, Func<string> valueFunc)
    {
        reportableData.Add(new ContextDataDictionary.DataItem(key, new DeferredReportable(valueFunc))
        {
            Reportable = true
        });
    }

    /// <summary>
    /// Adds a block of tracing to the scope for reporting when an assertion fails.
    /// </summary>
    public void AppendTracing(string tracingBlock)
    {
        tracing.Append(tracingBlock);
    }

    /// <summary>
    /// Returns all failures that happened up to this point and ensures they will not cause
    /// <see cref="Dispose"/> to fail the assertion.
    /// </summary>
    public string[] Discard()
    {
        if (assertionStrategy is IAssertionStrategy2 strategy2)
        {
            return strategy2.DiscardFailures().Select(f => f.ToString()).ToArray();
        }

        return assertionStrategy.DiscardFailures().ToArray();
    }

    public bool HasFailures()
    {
        if (assertionStrategy is IAssertionStrategy2 strategy2)
        {
            return strategy2.FailureCount > 0;
        }

        return assertionStrategy.FailureMessages.Any();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        CurrentScope.Value = parent;

        if (parent is not null)
        {
            if (assertionStrategy is IAssertionStrategy2 strategy2)
            {
                foreach (AssertionFailure failure in strategy2.Failures)
                {
                    parent.AddFailure(failure);
                }
            }
            else
            {
                foreach (string failureMessage in assertionStrategy.FailureMessages)
                {
                    parent.AddFailure(new AssertionFailure(failureMessage));
                }
            }

            parent.reportableData.Add(reportableData);
            parent.AppendTracing(tracing.ToString());

            parent = null;
        }
        else
        {
            if (tracing.Length > 0)
            {
                reportableData.Add(new ContextDataDictionary.DataItem("trace", tracing.ToString())
                {
                    Reportable = true
                });
            }

            assertionStrategy.ThrowIfAny(reportableData.GetReportable());
        }
    }

    private sealed class DeferredReportable(Func<string> valueFunc)
    {
        private readonly Lazy<string> lazyValue = new(valueFunc);

        public override string ToString() => lazyValue.Value;
    }
}
