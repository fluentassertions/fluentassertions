using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using FluentAssertions.Common;
using FluentAssertions.Formatting;

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

        private readonly char[] blanks = { '\r', '\n', ' ', '\t' };

        /// <summary>
        /// Represents the phrase that can be used in <see cref="FailWith"/> as a placeholder for the reason of an assertion.
        /// </summary>
        private const string ReasonTag = "{reason}";

        private string reason;
        private bool succeeded;
        private bool useLineBreaks;

        [ThreadStatic]
        private static AssertionScope current;

        private AssertionScope parent;

        internal static AssertionScope Current
        {
            get { return current ?? new AssertionScope(new DefaultAssertionStrategy()); }
            set { current = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionScope"/> class.
        /// </summary>
        public AssertionScope() : this(new CollectingAssertionStrategy((current != null) ? current.assertionStrategy : null))
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
        /// Specify the condition that must be satisfied.
        /// </summary>
        /// <param name="condition">If <c>true</c> the assertion will be succesful.</param>
        public AssertionScope ForCondition(bool condition)
        {
            succeeded = condition;
            return this;
        }

        /// <summary>
        /// Specify the reason why you expect the condition to be <c>true</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the condition should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AssertionScope BecauseOf(string reason, params object[] reasonArgs)
        {
            this.reason = SanitizeReason(reason, reasonArgs);
            return this;
        }

        private string SanitizeReason(string reason, object[] reasonArgs)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                reason = EnsureIsPrefixedWithBecause(reason);

                return StartsWithBlank(reason)
                    ? string.Format(reason, reasonArgs)
                    : " " + string.Format(reason, reasonArgs);
            }

            return "";
        }

        private string EnsureIsPrefixedWithBecause(string originalReason)
        {
            string blanksPrefix = ExtractTrailingBlanksFrom(originalReason);
            string textWithoutTrailingBlanks = originalReason.Substring(blanksPrefix.Length);

            return !textWithoutTrailingBlanks.StartsWith("because", StringComparison.CurrentCultureIgnoreCase)
                ? blanksPrefix + "because " + textWithoutTrailingBlanks
                : originalReason;
        }

        private string ExtractTrailingBlanksFrom(string text)
        {
            string trimmedText = text.TrimStart(blanks);
            int trailingBlanksCount = text.Length - trimmedText.Length;

            return text.Substring(0, trailingBlanksCount);
        }

        private bool StartsWithBlank(string text)
        {
            return (text.Length > 0) && blanks.Any(blank => text[0] == blank);
        }

        /// <summary>
        /// Define the failure message for the assertion.
        /// </summary>
        /// <remarks>
        /// If the <paramref name="failureMessage"/> contains the text "{reason}", this will be replaced by the reason as
        /// defined through <see cref="BecauseOf"/>. Only 10 <paramref name="failureArgs"/> are supported in combination with
        /// a {reason}.
        /// </remarks>
        /// <param name="failureMessage">The format string that represents the failure message.</param>
        /// <param name="failureArgs">Optional arguments for the <paramref name="failureMessage"/></param>
        public bool FailWith(string failureMessage, params object[] failureArgs)
        {
            try
            {
                if (!succeeded)
                {
                    string message = ReplaceReasonTag(failureMessage);
                    message = ReplaceTags(message);
                    message = BuildExceptionMessage(message, failureArgs);

                    assertionStrategy.HandleFailure(message);
                }

                return succeeded;
            }
            finally
            {
                succeeded = false;
            }
        }

        private string ReplaceReasonTag(string failureMessage)
        {
            return !string.IsNullOrEmpty(reason)
                ? ReplaceReasonTagWithFormatSpecification(failureMessage)
                : failureMessage.Replace(ReasonTag, string.Empty);
        }

        private static string ReplaceReasonTagWithFormatSpecification(string failureMessage)
        {
            if (failureMessage.Contains(ReasonTag))
            {
                string message = IncrementAllFormatSpecifiers(failureMessage);
                return message.Replace(ReasonTag, "{0}");
            }

            return failureMessage;
        }

        private string ReplaceTags(string message)
        {
            var regex = new Regex(@"\{(?<key>[a-z|A-Z]+)(?:\:(?<default>[a-z|A-Z|\s]+))?\}");
            return regex.Replace(message, match =>
            {
                string key = match.Groups["key"].Value;
                return contextData.AsStringOrDefault(key) ?? match.Groups["default"].Value;
            });
        }

        private static string IncrementAllFormatSpecifiers(string message)
        {
            for (int index = 9; index >= 0; index--)
            {
                int newIndex = index + 1;
                string oldTag = "{" + index + "}";
                string newTag = "{" + newIndex + "}";
                message = message.Replace(oldTag, newTag);
            }

            return message;
        }

        private string BuildExceptionMessage(string failureMessage, object[] failureArgs)
        {
            var values = new List<string>();
            if (!string.IsNullOrEmpty(reason))
            {
                values.Add(reason);
            }

            values.AddRange(failureArgs.Select(a => Formatter.ToString(a, useLineBreaks)));

            string formattedMessage = values.Any() ? string.Format(failureMessage, values.ToArray()) : failureMessage;
            return formattedMessage.Replace("{{{{", "{{").Replace("}}}}", "}}").Capitalize();
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

        /// <summary>
        /// Gets data associated with the current scope and identified by <paramref name="key"/>.
        /// </summary>
        public T Get<T>(string key)
        {
            return contextData.Get<T>(key);
        }
    }

    /// <summary>
    /// Determines whether data associated with an <see cref="AssertionScope"/> should be included in the assertion failure.
    /// </summary>
    internal enum Reportability
    {
        Hidden,
        Reportable
    }

    /// <summary>
    /// Represents a collection of data items that are associated with an <see cref="AssertionScope"/>.
    /// </summary>
    internal class ContextDataItems
    {
        private readonly List<DataItem> items = new List<DataItem>();

        public IDictionary<string, object> Reportable
        {
            get { return items.Where(item => item.Reportable).ToDictionary(item => item.Key, item => item.Value); }
        }

        public string AsStringOrDefault(string key)
        {
            DataItem item = items.SingleOrDefault(i => i.Key == key);
            if (item != null)
            {
                if ((key == "subject") || (key == "expectation"))
                {
                    return Formatter.ToString(item.Value);
                }
                
                return item.Value.ToString();
            }

            return null;
        }

        public void Add(ContextDataItems contextDataItems)
        {
            foreach (var item in contextDataItems.items)
            {
                Add(item.Clone());
            }
        }

        public void Add(string key, object value, Reportability reportability)
        {
            Add(new DataItem(key, value, reportability));
        }

        private void Add(DataItem item)
        {
            var existingItem = items.SingleOrDefault(i => i.Key == item.Key);
            if (existingItem != null)
            {
                items.Remove(existingItem);
            }

            items.Add(item);
        }

        public T Get<T>(string key)
        {
            DataItem item = items.SingleOrDefault(i => i.Key == key);
            return (T)((item != null) ? item.Value : default(T));
        }

        internal class DataItem
        {
            private readonly Reportability reportability;

            public DataItem(string key, object value, Reportability reportability)
            {
                Key = key;
                Value = value;
                this.reportability = reportability;
            }

            public string Key { get; private set; }

            public object Value { get; private set; }

            public bool Reportable
            {
                get { return reportability == Reportability.Reportable; }
            }

            public DataItem Clone()
            {
                var cloneable = Value as ICloneable2;
                return new DataItem(Key, (cloneable != null) ? cloneable.Clone() : Value, reportability);
            }
        }
    }

    /// <summary>
    /// Custom version of ICloneable that works on all frameworks.
    /// </summary>
    public interface ICloneable2
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// 
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object Clone();
    }
}