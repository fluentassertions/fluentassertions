namespace FluentAssertions
{
    public static class AtMost
    {
        public static OccurrenceConstraint Once() => new AtMostTimesConstraint(1);

        public static OccurrenceConstraint Twice() => new AtMostTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new AtMostTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new AtMostTimesConstraint(expected);

        private sealed class AtMostTimesConstraint : OccurrenceConstraint
        {
            internal AtMostTimesConstraint(int expectedCount) : base(expectedCount) { }

            internal override string Mode => "at most";

            internal override bool Assert(int actual) => actual <= ExpectedCount;
        }
    }
}
