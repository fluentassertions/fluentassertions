using System;
using System.Diagnostics;
using FluentAssertions.Primitives;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class StringContainmentConstraints : StringAssertions
    {
        private readonly StringComparison comparison;
        private int? occurrences;

        public StringContainmentConstraints(string subject, string expected, string containmentMode, StringComparison comparison)
            : base(subject)
        {
            this.comparison = comparison;
            Expected = expected;
            ContainmentMode = containmentMode;
            And = this;
        }

        internal string Expected { get; }

        internal string ContainmentMode { get; }

        public StringContainmentConstraints And { get; }

        public StringWithOccurenceConstraint Exactly => new StringWithOccurenceConstraint(this,
            "exactly",
            (occuredTimes, expectedTimes) => occuredTimes == expectedTimes);

        public StringWithOccurenceConstraint AtLeast => new StringWithOccurenceConstraint(this,
            "at least",
            (occuredTimes, expectedTimes) => occuredTimes >= expectedTimes);

        public StringWithOccurenceConstraint MoreThan => new StringWithOccurenceConstraint(this,
            "more than",
            (occuredTimes, expectedTimes) => occuredTimes > expectedTimes);

        public StringWithOccurenceConstraint AtMost => new StringWithOccurenceConstraint(this,
            "at most",
            (occuredTimes, expectedTimes) => occuredTimes <= expectedTimes);

        public StringWithOccurenceConstraint LessThan => new StringWithOccurenceConstraint(this,
            "less than",
            (occuredTimes, expectedTimes) => occuredTimes < expectedTimes);

        internal int Occurrences => (occurrences ?? (occurrences = CountOccurrences())).Value;

        private int CountOccurrences()
        {
            string actual = Subject ?? "";
            string substring = Expected ?? "";

            int count = 0;
            int index = 0;

            while ((index = actual.IndexOf(substring, index, comparison)) >= 0)
            {
                index += substring.Length;
                count++;
            }

            return count;
        }

        public AndConstraint<StringContainmentConstraints> Once(string because = "", params object[] becauseArgs) =>
            Exactly.Once(because, becauseArgs);

        public AndConstraint<StringContainmentConstraints> Twice(string because = "", params object[] becauseArgs) =>
            Exactly.Twice(because, becauseArgs);

        public AndConstraint<StringContainmentConstraints> Thrice(string because = "", params object[] becauseArgs) =>
            Exactly.Thrice(because, becauseArgs);

        public AndConstraint<StringContainmentConstraints> Times(int times, string because = "", params object[] becauseArgs) =>
            Exactly.Times(times, because, becauseArgs);
    }
}
