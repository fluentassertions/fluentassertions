using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssertions.EventMonitoring
{
    /// <summary>
    ///   Records activity for a single event.
    /// </summary>
    [DebuggerNonUserCode]
    public class EventRecorder : IEventRecorder
    {
        private readonly IList<RecordedEvent> raisedEvents = new List<RecordedEvent>();
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
            return raisedEvents.GetEnumerator();
        }

        /// <summary>
        ///   Enumerate raised events
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return raisedEvents.GetEnumerator();
        }

        /// <summary>
        ///   Called by the auto-generated IL, to record information about a raised event.
        /// </summary>
        public void RecordEvent(params object [] parameters)
        {
            raisedEvents.Add(new RecordedEvent(EventObject, parameters));
        }
    }
}