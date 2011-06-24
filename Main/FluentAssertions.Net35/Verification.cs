using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Formatting;
using FluentAssertions.Frameworks;
using FluentAssertions.Common;

namespace FluentAssertions
{
    /// <summary>
    /// Provides a fluent API for verifying an arbitrary condition.
    /// </summary>
    public class Verification
    {
        public const string ReasonTag = "[reason]";

        #region Private Definitions

        private bool succeeded;
        private string reason;
        private bool useLineBreaks;

        [ThreadStatic] private static string subjectName;

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

        public Verification ForCondition(bool condition)
        {
            succeeded = condition;
            return this;
        }

        public Verification ForCondition(Func<bool> condition)
        {
            succeeded = condition();
            return this;
        }

        public Verification BecauseOf(string reason, params object[] reasonArgs)
        {
            this.reason = SanitizeReason(reason, reasonArgs);
            return this;
        }

        private static string SanitizeReason(string reason, object[] reasonArgs)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                if (!reason.StartsWith("because", StringComparison.CurrentCultureIgnoreCase))
                {
                    reason = "because " + reason;
                }

                return " " + String.Format(reason, reasonArgs);
            }

            return "";
        }

        /// <summary>
        /// Define the failure message for the verification.
        /// </summary>
        /// <remarks>
        /// If the <paramref name="failureMessage"/> contains the text "[reason]", this will be replaced by the reason as
        /// defined through <see cref="BecauseOf"/>.
        /// </remarks>
        /// <param name="failureMessage">The format string that represents the failure message.</param>
        /// <param name="failureArgs">Optional arguments for the <paramref name="failureMessage"/></param>
        public void FailWith(string failureMessage, params object[] failureArgs)
        {
            try
            {
                if (!succeeded)
                {
                    string reNumberedFailureMessage = ReplaceReasonTag(failureMessage);
                    string exceptionMessage = BuildExceptionMessage(reNumberedFailureMessage, failureArgs);

                    AssertionHelper.Throw(exceptionMessage);
                }
            }
            finally
            {
                succeeded = false;
            }
        }

        private static string ReplaceReasonTag(string failureMessage)
        {
            string renumderedMessage = failureMessage;

            if (failureMessage.Contains(ReasonTag))
            {
                for (int index = 9; index >= 0; index--)
                {
                    int newIndex = index + 1;
                    string oldTag = "{" + index + "}";
                    string newTag = "{" + newIndex + "}";
                    renumderedMessage = renumderedMessage.Replace(oldTag, newTag);
                }

                renumderedMessage = renumderedMessage.Replace(ReasonTag, "{0}");
            }

            return renumderedMessage;
        }

        private string BuildExceptionMessage(string failureMessage, object[] failureArgs)
        {
            var values = new List<string>(new[] {reason});
            values.AddRange(failureArgs.Select(a => useLineBreaks ? Formatter.ToStringLine(a) : Formatter.ToString(a)));

            return string.Format(failureMessage, values.ToArray()).Replace("{{{{", "{{").Replace("}}}}", "}}");
        }

        /// <summary>
        /// Gets the name or identifier of the current subject, or a default value if the subject is not known.
        /// </summary>
        public static string SubjectNameOr(string defaultName)
        {
            return string.IsNullOrEmpty(SubjectName) ? defaultName : SubjectName;
        }
    }
}