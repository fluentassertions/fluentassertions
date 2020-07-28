using System;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Abstracts a stopwatch
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// The time elapsed since the timer was created through <see cref="IClock.StartTimer"/>.
        /// </summary>
        TimeSpan Elapsed { get; }
    }
}
