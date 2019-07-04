using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Events;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

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
                        Resources.Event_ExpectedEventFromSenderX0ButEventX1DoesNotIncludeArgsFormat,
                        expectedSender, eventRecorder.EventName));
                }

                object actualSender = recordedEvent.Parameters.First();
                Execute.Assertion
                    .ForCondition(ReferenceEquals(actualSender, expectedSender))
                    .FailWith(Resources.Event_ExpectedSenderX0Format + Resources.Common_CommaButFoundX1Format, expectedSender, actualSender);
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
                throw new ArgumentException(string.Format(Resources.Event_NoArgumentOfEventX0IsOfTypeX1Format, eventRecorder.EventName, typeof(T)));
            }

            if (eventRecorder.All(recordedEvent => !recordedEvent.Parameters.OfType<T>().Any(parameter => compiledPredicate(parameter))))
            {
                Execute.Assertion
                    .FailWith(Resources.Event_ExpectedAtLeastOneEventWithArgsMatchingX0ButFoundNoneFormat, predicate.Body);
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
                throw new ArgumentException(string.Format(Resources.Event_NoArgumentOfEventX0IsOfTypeX1Format, eventRecorder.EventName, typeof(T)));
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
                    .FailWith(Resources.Event_ExpectedAtLeastOneEventWithArgsMatchingX0ButFoundNoneFormat, string.Join(" | ", predicates.Where(p => p != null).Select(p => p.Body.ToString())));
            }

            return eventRecorder;
        }
    }
}
