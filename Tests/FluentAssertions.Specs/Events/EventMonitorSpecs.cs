#if NETFRAMEWORK
using System.Reflection.Emit;
#endif

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Xunit;

#pragma warning disable CS0067

namespace FluentAssertions.Specs.Events;

[Collection("EventMonitoring")]
public class EventMonitorSpecs
{
    public class MonitorDefaultBehavior
    {
        [Fact]
        public void When_default_configuration_is_active_broken_event_add_accessors_should_fail_the_test()
        {
            // Arrange
            var cut = new TestEventBrokenEventHandlerRaising();

            // Act
            // Assert
            cut.Invoking(c =>
            {
                using var monitor = c.Monitor<IAddFailingEvent>();
            }).Should().Throw<TargetInvocationException>();
        }

        [Fact]
        public void When_default_configuration_is_active_broken_event_remove_accessors_should_fail_the_test()
        {
            // Arrange
            var cut = new TestEventBrokenEventHandlerRaising();

            // Act
            // Assert
            cut.Invoking(c =>
            {
                using var monitor = c.Monitor<IRemoveFailingEvent>();
            }).Should().Throw<TargetInvocationException>();
        }
    }

    public class IgnoreMisbehavingEventAccessors
    {
        [Fact]
        public void
            When_IgnoreEventAccessorExceptions_is_set_monitoring_class_with_broken_event_add_accessor_should_not_fail_test()
        {
            // Arrange
            var classToMonitor = new TestEventBrokenEventHandlerRaising();

            //Act
            //Assert
            classToMonitor.Invoking(c =>
            {
                using var monitor = c.Monitor<IAddFailingEvent>(opt => opt.IgnoreEventAccessorExceptions());
            }).Should().NotThrow();
        }

        [Fact]
        public void
            When_IgnoreEventAccessorExceptions_is_set_monitoring_class_with_broken_event_remove_accessor_should_not_fail_test()
        {
            // Arrange
            var classToMonitor = new TestEventBrokenEventHandlerRaising();

            //Act

            //Assert
            classToMonitor.Invoking(c =>
            {
                using var monitor = c.Monitor<IRemoveFailingEvent>(opt => opt.IgnoreEventAccessorExceptions());
            }).Should().NotThrow();
        }
    }

    public class SettingTimestampProvider
    {
        [Fact]
        public void When_TimestampProvider_is_set_to_null_monitoring_options_should_keep_latest_value()
        {
            // Arrange
            var classToMonitor = new TestEventBrokenEventHandlerRaising();

            var toBeKeptDateTime = DateTime.UtcNow;

            // Act
            using var cut = classToMonitor.Monitor<IAddOkEvent>(opt =>
                opt.IgnoreEventAccessorExceptions()
                    .ConfigureTimestampProvider(() => toBeKeptDateTime)
                    .ConfigureTimestampProvider(null));

            classToMonitor.RaiseOkEvent();

            // Assert
            cut.OccurredEvents.First().TimestampUtc.Should().Be(toBeKeptDateTime);
        }
    }

    private interface IAddOkEvent
    {
        event EventHandler OkEvent;
    }

    private interface IAddFailingEvent
    {
        public event EventHandler AddFailingEvent;
    }

    private interface IRemoveFailingEvent
    {
        public event EventHandler RemoveFailingEvent;
    }

    [SuppressMessage("Usage", "CA1801:Check Unused Parameter", Justification = "This is on purpose for testing.")]
    private class TestEventBrokenEventHandlerRaising : IAddFailingEvent, IRemoveFailingEvent, IAddOkEvent
    {
        public event EventHandler AddFailingEvent
        {
            add { throw new InvalidOperationException("Add is failing"); }
            remove { OkEvent -= value; }
        }

        public event EventHandler OkEvent;

        public event EventHandler RemoveFailingEvent
        {
            add { OkEvent += value; }
            remove { throw new InvalidOperationException("Remove is failing"); }
        }

        public void RaiseOkEvent()
        {
            OkEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
