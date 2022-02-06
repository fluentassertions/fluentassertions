namespace FluentAssertions
{
    public static class AtMost
    {
        public static OccurrenceConstraint Once()
        {
            return new AtMostTimesConstraint(expectedCount: 1);
        }

        public static OccurrenceConstraint Twice()
        {
            return new AtMostTimesConstraint(expectedCount: 2);
        }

        public static OccurrenceConstraint Thrice()
        {
            return new AtMostTimesConstraint(expectedCount: 3);
        }

        public static OccurrenceConstraint Times(int expected)
        {
            return new AtMostTimesConstraint(expected);
        }

        private sealed class AtMostTimesConstraint : OccurrenceConstraint
        {
            internal AtMostTimesConstraint(int expectedCount)
                : base(expectedCount)
            {
            }

            internal override string Mode => "at most";

            internal override bool Assert(int actual)
            {
                return actual <= ExpectedCount;
            }
        }
    }
}
