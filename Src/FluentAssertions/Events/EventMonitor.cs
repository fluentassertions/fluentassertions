#if !NETSTANDARD2_0

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Events;

/// <summary>
/// Tracks the events an object raises.
/// </summary>
internal sealed class EventMonitor<T> : IMonitor<T>
{
    private readonly WeakReference subject;

    private readonly ConcurrentDictionary<string, EventRecorder> recorderMap = new(StringComparer.Ordinal);

    public EventMonitor(object eventSource, EventMonitorOptions options)
    {
        Guard.ThrowIfArgumentIsNull(eventSource, nameof(eventSource), "Cannot monitor the events of a <null> object.");
        Guard.ThrowIfArgumentIsNull(options, nameof(options), "Event monitor needs configuration.");

        this.options = options;

        subject = new WeakReference(eventSource);

        Attach(typeof(T), this.options.TimestampProvider);
    }

    public T Subject => (T)subject.Target;

    private readonly ThreadSafeSequenceGenerator threadSafeSequenceGenerator = new();
    private readonly EventMonitorOptions options;

    public EventMetadata[] MonitoredEvents =>
        recorderMap
            .Values
            .Select(recorder => new EventMetadata(recorder.EventName, recorder.EventHandlerType))
            .ToArray();

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
        return new EventAssertions<T>(this, AssertionChain.GetOrCreate());
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
            DisposeSafeIfRequested(recorder);
        }

        recorderMap.Clear();
    }

    private void DisposeSafeIfRequested(IDisposable recorder)
    {
        try
        {
            recorder.Dispose();
        }
        catch when (options.ShouldIgnoreEventAccessorExceptions)
        {
            // ignore
        }
    }

    private void AttachEventHandler(EventInfo eventInfo, Func<DateTime> utcNow)
    {
        if (!recorderMap.TryGetValue(eventInfo.Name, out _))
        {
            var recorder = new EventRecorder(subject.Target, eventInfo.Name, utcNow, threadSafeSequenceGenerator);

            if (recorderMap.TryAdd(eventInfo.Name, recorder))
            {
                AttachEventHandler(eventInfo, recorder);
            }
        }
    }

    private void AttachEventHandler(EventInfo eventInfo, EventRecorder recorder)
    {
        try
        {
            recorder.Attach(subject, eventInfo);
        }
        catch when (options.ShouldIgnoreEventAccessorExceptions)
        {
            if (!options.ShouldRecordEventsWithBrokenAccessor)
            {
                recorderMap.TryRemove(eventInfo.Name, out _);
            }
        }
    }
}

#endif
