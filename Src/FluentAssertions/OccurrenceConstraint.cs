using System;

namespace FluentAssertions
{
    public abstract class OccurrenceConstraint
    {
        internal int ExpectedCount { get; private set; }

        protected OccurrenceConstraint(int expectedCount)
        {
            if (expectedCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedCount), "Expected count cannot be negative.");
            }

            ExpectedCount = expectedCount;
        }

        internal abstract string Mode { get; }

        internal abstract bool IsMatch(int actual);
    }
}
