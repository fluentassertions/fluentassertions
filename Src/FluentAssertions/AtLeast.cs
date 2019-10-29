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

            protected override string Mode => "at least";

            protected override bool IsMatch => ActualCount >= expectedCount;
        }
    }
}
