namespace FluentAssertions
{
    public static class LessThan
    {
        public static OccurrenceConstraint Twice() => new LessThanTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new LessThanTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new LessThanTimesConstraint(expected);

        internal sealed class LessThanTimesConstraint : OccurrenceConstraint
        {
            internal LessThanTimesConstraint(int expectedCount) : base(expectedCount) { }

            internal override string Mode => "less than";

            internal override bool IsMatch(int actual) => actual < ExpectedCount;
        }
    }
}
