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
        ///  Asserts that at least one occurence of the events had one or more arguments of the expected
        ///  type <typeparamref name="T"/> which matched the given predicate.
        ///  Returns only the events that matched both type and optionally a predicate.
        /// </summary>
        public static IEventRecording WithArgs<T>(this IEventRecording eventRecording, Expression<Func<T, bool>> predicate)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            Func<T, bool> compiledPredicate = predicate.Compile();

            var eventsWithMatchingPredicate = new List<OccurredEvent>();

            foreach (OccurredEvent @event in eventRecording)
            {
                var typedParameters = @event.Parameters.OfType<T>().ToArray();

                if (typedParameters.Any(parameter => compiledPredicate(parameter)))
                {
                    eventsWithMatchingPredicate.Add(@event);
                }
            }

            bool foundMatchingEvent = eventsWithMatchingPredicate.Any();

            Execute.Assertion
                .ForCondition(foundMatchingEvent)
                .FailWith("Expected at least one event which arguments are of type <{0}> and matches {1}, but found none.",
                    typeof(T),
                    predicate.Body);

            return new FilteredEventRecording(eventRecording, eventsWithMatchingPredicate);
        }

        /// <summary>
        /// Asserts that at least one occurence of the events had one or more arguments of the expected
        /// type <typeparamref name="T"/> which matched the predicates in the same order.
        /// Returns only the events that matched both type and optionally predicates.
        /// </summary>
        /// <remarks>
        /// If a <c>null</c> is provided as predicate argument, the corresponding event parameter value is ignored.
        /// </remarks>
        public static IEventRecording WithArgs<T>(this IEventRecording eventRecording, params Expression<Func<T, bool>>[] predicates)
        {
            Func<T, bool>[] compiledPredicates = predicates.Select(p => p?.Compile()).ToArray();

            var eventsWithMatchingPredicate = new List<OccurredEvent>();

            foreach (OccurredEvent @event in eventRecording)
            {
                var typedParameters = @event.Parameters.OfType<T>().ToArray();
                bool hasArgumentOfRightType = typedParameters.Any();

                if (predicates.Length > typedParameters.Length)
                {
                    throw new ArgumentException(
                        $"Expected the event to have at least {predicates.Length} parameters of type {typeof(T)}, but only found {typedParameters.Length}.");
                }

                bool isMatch = hasArgumentOfRightType;
                for (int index = 0; index < predicates.Length && isMatch; index++)
                {
                    isMatch = compiledPredicates[index]?.Invoke(typedParameters[index]) ?? true;
                }

                if (isMatch)
                {
                    eventsWithMatchingPredicate.Add(@event);
                }
            }

            bool foundMatchingEvent = eventsWithMatchingPredicate.Any();

            if (!foundMatchingEvent)
            {
                Execute.Assertion
                    .FailWith("Expected at least one event which arguments are of type <{0}> and matches {1}, but found none.",
                        typeof(T),
                        string.Join(" | ", predicates.Where(p => p is not null).Select(p => p.Body.ToString())));
            }

            return new FilteredEventRecording(eventRecording, eventsWithMatchingPredicate);
        }
    }
}
