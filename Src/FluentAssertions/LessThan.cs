namespace FluentAssertions
{
    public static class LessThan
    {
        public static OccurrenceConstraint Twice()
        {
            return new LessThanTimesConstraint(expectedCount: 2);
        }

        public static OccurrenceConstraint Thrice()
        {
            return new LessThanTimesConstraint(expectedCount: 3);
        }

        public static OccurrenceConstraint Times(int expected)
        {
            return new LessThanTimesConstraint(expected);
        }

        private sealed class LessThanTimesConstraint : OccurrenceConstraint
        {
            internal LessThanTimesConstraint(int expectedCount)
                : base(expectedCount)
            {
            }

            internal override string Mode => "less than";

            internal override bool Assert(int actual)
            {
                return actual < ExpectedCount;
            }
        }
    }
}
