using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents an implicit or explicit scope within which multiple assertions can be collected.
    /// </summary>
    /// <remarks>
    /// This class is supposed to have a very short life time and is not safe to be used in assertion that cross thread-boundaries
    /// such as when using <c>async</c> or <c>await</c>.
    /// </remarks>
    public sealed class AssertionScope : IAssertionScope
    {
        #region Private Definitions

        private readonly FormattingOptions formattingOptions = AssertionOptions.FormattingOptions.Clone();
        private readonly IAssertionStrategy assertionStrategy;
        private readonly ContextDataItems contextData = new();
        private readonly StringBuilder tracing = new();

        private Func<string> reason;

        private static readonly AsyncLocal<AssertionScope> CurrentScope = new();
        private Func<string> callerIdentityProvider = () => CallerIdentifier.DetermineCallerIdentity();
        private AssertionScope parent;
        private Func<string> expectation;
        private string fallbackIdentifier = "object";
        private bool? succeeded;

        private sealed class DeferredReportable
        {
            private readonly Lazy<string> lazyValue;

            public DeferredReportable(Func<string> valueFunc)
            {
                lazyValue = new(valueFunc);
            }

            public override string ToString() => lazyValue.Value;
        }

        #endregion

        /// <summary>
        /// Starts a named scope within which multiple assertions can be executed
        /// and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope(string context)
            : this()
        {
            if (!string.IsNullOrEmpty(context))
            {
                Context = new Lazy<string>(() => context);
            }
        }

        /// <summary>
        /// Starts an unnamed scope within which multiple assertions can be executed
        /// and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope()
            : this(new CollectingAssertionStrategy())
        {
        }

        /// <summary>
        /// Starts a new scope based on the given assertion strategy.
        /// </summary>
        /// <param name="assertionStrategy">The assertion strategy for this scope.</param>
        /// <exception cref="ArgumentNullException">Thrown when trying to use a null strategy.</exception>
        public AssertionScope(IAssertionStrategy assertionStrategy)
            : this(assertionStrategy, GetCurrentAssertionScope())
        {
            SetCurrentAssertionScope(this);
        }

        /// <summary>
        /// Starts a named scope within which multiple assertions can be executed
        /// and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope(Lazy<string> context)
            : this()
        {
            Context = context;
        }

        /// <summary>
        /// Starts a new scope based on the given assertion strategy and parent assertion scope
        /// </summary>
        /// <param name="assertionStrategy">The assertion strategy for this scope.</param>
        /// <param name="parent">The parent assertion scope for this scope.</param>
        /// <exception cref="ArgumentNullException">Thrown when trying to use a null strategy.</exception>
        private AssertionScope(IAssertionStrategy assertionStrategy, AssertionScope parent)
        {
            this.assertionStrategy = assertionStrategy
                                     ?? throw new ArgumentNullException(nameof(assertionStrategy));
            this.parent = parent;

            if (parent is not null)
            {
                contextData.Add(parent.contextData);
                Context = parent.Context;
                callerIdentityProvider = parent.callerIdentityProvider;
            }
        }

        /// <summary>
        /// Gets or sets the context of the current assertion scope, e.g. the path of the object graph
        /// that is being asserted on. The context is provided by a <see cref="Lazy{String}"/> which
        /// only gets evaluated when its value is actually needed (in most cases during a failure).
        /// </summary>
        public Lazy<string> Context { get; set; }

        /// <summary>
        /// Gets the current thread-specific assertion scope.
        /// </summary>
        public static AssertionScope Current
        {
#pragma warning disable CA2000 // AssertionScope should not be disposed here
            get
            {
                return GetCurrentAssertionScope() ?? new AssertionScope(new DefaultAssertionStrategy(), parent: null);
            }
#pragma warning restore CA2000
            private set => SetCurrentAssertionScope(value);
        }

        /// <inheritdoc cref="IAssertionScope.UsingLineBreaks"/>
        public AssertionScope UsingLineBreaks
        {
            get
            {
                formattingOptions.UseLineBreaks = true;
                return this;
            }
        }

        /// <summary>
        /// Exposes the options the scope will use for formatting objects in case an assertion fails.
        /// </summary>
        public FormattingOptions FormattingOptions => formattingOptions;

        internal bool Succeeded
        {
            get => succeeded == true;
        }

        /// <summary>
        /// Adds an explanation of why the assertion is supposed to succeed to the scope.
        /// </summary>
        public AssertionScope BecauseOf(Reason reason)
        {
            return BecauseOf(reason.FormattedMessage, reason.Arguments);
        }

        /// <inheritdoc cref="IAssertionScope.BecauseOf(string, object[])"/>
        public AssertionScope BecauseOf(string because, params object[] becauseArgs)
        {
            reason = () =>
            {
                try
                {
                    string becauseOrEmpty = because ?? string.Empty;
                    return (becauseArgs?.Any() == true) ? string.Format(CultureInfo.InvariantCulture, becauseOrEmpty, becauseArgs) : becauseOrEmpty;
                }
                catch (FormatException formatException)
                {
                    return $"**WARNING** because message '{because}' could not be formatted with string.Format{Environment.NewLine}{formatException.StackTrace}";
                }
            };
            return this;
        }

        /// <inheritdoc cref="IAssertionScope.WithExpectation(string, object[])"/>
        public AssertionScope WithExpectation(string message, params object[] args)
        {
            Func<string> localReason = reason;
            expectation = () =>
            {
                var messageBuilder = new MessageBuilder(formattingOptions);
                string reason = localReason?.Invoke() ?? string.Empty;
                string identifier = GetIdentifier();

                return messageBuilder.Build(message, args, reason, contextData, identifier, fallbackIdentifier);
            };

            return this;
        }

        internal void TrackComparands(object subject, object expectation)
        {
            contextData.Add(new ContextDataItems.DataItem("subject", subject, reportable: false, requiresFormatting: true));
            contextData.Add(new ContextDataItems.DataItem("expectation", expectation, reportable: false, requiresFormatting: true));
        }

        /// <inheritdoc/>
        public Continuation ClearExpectation()
        {
            expectation = null;

            // SMELL: Isn't this always going to return null? Or this method also called without FailWidth (which sets the success state to null)
            return new Continuation(this, Succeeded);
        }

        public GivenSelector<T> Given<T>(Func<T> selector)
        {
            return new GivenSelector<T>(selector, this, continueAsserting: succeeded != false);
        }

        /// <inheritdoc cref="IAssertionScope.ForCondition(bool)"/>
        public AssertionScope ForCondition(bool condition)
        {
            succeeded = condition;

            return this;
        }

        /// <summary>
        /// Makes assertion fail when <paramref name="actualOccurrences"/> does not match <paramref name="constraint"/>.
        /// <para>
        /// The occurrence description in natural language could then be inserted in failure message by using
        /// <em>{expectedOccurrence}</em> placeholder in message parameters of <see cref="FailWith(string, object[])"/> and its
        /// overloaded versions.
        /// </para>
        /// </summary>
        /// <param name="constraint"><see cref="OccurrenceConstraint"/> defining the number of expected occurrences.</param>
        /// <param name="actualOccurrences">The number of actual occurrences.</param>
        public AssertionScope ForConstraint(OccurrenceConstraint constraint, int actualOccurrences)
        {
            constraint.RegisterReportables(this);
            succeeded = constraint.Assert(actualOccurrences);

            return this;
        }

        /// <inheritdoc/>
        public Continuation FailWith(Func<FailReason> failReasonFunc)
        {
            return FailWith(() =>
            {
                string localReason = reason?.Invoke() ?? string.Empty;
                var messageBuilder = new MessageBuilder(formattingOptions);
                string identifier = GetIdentifier();
                FailReason failReason = failReasonFunc();
                string result = messageBuilder.Build(failReason.Message, failReason.Args, localReason, contextData, identifier, fallbackIdentifier);
                return result;
            });
        }

        internal Continuation FailWithPreFormatted(string formattedFailReason) =>
            FailWith(() => formattedFailReason);

        private Continuation FailWith(Func<string> failReasonFunc)
        {
            try
            {
                bool failed = succeeded != true;
                if (failed)
                {
                    string result = failReasonFunc();

                    if (expectation is not null)
                    {
                        result = expectation() + result;
                    }

                    assertionStrategy.HandleFailure(result.Capitalize());

                    succeeded = false;
                }

                return new Continuation(this, continueAsserting: !failed);
            }
            finally
            {
                succeeded = null;
            }
        }

        /// <inheritdoc/>
        public Continuation FailWith(string message)
        {
            return FailWith(() => new FailReason(message, new object[0]));
        }

        /// <inheritdoc/>
        public Continuation FailWith(string message, params object[] args)
        {
            return FailWith(() => new FailReason(message, args));
        }

        /// <inheritdoc/>
        public Continuation FailWith(string message, params Func<object>[] argProviders)
        {
            return FailWith(() => new FailReason(message,
                argProviders.Select(a => a()).ToArray()));
        }

        private string GetIdentifier()
        {
            var identifier = Context?.Value;
            if (string.IsNullOrEmpty(identifier))
            {
                identifier = CallerIdentity;
            }

            return identifier;
        }

        /// <summary>
        /// Gets the identity of the caller associated with the current scope.
        /// </summary>
        public string CallerIdentity => callerIdentityProvider();

        /// <summary>
        /// Adds a pre-formatted failure message to the current scope.
        /// </summary>
        public void AddPreFormattedFailure(string formattedFailureMessage)
        {
            assertionStrategy.HandleFailure(formattedFailureMessage);
        }

        /// <summary>
        /// Adds a block of tracing to the scope for reporting when an assertion fails.
        /// </summary>
        public void AppendTracing(string tracingBlock)
        {
            tracing.Append(tracingBlock);
        }

        /// <summary>
        /// Tracks a keyed object in the current scope that is excluded from the failure message in case an assertion fails.
        /// </summary>
        public void AddNonReportable(string key, object value)
        {
            contextData.Add(new ContextDataItems.DataItem(key, value, reportable: false, requiresFormatting: false));
        }

        /// <summary>
        /// Adds some information to the assertion scope that will be included in the message
        /// that is emitted if an assertion fails.
        /// </summary>
        public void AddReportable(string key, string value)
        {
            contextData.Add(new ContextDataItems.DataItem(key, value, reportable: true, requiresFormatting: false));
        }

        /// <summary>
        /// Adds some information to the assertion scope that will be included in the message
        /// that is emitted if an assertion fails. The value is only calculated on failure.
        /// </summary>
        public void AddReportable(string key, Func<string> valueFunc)
        {
            contextData.Add(new ContextDataItems.DataItem(key, new DeferredReportable(valueFunc), reportable: true, requiresFormatting: false));
        }

        /// <summary>
        /// Returns all failures that happened up to this point and ensures they will not cause
        /// <see cref="Dispose"/> to fail the assertion.
        /// </summary>
        public string[] Discard()
        {
            return assertionStrategy.DiscardFailures().ToArray();
        }

        public bool HasFailures()
        {
            return assertionStrategy.FailureMessages.Any();
        }

        /// <summary>
        /// Gets data associated with the current scope and identified by <paramref name="key"/>.
        /// </summary>
        public T Get<T>(string key)
        {
            return contextData.Get<T>(key);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            SetCurrentAssertionScope(parent);

            if (parent is not null)
            {
                foreach (string failureMessage in assertionStrategy.FailureMessages)
                {
                    parent.assertionStrategy.HandleFailure(failureMessage);
                }

                parent.AppendTracing(tracing.ToString());

                parent = null;
            }
            else
            {
                IDictionary<string, object> reportable = contextData.GetReportable();

                if (tracing.Length > 0)
                {
                    reportable.Add("trace", tracing.ToString());
                }

                assertionStrategy.ThrowIfAny(reportable);
            }
        }

        /// <inheritdoc cref="IAssertionScope.WithDefaultIdentifier(string)"/>
        public AssertionScope WithDefaultIdentifier(string identifier)
        {
            fallbackIdentifier = identifier;
            return this;
        }

        /// <summary>
        /// Allows the scope to assume that all assertions that happen within this scope are going to
        /// be initiated by the same caller.
        /// </summary>
        public void AssumeSingleCaller()
        {
            // Since we know there's only one caller, we don't have to have every assertion determine the caller identity again
            var provider = new Lazy<string>(() => CallerIdentifier.DetermineCallerIdentity());
            callerIdentityProvider = () => provider.Value;
        }

        private static AssertionScope GetCurrentAssertionScope()
        {
            return CurrentScope.Value;
        }

        private static void SetCurrentAssertionScope(AssertionScope scope)
        {
            CurrentScope.Value = scope;
        }

        #region Explicit Implementation to support the interface

        IAssertionScope IAssertionScope.ForCondition(bool condition) => ForCondition(condition);

        IAssertionScope IAssertionScope.BecauseOf(string because, params object[] becauseArgs) => BecauseOf(because, becauseArgs);

        IAssertionScope IAssertionScope.WithExpectation(string message, params object[] args) => WithExpectation(message, args);

        IAssertionScope IAssertionScope.WithDefaultIdentifier(string identifier) => WithDefaultIdentifier(identifier);

        IAssertionScope IAssertionScope.UsingLineBreaks => UsingLineBreaks;

        #endregion
    }
}
