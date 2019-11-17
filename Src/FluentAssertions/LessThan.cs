namespace FluentAssertions
{
    public static class LessThan
    {
        public static OccurrenceConstraint Twice() => new LessThanTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new LessThanTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new LessThanTimesConstraint(expected);

        private sealed class LessThanTimesConstraint : OccurrenceConstraint
        {
            internal LessThanTimesConstraint(int expectedCount) : base(expectedCount) { }

            internal override string Mode => "less than";

            internal override bool Assert(int actual) => actual < ExpectedCount;
        }
    }
}
