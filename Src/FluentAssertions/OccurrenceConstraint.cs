using System;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    public abstract class OccurrenceConstraint
    {
        private int? actualCount;

        protected readonly int expectedCount;

        protected OccurrenceConstraint(int expectedCount)
        {
            this.expectedCount = expectedCount;
        }

        protected int ActualCount
        {
            get
            {
                if (!actualCount.HasValue)
                {
                    actualCount = Subject.CountSubstring(Expected, StringComparison);
                }

                return actualCount.Value;
            }
        }

        protected abstract string Mode { get; }

        protected abstract bool IsMatch { get; }

        private string Expected { get; set; }

        private string Subject { get; set; }

        private StringComparison StringComparison { get; set; }

        internal void AssertContain(string subject, string expected, string because, params object[] becauseArgs)
        {
            Subject = subject;
            Expected = expected;
            StringComparison = StringComparison.Ordinal;

            Execute.Assertion
                .ForCondition(IsMatch)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    $"Expected {{context:string}} {{0}} to contain {{1}} {Mode} {Times(expectedCount)}{{reason}}, but found {Times(ActualCount)}.",
                    Subject, expected);
        }

        internal void AssertContainEquivalentOf(string subject, string expected, string because, params object[] becauseArgs)
        {
            Subject = subject;
            Expected = expected;
            StringComparison = StringComparison.CurrentCultureIgnoreCase;

            Execute.Assertion
                .ForCondition(IsMatch)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    $"Expected {{context:string}} {{0}} to contain equivalent of {{1}} {Mode} {Times(expectedCount)}{{reason}}, but found {Times(ActualCount)}.",
                    Subject, expected);
        }

        private static string Times(int count) => count == 1 ? "1 time" : $"{count} times";
    }
}
