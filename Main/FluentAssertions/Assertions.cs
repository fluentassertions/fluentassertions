using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public abstract class AssertionsBase<TSubject>
    {
        protected TSubject Subject;
        
        private List<IFormatter> formatters = new List<IFormatter>
        {
            new NullFormatter(),
            new TimeSpanFormatter(),
            new StringFormatter(),
            new ExpressionFormatter(),
            new EnumerableFormatter(),
            new DefaultFormatter()
        };

        /// <summary>
        /// Asserts that the supplied <paramref name="condition"/> is met.
        /// </summary>
        /// <param name="condition">The condition to assert</param>
        /// <param name="failureMessage">
        /// The message that will be used in the <see cref="SpecificationMismatchException"/>. This should describe what
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
        /// VerifyThat(() => value == 0,
        ///     "Expected value to be &lt;{0}&gt;{2}, but found &lt;{1}&gt;",
        ///     expected,
        ///     reason,
        ///     reasonParameters);
        /// </code>
        /// </example>
        /// <exception cref="SpecificationMismatchException">when an  exception is thrown.</exception>
        protected void VerifyThat(Func<bool> condition, string failureMessage, object expected, object actual, string reason,
            params object[] reasonParameters)
        {
            VerifyThat(condition.Invoke(), failureMessage, expected, actual, reason, reasonParameters);
        }

        /// <summary>
        /// Asserts that the supplied <paramref name="action"/> does not throw any <see cref="Exception"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to perform</param>
        /// <param name="failureMessage">
        /// The message that will be used in the <see cref="SpecificationMismatchException"/>. This should describe what
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
        /// VerifyThat(() => value == 0,
        ///     "Expected value to be positive{2}, but found &lt;{1}&gt;",
        ///     reason,
        ///     reasonParameters);
        /// </code>
        /// </example>
        /// <exception cref="SpecificationMismatchException">when an  exception is thrown.</exception>
        protected void VerifyThat(Action action, string failureMessage, object expected, object actual, string reason,
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

            VerifyThat(conditionIsMet, failureMessage, expected, actual, reason, reasonParameters);
        }

        /// <summary>
        /// Asserts that the supplied <paramref name="condition"/> is <c>true</c>.
        /// </summary>
        /// <param name="condition">The condition to assert</param>
        /// <param name="failureMessage">
        /// The message that will be used in the <see cref="SpecificationMismatchException"/>. This should describe what
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
        /// VerifyThat(() => value == 0,
        ///     "Expected value to be &lt;{0}&gt;{2}, but found &lt;{1}&gt;",
        ///     expected,
        ///     reason,
        ///     reasonParameters);
        /// </code>
        /// </example>
        /// <exception cref="SpecificationMismatchException">when the condition is <c>false</c>.</exception>
        protected void VerifyThat(bool condition, string failureMessage, object expected, object actual, string reason,
            params object[] reasonParameters)
        {
            if (!condition)
            {
                FailWith(failureMessage, expected, actual, reason, reasonParameters);
            }
        }

        protected void FailWith(string format, object expected, object actual, string reason, object[] reasonParameters, params object[] args)
        {
            List<string> values = new List<string>
            {
                Format(expected),
                Format(actual),
                SanitizeReason(reason, reasonParameters),
            };

            values.AddRange(args.Select(Format));

            throw new SpecificationMismatchException(string.Format(format, values.ToArray()));
        }

        /// <summary>
        /// If the value is a collection, returns it as a comma-separated string.
        /// </summary>
        protected string Format(object value)
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

    public abstract class Assertions<TSubject, TAssertions> : AssertionsBase<TSubject> where TAssertions : Assertions<TSubject, TAssertions>
    {
        /// <summary>
        /// Asserts that the <paramref name="predicate"/> is statisfied.
        /// </summary>
        /// <param name="predicate">The predicate which must be satisfied by the <typeparamref name="TSubject"/>.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public AndConstraint<Assertions<TSubject, TAssertions>> Match(Expression<Func<TSubject, bool>> predicate)
        {
            return Match<TSubject>(predicate, String.Empty);
        }

        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">The predicate which must be statisfied by the <typeparamref name="TSubject"/>.</param>
        /// <param name="reason">The reason why the predicate should be satisfied.</param>
        /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public AndConstraint<Assertions<TSubject, TAssertions>> Match(Expression<Func<TSubject, bool>> predicate, string reason,
            params object[] reasonParameters)
        {
            return Match<TSubject>(predicate, reason, reasonParameters);
        }
        
        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">The predicate which must be statisfied by the <typeparamref name="TSubject"/>.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public AndConstraint<Assertions<TSubject, TAssertions>> Match<T>(Expression<Func<T, bool>> predicate)
            where T : TSubject
        {
            return Match(predicate, string.Empty);
        }
        
        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">The predicate which must be statisfied by the <typeparamref name="TSubject"/>.</param>
        /// <param name="reason">The reason why the predicate should be satisfied.</param>
        /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
        /// <returns>An <see cref="AndConstraint"/> which can be used to chain assertions.</returns>
        public AndConstraint<Assertions<TSubject, TAssertions>> Match<T>(Expression<Func<T, bool>> predicate, string reason,
            params object[] reasonParameters)
            where T : TSubject
        {
            if (predicate == null)
            {
                throw new NullReferenceException("Cannot match an object against a <null> predicate.");
            }

            VerifyThat(() => predicate.Compile()((T)Subject),
                "Expected {1} to match {0}{2}.",
                predicate.Body, Subject, reason, reasonParameters);

            return new AndConstraint<Assertions<TSubject, TAssertions>>(this);
        }
    }
}