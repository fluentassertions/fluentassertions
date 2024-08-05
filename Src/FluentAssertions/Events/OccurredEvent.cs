using System;
using System.ComponentModel;
using System.Linq;

namespace FluentAssertions.Events;

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

    /// <summary>
    /// Verifies if a property changed event is affecting a particular property.
    /// </summary>
    /// <param name="propertyName">
    /// The property name for which the property changed event should have been raised.
    /// </param>
    /// <returns>
    /// Returns <see langword="true"/> if the event is affecting the property specified, <see langword="false"/> otherwise.
    /// </returns>
    internal bool IsAffectingPropertyName(string propertyName)
    {
        return Parameters.OfType<PropertyChangedEventArgs>()
                         .Any(e => string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == propertyName);
    }
}
