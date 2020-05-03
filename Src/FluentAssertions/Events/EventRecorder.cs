using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Records activity for a single event.
    /// </summary>
    [DebuggerNonUserCode]
    internal sealed class EventRecorder : IEventRecording, IDisposable
    {
        private readonly Func<DateTime> utcNow;
        private readonly BlockingCollection<RecordedEvent> raisedEvents = new BlockingCollection<RecordedEvent>();
        private readonly object lockable = new object();
        private Action cleanup;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRecorder"/> class.
        /// </summary>
        /// <param name="eventRaiser">The object events are recorded from</param>
        /// <param name="eventName">The name of the event that's recorded</param>
        /// <param name="utcNow">A delegate to get the current date and time in UTC format.</param>
        public EventRecorder(object eventRaiser, string eventName, Func<DateTime> utcNow)
        {
            this.utcNow = utcNow;
            EventObject = eventRaiser;
            EventName = eventName;
        }

        /// <summary>
        /// The object events are recorded from
        /// </summary>
        public object EventObject { get; private set; }

        /// <inheritdoc />
        public string EventName { get; }

        public Type EventHandlerType { get; private set; }

        public void Attach(WeakReference subject, EventInfo eventInfo)
        {
            EventHandlerType = eventInfo.EventHandlerType;

            Delegate handler = EventHandlerFactory.GenerateHandler(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(subject.Target, handler);

            cleanup = () =>
            {
                if (!(subject.Target is null))
                {
                    eventInfo.RemoveEventHandler(subject.Target, handler);
                }
            };
        }

        public void Dispose()
        {
            if (cleanup != null)
            {
                cleanup?.Invoke();
                cleanup = null;
                EventObject = null;
                raisedEvents.Dispose();
            }
        }

        /// <summary>
        /// Called by the auto-generated IL, to record information about a raised event.
        /// </summary>
        [UsedImplicitly]
        public void RecordEvent(params object[] parameters)
        {
            lock (lockable)
            {
                raisedEvents.Add(new RecordedEvent(utcNow(), parameters));
            }
        }

        /// <summary>
        /// Resets recorder to clear records of events raised so far.
        /// </summary>
        public void Reset()
        {
            lock (lockable)
            {
                while (raisedEvents.Count > 0)
                {
                    raisedEvents.TryTake(out _);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<OccurredEvent> GetEnumerator()
        {
            foreach (RecordedEvent @event in raisedEvents.ToArray())
            {
                yield return new OccurredEvent
                {
                    EventName = EventName,
                    Parameters = @event.Parameters,
                    TimestampUtc = @event.TimestampUtc
                };
            }
        }
    }
}
