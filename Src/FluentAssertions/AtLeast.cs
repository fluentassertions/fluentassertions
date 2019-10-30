namespace FluentAssertions
{
    public static class AtLeast
    {
        public static OccurrenceConstraint Once() => new AtLeastTimesConstraint(1);

        public static OccurrenceConstraint Twice() => new AtLeastTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new AtLeastTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new AtLeastTimesConstraint(expected);

        internal sealed class AtLeastTimesConstraint : OccurrenceConstraint
        {
            internal AtLeastTimesConstraint(int expectedCount) : base(expectedCount) { }

            internal override string Mode => "at least";

            internal override bool IsMatch(int actual) => actual >= ExpectedCount;
        }
    }
}
