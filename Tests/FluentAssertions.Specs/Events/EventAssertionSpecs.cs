#if !NETCOREAPP2_0

using System;
using System.ComponentModel;
using System.Linq;
#if NET47
using System.Reflection;
using System.Reflection.Emit;
#endif
using FluentAssertions.Events;
using FluentAssertions.Extensions;
using FluentAssertions.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    [Collection("EventMonitoring")]
    public class EventAssertionSpecs
    {
        #region ShouldRaise

        [Fact]
        public void When_asserting_an_event_that_doesnt_exist_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitoredSubject = subject.Monitor();

            // Act
            // ReSharper disable once AccessToDisposedClosure
            Action act = () => monitoredSubject.Should().Raise("NonExistingEvent");

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage(
                "Not monitoring any events named \"NonExistingEvent\".");
        }

        [Fact]
        public void When_asserting_that_an_event_was_not_raised_and_it_doesnt_exist_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();

            // Act
            Action act = () => monitor.Should().NotRaise("NonExistingEvent");

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage(
                "Not monitoring any events named \"NonExistingEvent\".");
        }

        [Fact]
        public void When_an_event_was_not_raised_it_should_throw_and_use_the_reason()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();

            // Act
            Action act = () => monitor.Should().Raise("PropertyChanged", "{0} should cause the event to get raised", "Foo()");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                " to raise event \"PropertyChanged\" because Foo() should cause the event to get raised, but it did not.");
        }

        [Fact]
        public void When_the_expected_event_was_raised_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithoutSender();

            // Act
            Action act = () => monitor.Should().Raise("PropertyChanged");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_unexpected_event_was_raised_it_should_throw_and_use_the_reason()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithoutSender();

            // Act
            Action act = () =>
                monitor.Should().NotRaise("PropertyChanged", "{0} should cause the event to get raised", "Foo()");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected object " + Formatter.ToString(subject) +
                             " to not raise event \"PropertyChanged\" because Foo() should cause the event to get raised, but it did.");
        }

        [Fact]
        public void When_an_unexpected_event_was_not_raised_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();

            // Act
            Action act = () => monitor.Should().NotRaise("PropertyChanged");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_event_sender_is_not_the_expected_object_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithoutSender();

            // Act
            Action act = () => monitor.Should().Raise("PropertyChanged").WithSender(subject);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected sender {Formatter.ToString(subject)}, but found <null>.");
        }

        [Fact]
        public void When_the_event_sender_is_the_expected_object_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithSender();

            // Act
            Action act = () => monitor.Should().Raise("PropertyChanged").WithSender(subject);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_WithArgs_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            // Act
            Action act = () => monitor.Should()
                .Raise("NonConventionalEvent")
                .WithArgs<string>(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("predicate");
        }

        [Fact]
        public void When_the_event_parameters_dont_match_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithoutSender();

            // Act
            Action act = () => monitor
                .Should().Raise("PropertyChanged")
                .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "SomeProperty");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected at least one event with arguments matching (args.PropertyName == \"SomeProperty\"), but found none.");
        }

        [Fact]
        public void When_the_event_args_are_of_a_different_type_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            // Act
            Action act = () => monitor
                .Should().Raise("PropertyChanged")
                .WithArgs<CancelEventArgs>(args => args.Cancel);

            // Assert
            act
                .Should().Throw<ArgumentException>()
                .WithMessage("No argument of event PropertyChanged is of type *CancelEventArgs>*");
        }

        [Fact]
        public void When_the_event_parameters_do_match_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            // Act
            Action act = () => monitor
                .Should().Raise("PropertyChanged")
                .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "SomeProperty");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_running_in_parallel_it_should_not_throw()
        {
            // Arrange
            void Action(int _)
            {
                EventRaisingClass subject = new EventRaisingClass();
                using var monitor = subject.Monitor();
                subject.RaiseEventWithSender();
                monitor.Should().Raise("PropertyChanged");
            }

            // Act
            Action act = () => Enumerable.Range(0, 1000)
                .AsParallel()
                .ForAll(Action);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_monitored_class_event_has_fired_it_should_be_possible_to_reset_the_event_monitor()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var eventMonitor = subject.Monitor();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            // Act
            eventMonitor.Clear();

            // Assert
            eventMonitor.Should().NotRaise("PropertyChanged");
        }

        [Fact]
        public void When_a_non_conventional_event_with_a_specific_argument_was_raised_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            // Act
            Action act = () => monitor
                .Should().Raise("NonConventionalEvent")
                .WithArgs<string>(args => args == "third argument");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_non_conventional_event_with_many_specific_arguments_was_raised_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            // Act
            Action act = () => monitor
                .Should().Raise("NonConventionalEvent")
                .WithArgs<string>(null, args => args == "third argument");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_non_conventional_event_with_a_specific_argument_was_not_raised_it_should_throw()
        {
            // Arrange
            const int wrongArgument = 3;
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            // Act
            Action act = () => monitor
                .Should().Raise("NonConventionalEvent")
                .WithArgs<int>(args => args == wrongArgument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected at least one event with arguments matching (args == " + wrongArgument + "), but found none.");
        }

        [Fact]
        public void When_a_non_conventional_event_with_many_specific_arguments_was_not_raised_it_should_throw()
        {
            // Arrange
            const string wrongArgument = "not a third argument";
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            // Act
            Action act = () => monitor
                .Should().Raise("NonConventionalEvent")
                .WithArgs<string>(null, args => args == wrongArgument);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected at least one event with arguments matching \"(args == \"" + wrongArgument +
                "\")\", but found none.");
        }

        #endregion

        #region Should(Not)RaisePropertyChanged events

        [Fact]
        public void When_a_property_changed_event_was_raised_for_the_expected_property_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");
            subject.RaiseEventWithSenderAndPropertyName("SomeOtherProperty");

            // Act
            Action act = () => monitor.Should().RaisePropertyChangeFor(x => x.SomeProperty);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_expected_property_changed_event_was_raised_for_all_properties_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithSenderAndPropertyName(null);

            // Act
            Action act = () => monitor.Should().RaisePropertyChangeFor(null);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_property_changed_event_was_raised_by_monitored_class_it_should_be_possible_to_reset_the_event_monitor()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var eventMonitor = subject.Monitor();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            // Act
            eventMonitor.Clear();

            // Assert
            eventMonitor.Should().NotRaisePropertyChangeFor(e => e.SomeProperty);
        }

        [Fact]
        public void When_a_property_changed_event_for_an_unexpected_property_was_raised_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            // Act
            Action act = () => monitor.Should().NotRaisePropertyChangeFor(x => x.SomeProperty, "nothing happened");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect object " + Formatter.ToString(subject) +
                " to raise the \"PropertyChanged\" event for property \"SomeProperty\" because nothing happened, but it did.");
        }

        [Fact]
        public void When_a_property_changed_event_for_a_specific_property_was_not_raised_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();

            // Act
            Action act = () => monitor.Should().RaisePropertyChangeFor(x => x.SomeProperty, "the property was changed");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                " to raise event \"PropertyChanged\" for property \"SomeProperty\" because the property was changed, but it did not.");
        }

        [Fact]
        public void When_a_property_agnostic_property_changed_event_for_was_not_raised_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();

            // Act
            Action act = () => monitor.Should().RaisePropertyChangeFor(null);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                " to raise event \"PropertyChanged\" for property <null>, but it did not.");
        }

        [Fact]
        public void When_a_property_changed_event_for_another_than_the_unexpected_property_was_raised_it_should_not_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseEventWithSenderAndPropertyName("SomeOtherProperty");

            // Act
            Action act = () => monitor.Should().NotRaisePropertyChangeFor(x => x.SomeProperty);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_monitoring_a_class_it_should_be_possible_to_attach_to_additional_interfaces_on_the_same_object()
        {
            // Arrange
            var subject = new TestEventRaising();
            using var outerMonitor = subject.Monitor<IEventRaisingInterface>();
            using var innerMonitor = subject.Monitor<IEventRaisingInterface2>();

            // Act
            subject.RaiseBothEvents();

            // Assert
            outerMonitor.Should().Raise("InterfaceEvent");
            innerMonitor.Should().Raise("Interface2Event");
        }

        #endregion

        #region Precondition Checks

        [Fact]
        public void When_monitoring_a_null_object_it_should_throw()
        {
            // Arrange
            EventRaisingClass subject = null;

            // Act
            Action act = () => subject.Monitor();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Cannot monitor the events of a <null> object*");
        }

        [Fact]
        public void When_nesting_monitoring_requests_scopes_should_be_isolated()
        {
            // Arrange
            var eventSource = new EventRaisingClass();
            using var outerScope = eventSource.Monitor();

            // Act
            using var innerScope = eventSource.Monitor();
            // Assert
            ((object)innerScope).Should().NotBeSameAs(outerScope);
        }

        [Fact]
        public void When_monitoring_an_object_with_invalid_property_expression_it_should_throw()
        {
            // Arrange
            var eventSource = new EventRaisingClass();
            using var monitor = eventSource.Monitor();
            Func<EventRaisingClass, int> func = e => e.SomeOtherProperty;

            // Act
            Action act = () => monitor.Should().RaisePropertyChangeFor(e => func(e));

            // Assert
            act.Should().Throw<ArgumentException>()
                .Which.ParamName.Should().Be("expression");
        }

        #endregion

        #region Metadata

        [Fact]
        public void When_monitoring_an_object_it_should_monitor_all_the_events_it_exposes()
        {
            // Arrange
            var eventSource = new ClassThatRaisesEventsItself();
            using var eventMonitor = eventSource.Monitor();

            // Act
            EventMetadata[] metadata = eventMonitor.MonitoredEvents;

            // Assert
            metadata.Should().BeEquivalentTo(new[]
            {
                new
                {
                    EventName = nameof(ClassThatRaisesEventsItself.InterfaceEvent),
                    HandlerType = typeof(EventHandler)
                },
                new
                {
                    EventName = nameof(ClassThatRaisesEventsItself.PropertyChanged),
                    HandlerType = typeof(PropertyChangedEventHandler)
                }
             });
        }

        [Fact]
        public void When_monitoring_an_object_through_an_interface_it_should_monitor_only_the_events_it_exposes()
        {
            // Arrange
            var eventSource = new ClassThatRaisesEventsItself();
            using var monitor = eventSource.Monitor<IEventRaisingInterface>();

            // Act
            EventMetadata[] metadata = monitor.MonitoredEvents;

            // Assert
            metadata.Should().BeEquivalentTo(new[]
            {
                new
                {
                    EventName = nameof(IEventRaisingInterface.InterfaceEvent),
                    HandlerType = typeof(EventHandler)
                }
            });
        }

