using System;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Monitors events on a given source
    /// </summary>
    public interface IEventMonitor
    {
        /// <summary>
        /// Attaches event monitoring for the events defined by <paramref name="typeDefiningEventsToMonitor"/>.
        /// </summary>
        /// <param name="typeDefiningEventsToMonitor">A type implemented by the monitored object</param>
        void Attach(Type typeDefiningEventsToMonitor);

        /// <summary>
        /// Gets the <see cref="IEventRecorder"/> for the event with the given name.
        /// </summary>
        /// <param name="eventName">The name of the event for which the <see cref="IEventRecorder"/> is required.</param>
        /// <returns>The <see cref="IEventRecorder"/> for the event with the given name, if it exists; otherwise, null.</returns>
        IEventRecorder GetEventRecorder(string eventName);

        /// <summary>
        /// Resets monitor to clear records of events raised so far.
        /// </summary>
        void Reset();
    }
}