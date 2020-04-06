using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace FluentAssertions.Reactive
{
    /// <summary>
    /// Clearable <see cref="ReplaySubject{T}"/> taken from James World: https://stackoverflow.com/a/28945444/4340541
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RollingReplaySubject<T> : ISubject<T>, IDisposable
    {
        private readonly ReplaySubject<IObservable<T>> _subjects;
        private readonly IObservable<T> _concatenatedSubjects;
        private ISubject<T> _currentSubject;

        public RollingReplaySubject()
        {
            _subjects = new ReplaySubject<IObservable<T>>(1);
            _concatenatedSubjects = _subjects.Concat();
            _currentSubject = new ReplaySubject<T>();
            _subjects.OnNext(_currentSubject);
        }

        public void Clear()
        {
            _currentSubject.OnCompleted();
            _currentSubject = new ReplaySubject<T>();
            _subjects.OnNext(_currentSubject);
        }

        public void OnNext(T value) => _currentSubject.OnNext(value);

        public void OnError(Exception error) => _currentSubject.OnError(error);

        public void OnCompleted()
        {
            _currentSubject.OnCompleted();
            _subjects.OnCompleted();
            // a quick way to make the current ReplaySubject unreachable
            // except to in-flight observers, and not hold up collection
            _currentSubject = new Subject<T>();
        }

        public IDisposable Subscribe(IObserver<T> observer) => _concatenatedSubjects.Subscribe(observer);

        public IEnumerable<T> GetSnapshot()
        {
            var snapshot = new List<T>();
            using (this.Subscribe(item => snapshot.Add(item)))
            {
                // Deliberately empty; subscribing will add everything to the list.
            }
            return snapshot;
        }

        public void Dispose()
        {
            OnCompleted();
            _subjects?.Dispose();
        }
    }
}
