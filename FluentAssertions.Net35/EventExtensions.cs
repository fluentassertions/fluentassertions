using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssertions.Events;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    /// <summary>
    ///   Provides extension methods for monitoring and querying events.
    /// </summary>
    public static class EventExtensions
    {
        private static readonly EventRecordersMap eventRecordersMap = new EventRecordersMap();

        public static IEnumerable<EventRecorder> MonitorEventsRaisedBy(object eventSource)
        {
            if (eventSource == null)
            {
                throw new NullReferenceException("Cannot monitor the events of a <null> object.");
            }

            EventRecorder[] recorders = BuildRecorders(eventSource);

            eventRecordersMap.Add(eventSource, recorders);

            return recorders;
        }

        private static EventRecorder[] BuildRecorders(object eventSource)
        {
            var recorders =
                eventSource.GetType()
                    .GetEvents()
                    .Select(@event => CreateEventHandler(eventSource, @event)).ToArray();

            if (!recorders.Any())
            {
                throw new InvalidOperationException(
                    string.Format("Type {0} does not expose any events.", eventSource.GetType().Name));
            }

            return recorders;
        }

        private static EventRecorder CreateEventHandler(object eventSource, EventInfo eventInfo)
        {
            var eventRecorder = new EventRecorder(eventSource, eventInfo.Name);

            Delegate handler = EventHandlerFactory.GenerateHandler(eventInfo.EventHandlerType, eventRecorder);
            eventInfo.AddEventHandler(eventSource, handler);

            return eventRecorder;
        }

        /// <summary>
        /// Asserts that an object has raised a particular event at least once.
        /// </summary>
        /// <param name="eventSource">The object exposing the event.</param>
        /// <param name="eventName">The name of the event that should have been raised.</param>
        /// <returns></returns>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static EventRecorder ShouldRaise(this object eventSource, string eventName)
        {
            return ShouldRaise(eventSource, eventName, string.Empty);
        }

        /// <summary>
        /// Asserts that an object has raised a particular event at least once.
        /// </summary>
        /// <param name="eventSource">The object exposing the event.</param>
        /// <param name="eventName">
        /// The name of the event that should have been raised.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static EventRecorder ShouldRaise(
            this object eventSource, string eventName, string reason, params object[] reasonArgs)
        {
            EventRecorder eventRecorder = eventSource.GetRecorderForEvent(eventName);

            if (!eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected object {0} to raise event {1}{reason}, but it did not.", eventSource, eventName);
            }

            return eventRecorder;
        }

        /// <summary>
        /// Asserts that an object has not raised a particular event.
        /// </summary>
        /// <param name="eventSource">The object exposing the event.</param>
        /// <param name="eventName">
        /// The name of the event that should not be raised.
        /// </param>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static void ShouldNotRaise(this object eventSource, string eventName)
        {
            ShouldNotRaise(eventSource, eventName, string.Empty);
        }

        /// <summary>
        /// Asserts that an object has not raised a particular event.
        /// </summary>
        /// <param name="eventSource">The object exposing the event.</param>
        /// <param name="eventName">
        /// The name of the event that should not be raised.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static void ShouldNotRaise(
            this object eventSource, string eventName, string reason, params object[] reasonArgs)
        {
            EventRecorder eventRecorder = eventRecordersMap[eventSource].FirstOrDefault(r => r.EventName == eventName);
            if (eventRecorder == null)
            {
                string typeName = null;
                typeName = eventSource.GetType().Name;
                throw new InvalidOperationException(string.Format(
                    "Type <{0}> does not expose an event named \"{1}\".", typeName, eventName));
            }

            if (eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected object {0} to not raise event {1}{reason}, but it did.", eventSource, eventName);
            }
        }
    }
}