using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Formatting;
using FluentAssertions.Frameworks;

namespace FluentAssertions
{
    /// <summary>
    /// Provides a fluent API for verifying an arbitrary condition.
    /// </summary>
    public class Verification
    {
        private readonly char[] blanks = new[] { '\r', '\n', ' ', '\t' };

        /// <summary>
        /// Represents the phrase that can be used in <see cref="FailWith"/> as a placeholder for the reason of an assertion.
        /// </summary>
        public const string ReasonTag = "{reason}";

        #region Private Definitions

        [ThreadStatic] private static string subjectName;
        private string reason;
        private bool succeeded;
        private bool useLineBreaks;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Verification"/> class.
        /// </summary>
        internal Verification()
        {
        }

        /// <summary>
        /// Indicates that every argument passed into <see cref="FailWith"/> is displayed on a separate line.
        /// </summary>
        public Verification UsingLineBreaks
        {
            get
            {
                useLineBreaks = true;
                return this;
            }
        }

        /// <summary>
        /// Gets or sets the name of the subject for the next verification.
        /// </summary>
        public static string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }

        /// <summary>
        /// Gets the name or identifier of the current subject, or a default value if the subject is not known.
        /// </summary>
        public static string SubjectNameOr(string defaultName)
        {
            return string.IsNullOrEmpty(SubjectName) ? defaultName : SubjectName;
        }

        /// <summary>
        /// Specify the condition that must be satisfied.
        /// </summary>
        /// <param name="condition">If <c>true</c> the verification will be succesful.</param>
        public Verification ForCondition(bool condition)
        {
            succeeded = condition;
            return this;
        }

        /// <summary>
        /// Specify a predicate that with the condition that must be satisfied.
        /// </summary>
        public Verification ForCondition(Func<bool> condition)
        {
            succeeded = condition();
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
        public Verification BecauseOf(string reason, params object[] reasonArgs)
        {
            this.reason = SanitizeReason(reason, reasonArgs);
            return this;
        }

        private string SanitizeReason(string reason, object [] reasonArgs)
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
            return (text.Length > 0) && blanks.Any(blank => text.First() == blank);
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
        public void FailWith(string failureMessage, params object [] failureArgs)
        {
            try
            {
                if (!succeeded)
                {
                    var message = ReplaceReasonTag(failureMessage);
                    string exceptionMessage = BuildExceptionMessage(message, failureArgs);

                    AssertionHelper.Throw(exceptionMessage);
                }
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
            if (!failureMessage.Contains(ReasonTag))
            {
                throw new InvalidOperationException(
                    @"Reason is specified through 'BecauseOf(...)', but format string does not contain a placeholder for the reason!");
            }

            string message = IncrementAllFormatSpecifiers(failureMessage);
            message = message.Replace(ReasonTag, "{0}");

            return message;
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

        private string BuildExceptionMessage(string failureMessage, object [] failureArgs)
        {
            var values = new List<string>();
            if (!string.IsNullOrEmpty(reason))
            {
                values.Add(reason);
            }

            values.AddRange(failureArgs.Select(a => useLineBreaks ? Formatter.ToStringLine(a) : Formatter.ToString(a)));

            return string.Format(failureMessage, values.ToArray()).Replace("{{{{", "{{").Replace("}}}}", "}}");
        }
    }
}