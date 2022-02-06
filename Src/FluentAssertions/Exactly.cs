namespace FluentAssertions
{
    public static class Exactly
    {
        public static OccurrenceConstraint Once()
        {
            return new ExactlyTimesConstraint(expectedCount: 1);
        }

        public static OccurrenceConstraint Twice()
        {
            return new ExactlyTimesConstraint(expectedCount: 2);
        }

        public static OccurrenceConstraint Thrice()
        {
            return new ExactlyTimesConstraint(expectedCount: 3);
        }

        public static OccurrenceConstraint Times(int expected)
        {
            return new ExactlyTimesConstraint(expected);
        }

        private sealed class ExactlyTimesConstraint : OccurrenceConstraint
        {
            internal ExactlyTimesConstraint(int expectedCount)
                : base(expectedCount)
            {
            }

            internal override string Mode => "exactly";

            internal override bool Assert(int actual)
            {
                return actual == ExpectedCount;
            }
        }
    }
}
