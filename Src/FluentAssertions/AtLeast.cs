namespace FluentAssertions
{
    public static class AtLeast
    {
        public static OccurrenceConstraint Once()
        {
            return new AtLeastTimesConstraint(expectedCount: 1);
        }

        public static OccurrenceConstraint Twice()
        {
            return new AtLeastTimesConstraint(expectedCount: 2);
        }

        public static OccurrenceConstraint Thrice()
        {
            return new AtLeastTimesConstraint(expectedCount: 3);
        }

        public static OccurrenceConstraint Times(int expected)
        {
            return new AtLeastTimesConstraint(expected);
        }

        private sealed class AtLeastTimesConstraint : OccurrenceConstraint
        {
            internal AtLeastTimesConstraint(int expectedCount)
                : base(expectedCount)
            {
            }

            internal override string Mode => "at least";

            internal override bool Assert(int actual)
            {
                return actual >= ExpectedCount;
            }
        }
    }
}
