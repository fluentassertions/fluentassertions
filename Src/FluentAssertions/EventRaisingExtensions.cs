using System;
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
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

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
