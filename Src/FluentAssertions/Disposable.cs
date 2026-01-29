using System;

namespace FluentAssertions;

[System.Diagnostics.StackTraceHidden]
internal sealed class Disposable : IDisposable
{
    private readonly Action action;

    public Disposable(Action action)
    {
        this.action = action;
    }

    public void Dispose()
    {
        action();
    }
}

