using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Primitives;

namespace FluentAssertionsAsync.Events;

/// <summary>
/// Provides convenient assertion methods on a <see cref="IMonitor{T}"/> that can be
/// used to assert that certain events have been raised.
/// </summary>
public class EventAssertions<T> : ReferenceTypeAssertions<T, EventAssertions<T>>
{
    private const string PropertyChangedEventName = "PropertyChanged";

    protected internal EventAssertions(IMonitor<T> monitor)
        : base(monitor.Subject)
    {
        Monitor = monitor;
    }

    /// <summary>
    /// Gets the <see cref="IMonitor{T}"/> which is being asserted.
    /// </summary>
    public IMonitor<T> Monitor { get; }

    /// <summary>
    /// Asserts that an object has raised a particular event at least once.
    /// </summary>
    /// <param name="eventName">
    /// The name of the event that should have been raised.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public IEventRecording Raise(string eventName, string because = "", params object[] becauseArgs)
    {
        IEventRecording recording = Monitor.GetRecordingFor(eventName);

        if (!recording.Any())
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected object {0} to raise event {1}{reason}, but it did not.", Monitor.Subject, eventName);
        }

        return recording;
    }

    /// <summary>
    /// Asserts that an object has not raised a particular event.
    /// </summary>
    /// <param name="eventName">
    /// The name of the event that should not be raised.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public void NotRaise(string eventName, string because = "", params object[] becauseArgs)
    {
        IEventRecording events = Monitor.GetRecordingFor(eventName);

        if (events.Any())
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected object {0} to not raise event {1}{reason}, but it did.", Monitor.Subject, eventName);
        }
    }

    /// <summary>
    /// Asserts that an object has raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression referring to the property for which the property changed event should have been raised, or
    /// <see langword="null"/> to refer to all properties.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public IEventRecording RaisePropertyChangeFor(Expression<Func<T, object>> propertyExpression,
        string because = "", params object[] becauseArgs)
    {
        string propertyName = propertyExpression?.GetPropertyInfo().Name;

        IEventRecording recording = Monitor.GetRecordingFor(PropertyChangedEventName);

        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(recording.Any())
            .FailWith(
                "Expected object {0} to raise event {1} for property {2}{reason}, but it did not raise that event at all.",
                Monitor.Subject, PropertyChangedEventName, propertyName);

        if (success)
        {
            var actualPropertyNames = recording
                .SelectMany(@event => @event.Parameters.OfType<PropertyChangedEventArgs>())
                .Select(eventArgs => eventArgs.PropertyName)
                .Distinct()
                .ToArray();

            Execute.Assertion
                .ForCondition(actualPropertyNames.Contains(propertyName))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected object {0} to raise event {1} for property {2}{reason}, but it was only raised for {3}.",
                    Monitor.Subject, PropertyChangedEventName, propertyName, actualPropertyNames);
        }

        return recording;
    }

    /// <summary>
    /// Asserts that an object has not raised the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a particular property.
    /// </summary>
    /// <param name="propertyExpression">
    /// A lambda expression referring to the property for which the property changed event should have been raised.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public void NotRaisePropertyChangeFor(Expression<Func<T, object>> propertyExpression,
        string because = "", params object[] becauseArgs)
    {
        IEventRecording recording = Monitor.GetRecordingFor(PropertyChangedEventName);

        string propertyName = propertyExpression.GetPropertyInfo().Name;

        if (recording.Any(@event => GetAffectedPropertyName(@event) == propertyName))
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect object {0} to raise the {1} event for property {2}{reason}, but it did.",
                    Monitor.Subject, PropertyChangedEventName, propertyName);
        }
    }

    private static string GetAffectedPropertyName(OccurredEvent @event)
    {
        return @event.Parameters.OfType<PropertyChangedEventArgs>().Single().PropertyName;
    }

    protected override string Identifier => "subject";
}
