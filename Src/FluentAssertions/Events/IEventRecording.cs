using System;
using System.Collections.Generic;

namespace FluentAssertionsAsync.Events;

/// <summary>
/// Represents an (active) recording of all events that happen(ed) while monitoring an object.
/// </summary>
public interface IEventRecording : IEnumerable<OccurredEvent>
{
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
