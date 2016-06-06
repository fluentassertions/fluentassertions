namespace FluentAssertions.Events
{
    /// <summary>
    /// Monitors events on a given source
    /// </summary>
    public interface IEventMonitor
    {
        /// <summary>
        /// Resets monitor to clear records of events raised so far.
        /// </summary>
        void Reset();
    }
}