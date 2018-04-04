using System;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Provides the logic and the display text for a <see cref="TimeSpanCondition"/>.
    /// </summary>
    internal class TimeSpanPredicate
    {
        private readonly string displayText;
        private readonly Func<TimeSpan, TimeSpan, bool> lambda;

        public TimeSpanPredicate(Func<TimeSpan, TimeSpan, bool> lambda, string displayText)
        {
            this.lambda = lambda;
            this.displayText = displayText;
        }

        public string DisplayText => displayText;

        public bool IsMatchedBy(TimeSpan actual, TimeSpan expected)
        {
            return lambda(actual, expected);
        }
    }
}
