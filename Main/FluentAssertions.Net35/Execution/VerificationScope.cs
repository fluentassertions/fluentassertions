using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Provides a fluent API for verifying an arbitrary condition.
    /// </summary>
    public class VerificationScope : IDisposable
    {
        #region Private Definitions

        private readonly IVerificationStrategy verificationStrategy;
        private readonly Dictionary<string, string> contextData = new Dictionary<string, string>();
        
        private readonly char[] blanks = { '\r', '\n', ' ', '\t' };

        /// <summary>
        /// Represents the phrase that can be used in <see cref="FailWith"/> as a placeholder for the reason of an assertion.
        /// </summary>
        private const string ReasonTag = "{reason}";
        
        private string reason;
        private bool succeeded;
        private bool useLineBreaks;

        [ThreadStatic]
        private static VerificationScope current;

        [ThreadStatic]
        private static VerificationScope parentScope;

        internal static VerificationScope Current
        {
            get { return current ?? new VerificationScope(new DefaultVerificationStrategy()); }
            set { current = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationScope"/> class.
        /// </summary>
        internal VerificationScope() : this(new CollectingVerificationStrategy((current != null) ? current.verificationStrategy : null))
        {
            parentScope = current;
            current = this;
        }

        private VerificationScope(IVerificationStrategy verificationStrategy)
        {
            this.verificationStrategy = verificationStrategy;
            parentScope = null;
        }

        /// <summary>
        /// Indicates that every argument passed into <see cref="FailWith"/> is displayed on a separate line.
        /// </summary>
        public VerificationScope UsingLineBreaks
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
        /// <param name="condition">If <c>true</c> the verification will be succesful.</param>
        public VerificationScope ForCondition(bool condition)
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
        public VerificationScope BecauseOf(string reason, params object[] reasonArgs)
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
        /// Define the failure message for the verification.
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

                    verificationStrategy.HandleFailure(message);
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
            var regex = new Regex(@"\{(?<key>\w+)\:(?<default>[\w|\s]+)\}");
            return regex.Replace(message, match =>
            {
                string key = match.Groups["key"].Value;
                return contextData.ContainsKey(key) ? contextData[key ] : match.Groups["default"].Value;
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

        public void AddContext(string key, string value)
        {
            contextData[key] = value;
        }

        /// <summary>
        /// Discards and returns the failures that happened up to now.
        /// </summary>
        public string[] Discard()
        {
            return verificationStrategy.DiscardFailures().ToArray();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Current = parentScope;
            parentScope = null;

            verificationStrategy.ThrowIfAny(contextData);
        }
    }
}