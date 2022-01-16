using System.Threading;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Generates a thread-safe sequence.
    /// </summary>
    internal sealed class ThreadSafeSequenceGenerator
    {
        private int sequence = -1;

        /// <summary>
        /// Increments the current sequence.
        /// </summary>
        public int Increment()
        {
            return Interlocked.Increment(ref sequence);
        }
    }
}
