namespace FluentAssertions
{
    public static class Exactly
    {
        public static OccurrenceConstraint Once() => new ExactlyTimesConstraint(1);

        public static OccurrenceConstraint Twice() => new ExactlyTimesConstraint(2);

        public static OccurrenceConstraint Thrice() => new ExactlyTimesConstraint(3);

        public static OccurrenceConstraint Times(int expected) => new ExactlyTimesConstraint(expected);

        private sealed class ExactlyTimesConstraint : OccurrenceConstraint
        {
            internal ExactlyTimesConstraint(int expectedCount) : base(expectedCount) { }

            internal override string Mode => "exactly";

            internal override bool Assert(int actual) => actual == ExpectedCount;
        }
    }
}
