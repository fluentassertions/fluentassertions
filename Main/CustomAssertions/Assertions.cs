using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions
{
    public static partial class CustomAssertionExtensions
    {
        //[DebuggerNonUserCode]
        public abstract class Assertions
        {
            /// <summary>
            /// Asserts that the supplied <paramref name="condition"/> is met.
            /// Throws an <see cref="AssertFailedException"/> when the condition is not met.
            /// </summary>
            /// <param name="condition">The condition to assert</param>
            /// <param name="failureMessage">
            /// The message that will be used in the <see cref="AssertFailedException"/>. This should describe what
            /// was expected and why. This message should contain the following 3 placeholders:<br />
            /// <list type="bullet">
            /// <item>{0} = expected value</item>
            /// <item>{1} = actual value</item>
            /// <item>{2} = the reason for the expectation</item>
            /// </list>
            /// </param>
            /// <param name="expected">The expected value, or <c>null</c> if there is no explicit expected value</param>
            /// <param name="actual">The actual value, or <c>null</c> if there is no explicit actual value</param>
            /// <param name="reason">Should describe the reason for the expectation</param>
            /// <param name="reasonParameters">Optional parameters for the <paramref name="reason"/></param>
            /// <example>
            /// <code>
            /// AssertThat(() => value == 0,
            ///     "Expected value to be &lt;{0}&gt;{2}, but found &lt;{1}&gt;",
            ///     expected,
            ///     reason,
            ///     reasonParameters);
            /// </code>
            /// </example>
            protected void AssertThat(Func<bool> condition, string failureMessage, object expected, object actual, string reason,
                                      params object[] reasonParameters)
            {
                AssertThat(condition.Invoke(), failureMessage, expected, actual, reason, reasonParameters);
            }

            /// <summary>
            /// Asserts that the supplied <paramref name="action"/> does not throw any <see cref="Exception"/>
            /// and throws an <see cref="AssertFailedException"/> when this does happen.
            /// </summary>
            /// <param name="action">The <see cref="Action"/> to perform</param>
            /// <param name="failureMessage">
            /// The message that will be used in the <see cref="AssertFailedException"/>. This should describe what
            /// was expected and why. This message can contain the following placeholder:<br />
            /// <list type="bullet">
            /// <item>{2} = the reason for the expectation</item>
            /// </list>
            /// </param>
            /// <param name="expected">The expected value, or <c>null</c> if there is no explicit expected value</param>
            /// <param name="actual">The actual value, or <c>null</c> if there is no explicit actual value</param>
            /// <param name="reason">Should describe the reason for the expectation</param>
            /// <param name="reasonParameters">Optional parameters for the <paramref name="reason"/></param>
            /// <example>
            /// <code>
            /// AssertThat(() => value == 0,
            ///     "Expected value to be positive{2}, but found &lt;{1}&gt;",
            ///     reason,
            ///     reasonParameters);
            /// </code>
            /// </example>
            protected void AssertThat(Action action, string failureMessage, object expected, object actual, string reason,
                                      params object[] reasonParameters)
            {
                bool conditionIsMet = true;
                try
                {
                    action.Invoke();
                }
                catch (AssertFailedException)
                {
                    conditionIsMet = false;
                }

                AssertThat(conditionIsMet, failureMessage, expected, actual, reason, reasonParameters);
            }

            /// <summary>
            /// Asserts that the supplied <paramref name="condition"/> is <c>true</c>.
            /// Throws an <see cref="AssertFailedException"/> when the condition is <c>false</c>.
            /// </summary>
            /// <param name="condition">The condition to assert</param>
            /// <param name="failureMessage">
            /// The message that will be used in the <see cref="AssertFailedException"/>. This should describe what
            /// was expected and why. This message should contain the following 3 placeholders:<br />
            /// <list type="bullet">
            /// <item>{0} = expected value</item>
            /// <item>{1} = actual value</item>
            /// <item>{2} = the reason for the expectation</item>
            /// </list>
            /// </param>
            /// <param name="expected">The expected value</param>
            /// <param name="actual">The actual value</param>
            /// <param name="reason">Should describe the reason for the expectation</param>
            /// <param name="reasonParameters">Optional parameters for the <paramref name="reason"/></param>
            /// <example>
            /// <code>
            /// AssertThat(() => value == 0,
            ///     "Expected value to be &lt;{0}&gt;{2}, but found &lt;{1}&gt;",
            ///     expected,
            ///     reason,
            ///     reasonParameters);
            /// </code>
            /// </example>
            protected void AssertThat(bool condition, string failureMessage, object expected, object actual, string reason,
                                      params object[] reasonParameters)
            {
                if (!condition)
                {
                    throw new AssertFailedException(string.Format(
                                                        failureMessage,
                                                        Expand(expected), Expand(actual),
                                                        SanitizeReason(reason, reasonParameters)));
                }
            }

            /// <summary>
            /// If the value is a collection, returns it as a comma-separated string.
            /// </summary>
            private static object Expand(object expected)
            {
                var enumerable = expected as IEnumerable;
                if ((enumerable != null) && !(expected is string))
                {
                    return string.Join(", ", enumerable.Cast<object>().Select(o => o.ToString()).ToArray());
                }
                else
                {
                    return expected;
                }
            }

            private static string SanitizeReason(string reason, object[] reasonParameters)
            {
                if (!string.IsNullOrEmpty(reason))
                {
                    if (!reason.StartsWith("because", StringComparison.CurrentCultureIgnoreCase))
                    {
                        reason = "because " + reason;
                    }

                    return " " + string.Format(reason, reasonParameters);
                } 
                else
                {
                    return "";    
                }
            }
        }
    }
}