#if NET47 // DefineDynamicAssembly is obsolete in .NET Core

        [Fact]
        public void When_an_object_doesnt_expose_any_events_it_should_throw()
        {
            // Arrange
            object eventSource = CreateProxyObject();

            // Act
            Action act = () => eventSource.Monitor();

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("*not expose any events*");
        }

        [Fact]
        public void When_monitoring_interface_of_a_class_and_no_recorder_exists_for_an_event_it_should_throw()
        {
            // Arrange
            var eventSource = (IEventRaisingInterface)CreateProxyObject();
            using var eventMonitor = eventSource.Monitor();

            // Act
            Action action = () => eventMonitor.GetEventRecorder("SomeEvent");

            // Assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("Not monitoring any events named \"SomeEvent\".");
        }

        private object CreateProxyObject()
        {
            Type baseType = typeof(EventRaisingClass);
            Type interfaceType = typeof(IEventRaisingInterface);
            AssemblyName assemblyName = new AssemblyName { Name = baseType.Assembly.FullName + ".GeneratedForTest" };
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName,
                AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
            string typeName = baseType.Name + "_GeneratedForTest";
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public, baseType, new[] { interfaceType });

            MethodBuilder addHandler = emitAddRemoveEventHandler("add");
            typeBuilder.DefineMethodOverride(addHandler, interfaceType.GetMethod("add_InterfaceEvent"));
            MethodBuilder removeHandler = emitAddRemoveEventHandler("remove");
            typeBuilder.DefineMethodOverride(removeHandler, interfaceType.GetMethod("remove_InterfaceEvent"));

            Type generatedType = typeBuilder.CreateType();
            return Activator.CreateInstance(generatedType);

            MethodBuilder emitAddRemoveEventHandler(string methodName)
            {
                MethodBuilder method =
                    typeBuilder.DefineMethod(string.Format("{0}.{1}_InterfaceEvent", interfaceType.FullName, methodName),
                        MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final |
                        MethodAttributes.HideBySig |
                        MethodAttributes.NewSlot);
                method.SetReturnType(typeof(void));
                method.SetParameters(typeof(EventHandler));
                ILGenerator gen = method.GetILGenerator();
                gen.Emit(OpCodes.Ret);
                return method;
            }
        }

