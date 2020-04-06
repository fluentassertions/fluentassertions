using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using Microsoft.Reactive.Testing;

namespace FluentAssertions.Reactive
{
    public static class ReactiveExtensions
    {
        /// <summary>
        /// Create a new <see cref="FluentTestObserver{TPayload}"/> subscribed to this <paramref name="observable"/>
        /// </summary>
        public static FluentTestObserver<T> Observe<T>(this IObservable<T> observable) => new FluentTestObserver<T>(observable);

        /// <summary>
        /// Create a new <see cref="FluentTestObserver{TPayload}"/> subscribed to this <paramref name="observable"/>
        /// </summary>
        public static FluentTestObserver<T> Observe<T>(this IObservable<T> observable, IScheduler scheduler) => new FluentTestObserver<T>(observable, scheduler);

        /// <summary>
        /// Create a new <see cref="FluentTestObserver{TPayload}"/> subscribed to this <paramref name="observable"/>
        /// </summary>
        public static FluentTestObserver<T> Observe<T>(this IObservable<T> observable, TestScheduler scheduler) => new FluentTestObserver<T>(observable, scheduler);

        /// <summary>
        /// Asserts that the recorded messages contain at lease one item which matches the <paramref name="predicate"/>
        /// </summary>
        public static AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>> WithMessage<TPayload>(
            this AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>> recorderConstraint, Expression<Func<TPayload, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            
            var compiledPredicate = predicate.Compile();
            bool match = recorderConstraint.Subject.Any(compiledPredicate);
            
            Execute.Assertion
                .ForCondition(match)
                .FailWith("Expected at least one message from {0} to match {1}, but found none.", recorderConstraint.And.Subject, predicate.Body);

            return recorderConstraint;
        }

        /// <summary>
        /// Asserts that the last recorded message matches the <paramref name="predicate"/>
        /// </summary>
        public static AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>> WithLastMessage<TPayload>(
            this AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>> recorderConstraint, Expression<Func<TPayload, bool>> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            bool match = predicate.Compile().Invoke(recorderConstraint.GetLastMessage());

            Execute.Assertion
                .ForCondition(match)
                .FailWith("Expected the last message from {0} to match {1}, but it did not.", recorderConstraint.And.Subject, predicate.Body);
            
            return recorderConstraint;
        }

        /// <summary>
        /// Extracts the last recorded message
        /// </summary>
        public static TPayload GetLastMessage<TPayload>(
            this AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>>
                recorderConstraint) =>
            recorderConstraint.Subject.LastOrDefault();

        /// <summary>
        /// Extracts the last recorded message
        /// </summary>
        public static async Task<TPayload> GetLastMessageAsync<TPayload>(
            this Task<AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>>>
                assertionTask)
        {
            var constraint = await assertionTask;
            return constraint.Subject.LastOrDefault();
        }
        
        /// <summary>
        /// Extracts the recorded messages from a number of recorded notifications
        /// </summary>
        public static IEnumerable<TPayload> GetMessages<TPayload>(
            this IEnumerable<Recorded<Notification<TPayload>>> recordedNotifications) => recordedNotifications
            .Where(r => r.Value.Kind == NotificationKind.OnNext)
            .Select(recorded => recorded.Value.Value);

        /// <summary>
        /// Extracts the last recorded message from a number of recorded notifications
        /// </summary>
        public static TPayload GetLastMessage<TPayload>(
            this IEnumerable<Recorded<Notification<TPayload>>> recordedNotifications) =>
            recordedNotifications.GetMessages().LastOrDefault();
        
        /// <summary>
        /// Clears the recorded notifications on the underlying <see cref="FluentTestObserver{TPayload}"/>
        /// </summary>
        public static void Clear<TPayload>(
            this AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<Recorded<Notification<TPayload>>>>
                recorderConstraint) => recorderConstraint.And.Observer.Clear();
    }
}
