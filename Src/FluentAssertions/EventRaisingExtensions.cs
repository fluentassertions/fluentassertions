using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Events;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    /// <summary>
    /// Provides extension methods for monitoring and querying events.
    /// </summary>
    public static class EventRaisingExtensions
    {
        /// <summary>
        /// Asserts that all occurrences of the event originated from the <param name="expectedSender"/> and
        /// returns only the events that came from that sender.
        /// </summary>
        public static IEventRecording WithSender(this IEventRecording eventRecording, object expectedSender)
        {
            var eventsForSender = new List<OccurredEvent>();
            var otherSenders = new List<object>();

            foreach (OccurredEvent @event in eventRecording)
            {
                bool hasSender = Execute.Assertion
                    .ForCondition(@event.Parameters.Any())
                    .FailWith("Expected event from sender {0}, " +
                              $"but event {eventRecording.EventName} does not have any parameters", expectedSender);

                if (hasSender)
                {
                    object sender = @event.Parameters.First();
                    if (ReferenceEquals(sender, expectedSender))
                    {
                        eventsForSender.Add(@event);
                    }
                    else
                    {
                        otherSenders.Add(sender);
                    }
                }
            }

            Execute.Assertion
                .ForCondition(eventsForSender.Any())
                .FailWith("Expected sender {0}, but found {1}.",
                    () => expectedSender,
                    () => otherSenders.Distinct());

            return new FilteredEventRecording(eventRecording, eventsForSender);
        }

        /// <summary>
        /// Asserts that at least one occurrence of the events had at least one of the arguments matching a predicate. Returns
        /// only the events that matched that predicate.
        /// </summary>
        public static IEventRecording WithArgs<T>(this IEventRecording eventRecording, Expression<Func<T, bool>> predicate)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            Func<T, bool> compiledPredicate = predicate.Compile();

            bool hasArgumentOfRightType = false;
            var eventsMatchingPredicate = new List<OccurredEvent>();

            foreach (OccurredEvent @event in eventRecording)
            {
                var typedParameters = @event.Parameters.OfType<T>().ToArray();
                if (typedParameters.Any())
                {
                    hasArgumentOfRightType = true;
                }

                if (typedParameters.Any(parameter => compiledPredicate(parameter)))
                {
                    eventsMatchingPredicate.Add(@event);
                }
            }

            if (!hasArgumentOfRightType)
            {
                throw new ArgumentException("No argument of event " + eventRecording.EventName + " is of type <" + typeof(T) + ">.");
            }

            if (!eventsMatchingPredicate.Any())
            {
                Execute.Assertion
                    .FailWith("Expected at least one event with arguments matching {0}, but found none.", predicate.Body);
            }

            return new FilteredEventRecording(eventRecording, eventsMatchingPredicate);
        }

        /// <summary>
        /// Asserts that at least one of the occurred events has arguments the match the predicates in the same order. Returns
        /// only the events that matched those predicates.
        /// </summary>
        /// <remarks>
        /// If a <c>null</c> is provided as predicate argument, the corresponding event parameter value is ignored.
        /// </remarks>
        public static IEventRecording WithArgs<T>(this IEventRecording eventRecording, params Expression<Func<T, bool>>[] predicates)
        {
            Func<T, bool>[] compiledPredicates = predicates.Select(p => p?.Compile()).ToArray();

            bool hasArgumentOfRightType = false;
            var eventsMatchingPredicate = new List<OccurredEvent>();

            foreach (OccurredEvent @event in eventRecording)
            {
                var typedParameters = @event.Parameters.OfType<T>().ToArray();
                if (typedParameters.Any())
                {
                    hasArgumentOfRightType = true;
                }

                if (predicates.Length > typedParameters.Length)
                {
                    throw new ArgumentException(
                        $"Expected the event to have at least {predicates.Length} parameters of type {typeof(T)}, but only found {typedParameters.Length}.");
                }

                bool isMatch = true;
                for (int index = 0; index < predicates.Length && isMatch; index++)
                {
                    isMatch = compiledPredicates[index]?.Invoke(typedParameters[index]) ?? true;
                }

                if (isMatch)
                {
                    eventsMatchingPredicate.Add(@event);
                }
            }

            if (!hasArgumentOfRightType)
            {
                throw new ArgumentException("No argument of event " + eventRecording.EventName + " is of type <" + typeof(T) + ">.");
            }

            if (!eventsMatchingPredicate.Any())
            {
                Execute
                    .Assertion
                    .FailWith("Expected at least one event with arguments matching {0}, but found none.",
                    string.Join(" | ", predicates.Where(p => p is not null).Select(p => p.Body.ToString())));
            }

            return new FilteredEventRecording(eventRecording, eventsMatchingPredicate);
        }
    }
}
