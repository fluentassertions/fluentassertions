using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.EventMonitoring
{
    /// <summary>
    ///   Provides extension methods for monitoring and querying events.
    /// </summary>
    public static class EventMonitoringExtensions
    {
        private static readonly WeakDictionary<object, EventRecorder[]> eventRecordersMap =
            new WeakDictionary<object, EventRecorder[]>();

        /// <summary>
        ///   Starts monitoring an object for its events.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown if eventSource is Null.</exception>
        public static IEnumerable<EventRecorder> MonitorEvents(this object eventSource)
        {
            if (eventSource == null)
            {
                throw new NullReferenceException("Cannot monitor the events of a <null> object.");
            }

            var recorders =
                eventSource.GetType().GetEvents().Select(@event => CreateEventHandler(eventSource, @event)).ToArray();

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

        private static EventRecorder CreateEventHandler(object eventSource, EventInfo eventInfo)
        {
            var eventRecorder = new EventRecorder(eventSource, eventInfo.Name);

            Delegate handler = EventHandlerFactory.GenerateHandler(eventInfo.EventHandlerType, eventRecorder);
            eventInfo.AddEventHandler(eventSource, handler);

            return eventRecorder;
        }


        /// <summary>
        /// Verifies that an object has raised a particular event at least once.
        /// </summary>
        /// <param name="eventName">
        /// The name of the event that should have been raised.
        /// </param>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static EventRecorder ShouldRaise(this object eventSource, string eventName)
        {
            return ShouldRaise(eventSource, eventName, "");
        }

        /// <summary>
        /// Verifies that an object has raised a particular event at least once.
        /// </summary>
        /// <param name="eventName">
        /// The name of the event that should have been raised.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static EventRecorder ShouldRaise(
            this object eventSource, string eventName, string reason, params object[] reasonParameters)
        {
            if (!eventRecordersMap.ContainsKey(eventSource))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Object <{0}> is not being monitored for events. Use the MonitorEvents() extension method to start monitoring events.",
                        eventSource));
            }

            EventRecorder eventRecorder = eventRecordersMap[eventSource].FirstOrDefault(r => r.EventName == eventName);
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

            return eventRecorder;
        }

        /// <summary>
        /// Verifies that an object has raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static IEventRecorder ShouldRaisePropertyChangeFor<T>(
            this T eventSource, Expression<Func<T, object>> propertyExpression)
        {
            return ShouldRaisePropertyChangeFor(eventSource, propertyExpression, "");
        }

        /// <summary>
        /// Verifies that an object has raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static IEventRecorder ShouldRaisePropertyChangeFor<T>(
            this T eventSource, Expression<Func<T, object>> propertyExpression,
            string reason, params object[] reasonParameters)
        {
            return ShouldRaise(eventSource, "PropertyChanged", reason, reasonParameters).WithArgs<PropertyChangedEventArgs>(
                    args => args.PropertyName == propertyExpression.GetPropertyInfo().Name);
        }

        /// <summary>
        /// Verifies that all occurences of the event originated from the <param name="expectedSender"/>.
        /// </summary>
        public static IEventRecorder WithSender(this IEventRecorder eventRecorder, object expectedSender)
        {
            foreach (RecordedEvent recordedEvent in eventRecorder)
            {
                if (recordedEvent.Parameters.Length == 0)
                {
                    throw new ArgumentException(string.Format(
                        "Expected event from sender <{0}>, but event {1} does not include any arguments",
                        expectedSender, eventRecorder.EventName));
                }

                object actualSender = recordedEvent.Parameters.First();
                Verification.Verify(ReferenceEquals(actualSender, expectedSender),
                    "Expected sender {0}, but found {1}.", expectedSender, actualSender, "");
            }

            return eventRecorder;
        }

        /// <summary>
        /// Verifies that at least one occurrence of the event had an <see cref="EventArgs"/> object matching a predicate.
        /// </summary>
        public static IEventRecorder WithArgs<T>(this IEventRecorder eventRecorder, Expression<Func<T, bool>> predicate) where T : EventArgs
        {
            Func<T, bool> compiledPredicate = predicate.Compile();

            if (eventRecorder.First().Parameters.OfType<T>().Count() == 0)
            {
                throw new ArgumentException("No argument of event " + eventRecorder.EventName + " is of type <" + typeof(T) + ">.");
            }

            if (!eventRecorder.Any(@event => compiledPredicate(@event.Parameters.OfType<T>().Single())))
            {
                Verification.Fail(
                    "Expected at least one event with arguments matching {0}, but found none.", 
                    predicate.Body, null, "", null);
            }

            return eventRecorder;
        }
    }
}