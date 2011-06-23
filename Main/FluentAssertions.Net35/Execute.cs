using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Formatting;
using FluentAssertions.Frameworks;

namespace FluentAssertions
{
    /// <summary>
    ///   Helper class for verifying a condition and/or throwing a test harness specific exception representing an assertion failure.
    /// </summary>
    public static class Execute
    {
        /// <summary>
        ///   Handles an assertion failure.
        /// </summary>
        /// <param name = "failureMessage">
        ///   The message that will be used in the exception. This should describe what was expected and why. This message 
        ///   can contain the following three placeholders:<br />
        ///   <list type = "bullet">
        ///     <item>{0} = the expected value</item>
        ///     <item>{1} = the actual value</item>
        ///     <item>{2} = a reason explaining the expectations</item>
        ///   </list><br />
        ///   Any additional placeholders are allowed and will be satisfied using the <paramref name = "failureMessageArgs" />.
        /// </param>
        /// <param name = "expected">
        ///   The expected value, or <c>null</c> if there is no explicit expected value.
        /// </param>
        /// <param name = "actual">The actual value, or <c>null</c> if there is no explicit actual value.</param>
        /// <param name = "reason">Should describe the reason for the expectation.</param>
        /// <param name = "reasonArgs">Optional args for formatting placeholders in the <paramref name = "reason" />.</param>
        /// <param name = "failureMessageArgs">
        ///   Optional arguments to satisfy any additional placeholders in the <paramref name = "failureMessage" />
        /// </param>
        public static void Fail(string failureMessage, object expected, object actual, string reason,
            object[] reasonArgs,
            params object[] failureMessageArgs)
        {
            var values = new List<string>
            {
                Formatter.ToString(expected),
                Formatter.ToString(actual),
                SanitizeReason(reason, reasonArgs),
            };

            values.AddRange(failureMessageArgs.Select(Formatter.ToString));

            AssertionHelper.Throw(string.Format(failureMessage, values.ToArray()));
        }

        private static string SanitizeReason(string reason, object[] reasonArgs)
        {
            if (!String.IsNullOrEmpty(reason))
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
        ///   Gets an object that wraps and executes a conditional or unconditional verification.
        /// </summary>
        public static Verification Verification
        {
            get { return new Verification(); }
        }
    }
}