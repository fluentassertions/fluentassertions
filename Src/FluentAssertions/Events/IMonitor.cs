using System;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Monitors events on a given source
    /// </summary>
    public interface IMonitor<T> : IDisposable
    {
        /// <summary>
        /// Gets the object that is being monitored or <c>null</c> if the object has been GCed.
        /// </summary>
        T Subject { get; }

        /// <summary>
        /// Clears all recorded events from the monitor and continues monitoring.
        /// </summary>
        void Clear();

        /// <summary>
        /// Provides access to several assertion methods.
        /// </summary>
        EventAssertions<T> Should();

        IEventRecording GetRecordingFor(string eventName);

        /// <summary>
        /// Gets the metadata of all the events that are currently being monitored.
        /// </summary>
        EventMetadata[] MonitoredEvents { get; }

        /// <summary>
        /// Gets a collection of all events that have occurred since the monitor was created or
        /// <see cref="Clear"/> was called.
        /// </summary>
        OccurredEvent[] OccurredEvents { get; }
    }
}
