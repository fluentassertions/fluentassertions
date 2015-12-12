using System;
using System.Linq;

namespace FluentAssertions.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventMonitor
    {
        #region Private Definitions

        [ThreadStatic]
        private static EventRecordersMap eventRecordersMap;

        public static EventRecordersMap Map
        {
            get
            {
                if (eventRecordersMap == null)
                {
                    eventRecordersMap = new EventRecordersMap();
                }

                return eventRecordersMap;
            }
        }

        #endregion

        public static void AddRecordersFor(object eventSource, Func<object, EventRecorder[]> recorderFactory)
        {
            if (eventSource == null)
            {
                throw new NullReferenceException("Cannot monitor the events of a <null> object.");
            }

            Map.Add(eventSource, recorderFactory(eventSource));
        }

        /// <summary>
        /// Obtains the <see cref="EventRecorder"/> for a particular event of the <paramref name="eventSource"/>.
        /// </summary>
        /// <param name="eventSource">The object for which to get an event recorder.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns></returns>
        public static EventRecorder GetRecorderForEvent<T>(this T eventSource, string eventName)
        {
            EventRecorder eventRecorder = Map[eventSource].FirstOrDefault(r => r.EventName == eventName);
            if (eventRecorder == null)
            {
                string name = eventSource.GetType().Name;

                throw new InvalidOperationException(String.Format(
                    "Type <{0}> does not expose an event named \"{1}\".", name, eventName));
            }

            return eventRecorder;
        }
    }
}