#if !NETSTANDARD1_3 && !NETSTANDARD1_6 && !NETSTANDARD2_0

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Events
{
    /// <summary>
    /// Provides convenient assertion methods on a <see cref="IMonitor{T}"/> that can be
    /// used to assert that certain events have been raised.
    /// </summary>
    public class EventAssertions<T> : ReferenceTypeAssertions<T, EventAssertions<T>>
    {
        private readonly IMonitor<T> monitor;
        private const string PropertyChangedEventName = "PropertyChanged";

        protected internal EventAssertions(IMonitor<T> monitor) : base(monitor.Subject)
        {
            this.monitor = monitor;
        }

        /// <summary>
        /// Asserts that an object has raised a particular event at least once.
        /// </summary>
        /// <param name="eventName">
        /// The name of the event that should have been raised.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="AssertionExtensions.Monitor"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public IEventRecorder Raise(string eventName, string because = "", params object[] becauseArgs)
        {
            IEventRecorder eventRecorder = monitor.GetEventRecorder(eventName);

            if (!eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected object {0} to raise event {1}{reason}, but it did not.", monitor.Subject, eventName);
            }

            return eventRecorder;
        }

        /// <summary>
        /// Asserts that an object has not raised a particular event.
        /// </summary>
        /// <param name="eventName">
        /// The name of the event that should not be raised.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public void NotRaise(string eventName, string because = "", params object[] becauseArgs)
        {
            IEventRecorder eventRecorder = monitor.GetEventRecorder(eventName);
            if (eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected object {0} to not raise event {1}{reason}, but it did.", monitor.Subject, eventName);
            }
        }

        /// <summary>
        /// Asserts that an object has raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="propertyExpression">
        /// A lambda expression referring to the property for which the property changed event should have been raised, or
        /// <c>null</c> to refer to all properties.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="AssertionExtensions.Monitor"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public IEventRecorder RaisePropertyChangeFor(Expression<Func<T, object>> propertyExpression,
            string because = "", params object[] becauseArgs)
        {
            IEventRecorder eventRecorder = monitor.GetEventRecorder(PropertyChangedEventName);
            string propertyName = propertyExpression?.GetPropertyInfo().Name;

            if (!eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected object {0} to raise event {1} for property {2}{reason}, but it did not.",
                        monitor.Subject, PropertyChangedEventName, propertyName);
            }

            return eventRecorder.WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == propertyName);
        }

        /// <summary>
        /// Asserts that an object has not raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <param name="propertyExpression">
        /// A lambda expression referring to the property for which the property changed event should have been raised.
        /// </param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="AssertionExtensions.Monitor"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public void NotRaisePropertyChangeFor(Expression<Func<T, object>> propertyExpression,
            string because = "", params object[] becauseArgs)
        {
            IEventRecorder eventRecorder = monitor.GetEventRecorder(PropertyChangedEventName);

            string propertyName = propertyExpression.GetPropertyInfo().Name;

            if (eventRecorder.Any(@event => GetAffectedPropertyName(@event) == propertyName))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect object {0} to raise the {1} event for property {2}{reason}, but it did.",
                        monitor.Subject, PropertyChangedEventName, propertyName);
            }
        }

        private static string GetAffectedPropertyName(RecordedEvent @event)
        {
            return @event.Parameters.OfType<PropertyChangedEventArgs>().Single().PropertyName;
        }

        protected override string Identifier => "subject";
    }
}

#endif
