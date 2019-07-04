

using FluentAssertions.Localization;
#if !NETSTANDARD1_3 && !NETSTANDARD1_6 && !NETSTANDARD2_0
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Tracks the events an object raises.
    /// </summary>
    internal class EventMonitor<T> : IMonitor<T>
    {
        private readonly WeakReference subject;

        private readonly ConcurrentDictionary<string, IEventRecorder> recorderMap =
            new ConcurrentDictionary<string, IEventRecorder>();

        public EventMonitor(object eventSource, Func<DateTime> utcNow)
        {
            if (eventSource is null)
            {
                throw new ArgumentNullException(nameof(eventSource), Resources.Event_CannotMonitorEventsOfNullObject);
            }

            subject = new WeakReference(eventSource);

            Attach(typeof(T), utcNow);
        }

        public T Subject => (T)subject.Target;

        public EventMetadata[] MonitoredEvents
        {
            get
            {
                return recorderMap.ToArray()
                    .Select(r => new EventMetadata(r.Value.EventName, r.Value.EventHandlerType))
                    .ToArray();
            }
        }

        public OccurredEvent[] OccurredEvents
        {
            get
            {
                var query =
                    from mapItem in recorderMap.ToArray()
                    let eventName = mapItem.Key
                    let recorder = mapItem.Value
                    from occurrence in mapItem.Value
                    orderby occurrence.TimestampUtc
                    select new OccurredEvent
                    {
                        EventName = eventName,
                        Parameters = occurrence.Parameters.ToArray(),
                        TimestampUtc = occurrence.TimestampUtc
                    };

                return query.ToArray();
            }
        }

        public void Clear()
        {
            foreach (IEventRecorder recorder in recorderMap.Values)
            {
                recorder.Reset();
            }
        }

        public EventAssertions<T> Should()
        {
            return new EventAssertions<T>(this);
        }

        private void Attach(Type typeDefiningEventsToMonitor, Func<DateTime> utcNow)
        {
            if (subject.Target is null)
            {
                throw new InvalidOperationException(Resources.Event_CannotMonitorEventsOnGarbageCollectedObject);
            }

            EventInfo[] events = GetPublicEvents(typeDefiningEventsToMonitor);
            if (!events.Any())
            {
                throw new InvalidOperationException(string.Format(Resources.Event_TypeX0DoesNotExposeAnyEventFormat, typeDefiningEventsToMonitor.Name));
            }

            foreach (var eventInfo in events)
            {
                AttachEventHandler(eventInfo, utcNow);
            }
        }

        private EventInfo[] GetPublicEvents(Type type)
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
            foreach (IEventRecorder recorder in recorderMap.Values)
            {
                recorder.Dispose();
            }

            recorderMap.Clear();
        }

        private void AttachEventHandler(EventInfo eventInfo, Func<DateTime> utcNow)
        {
            if (!recorderMap.TryGetValue(eventInfo.Name, out _))
            {
                var recorder = new EventRecorder(subject.Target, eventInfo.Name, utcNow);
                if (recorderMap.TryAdd(eventInfo.Name, recorder))
                {
                    recorder.Attach(subject, eventInfo);
                }
            }
        }

        public IEventRecorder GetEventRecorder(string eventName)
        {
            if (!recorderMap.TryGetValue(eventName, out IEventRecorder recorder))
            {
                throw new InvalidOperationException(string.Format(Resources.Event_NotMonitoringAnyEventsNamedX0Format, eventName));
            }

            return recorder;
        }
    }
}

#endif
