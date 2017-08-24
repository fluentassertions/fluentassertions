#region

using System;
using System.Linq;
using FluentAssertions.Common;

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
        private readonly bool evaluateCondition = true;

        #endregion

        private AssertionScope(IAssertionStrategy _assertionStrategy)
        {
            assertionStrategy = _assertionStrategy;
            parent = null;
        }

        /// <summary>
        /// Starts an unnamed scope within which multiple assertions can be executed and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope()
            : this(new CollectingAssertionStrategy())
        {
            parent = current;
            current = this;

            if (parent != null)
            {
                contextData.Add(parent.contextData);
            }
        }

        /// <summary>
        /// Starts a named scope within which multiple assertions can be executed and which will not throw until the scope is disposed.
        /// </summary>
        public AssertionScope(string context) : this()
        {
            AddNonReportable("context", context);
        }

        /// <summary>
        /// Creates a nested scope used during chaining.
        /// </summary>
        internal AssertionScope(AssertionScope sourceScope, bool sourceSucceeded)
        {
            assertionStrategy = sourceScope.assertionStrategy;
            contextData = sourceScope.contextData;
            reason = sourceScope.reason;
            useLineBreaks = sourceScope.useLineBreaks;
            parent = sourceScope.parent;
            expectation = sourceScope.expectation;
            evaluateCondition = sourceSucceeded;
        }

        /// <summary>
        /// Gets the current thread-specific assertion scope. 
        /// </summary>
        public static AssertionScope Current
        {
            get { return current ?? new AssertionScope(new DefaultAssertionStrategy()); }
            private set { current = value; }
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
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AssertionScope BecauseOf(string because, params object[] becauseArgs)
        {
            reason = string.Format(because ?? "", becauseArgs ?? new object[0]);
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
        ///  <param name="message">The format string that represents the failure message.</param>
        /// <param name="args">Optional arguments to any numbered placeholders.</param>
        public AssertionScope WithExpectation(string expectation, params object[] args)
        {
            this.expectation = new MessageBuilder(useLineBreaks).Build(expectation, args, reason, contextData);
            return this;
        }

        /// <summary>
        /// Allows to safely select the subject for successive assertions, even when the prior assertion has failed.
        /// </summary>
        /// <paramref name="selector">
        /// Selector which result is passed to successive calls to <see cref="ForCondition"/>.
        /// </paramref>
        public GivenSelector<T> Given<T>(Func<T> selector)
        {
            return new GivenSelector<T>(selector, evaluateCondition, this);
        }

        /// <summary>
        /// Specify the condition that must be satisfied.
        /// </summary>
        /// <param name="condition">
        /// If <c>true</c> the assertion will be treated as successful and no exceptions will be thrown.
        /// </param>
        public AssertionScope ForCondition(bool condition)
        {
            if (evaluateCondition)
            {
                Succeeded = condition;
            }

            return this;
        }

        /// <summary>
        /// Sets the failure message when the assertion is not met, or completes the failure message set to a 
        /// prior call to to <see cref="WithExpectation"/>.
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
        public Continuation FailWith(string message, params object[] args)
        {
            try
            {
                if (evaluateCondition && !Succeeded)
                {
                    string result = new MessageBuilder(useLineBreaks).Build(message, args, reason, contextData);

                    if (!string.IsNullOrEmpty(expectation))
                    {
                        result = expectation + result;
                    }

                    assertionStrategy.HandleFailure(result.Capitalize());
                }

                return new Continuation(this, Succeeded);
            }
            finally
            {
                Succeeded = false;
            }
        }

        /// <summary>
        /// Adds a pre-formatted failure message to the current scope. 
        /// </summary>
        public void AddFailure(string formattedFailureMessage)
        {
            assertionStrategy.HandleFailure(formattedFailureMessage);
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
}