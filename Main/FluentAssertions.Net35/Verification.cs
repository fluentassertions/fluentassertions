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

        public void FailWith(string failureMessage, params object[] failureArgs)
        {
            try
            {
                if (!succeeded)
                {
                    string exceptionMessage = BuildExceptionMessage(failureMessage, failureArgs);

                    AssertionHelper.Throw(exceptionMessage);
                }
            }
            finally
            {
                succeeded = false;
            }
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