#endif

        [Fact]
        public void When_event_exists_on_class_but_not_on_monitored_interface_it_should_not_allow_monitoring_it()
        {
            // Arrange
            var eventSource = new ClassThatRaisesEventsItself();
            using var eventMonitor = eventSource.Monitor<IEventRaisingInterface>();

            // Act
            Action action = () => eventMonitor.GetEventRecorder("PropertyChanged");

            // Assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("Not monitoring any events named \"PropertyChanged\".");
        }

        [Fact]
        public void When_an_object_raises_two_events_it_should_provide_the_data_about_those_occurrences()
        {
            // Arrange
            DateTime utcNow = 17.September(2017).At(21, 00);

            var eventSource = new EventRaisingClass();
            using var monitor = eventSource.Monitor(() => utcNow);

            // Act
            eventSource.RaiseEventWithSenderAndPropertyName("theProperty");

            utcNow += 1.Hours();

            eventSource.RaiseNonConventionalEvent("first", 123, "third");

            // Assert
            monitor.OccurredEvents.Should().BeEquivalentTo(new[]
            {
                new
                {
                    EventName = "PropertyChanged",
                    TimestampUtc = utcNow - 1.Hours(),
                    Parameters = new object[] { eventSource, new PropertyChangedEventArgs("theProperty") }
                },
                new
                {
                    EventName = "NonConventionalEvent",
                    TimestampUtc = utcNow,
                    Parameters = new object[] { "first", 123, "third" }
                }
            }, o => o.WithStrictOrdering());
        }

        [Fact]
        public void When_monitoring_interface_with_inherited_event_it_should_not_throw()
        {
            // Arrange
            var eventSource = (IInheritsEventRaisingInterface)new ClassThatRaisesEventsItself();

            // Act
            Action action = () => eventSource.Monitor();

            // Assert
            action.Should().NotThrow<InvalidOperationException>();
        }

        #endregion

        public class ClassThatRaisesEventsItself : IInheritsEventRaisingInterface
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public event EventHandler InterfaceEvent;

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChanged?.Invoke(this, e);
            }

            protected virtual void OnInterfaceEvent()
            {
                InterfaceEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public class TestEventRaising : IEventRaisingInterface, IEventRaisingInterface2
        {
            public event EventHandler InterfaceEvent;
            public event EventHandler Interface2Event;

            public void RaiseBothEvents()
            {
                InterfaceEvent?.Invoke(this, EventArgs.Empty);
                Interface2Event?.Invoke(this, EventArgs.Empty);
            }
        }

        public interface IEventRaisingInterface
        {
            event EventHandler InterfaceEvent;
        }

        public interface IEventRaisingInterface2
        {
            event EventHandler Interface2Event;
        }

        public interface IInheritsEventRaisingInterface : IEventRaisingInterface { }

        public class EventRaisingClass : INotifyPropertyChanged
        {
            public string SomeProperty { get; set; }

            public int SomeOtherProperty { get; set; }

            public event PropertyChangedEventHandler PropertyChanged = delegate { };

            public event Action<string, int, string> NonConventionalEvent = delegate { };

            public void RaiseNonConventionalEvent(string first, int second, string third)
            {
                NonConventionalEvent.Invoke(first, second, third);
            }

            public void RaiseEventWithoutSender()
            {
                PropertyChanged(null, new PropertyChangedEventArgs(""));
            }

            public void RaiseEventWithSender()
            {
                PropertyChanged(this, new PropertyChangedEventArgs(""));
            }

            public void RaiseEventWithSenderAndPropertyName(string propertyName)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

#endif
