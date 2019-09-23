using System;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    public abstract class TimesConstraint
    {
        private int? actualCount;

        protected readonly int expectedCount;

        public TimesConstraint(int expectedCount)
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

        public abstract bool IsMatch { get; }

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

    public class AtLeastTimesConstraint : TimesConstraint
    {
        internal AtLeastTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "at least";

        public override bool IsMatch => ActualCount >= expectedCount;
    }

    public class AtMostTimesConstraint : TimesConstraint
    {
        internal AtMostTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "at most";

        public override bool IsMatch => ActualCount <= expectedCount;
    }

    public class MoreThanTimesConstraint : TimesConstraint
    {
        internal MoreThanTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "more than";

        public override bool IsMatch => ActualCount > expectedCount;
    }

    public class LessThanTimesConstraint : TimesConstraint
    {
        internal LessThanTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "less than";

        public override bool IsMatch => ActualCount < expectedCount;
    }

    public class ExactlyTimesConstraint : TimesConstraint
    {
        internal ExactlyTimesConstraint(int expectedCount) : base(expectedCount) { }

        protected override string Mode => "exactly";

        public override bool IsMatch => ActualCount == expectedCount;
    }
}
