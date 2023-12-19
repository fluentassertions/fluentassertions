#if !NETSTANDARD2_0

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Events;

/// <summary>
/// Tracks the events an object raises.
/// </summary>
internal sealed class EventMonitor<T> : IMonitor<T>
{
    private readonly WeakReference subject;

    private readonly ConcurrentDictionary<string, EventRecorder> recorderMap = new();

    public EventMonitor(object eventSource, Func<DateTime> utcNow)
    {
        Guard.ThrowIfArgumentIsNull(eventSource, nameof(eventSource), "Cannot monitor the events of a <null> object.");

        subject = new WeakReference(eventSource);

        Attach(typeof(T), utcNow);
    }

    public T Subject => (T)subject.Target;

    private readonly ThreadSafeSequenceGenerator threadSafeSequenceGenerator = new();

    public EventMetadata[] MonitoredEvents
    {
        get
        {
            return recorderMap
                .Values
                .Select(recorder => new EventMetadata(recorder.EventName, recorder.EventHandlerType))
                .ToArray();
        }
    }

    public OccurredEvent[] OccurredEvents
    {
        get
        {
            IEnumerable<OccurredEvent> query =
                from eventName in recorderMap.Keys
                let recording = GetRecordingFor(eventName)
                from @event in recording
                orderby @event.Sequence
                select @event;

            return query.ToArray();
        }
    }

    public void Clear()
    {
        foreach (EventRecorder recorder in recorderMap.Values)
        {
            recorder.Reset();
        }
    }

    public EventAssertions<T> Should()
    {
        return new EventAssertions<T>(this);
    }

    public IEventRecording GetRecordingFor(string eventName)
    {
        if (!recorderMap.TryGetValue(eventName, out EventRecorder recorder))
        {
            throw new InvalidOperationException($"Not monitoring any events named \"{eventName}\".");
        }

        return recorder;
    }

    private void Attach(Type typeDefiningEventsToMonitor, Func<DateTime> utcNow)
    {
        if (subject.Target is null)
        {
            throw new InvalidOperationException("Cannot monitor events on garbage-collected object");
        }

        EventInfo[] events = GetPublicEvents(typeDefiningEventsToMonitor);

        if (events.Length == 0)
        {
            throw new InvalidOperationException($"Type {typeDefiningEventsToMonitor.Name} does not expose any events.");
        }

        foreach (EventInfo eventInfo in events)
        {
            AttachEventHandler(eventInfo, utcNow);
        }
    }

    private static EventInfo[] GetPublicEvents(Type type)
    {
        if (!type.IsInterface)
        {
            return type.GetEvents();
        }

        return new[] { type }
            .Concat(type.GetInterfaces())
            .SelectMany(i => i.GetEvents())
            .ToArray();
    }

    public void Dispose()
    {
        foreach (EventRecorder recorder in recorderMap.Values)
        {
            recorder.Dispose();
        }

        recorderMap.Clear();
    }

    private void AttachEventHandler(EventInfo eventInfo, Func<DateTime> utcNow)
    {
        if (!recorderMap.TryGetValue(eventInfo.Name, out _))
        {
            var recorder = new EventRecorder(subject.Target, eventInfo.Name, utcNow, threadSafeSequenceGenerator);

            if (recorderMap.TryAdd(eventInfo.Name, recorder))
            {
                recorder.Attach(subject, eventInfo);
            }
        }
    }
}

#endif
