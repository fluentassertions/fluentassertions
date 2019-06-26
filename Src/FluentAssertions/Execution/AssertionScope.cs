using System;
using System.Linq;
#if NOTNET45
using Microsoft.VisualStudio.Threading;
#else
using System.Runtime.Remoting.Messaging;
#endif
using FluentAssertions.Common;



namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents an implicit or explicit scope within which multiple assertions can be collected.
    /// </summary>
    /// <remarks>
    /// This class is supposed to have a very short life time and is not safe to be used in assertion that cross thread-boundaries such as when
    /// using <c>async</c> or <c>await</c>.
    /// </remarks>
    [Serializable]
    public class AssertionScope : IAssertionScope
    {
        #region Private Definitions

        private readonly IAssertionStrategy assertionStrategy;
        private readonly ContextDataItems contextData = new ContextDataItems();

        private Func<string> reason;
        private bool useLineBreaks;

#if NOTNET45
        private static AsyncLocal<AssertionScope> current = new AsyncLocal<AssertionScope>();
#endif
        private AssertionScope parent;
        private Func<string> expectation;
        private string fallbackIdentifier = "object";
        private bool? succeeded;

#endregion

        private AssertionScope(IAssertionStrategy _assertionStrategy)
        {
            assertionStrategy = _assertionStrategy;
            parent = null;
        }

        /// <summary>
        /// Starts an unnamed scope within which multiple assertions can be executed
        /// and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope()
            : this(new CollectingAssertionStrategy())
        {

#if NOTNET45
            parent = current.Value;
            current.Value = this;
#else
            
            parent = (AssertionScope) CallContext.LogicalGetData("this");
            CallContext.LogicalSetData("this", this);
#endif
            if (parent != null)
            {
                contextData.Add(parent.contextData);
                Context = parent.Context;
            }
        }

        /// <summary>
        /// Starts a named scope within which multiple assertions can be executed and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope(string context)
            : this()
        {
            Context = context;
        }

        /// <summary>
        /// Gets or sets the context of the current assertion scope, e.g. the path of the object graph
        /// that is being asserted on.
        /// </summary>
        public string Context { get; set; }

#if NOTNET45
        /// <summary>
        /// Gets the current thread-specific assertion scope.
        /// </summary>
        public static AssertionScope Current
        {

            get => current.Value ?? new AssertionScope(new DefaultAssertionStrategy());
            private set => current.Value = value;

        }
#else
        /// <summary>
        /// Gets the current thread-specific assertion scope.
        /// </summary>
        public static AssertionScope Current => LogicalCurrent(); 

        public static AssertionScope LogicalCurrent()
        {
            if (CallContext.LogicalGetData("this") == null)
            {
                CallContext.LogicalSetData("this", new AssertionScope(new DefaultAssertionStrategy()));
            }

            return CallContext.LogicalGetData("this").As<AssertionScope>();
        }
#endif
        public AssertionScope UsingLineBreaks
        {
            get
            {
                useLineBreaks = true;
                return this;
            }
        }

        public bool Succeeded
        {
            get => succeeded.HasValue && succeeded.Value;
        }

        public AssertionScope BecauseOf(string because, params object[] becauseArgs)
        {
            reason = () =>
            {
                try
                {
                    string becauseOrEmpty = because ?? "";
                    return (becauseArgs?.Any() == true) ? string.Format(becauseOrEmpty, becauseArgs) : becauseOrEmpty;
                }
                catch (FormatException formatException)
                {
                    return $"**WARNING** because message '{because}' could not be formatted with string.Format{Environment.NewLine}{formatException.StackTrace}";
                }
            };
            return this;
        }

        /// <summary>
        /// Sets the expectation part of the failure message when the assertion is not met.
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format(string,object[])"/>-style placeholders, messages may contain a few
        /// specialized placeholders as well. For instance, {reason} will be replaced with the reason of the assertion as passed
        /// to <see cref="BecauseOf"/>. Other named placeholders will be replaced with the <see cref="Current"/> scope data
        /// passed through <see cref="AddNonReportable"/> and <see cref="AddReportable"/>. Finally, a description of the
        /// current subject can be passed through the {context:description} placeholder. This is used in the message if no
        /// explicit context is specified through the <see cref="AssertionScope"/> constructor.
        /// Note that only 10 <paramref name="args"/> are supported in combination with a {reason}.
        /// If an expectation was set through a prior call to <see cref="WithExpectation"/>, then the failure message is appended to that
        /// expectation.
        /// </remarks>
        /// <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        public AssertionScope WithExpectation(string message, params object[] args)
        {
            var localReason = reason;
            expectation = () =>
            {
                var messageBuilder = new MessageBuilder(useLineBreaks);
                string reason = localReason?.Invoke() ?? "";
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

        public Continuation ClearExpectation()
        {
            expectation = null;

            return new Continuation(this, !succeeded.HasValue || succeeded.Value);
        }

        public GivenSelector<T> Given<T>(Func<T> selector)
        {
            return new GivenSelector<T>(selector, !succeeded.HasValue || succeeded.Value, this);
        }

        public AssertionScope ForCondition(bool condition)
        {
            succeeded = condition;

            return this;
        }

        public Continuation FailWith(Func<FailReason> failReasonFunc)
        {
            try
            {
                if (!succeeded.HasValue || !succeeded.Value)
                {
                    string localReason = reason?.Invoke() ?? "";
                    var messageBuilder = new MessageBuilder(useLineBreaks);
                    string identifier = GetIdentifier();
                    var failReason = failReasonFunc();
                    string result = messageBuilder.Build(failReason.Message, failReason.Args, localReason, contextData, identifier, fallbackIdentifier);

                    if (expectation != null)
                    {
                        result = expectation() + result;
                    }

                    assertionStrategy.HandleFailure(result.Capitalize());

                    succeeded = false;
                }

                return new Continuation(this, succeeded.Value);
            }
            finally
            {
                succeeded = null;
            }
        }

        public Continuation FailWith(string message, params object[] args)
        {
            return FailWith(() => new FailReason(message, args));
        }

        private string GetIdentifier()
        {
            if (!string.IsNullOrEmpty(Context))
            {
                return Context;
            }

            return CallerIdentifier.DetermineCallerIdentity();
        }

        /// <summary>
        /// Adds a pre-formatted failure message to the current scope.
        /// </summary>
        public void AddPreFormattedFailure(string formattedFailureMessage)
        {
            assertionStrategy.HandleFailure(formattedFailureMessage);
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
#if NOTNET45
            Current = parent;
#else
            CallContext.LogicalSetData("this", parent);
#endif
            if (parent != null)
            {
                foreach (string failureMessage in assertionStrategy.FailureMessages)
                {
                    parent.assertionStrategy.HandleFailure(failureMessage);
                }

                parent = null;
            }
            else
            {
                assertionStrategy.ThrowIfAny(contextData.GetReportable());
            }
        }

        public AssertionScope WithDefaultIdentifier(string identifier)
        {
            fallbackIdentifier = identifier;
            return this;
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
