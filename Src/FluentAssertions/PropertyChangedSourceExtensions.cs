using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Events;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    /// <summary>
    ///   Provides extension methods for monitoring and querying events.
    /// </summary>
    public static class PropertyChangedSourceExtensions
    {
        private const string PropertyChangedEventName = "PropertyChanged";

        /// <summary>
        /// Asserts that an object has raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <remarks>
        /// You must call <see cref="AssertionExtensions.MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
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
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="AssertionExtensions.MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static IEventRecorder ShouldRaisePropertyChangeFor<T>(
            this T eventSource, Expression<Func<T, object>> propertyExpression,
            string because, params object[] becauseArgs)
        {
            IEventRecorder eventRecorder = EventMonitor.Get(eventSource).GetEventRecorder(PropertyChangedEventName);
            string propertyName = (propertyExpression != null) ? propertyExpression.GetPropertyInfo().Name : null;

            if (!eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected object {0} to raise event {1} for property {2}{reason}, but it did not.",
                        eventSource, PropertyChangedEventName, propertyName);
            }

            return eventRecorder.WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == propertyName);
        }

        /// <summary>
        /// Asserts that an object has not raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
        /// </summary>
        /// <remarks>
        /// You must call <see cref="AssertionExtensions.MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
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
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <remarks>
        /// You must call <see cref="AssertionExtensions.MonitorEvents"/> on the same object prior to this call so that Fluent Assertions can
        /// subscribe for the events of the object.
        /// </remarks>
        public static void ShouldNotRaisePropertyChangeFor<T>(
            this T eventSource, Expression<Func<T, object>> propertyExpression,
            string because, params object[] becauseArgs)
        {
            IEventRecorder eventRecorder = EventMonitor.Get(eventSource).GetEventRecorder(PropertyChangedEventName);

            string propertyName = propertyExpression.GetPropertyInfo().Name;

            if (eventRecorder.Any(@event => GetAffectedPropertyName(@event) == propertyName))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect object {0} to raise the {1} event for property {2}{reason}, but it did.",
                        eventSource, PropertyChangedEventName, propertyName);
            }
        }

        private static string GetAffectedPropertyName(RecordedEvent @event)
        {
            return @event.Parameters.OfType<PropertyChangedEventArgs>().Single().PropertyName;
        }

        /// <summary>
        /// Asserts that all occurrences of the event originated from the <param name="expectedSender"/>.
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
        /// Asserts that at least one occurrence of the event had at least one of the arguments matching a predicate.
        /// </summary>
        public static IEventRecorder WithArgs<T>(this IEventRecorder eventRecorder, Expression<Func<T, bool>> predicate)
        {
            Func<T, bool> compiledPredicate = predicate.Compile();

            if (!eventRecorder.First().Parameters.OfType<T>().Any())
            {
                throw new ArgumentException("No argument of event " + eventRecorder.EventName + " is of type <" + typeof(T) + ">.");
            }

            if (eventRecorder.All(recordedEvent => !recordedEvent.Parameters.OfType<T>().Any(parameter => compiledPredicate(parameter))))
            {
                Execute.Assertion
                    .FailWith("Expected at least one event with arguments matching {0}, but found none.", predicate.Body);
            }

            return eventRecorder;
        }

        /// <summary>
        /// Asserts that at least one occurrence of the event had arguments matching all predicates.
        /// </summary>
        public static IEventRecorder WithArgs<T>(this IEventRecorder eventRecorder, params Expression<Func<T, bool>>[] predicates)
        {
            Func<T, bool>[] compiledPredicates = predicates.Select(p => p?.Compile()).ToArray();

            if (!eventRecorder.First().Parameters.OfType<T>().Any())
            {
                throw new ArgumentException("No argument of event " + eventRecorder.EventName + " is of type <" + typeof(T) + ">.");
            }

            bool expected = eventRecorder.Any(recordedEvent =>
            {
                T[] parameters = recordedEvent.Parameters.OfType<T>().ToArray();
                int parametersToCheck = Math.Min(parameters.Length, predicates.Length);

                bool isMatch = true;
                for (int i = 0; i < parametersToCheck && isMatch; i++)
                {
                    isMatch = compiledPredicates[i]?.Invoke(parameters[i]) ?? true;
                }

                return isMatch;
            });

            if (!expected)
            {
                Execute.Assertion
                    .FailWith("Expected at least one event with arguments matching {0}, but found none.", string.Join(" | ", predicates.Where(p => p != null).Select(p => p.Body.ToString())));
            }

            return eventRecorder;
        }
    }
}