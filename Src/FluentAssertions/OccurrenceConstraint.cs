using System;

namespace FluentAssertions
{
    public abstract class OccurrenceConstraint
    {
        protected OccurrenceConstraint(int expectedCount)
        {
            if (expectedCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedCount), "Expected count cannot be negative.");
            }

            ExpectedCount = expectedCount;
        }

        internal int ExpectedCount { get; private set; }

        internal abstract string Mode { get; }

        internal abstract bool Assert(int actual);
    }
}
