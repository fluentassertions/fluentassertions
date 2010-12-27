using System.Collections;
using System.Collections.Generic;

namespace FluentAssertions.EventMonitoring
{
    /// <summary>
    ///   Records activity for a single event.
    /// </summary>
    internal class EventRecorder : IEventRecorder
    {
        private readonly IList<RecordedEvent> raisedEvents = new List<RecordedEvent>();

        /// <summary>
        ///   The object events are recorded from
        /// </summary>
        public object EventObject { get; private set; }

        /// <summary>
        ///   The name of the event that's recorded
        /// </summary>
        public string EventName { get; private set; }

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
        public void RecordEvent(params object[] parameters)
        {
            raisedEvents.Add(new RecordedEvent(parameters));
        }
    }
}