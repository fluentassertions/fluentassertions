namespace FluentAssertions
{
    public static class MoreThan
    {
        public static OccurrenceConstraint Once()
        {
            return new MoreThanTimesConstraint(expectedCount: 1);
        }

        public static OccurrenceConstraint Twice()
        {
            return new MoreThanTimesConstraint(expectedCount: 2);
        }

        public static OccurrenceConstraint Thrice()
        {
            return new MoreThanTimesConstraint(expectedCount: 3);
        }

        public static OccurrenceConstraint Times(int expected)
        {
            return new MoreThanTimesConstraint(expected);
        }

        private sealed class MoreThanTimesConstraint : OccurrenceConstraint
        {
            internal MoreThanTimesConstraint(int expectedCount)
                : base(expectedCount)
            {
            }

            internal override string Mode => "more than";

            internal override bool Assert(int actual)
            {
                return actual > ExpectedCount;
            }
        }
    }
}
