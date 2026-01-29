using System;
using FluentAssertions.Common;

namespace FluentAssertions;

[System.Diagnostics.StackTraceHidden]
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

    internal int ExpectedCount { get; }

    internal abstract string Mode { get; }

    internal abstract bool Assert(int actual);

    internal void RegisterContextData(Action<string, object> register)
    {
        register("expectedOccurrence", $"{Mode} {ExpectedCount.Times()}");
    }
}

