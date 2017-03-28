#if !WINRT && !SILVERLIGHT && !WINDOWS_PHONE_APP && !CORE_CLR
using System.Linq;

using FluentAssertions.Events;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    /// <summary>
    ///   Provides extension methods for monitoring and querying events.
    /// </summary>
    public static class EventSourceExtensions
    {
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
        public static IEventRecorder ShouldRaise(this object eventSource, string eventName)
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
        public static IEventRecorder ShouldRaise(
            this object eventSource, string eventName, string because, params object[] becauseArgs)
        {
            IEventRecorder eventRecorder = EventMonitor.Get(eventSource).GetEventRecorder(eventName);

            if (!eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
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
        public static void ShouldNotRaise(
            this object eventSource, string eventName, string because, params object[] becauseArgs)
        {
            IEventRecorder eventRecorder = EventMonitor.Get(eventSource).GetEventRecorder(eventName);
            if (eventRecorder.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected object {0} to not raise event {1}{reason}, but it did.", eventSource, eventName);
            }
        }
    }
}
#endif