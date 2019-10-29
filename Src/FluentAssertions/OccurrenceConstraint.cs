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

        private static string Times(int count) => count == 1 ? "1 time" : $"{count} times";

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
    }

    internal sealed class AtLeastTimesConstraint : OccurrenceConstraint
    {
        internal AtLeastTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "at least";

        protected override bool IsMatch => ActualCount >= expectedCount;
    }

    internal sealed class AtMostTimesConstraint : OccurrenceConstraint
    {
        internal AtMostTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "at most";

        protected override bool IsMatch => ActualCount <= expectedCount;
    }

    internal sealed class MoreThanTimesConstraint : OccurrenceConstraint
    {
        internal MoreThanTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "more than";

        protected override bool IsMatch => ActualCount > expectedCount;
    }

    internal sealed class LessThanTimesConstraint : OccurrenceConstraint
    {
        internal LessThanTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "less than";

        protected override bool IsMatch => ActualCount < expectedCount;
    }

    internal sealed class ExactlyTimesConstraint : OccurrenceConstraint
    {
        internal ExactlyTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "exactly";

        protected override bool IsMatch => ActualCount == expectedCount;
    }

    public static class AtLeast
    {
        public static OccurrenceConstraint Once() => new AtLeastTimesConstraint(1);

        public static OccurrenceConstraint Twice() => new AtLeastTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new AtLeastTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new AtLeastTimesConstraint(expected);
    }

    public static class AtMost
    {
        public static OccurrenceConstraint Once() => new AtMostTimesConstraint(1);

        public static OccurrenceConstraint Twice() => new AtMostTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new AtMostTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new AtMostTimesConstraint(expected);
    }

    public static class MoreThan
    {
        public static OccurrenceConstraint Once() => new MoreThanTimesConstraint(1);

        public static OccurrenceConstraint Twice() => new MoreThanTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new MoreThanTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new MoreThanTimesConstraint(expected);
    }

    public static class LessThan
    {
        public static OccurrenceConstraint Twice() => new LessThanTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new LessThanTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new LessThanTimesConstraint(expected);
    }

    public static class Exactly
    {
        public static OccurrenceConstraint Once() => new ExactlyTimesConstraint(1);

        public static OccurrenceConstraint Twice() => new ExactlyTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new ExactlyTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new ExactlyTimesConstraint(expected);
    }
}
