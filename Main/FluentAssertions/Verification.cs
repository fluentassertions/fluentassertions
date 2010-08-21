using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public static class Verification
    {
        private static readonly List<IValueFormatter> formatters = new List<IValueFormatter>
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
        ///   Asserts that the supplied <paramref name = "condition" /> is met.
        /// </summary>
        /// <param name = "condition">The condition to assert</param>
        /// <param name = "failureMessage">
        ///   The message that will be used in the <see cref = "SpecificationMismatchException" />. This should describe what
        ///   was expected and why. This message should contain the following 3 placeholders:<br />
        ///   <list type = "bullet">
        ///     <item>{0} = expected value</item>
        ///     <item>{1} = actual value</item>
        ///     <item>{2} = the reason for the expectation</item>
        ///   </list>
        /// </param>
        /// <param name = "expected">The expected value, or <c>null</c> if there is no explicit expected value</param>
        /// <param name = "actual">The actual value, or <c>null</c> if there is no explicit actual value</param>
        /// <param name = "reason">Should describe the reason for the expectation</param>
        /// <param name = "reasonParameters">Optional parameters for the <paramref name = "reason" /></param>
        /// <example>
        ///   <code>
        ///     Verification.Verify(() => value == 0,
        ///     "Expected value to be &lt;{0}&gt;{2}, but found &lt;{1}&gt;",
        ///     expected,
        ///     reason,
        ///     reasonParameters);
        ///   </code>
        /// </example>
        /// <exception cref = "SpecificationMismatchException">when an  exception is thrown.</exception>
        public static void Verify(Func<bool> condition, string failureMessage, object expected, object actual, string reason,
            params object[] reasonParameters)
        {
            Verify(condition.Invoke(), failureMessage, expected, actual, reason, reasonParameters);
        }

        /// <summary>
        ///   Asserts that the supplied <paramref name = "action" /> does not throw any <see cref = "Exception" />.
        /// </summary>
        /// <param name = "action">The <see cref = "Action" /> to perform</param>
        /// <param name = "failureMessage">
        ///   The message that will be used in the <see cref = "SpecificationMismatchException" />. This should describe what
        ///   was expected and why. This message can contain the following placeholder:<br />
        ///   <list type = "bullet">
        ///     <item>{2} = the reason for the expectation</item>
        ///   </list>
        /// </param>
        /// <param name = "expected">The expected value, or <c>null</c> if there is no explicit expected value</param>
        /// <param name = "actual">The actual value, or <c>null</c> if there is no explicit actual value</param>
        /// <param name = "reason">Should describe the reason for the expectation</param>
        /// <param name = "reasonParameters">Optional parameters for the <paramref name = "reason" /></param>
        /// <example>
        ///   <code>
        ///     Verification.Verify(() => value == 0,
        ///     "Expected value to be positive{2}, but found &lt;{1}&gt;",
        ///     reason,
        ///     reasonParameters);
        ///   </code>
        /// </example>
        /// <exception cref = "SpecificationMismatchException">when an  exception is thrown.</exception>
        public static void Verify(Action action, string failureMessage, object expected, object actual, string reason,
            params object[] reasonParameters)
        {
            bool conditionIsMet = true;
            try
            {
                action.Invoke();
            }
            catch (SpecificationMismatchException)
            {
                conditionIsMet = false;
            }

            Verify(conditionIsMet, failureMessage, expected, actual, reason, reasonParameters);
        }

        /// <summary>
        ///   Asserts that the supplied <paramref name = "condition" /> is <c>true</c>.
        /// </summary>
        /// <param name = "condition">The condition to assert</param>
        /// <param name = "failureMessage">
        ///   The message that will be used in the <see cref = "SpecificationMismatchException" />. This should describe what
        ///   was expected and why. This message should contain the following 3 placeholders:<br />
        ///   <list type = "bullet">
        ///     <item>{0} = expected value</item>
        ///     <item>{1} = actual value</item>
        ///     <item>{2} = the reason for the expectation</item>
        ///   </list>
        /// </param>
        /// <param name = "expected">The expected value</param>
        /// <param name = "actual">The actual value</param>
        /// <param name = "reason">Should describe the reason for the expectation</param>
        /// <param name = "reasonParameters">Optional parameters for the <paramref name = "reason" /></param>
        /// <example>
        ///   <code>
        ///     Verification.Verify(() => value == 0,
        ///     "Expected value to be &lt;{0}&gt;{2}, but found &lt;{1}&gt;",
        ///     expected,
        ///     reason,
        ///     reasonParameters);
        ///   </code>
        /// </example>
        /// <exception cref = "SpecificationMismatchException">when the condition is <c>false</c>.</exception>
        public static void Verify(bool condition, string failureMessage, object expected, object actual, string reason,
            params object[] reasonParameters)
        {
            if (!condition)
            {
                Fail(failureMessage, expected, actual, reason, reasonParameters);
            }
        }

        public static void Fail(string format, object expected, object actual, string reason, object[] reasonParameters,
            params object[] args)
        {
            var values = new List<string>
            {
                ToString(expected),
                ToString(actual),
                SanitizeReason(reason, reasonParameters),
            };

            values.AddRange(args.Select(ToString));

            throw new SpecificationMismatchException(string.Format(format, values.ToArray()));
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
    }
}
