using System.Threading;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Tracks the order of events raised by <see cref="FluentAssertions.Events.EventMonitor{T}"/>.
    /// </summary>
    internal sealed class EventRaisedOrder
    {
        private int orderIndex = -1;

        /// <summary>
        /// Increments the current order index.
        /// </summary>
        public int Increment()
        {
            return Interlocked.Increment(ref orderIndex);
        }
    }
}
