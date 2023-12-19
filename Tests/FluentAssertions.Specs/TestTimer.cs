using System;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Specs;

internal sealed class TestTimer : ITimer
{
    private readonly Func<TimeSpan> getElapsed;

    public TestTimer(Func<TimeSpan> getElapsed)
    {
        this.getElapsed = getElapsed;
    }

    public TimeSpan Elapsed => getElapsed();

    public void Dispose()
    {
    }
}
