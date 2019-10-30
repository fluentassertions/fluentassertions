namespace FluentAssertions
{
    public static class MoreThan
    {
        public static OccurrenceConstraint Once() => new MoreThanTimesConstraint(1);

        public static OccurrenceConstraint Twice() => new MoreThanTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new MoreThanTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new MoreThanTimesConstraint(expected);

        internal sealed class MoreThanTimesConstraint : OccurrenceConstraint
        {
            internal MoreThanTimesConstraint(int expectedCount) : base(expectedCount) { }

            internal override string Mode => "more than";

            internal override bool IsMatch(int actual) => actual > ExpectedCount;
        }
    }
}
