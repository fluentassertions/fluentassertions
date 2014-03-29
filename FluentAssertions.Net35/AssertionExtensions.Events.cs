using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Events;
using FluentAssertions.Execution;


namespace FluentAssertions
{
    /// <summary>
    ///   Provides extension methods for monitoring and querying events.
    /// </summary>
    public static partial class AssertionExtensions
    {
        private const string PropertyChangedEventName = "PropertyChanged";

        private static readonly EventRecordersMap eventRecordersMap = new EventRecordersMap();

#if !SILVERLIGHT && !WINRT
        /// <summary>
        ///   Starts monitoring an object for its events.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown if eventSource is Null.</exception>
        public static IEnumerable<EventRecorder> MonitorEvents(this object eventSource)
        {
            return MonitorEventsRaisedBy(eventSource);
        }
#else
        /// <summary>
        ///   Starts monitoring an object for its <see cref="INotifyPropertyChanged.PropertyChanged"/> events.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown if eventSource is Null.</exception>
        public static IEnumerable<EventRecorder> MonitorEvents(this INotifyPropertyChanged eventSource)
        {
            return MonitorEventsRaisedBy(eventSource);
        }
#endif

        private static IEnumerable<EventRecorder> MonitorEventsRaisedBy(object eventSource)
        {
            if (eventSource == null)
            {
                throw new NullReferenceException("Cannot monitor the events of a <null> object.");
            }

            EventRecorder[] recorders = BuildRecorders(eventSource);

            eventRecordersMap.Add(eventSource, recorders);

            return recorders;
        }

#if !SILVERLIGHT && !WINRT && !__IOS__
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
#else
        private static EventRecorder[] BuildRecorders(object eventSource)
        {
            var eventRecorder = new EventRecorder(eventSource, PropertyChangedEventName);

            ((INotifyPropertyChanged)eventSource).PropertyChanged += (sender, args) => eventRecorder.RecordEvent(sender, args);
            return new[] {eventRecorder};
        }
#endif

#if !SILVERLIGHT && !WINRT
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

#endif
        
        /// <summary>
        /// Asserts that an object has raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static IEventRecorder ShouldRaisePropertyChangeFor<T>(
            this T eventSource, Expression<Func<T, object>> propertyExpression)
        {
            return ShouldRaisePropertyChangeFor(eventSource, propertyExpression, string.Empty);
        }

        /// <summary>
        /// Asserts that an object has raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="eventSource">The object exposing the event.</param>
        /// <param name="propertyExpression">
        /// A lambda expression referring to the property for which the property changed event should have been raised, or
        /// <c>null</c> to refer to all properties.
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
        public static IEventRecorder ShouldRaisePropertyChangeFor<T>(
            this T eventSource, Expression<Func<T, object>> propertyExpression,
            string reason, params object[] reasonArgs)
        {
            EventRecorder eventRecorder = eventSource.GetRecorderForEvent(PropertyChangedEventName);
            string propertyName = (propertyExpression != null) ? propertyExpression.GetPropertyInfo().Name : null;

            if (!eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected object {0} to raise event {1} for property {2}{reason}, but it did not.",
                    eventSource, PropertyChangedEventName, propertyName);
            }

            return eventRecorder.WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == propertyName);
        }
        
        /// <summary>
        /// Asserts that an object has not raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static void ShouldNotRaisePropertyChangeFor<T>(
            this T eventSource, Expression<Func<T, object>> propertyExpression)
        {
            ShouldNotRaisePropertyChangeFor(eventSource, propertyExpression, string.Empty);
        }

        /// <summary>
        /// Asserts that an object has not raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="eventSource">The object exposing the event.</param>
        /// <param name="propertyExpression">
        /// A lambda expression referring to the property for which the property changed event should have been raised.
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
        public static void ShouldNotRaisePropertyChangeFor<T>(
            this T eventSource, Expression<Func<T, object>> propertyExpression,
            string reason, params object[] reasonArgs)
        {
            EventRecorder eventRecorder = eventSource.GetRecorderForEvent(PropertyChangedEventName);

            string propertyName = propertyExpression.GetPropertyInfo().Name;

            if (eventRecorder.Any(@event => GetAffectedPropertyName(@event) == propertyName))
            {
                Execute.Assertion
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Did not expect object {0} to raise the {1} event for property {2}{reason}, but it did.",
                        eventSource, PropertyChangedEventName, propertyName);
            }
        }

        /// <summary>
        /// Obtains the <see cref="EventRecorder"/> for a particular event of the <paramref name="eventSource"/>.
        /// </summary>
        /// <param name="eventSource">The object for which to get an event recorder.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns></returns>
        public static EventRecorder GetRecorderForEvent<T>(this T eventSource, string eventName)
        {
            EventRecorder eventRecorder = eventRecordersMap[eventSource].FirstOrDefault(r => r.EventName == eventName);
            if (eventRecorder == null)
            {
                string name;
#if !WINRT
                name = eventSource.GetType().Name;
#else
                name = eventSource.GetType().GetTypeInfo().Name;
#endif
                throw new InvalidOperationException(string.Format(
                    "Type <{0}> does not expose an event named \"{1}\".", name, eventName));
            }

            return eventRecorder;
        }

        private static string GetAffectedPropertyName(RecordedEvent @event)
        {
            return @event.Parameters.OfType<PropertyChangedEventArgs>().Single().PropertyName;
        }

        /// <summary>
        /// Asserts that all occurences of the event originated from the <param name="expectedSender"/>.
        /// </summary>
        public static IEventRecorder WithSender(this IEventRecorder eventRecorder, object expectedSender)
        {
            foreach (RecordedEvent recordedEvent in eventRecorder)
            {
                if (!recordedEvent.Parameters.Any())
                {
                    throw new ArgumentException(string.Format(
                        "Expected event from sender <{0}>, but event {1} does not include any arguments",
                        expectedSender, eventRecorder.EventName));
                }

                object actualSender = recordedEvent.Parameters.First();
                Execute.Assertion
                    .ForCondition(ReferenceEquals(actualSender, expectedSender))
                    .FailWith("Expected sender {0}, but found {1}.", expectedSender, actualSender);
            }

            return eventRecorder;
        }

        /// <summary>
        /// Asserts that at least one occurrence of the event had an <see cref="EventArgs"/> object matching a predicate.
        /// </summary>
        public static IEventRecorder WithArgs<T>(this IEventRecorder eventRecorder, Expression<Func<T, bool>> predicate) where T : EventArgs
        {
            Func<T, bool> compiledPredicate = predicate.Compile();

            if (!eventRecorder.First().Parameters.OfType<T>().Any())
            {
                throw new ArgumentException("No argument of event " + eventRecorder.EventName + " is of type <" + typeof(T) + ">.");
            }

            if (!eventRecorder.Any(@event => compiledPredicate(@event.Parameters.OfType<T>().Single())))
            {
                Execute.Assertion
                    .FailWith("Expected at least one event with arguments matching {0}, but found none.", predicate.Body);
            }

            return eventRecorder;
        }
    }
}