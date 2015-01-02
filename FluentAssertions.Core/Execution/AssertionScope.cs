#region

using System;
using System.Linq;

#endregion

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents an implicit or explicit scope within which multiple assertions can be collected.
    /// </summary>
    public class AssertionScope : IDisposable
    {
        #region Private Definitions

        private readonly IAssertionStrategy assertionStrategy;
        private readonly ContextDataItems contextData = new ContextDataItems();

        private string reason;
        private bool useLineBreaks;

        [ThreadStatic] 
        private static AssertionScope current;

        private AssertionScope parent;
        private string expectation = "";

        public static AssertionScope Current
        {
            get { return current ?? new AssertionScope(new DefaultAssertionStrategy()); }
            set { current = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionScope"/> class.
        /// </summary>
        public AssertionScope()
            : this(new CollectingAssertionStrategy((current != null) ? current.assertionStrategy : null))
        {
            parent = current;
            current = this;

            if (parent != null)
            {
                contextData.Add(parent.contextData);
            }
        }

        public AssertionScope(string context) : this()
        {
            AddNonReportable("context", context);
        }

        private AssertionScope(IAssertionStrategy _assertionStrategy)
        {
            assertionStrategy = _assertionStrategy;
            parent = null;
        }

        /// <summary>
        /// Indicates that every argument passed into <see cref="FailWith"/> is displayed on a separate line.
        /// </summary>
        public AssertionScope UsingLineBreaks
        {
            get
            {
                useLineBreaks = true;
                return this;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the last assertion executed through this scope succeeded.
        /// </summary>
        public bool Succeeded { get; private set; }

        /// <summary>
        /// Specify the reason why you expect the condition to be <c>true</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase explaining why the condition should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AssertionScope BecauseOf(string because, params object[] reasonArgs)
        {
            reason = string.Format(because ?? "", reasonArgs ?? new object[0]);
            return this;
        }

        /// <summary>
        /// Sets the expectation part of the failure message when the assertion is not met. 
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format"/>-style placeholders, messages may contain a few 
        /// specialized placeholders as well. For instance, {reason} will be replaced with the reason of the assertion as passed 
        /// to <see cref="BecauseOf"/>. Other named placeholders will be replaced with the <see cref="Current"/> scope data 
        /// passed through <see cref="AddNonReportable"/> and <see cref="AddReportable"/>. Finally, a description of the 
        /// current subject can be passed through the {context:description} placeholder. This is used in the message if no 
        /// explicit context is specified through the <see cref="AssertionScope"/> constructor. 
        /// Note that only 10 <paramref name="becauseArgs"/> are supported in combination with a {reason}.
        /// If an expectation was set through a prior call to <see cref="WithExpectation"/>, then the failure message is appended to that
        /// expectation. 
        /// </remarks>
        public AssertionScope WithExpectation(string expectation, params object[] args)
        {
            this.expectation = new MessageBuilder(useLineBreaks).Build(expectation, args, reason, contextData);
            return this;
        }

        /// <summary>
        /// Specify the condition that must be satisfied.
        /// </summary>
        /// <param name="condition">If <c>true</c> the assertion will be succesful.</param>
        public AssertionScope ForCondition(bool condition)
        {
            Succeeded = condition;
            return this;
        }


        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a 
        /// prior call to to <see cref="WithExpectation"/>.
        /// </summary>
        /// <remarks>
        /// In addition to the numbered <see cref="string.Format"/>-style placeholders, messages may contain a few 
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
        public bool FailWith(string message, params object[] args)
        {
            try
            {
                if (!Succeeded)
                {
                    string result = new MessageBuilder(useLineBreaks).Build(message, args, reason, contextData);

                    if (!string.IsNullOrEmpty(expectation))
                    {
                        result = expectation + result;
                    }

                    assertionStrategy.HandleFailure(result);
                }

                return Succeeded;
            }
            finally
            {
                Succeeded = false;
            }
        }

        public void AddNonReportable(string key, object value)
        {
            contextData.Add(key, value, Reportability.Hidden);
        }

        public void AddReportable(string key, string value)
        {
            contextData.Add(key, value, Reportability.Reportable);
        }

        /// <summary>
        /// Discards and returns the failures that happened up to now.
        /// </summary>
        public string[] Discard()
        {
            return assertionStrategy.DiscardFailures().ToArray();
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
            Current = parent;

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
                assertionStrategy.ThrowIfAny(contextData.Reportable);
            }
        }
    }

    public class Continuation
    {
        private readonly AssertionScope scope;

        public Continuation(AssertionScope scope)
        {
            this.scope = scope;
        }

        public AssertionScope Then
        {
            get { return scope; }
        }

        public bool Succeeded
        {
            get { return scope.Succeeded; }
        }
    }
}