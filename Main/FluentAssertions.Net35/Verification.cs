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
        #region Private Definitions

        private bool succeeded;
        private string reason;
        private bool useLineBreaks;

        [ThreadStatic] private static string subjectName;

        #endregion

        /// <summary>
        ///   A list of objects responsible for formatting the objects represented by placeholders.
        /// </summary>
        public static readonly List<IValueFormatter> formatters = new List<IValueFormatter>
        {
            new NullValueFormatter(),
            new DateTimeValueFormatter(),
            new TimeSpanValueFormatter(),
            new StringValueFormatter(),
            new ExpressionValueFormatter(),
            new EnumerableValueFormatter(),
            new DefaultValueFormatter()
        };

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
                    var values = new List<string>(new[] {reason});
                    values.AddRange(failureArgs.Select(ToString));

                    AssertionHelper.Throw(string.Format(failureMessage, values.ToArray()).Replace("}}", "}").Replace("{{", "{"));
                }
            }
            finally
            {
                succeeded = false;
            }
        }

        /// <summary>
        ///   Returns a human-readable representation of a particular object.
        /// </summary>
        public string ToString(object value)
        {
            var formatter = formatters.First(f => f.CanHandle(value));
            string representation = formatter.ToString(value);

            if (useLineBreaks)
            {
                representation = Environment.NewLine + representation;
            }

            return representation;
        }

        public static string SubjectNameOr(string defaultName)
        {
            return string.IsNullOrEmpty(SubjectName) ? defaultName : SubjectName;
        }
    }
}