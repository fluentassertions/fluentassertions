using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.Reactive.Testing;

namespace FluentAssertions.Reactive
{
    /// <summary>
    /// Provides methods to assert an <see cref="IObservable{T}"/> observed by a <see cref="FluentTestObserver{TPayload}"/>
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public class ReactiveAssertions<TPayload> : ReferenceTypeAssertions<IObservable<TPayload>, ReactiveAssertions<TPayload>>
    {
        public FluentTestObserver<TPayload> Observer { get; }

        protected internal ReactiveAssertions(FluentTestObserver<TPayload> observer): base(observer.Subject)
        {
            Observer = observer;
        }

        protected override string Identifier => "Subscription";

        /// <summary>
        /// Asserts that at least <paramref name="numberOfNotifications"/> notifications were pushed to the <see cref="FluentTestObserver{TPayload}"/> within the specified <paramref name="timeout"/>.<br />
        /// This includes any previously recorded notifications since it has been created or cleared.
        /// </summary> 
        /// <param name="numberOfNotifications">the number of notifications the observer should have recorded by now</param>
        /// <param name="timeout">the maximum time to wait for the notifications to arrive</param>
        /// <param name="because"></param>
        /// <param name="becauseArgs"></param>
        public AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>> Push(int numberOfNotifications, TimeSpan timeout,
            string because = "", params object[] becauseArgs)
        {
            IList<TPayload> notifications = new List<TPayload>();
            var assertion = Execute.Assertion
                .WithExpectation($"Expected {{context}} to push at least {numberOfNotifications} {(numberOfNotifications == 1 ? "notification" : "notifications")}, ")
                .BecauseOf(because, becauseArgs);

            try
            {
                notifications = Observer.RecordedNotificationStream
                    .Select(r => r.Value)
                    .Dematerialize()
                    .Take(numberOfNotifications)
                    .Timeout(timeout)
                    .Catch<TPayload, TimeoutException>(exception => Observable.Empty<TPayload>())
                    .ToList()
                    .ToTask()
                    .ExecuteInDefaultSynchronizationContext();
            }
            catch (Exception e)
            {
                assertion.FailWith("but it failed with exception {0}.", e);
            }

            assertion
                .ForCondition(notifications.Count >= numberOfNotifications)
                .FailWith("but {0} were received within {1}.", notifications.Count, timeout);

            return new AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>>(this, notifications);
        }

        /// <inheritdoc cref="Push(int,TimeSpan,string,object[])"/>
        public async Task<AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>>> PushAsync(int numberOfNotifications, TimeSpan timeout,
            string because = "", params object[] becauseArgs)
        {
            IList<TPayload> notifications = new List<TPayload>();
            var assertion = Execute.Assertion
                .WithExpectation($"Expected {{context}} to push at least {numberOfNotifications} {(numberOfNotifications == 1 ? "notification" : "notifications")}, ")
                .BecauseOf(because, becauseArgs);

            try
            {
                notifications = await Observer.RecordedNotificationStream
                    .Select(r => r.Value)
                    .Dematerialize()
                    .Take(numberOfNotifications)
                    .Timeout(timeout)
                    .Catch<TPayload, TimeoutException>(exception => Observable.Empty<TPayload>())
                    .ToList()
                    .ToTask();
            }
            catch (Exception e)
            {
                assertion.FailWith("but it failed with exception {0}.", e);
            }

            assertion
                .ForCondition(notifications.Count >= numberOfNotifications)
                .FailWith("but {0} were received within {1}.", notifications.Count, timeout);

            return new AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>>(this, notifications);
        }

        /// <summary>
        /// Asserts that at least <paramref name="numberOfNotifications"/> notifications are pushed to the <see cref="FluentTestObserver{TPayload}"/> within the next 1 second.<br />
        /// This includes any previously recorded notifications since it has been created or cleared. 
        /// </summary>
        /// <param name="numberOfNotifications">the number of notifications the observer should have recorded by now</param>
        /// <param name="because"></param>
        /// <param name="becauseArgs"></param>
        public AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>> Push(int numberOfNotifications, string because = "", params object[] becauseArgs)
            => Push(numberOfNotifications, TimeSpan.FromSeconds(10), because, becauseArgs);

        /// <inheritdoc cref="Push(int,string,object[])"/>
        public Task<AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>>> PushAsync(int numberOfNotifications, string because = "", params object[] becauseArgs)
            => PushAsync(numberOfNotifications, TimeSpan.FromSeconds(10), because, becauseArgs);

        /// <summary>
        /// Asserts that at least 1 notification is pushed to the <see cref="FluentTestObserver{TPayload}"/> within the next 1 second.<br />
        /// This includes any previously recorded notifications since it has been created or cleared. 
        /// </summary>
        public AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>> Push(string because = "", params object[] becauseArgs)
            => Push(1, TimeSpan.FromSeconds(1), because, becauseArgs);

        /// <inheritdoc cref="Push(string,object[])"/>
        public Task<AndWhichConstraint<ReactiveAssertions<TPayload>, IEnumerable<TPayload>>> PushAsync(string because = "", params object[] becauseArgs)
            => PushAsync(1, TimeSpan.FromSeconds(1), because, becauseArgs);

        /// <summary>
        /// Asserts that the <see cref="FluentTestObserver{TPayload}"/> does not receive any notifications within the specified <paramref name="timeout"/>.<br />
        /// This includes any previously recorded notifications since it has been created or cleared. 
        /// </summary>
        public AndConstraint<ReactiveAssertions<TPayload>> NotPush(TimeSpan timeout,
            string because = "", params object[] becauseArgs)
        {
            bool anyNotifications = Observer.RecordedNotificationStream
                .Any(recorded => recorded.Value.Kind == NotificationKind.OnNext)
                .Timeout(timeout)
                .Catch(Observable.Return(false))
                .ToTask()
                .ExecuteInDefaultSynchronizationContext();

            Execute.Assertion
                .ForCondition(!anyNotifications)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to not push any notifications{reason}, but it did.");
            
            return new AndConstraint<ReactiveAssertions<TPayload>>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="FluentTestObserver{TPayload}"/> does not receive any notifications within the next 100 milliseconds.<br />
        /// This includes any previously recorded notifications since it has been created or last cleared. 
        /// </summary>
        public AndConstraint<ReactiveAssertions<TPayload>> NotPush(string because = "", params object[] becauseArgs)
            => NotPush(TimeSpan.FromMilliseconds(100), because, becauseArgs);

        /// <summary>
        /// Asserts that the <see cref="IObservable{T}"/> observed by the <see cref="FluentTestObserver{TPayload}"/> fails within the specified <paramref name="timeout"/>. 
        /// </summary>
        public AndWhichConstraint<ReactiveAssertions<TPayload>, Exception> Fail(TimeSpan timeout,
            string because = "", params object[] becauseArgs)
        {
            var exception = Observer.RecordedNotificationStream
                .Timeout(timeout)
                .Catch(Observable.Empty<Recorded<Notification<TPayload>>>())
                .FirstOrDefaultAsync(recorded => recorded.Value.Kind == NotificationKind.OnError)
                .Select(recorded => recorded.Value.Exception)
                .ToTask()
                .ExecuteInDefaultSynchronizationContext();

            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to fail within {0}{reason}, but it did not.", timeout);
            
            return new AndWhichConstraint<ReactiveAssertions<TPayload>, Exception>(this, exception);
        }

        /// <inheritdoc cref="Fail(TimeSpan,string,object[])"/>
        public async Task<AndWhichConstraint<ReactiveAssertions<TPayload>, Exception>> FailAsync(TimeSpan timeout,
            string because = "", params object[] becauseArgs)
        {
            var exception = await Observer.RecordedNotificationStream
                .Timeout(timeout)
                .Catch(Observable.Empty<Recorded<Notification<TPayload>>>())
                .FirstOrDefaultAsync(recorded => recorded.Value.Kind == NotificationKind.OnError)
                .Select(recorded => recorded.Value.Exception)
                .ToTask();

            Execute.Assertion
                .ForCondition(exception != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to fail within {0}{reason}, but it did not.", timeout);

            return new AndWhichConstraint<ReactiveAssertions<TPayload>, Exception>(this, exception);
        }

        /// <summary>
        /// Asserts that the <see cref="IObservable{T}"/> observed by the <see cref="FluentTestObserver{TPayload}"/> fails within the next 1 second. 
        /// </summary>
        public AndWhichConstraint<ReactiveAssertions<TPayload>, Exception> Fail(string because = "", params object[] becauseArgs)
            => Fail(TimeSpan.FromSeconds(1), because, becauseArgs);

        /// <inheritdoc cref="Fail(string,object[])"/>
        public Task<AndWhichConstraint<ReactiveAssertions<TPayload>, Exception>> FailAsync(string because = "", params object[] becauseArgs)
            => FailAsync(TimeSpan.FromSeconds(1), because, becauseArgs);

        /// <summary>
        /// Asserts that the <see cref="IObservable{T}"/> observed by the <see cref="FluentTestObserver{TPayload}"/> completes within the specified <paramref name="timeout"/>. 
        /// </summary>
        public AndConstraint<ReactiveAssertions<TPayload>> Complete(TimeSpan timeout,
            string because = "", params object[] becauseArgs)
        {
            bool completed = Observer.RecordedNotificationStream
                .Any(recorded => recorded.Value.Kind == NotificationKind.OnCompleted)
                .Timeout(timeout)
                .Catch(Observable.Return(false))
                .ToTask()
                .ExecuteInDefaultSynchronizationContext();
            
            Execute.Assertion
                .ForCondition(completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to complete within {0}{reason}, but it did not.", timeout);

            return new AndConstraint<ReactiveAssertions<TPayload>>(this);
        }

        /// <inheritdoc cref="Complete(System.TimeSpan,string,object[])"/>
        public async Task<AndConstraint<ReactiveAssertions<TPayload>>> CompleteAsync(TimeSpan timeout,
            string because = "", params object[] becauseArgs)
        {
            bool completed = await Observer.RecordedNotificationStream
                .Any(recorded => recorded.Value.Kind == NotificationKind.OnCompleted)
                .Timeout(timeout)
                .Catch(Observable.Return(false))
                .ToTask();

            Execute.Assertion
                .ForCondition(completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to complete within {0}{reason}, but it did not.", timeout);

            return new AndConstraint<ReactiveAssertions<TPayload>>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="IObservable{T}"/> observed by the <see cref="FluentTestObserver{TPayload}"/> completes within the next 1 second. 
        /// </summary>
        public AndConstraint<ReactiveAssertions<TPayload>> Complete(string because = "", params object[] becauseArgs)
            => Complete(TimeSpan.FromSeconds(1), because, becauseArgs);

        /// <inheritdoc cref="Complete(string,object[])"/>
        public Task<AndConstraint<ReactiveAssertions<TPayload>>> CompleteAsync(string because = "", params object[] becauseArgs)
            => CompleteAsync(TimeSpan.FromSeconds(1), because, becauseArgs);

        /// <summary>
        /// Asserts that the <see cref="IObservable{T}"/> observed by the <see cref="FluentTestObserver{TPayload}"/> does not complete within the specified <paramref name="timeout"/>. 
        /// </summary>
        public AndConstraint<ReactiveAssertions<TPayload>> NotComplete(TimeSpan timeout,
            string because = "", params object[] becauseArgs)
        {
            bool completed = Observer.RecordedNotificationStream
                .Any(recorded => recorded.Value.Kind == NotificationKind.OnCompleted)
                .Timeout(timeout)
                .Catch(Observable.Return(false))
                .ToTask()
                .ExecuteInDefaultSynchronizationContext();

            Execute.Assertion
                .ForCondition(!completed)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context} to not complete{reason}, but it did.");
            
            return new AndConstraint<ReactiveAssertions<TPayload>>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="IObservable{T}"/> observed by the <see cref="FluentTestObserver{TPayload}"/> does not complete within the next 100 milliseconds. 
        /// </summary>
        public AndConstraint<ReactiveAssertions<TPayload>> NotComplete(string because = "", params object[] becauseArgs)
            => NotComplete(TimeSpan.FromMilliseconds(100), because, becauseArgs);
    }
}
