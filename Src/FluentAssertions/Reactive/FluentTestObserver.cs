using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;

namespace FluentAssertions.Reactive
{
    /// <summary>
    /// Observer for testing <see cref="Observable"/>s using the FluentAssertions framework
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public class FluentTestObserver<TPayload> : IObserver<TPayload>, IDisposable
    {
        private readonly IDisposable _subscription;
        private readonly EventLoopScheduler _observeScheduler = new EventLoopScheduler();
        private readonly RollingReplaySubject<Recorded<Notification<TPayload>>> _rollingReplaySubject = new RollingReplaySubject<Recorded<Notification<TPayload>>>();

        /// <summary>
        /// The observable which is observed by this instance
        /// </summary>
        public IObservable<TPayload> Subject { get; }

        /// <summary>
        /// The stream of recorded <see cref="Notification{T}"/>s
        /// </summary>
        public IObservable<Recorded<Notification<TPayload>>> RecordedNotificationStream => _rollingReplaySubject.AsObservable();

        /// <summary>
        /// The recorded <see cref="Notification{T}"/>s
        /// </summary>
        public IEnumerable<Recorded<Notification<TPayload>>> RecordedNotifications =>
            _rollingReplaySubject.GetSnapshot();

        /// <summary>
        /// The recorded messages
        /// </summary>
        public IEnumerable<TPayload> RecordedMessages =>
            RecordedNotifications.GetMessages();

        /// <summary>
        /// Creates a new <see cref="FluentTestObserver{TPayload}"/> which subscribes to the supplied <see cref="IObservable{T}"/>
        /// </summary>
        /// <param name="subject">the <see cref="IObservable{T}"/> under test</param>
        public FluentTestObserver(IObservable<TPayload> subject)
        {
            Subject = subject;
            _observeScheduler.Schedule(() =>
                Debug.WriteLine($"Observe scheduler thread id: {Thread.CurrentThread.ManagedThreadId}"));

            _subscription = subject.ObserveOn(_observeScheduler).Subscribe(this);
        }

        /// <summary>
        /// Clears the recorded notifications and messages as well as the recorded notifications stream buffer
        /// </summary>
        public void Clear() => _rollingReplaySubject.Clear();

        /// <inheritdoc />
        public void OnNext(TPayload value) =>
            _rollingReplaySubject.OnNext(new Recorded<Notification<TPayload>>(_observeScheduler.Now.UtcTicks, Notification.CreateOnNext(value)));

        /// <inheritdoc />
        public void OnError(Exception exception) =>
            _rollingReplaySubject.OnNext(new Recorded<Notification<TPayload>>(_observeScheduler.Now.UtcTicks, Notification.CreateOnError<TPayload>(exception)));

        /// <inheritdoc />
        public void OnCompleted() =>
            _rollingReplaySubject.OnNext(new Recorded<Notification<TPayload>>(_observeScheduler.Now.UtcTicks, Notification.CreateOnCompleted<TPayload>()));
        
        /// <inheritdoc />
        public void Dispose()
        {
            _subscription?.Dispose();
            _observeScheduler.Dispose();
            _rollingReplaySubject?.Dispose();
        }

        /// <summary>
        /// Returns an <see cref="ReactiveAssertions{TPayload}"/> object that can be used to assert the observed <see cref="IObservable{T}"/>
        /// </summary>
        /// <returns></returns>
        public ReactiveAssertions<TPayload> Should() => new ReactiveAssertions<TPayload>(this);
    }
}
