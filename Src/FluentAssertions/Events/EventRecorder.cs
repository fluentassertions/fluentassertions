#if !NETSTANDARD1_3 && !NETSTANDARD1_6 && !NETSTANDARD2_0

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Records activity for a single event.
    /// </summary>
    [DebuggerNonUserCode]
    public class EventRecorder : IEventRecorder
    {
        private readonly Func<DateTime> utcNow;
        private readonly BlockingCollection<RecordedEvent> raisedEvents = new BlockingCollection<RecordedEvent>();
        private readonly object lockable = new object();
        private WeakReference eventObject;
        private Action cleanup;

        /// <summary>
        /// </summary>
        /// <param name = "eventRaiser">The object events are recorded from</param>
        /// <param name = "eventName">The name of the event that's recorded</param>
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
        public object EventObject
        {
            get => eventObject?.Target;
            private set => eventObject = new WeakReference(value);
        }

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
                eventObject = null;
            }
        }

        /// <summary>
        /// Enumerate raised events
        /// </summary>
        public IEnumerator<RecordedEvent> GetEnumerator()
        {
            lock (lockable)
            {
                return raisedEvents.ToList().GetEnumerator();
            }
        }

        /// <summary>
        /// Enumerate raised events
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (lockable)
            {
                return raisedEvents.ToList().GetEnumerator();
            }
        }

        /// <summary>
        /// Called by the auto-generated IL, to record information about a raised event.
        /// </summary>
        public void RecordEvent(params object[] parameters)
        {
            lock (lockable)
            {
                raisedEvents.Add(new RecordedEvent(utcNow(), EventObject, parameters));
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
    }
}

#endif
