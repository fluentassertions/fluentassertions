using System;
using System.Collections.Generic;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Records raised events for one event on one object
    /// </summary>
    public interface IEventRecorder : IEnumerable<RecordedEvent>, IDisposable
    {
        /// <summary>
        /// Store information about a raised event
        /// </summary>
        /// <param name = "parameters">Parameters the event was raised with</param>
        void RecordEvent(params object[] parameters);

        /// <summary>
        /// Resets the event recorder, removing any events recorded thus far.
        /// </summary>
        void Reset();

        /// <summary>
        /// The object events are recorded from
        /// </summary>
        object EventObject { get; }

        /// <summary>
        /// The name of the event that's recorded
        /// </summary>
        string EventName { get; }

        /// <summary>
        /// The type of the event handler identified by <see cref="EventName"/>.
        /// </summary>
        Type EventHandlerType { get; }
    }
}
