using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.EventMonitoring
{
    /// <summary>
    ///   Provides extension methods for monitoring and querying events.
    /// </summary>
    public static class EventMonitoringExtensions
    {
        [ThreadStatic] private static readonly IDictionary<object, IList<IEventRecorder>> eventRecordersMap =
            new Dictionary<object, IList<IEventRecorder>>();

        /// <summary>
        ///   Starts monitoring events on an object.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown if eventSource is Null.</exception>
        public static IEnumerable<IEventRecorder> MonitorEvents(this object eventSource)
        {
            if (eventSource == null)
            {
                throw new NullReferenceException("Cannot monitor the events of a <null> object.");
            }

            var recorders =
                eventSource.GetType().GetEvents().Select(@event => MonitorEvents(eventSource, @event)).Cast
                    <IEventRecorder>().
                    ToList();

            if (!recorders.Any())
            {
                throw new InvalidOperationException(
                    string.Format("Object {0} does not expose any events.", eventSource));
            }

            if (eventRecordersMap.ContainsKey(eventSource))
            {
                eventRecordersMap.Remove(eventSource);
            }

            eventRecordersMap.Add(eventSource, recorders);

            return recorders;
        }

        /// <summary>
        ///   Creates an Event Handler for the EventInfo Passed to Hook it the Event Up to an Event Recorder.
        /// </summary>
        private static EventRecorder MonitorEvents(object eventSource, EventInfo eventInfo)
        {
            var eventRecorder = new EventRecorder(eventSource, eventInfo.Name);

            Delegate handler = EventHandlerFactory.GenerateHandler(eventInfo.EventHandlerType, eventRecorder);
            eventInfo.AddEventHandler(eventSource, handler);

            return eventRecorder;
        }

        public static IEnumerable<IEventRecorder> ShouldRaise(this object eventSource, string eventName)
        {
            return ShouldRaise(eventSource, eventName, "");
        }

        public static IEnumerable<IEventRecorder> ShouldRaise(
            this object eventSource, string eventName, string reason, params object[] reasonParameters)
        {
            if (!eventRecordersMap.ContainsKey(eventSource))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Object <{0}> is not being monitored for events. Use the MonitorEvents() extension method to start monitoring events.",
                        eventSource));
            }

            var eventRecorder = eventRecordersMap[eventSource].FirstOrDefault(r => r.EventName == eventName);
            if (eventRecorder == null)
            {
                throw new InvalidOperationException(string.Format(
                    "Object <{0}> does not expose an event named \"{1}\".", eventSource, eventName));
            }

            if (!eventRecorder.Any())
            {
                Verification.Fail("Expected object {1} to raise event {0}{2}, but it did not.", eventName, eventSource,
                    reason, reasonParameters);
            }

            return eventRecordersMap[eventSource];
        }
    }
}