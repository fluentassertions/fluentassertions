#if !NETSTANDARD1_3 && !NETSTANDARD1_6 && !NETSTANDARD2_0

using System;
using System.Reflection;

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

        /// <summary>
        /// Gets an object that tracks the occurrences of a particular <paramref name="eventName"/>.
        /// </summary>
        IEventRecorder GetEventRecorder(string eventName);

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

    /// <summary>
    /// Represents an occurrence of a particular event.
    /// </summary>
    public class OccurredEvent
    {
        /// <summary>
        /// The name of the event as defined on the monitored object.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// The parameters that were passed to the event handler.
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// The exact date and time of the occurrence in <see cref="DateTimeKind.Local"/>.
        /// </summary>
        public DateTime TimestampUtc { get; set; }
    }

    /// <summary>
    /// Provides the metadata of a monitored event.
    /// </summary>
    public class EventMetadata
    {
        /// <summary>
        /// The name of the event member on the monitored object
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// The type of the event handler and event args.
        /// </summary>
        public Type HandlerType { get; }

        public EventMetadata(string eventName, Type handlerType)
        {
            EventName = eventName;
            HandlerType = handlerType;
        }
    }
}

#endif
