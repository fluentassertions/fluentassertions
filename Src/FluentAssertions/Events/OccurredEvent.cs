using System;

namespace FluentAssertionsAsync.Events;

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

    /// <summary>
    /// The order in which this event was raised on the monitored object.
    /// </summary>
    public int Sequence { get; set; }
}
