using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Formatting;
using FluentAssertions.Frameworks;

namespace FluentAssertions
{
    /// <summary>
    /// Helper class for verifying a condition and/or throwing a test harness specific exception representing an assertion failure.
    /// </summary>
    public static class Execute
    {
        /// <summary>
        /// A list of objects responsible for formatting the objects represented by placeholders.
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
        /// Asserts that the supplied <paramref name = "condition" /> is met.
        /// </summary>
        /// <param name = "condition">The condition to assert.</param>
        /// <param name = "failureMessage">
        /// The message that will be used in the exception. This should describe what was expected and why. This message 
        /// can contain the following three placeholders:<br />
        ///   <list type = "bullet">
        ///     <item>{0} = the expected value</item>
        ///     <item>{1} = the actual value</item>
        ///     <item>{2} = a reason explaining the expectations</item>
        ///   </list><br/>
        /// Any additional placeholders are allowed and will be satisfied using the <paramref name="failureMessageArgs"/>.
        /// </param>
        /// <param name = "expected">
        /// The expected value, or <c>null</c> if there is no explicit expected value.
        /// </param>
        /// <param name = "actual">The actual value, or <c>null</c> if there is no explicit actual value.</param>
        /// <param name = "reason">Should describe the reason for the expectation.</param>
        /// <param name = "reasonArgs">Optional args for formatting placeholders in the <paramref name = "reason" />.</param>
        /// <param name="failureMessageArgs">
        /// Optional arguments to satisfy any additional placeholders in the <paramref name="failureMessage"/>
        /// </param>
        public static void Verify(Func<bool> condition, string failureMessage, object expected, object actual, string reason,
            object[] reasonArgs, params object[] failureMessageArgs)
        {
            Verify(condition.Invoke(), failureMessage, expected, actual, reason, reasonArgs, failureMessageArgs);
        }

        /// <summary>
        /// Asserts that the supplied <paramref name = "condition" /> is met.
        /// </summary>
        /// <param name = "condition">The condition to assert.</param>
        /// <param name = "failureMessage">
        /// The message that will be used in the exception. This should describe what was expected and why. This message 
        /// can contain the following three placeholders:<br />
        ///   <list type = "bullet">
        ///     <item>{0} = the expected value</item>
        ///     <item>{1} = the actual value</item>
        ///     <item>{2} = a reason explaining the expectations</item>
        ///   </list><br/>
        /// Any additional placeholders are allowed and will be satisfied using the <paramref name="failureMessageArgs"/>.
        /// </param>
        /// <param name = "expected">
        /// The expected value, or <c>null</c> if there is no explicit expected value.
        /// </param>
        /// <param name = "actual">The actual value, or <c>null</c> if there is no explicit actual value.</param>
        /// <param name = "reason">Should describe the reason for the expectation.</param>
        /// <param name = "reasonArgs">Optional args for formatting placeholders in the <paramref name = "reason" />.</param>
        /// <param name="failureMessageArgs">
        /// Optional arguments to satisfy any additional placeholders in the <paramref name="failureMessage"/>
        /// </param>
        public static void Verify(bool condition, string failureMessage, object expected, object actual, string reason,
            object[] reasonParameters, params object[] failureMessageArgs)
        {
            if (!condition)
            {
                Fail(failureMessage, expected, actual, reason, reasonParameters);
            }
        }

        /// <summary>
        /// Handles an assertion failure.
        /// </summary>
        /// <param name = "failureMessage">
        /// The message that will be used in the exception. This should describe what was expected and why. This message 
        /// can contain the following three placeholders:<br />
        ///   <list type = "bullet">
        ///     <item>{0} = the expected value</item>
        ///     <item>{1} = the actual value</item>
        ///     <item>{2} = a reason explaining the expectations</item>
        ///   </list><br/>
        /// Any additional placeholders are allowed and will be satisfied using the <paramref name="failureMessageArgs"/>.
        /// </param>
        /// <param name = "expected">
        /// The expected value, or <c>null</c> if there is no explicit expected value.
        /// </param>
        /// <param name = "actual">The actual value, or <c>null</c> if there is no explicit actual value.</param>
        /// <param name = "reason">Should describe the reason for the expectation.</param>
        /// <param name = "reasonArgs">Optional args for formatting placeholders in the <paramref name = "reason" />.</param>
        /// <param name="failureMessageArgs">
        /// Optional arguments to satisfy any additional placeholders in the <paramref name="failureMessage"/>
        /// </param>
        public static void Fail(string failureMessage, object expected, object actual, string reason, object[] reasonArgs,
            params object[] failureMessageArgs)
        {
            var values = new List<string>
            {
                ToString(expected),
                ToString(actual),
                SanitizeReason(reason, reasonArgs),
            };

            values.AddRange(failureMessageArgs.Select(ToString));

            AssertionHelper.Throw(string.Format(failureMessage, values.ToArray()));
        }

        /// <summary>
        ///   If the value is a collection, returns it as a comma-separated string.
        /// </summary>
        public static string ToString(object value)
        {
            var formatter = formatters.First(f => f.CanHandle(value));
            return formatter.ToString(value);
        }

        private static string SanitizeReason(string reason, object[] reasonParameters)
        {
            if (!String.IsNullOrEmpty(reason))
            {
                if (!reason.StartsWith("because", StringComparison.CurrentCultureIgnoreCase))
                {
                    reason = "because " + reason;
                }

                return " " + String.Format(reason, reasonParameters);
            }

            return "";
        }

        /// <summary>
        /// Gets an object that wraps and executes a conditional or unconditional verification.
        /// </summary>
        public static Verification Verification
        {
            get { return new Verification(); }
        }
    }
}
