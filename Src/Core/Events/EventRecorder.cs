using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Events
{
    /// <summary>
    ///   Records activity for a single event.
    /// </summary>
    [DebuggerNonUserCode]
    public class EventRecorder : IEventRecorder
    {
        private readonly IList<RecordedEvent> raisedEvents = new List<RecordedEvent>();
        private readonly object lockable = new object();
        private WeakReference eventObject;

        /// <summary>
        /// </summary>
        /// <param name = "eventRaiser">The object events are recorded from</param>
        /// <param name = "eventName">The name of the event that's recorded</param>
        public EventRecorder(object eventRaiser, string eventName)
        {
            EventObject = eventRaiser;
            EventName = eventName;
        }

        /// <summary>
        ///   The object events are recorded from
        /// </summary>
        public object EventObject
        {
            get { return (eventObject == null) ? null : eventObject.Target; }
            private set { eventObject = new WeakReference(value); }
        }

        /// <summary>
        ///   The name of the event that's recorded
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        ///   Enumerate raised events
        /// </summary>
        public IEnumerator<RecordedEvent> GetEnumerator()
        {
            lock (lockable)
            {
                return raisedEvents.ToList().GetEnumerator();
            }
        }

        /// <summary>
        ///   Enumerate raised events
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
        ///   Called by the auto-generated IL, to record information about a raised event.
        /// </summary>
        public void RecordEvent(params object [] parameters)
        {
            lock (lockable)
            {
                raisedEvents.Add(new RecordedEvent(EventObject, parameters));
            }
        }

        /// <summary>
        ///   Resets recorder to clear records of events raised so far.
        /// </summary>
        public void Reset()
        {
            lock ( lockable )
            {
                raisedEvents.Clear();
            }
        }
    }
}