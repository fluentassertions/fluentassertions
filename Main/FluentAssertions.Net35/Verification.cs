using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Formatting;
using FluentAssertions.Frameworks;

namespace FluentAssertions
{
    public class Verification
    {
        private bool succeeded;
        private string reason;

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

        internal Verification()
        {
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
                if (!reason.StartsWith("because", StringComparison.CurrentCultureIgnoreCase) 
                    && !reason.StartsWith("for ", StringComparison.CurrentCultureIgnoreCase))
                {
                    reason = "because " + reason;
                }

                return " " + String.Format(reason, reasonArgs);
            }

            return "";
        }

        public void FailWith(string failureMessage, params object[] failureArgs)
        {
            if (!succeeded)
            {
                var values = new List<string>(new[] { reason });
                values.AddRange(failureArgs.Select(ToString));

                AssertionHelper.Throw(string.Format(failureMessage, values.ToArray()));
            }
        }

        /// <summary>
        ///   If the value is a collection, returns it as a comma-separated string.
        /// </summary>
        public static string ToString(object value)
        {
            var formatter = formatters.First(f => f.CanHandle(value));
            return formatter.ToString(value);
        }
    }
}
