
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentAssertions.Formatting;
using FluentAssertions.Reactive;
using Microsoft.Reactive.Testing;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    [Collection("Reactive")]
    public class ReactiveAssertionSpecs: ReactiveTest
    {
        [Fact]
        public void When_the_expected_number_of_notifications_where_pushed_it_should_not_throw()
        {
            var scheduler = new TestScheduler();
            var observable = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3));

            // observe the sequence
            using var observer = observable.Observe(scheduler);
            // push subscriptions
            scheduler.AdvanceTo(400);

            // Act
            Action act = () => observer.Should().Push(3);

            // Assert
            act.Should().NotThrow();

            observer.RecordedNotifications.Should().BeEquivalentTo(observable.Messages);
        }

        [Fact]
        public void When_the_expected_number_of_notifications_where_not_pushed_it_should_throw()
        {
            var scheduler = new TestScheduler();
            var observable = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3));

            // observe the sequence
            using var observer = observable.Observe(scheduler);

            // assert a single notification
            // Act
            Action act = () => observer.Should().Push(1, TimeSpan.Zero);
            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"Expected observable to push at least 1 notification, but 0 were received within {Formatter.ToString(TimeSpan.Zero)}.");
            observer.RecordedNotifications.Should().BeEmpty("because no messages have been pushed");

            // assert multiple notifications
            scheduler.AdvanceTo(250);

            // Act
            act = () => observer.Should().Push(3, TimeSpan.Zero);
            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"Expected observable to push at least 3 notifications, but 2 were received within {Formatter.ToString(TimeSpan.Zero)}.");
            observer.RecordedNotifications.Should().BeEquivalentTo(observable.Messages.Take(2));
        }

        [Fact]
        public void When_the_observable_fails_instead_of_pushing_notifications_it_should_throw()
        {
            var exception = new ArgumentException("That was wrong.");
            var scheduler = new TestScheduler();
            var observable = scheduler.CreateColdObservable(
                OnError<Unit>(1, exception));
            
            // observe the sequence
            using var observer = observable.Observe(scheduler);
            scheduler.AdvanceTo(10);
            // Act
            Action act = () => observer.Should().Push();
            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"Expected observable to push at least 1 notification, but it failed with a {Formatter.ToString(exception)}.");
            observer.Error.Should().BeEquivalentTo(exception);
        }

        [Fact]
        public void When_the_observable_fails_as_expected_it_should_not_throw()
        {
            var exception = new ArgumentException("That was wrong.");
            var scheduler = new TestScheduler();
            var observable = scheduler.CreateColdObservable(
                OnError<Unit>(1, exception));

            // observe the sequence
            using var observer = observable.Observe(scheduler);
            scheduler.AdvanceTo(10);

            // Act
            Action act = () => observer.Should().Throw<ArgumentException>()
                .WithMessage(exception.Message);

            // Assert
            act.Should().NotThrow();
            observer.Error.Should().BeEquivalentTo(exception);
        }

        [Fact]
        public void When_the_observable_is_expected_to_fail_but_does_not_it_should_throw()
        {
            var exception = new ArgumentException("That was wrong.");
            var observable = Observable.Return(false);

            // observe the sequence
            using var observer = observable.Observe();

            // Act
            Action act = () => observer.Should().Throw<ArgumentException>(TimeSpan.Zero);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"Expected observable to throw a <System.ArgumentException>, but no exception was thrown.");

            observer.Error.Should().BeNull();
        }


        [Fact]
        public void When_the_observable_completes_as_expected_it_should_not_throw()
        {
            var observable = Observable.Empty<Unit>();

            // observe the sequence
            using var observer = observable.Observe();

            // Act
            Action act = () => observer.Should().Complete();
            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_observable_is_expected_to_complete_but_does_not_it_should_throw()
        {
            var exception = new ArgumentException("That was wrong.");
            var observable = new Subject<Unit>();

            // observe the sequence
            using var observer = observable.Observe();

            // Act
            Action act = () => observer.Should().Complete(TimeSpan.Zero);
            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                $"Expected observable to complete within {Formatter.ToString(TimeSpan.Zero)}, but it did not.");
            observer.Error.Should().BeNull();
        }

    }
}
