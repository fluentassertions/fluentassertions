using System;
using FluentAssertions.Common;

namespace FluentAssertions.Primitives
{
    public abstract class TimesConstraint
    {
        private int? actualCount;

        protected readonly int expectedCount;

        internal TimesConstraint(int expectedCount)
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

        public string Expected { get; set; }

        public string Subject { get; set; }

        public string Containment { get; set; }

        public StringComparison StringComparison { get; set; }

        public string MessageFormat =>
            $"Expected {{context:string}} {{0}} to {Containment} {{1}} {Mode} {Times(expectedCount)}{{reason}}, but found {Times(ActualCount)}.";

        private static string Times(int count) => count == 1 ? "1 time" : $"{count} times";
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
