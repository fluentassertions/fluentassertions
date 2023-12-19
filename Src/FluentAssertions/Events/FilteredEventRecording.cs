using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertionsAsync.Events;

internal class FilteredEventRecording : IEventRecording
{
    private readonly OccurredEvent[] occurredEvents;

    public FilteredEventRecording(IEventRecording eventRecorder, IEnumerable<OccurredEvent> events)
    {
        EventObject = eventRecorder.EventObject;
        EventName = eventRecorder.EventName;
        EventHandlerType = eventRecorder.EventHandlerType;

        occurredEvents = events.ToArray();
    }

    public object EventObject { get; }

    public string EventName { get; }

    public Type EventHandlerType { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<OccurredEvent> GetEnumerator()
    {
        foreach (var occurredEvent in occurredEvents)
        {
            yield return new OccurredEvent
            {
                EventName = EventName,
                Parameters = occurredEvent.Parameters,
                TimestampUtc = occurredEvent.TimestampUtc
            };
        }
    }
}
