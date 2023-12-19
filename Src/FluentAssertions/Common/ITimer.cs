using System;

namespace FluentAssertionsAsync.Common;

/// <summary>
/// Abstracts a stopwatch so we can control time in unit tests.
/// </summary>
public interface ITimer : IDisposable
{
    /// <summary>
    /// The time elapsed since the timer was created through <see cref="IClock.StartTimer"/>.
    /// </summary>
    TimeSpan Elapsed { get; }
}
