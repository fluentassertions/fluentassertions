#if NET47
using System.Reflection;
using System.Reflection.Emit;
#endif

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync.Events;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Extensions;
using FluentAssertionsAsync.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Events;

[Collection("EventMonitoring")]
public class EventAssertionSpecs
{
    public class ShouldRaise
    {
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
                .WithMessage($"Expected sender {Formatter.ToString(subject)}, but found {{<null>}}.");
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
                .WithParameterName("predicate");
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
                    "Expected at least one event with some argument of type*PropertyChangedEventArgs*matches*(args.PropertyName == \"SomeProperty\"), but found none.");
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
                .Should().Throw<XunitException>()
                .WithMessage("Expected*event*argument*type*CancelEventArgs>*");
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
                EventRaisingClass subject = new();
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
        public void When_a_predicate_based_parameter_assertion_expects_more_parameters_then_an_event_has_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            // Act
            Action act = () => monitor
                .Should().Raise(nameof(EventRaisingClass.NonConventionalEvent))
                .WithArgs<string>(null, null, null, args => args == "fourth argument");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*4 parameters*String*, but*2*");
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
                "Expected at least one event with some argument*type*Int32*matches*(args == " + wrongArgument +
                "), but found none.");
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
                "Expected at least one event with some arguments*match*\"(args == \"" + wrongArgument +
                "\")\", but found none.");
        }

        [Fact]
        public void When_a_specific_event_is_expected_it_should_return_only_relevant_events()
        {
            // Arrange
            var observable = new EventRaisingClass();
            using var monitor = observable.Monitor();

            // Act
            observable.RaiseEventWithSpecificSender("Foo");
            observable.RaiseEventWithSpecificSender("Bar");
            observable.RaiseNonConventionalEvent("don't care", 123, "don't care");

            // Assert
            var recording = monitor
                .Should()
                .Raise(nameof(observable.PropertyChanged));

            recording.EventName.Should().Be(nameof(observable.PropertyChanged));
            recording.EventObject.Should().BeSameAs(observable);
            recording.EventHandlerType.Should().Be(typeof(PropertyChangedEventHandler));
            recording.Should().HaveCount(2, "because only two property changed events were raised");
        }

        [Fact]
        public void When_a_specific_sender_is_expected_it_should_return_only_relevant_events()
        {
            // Arrange
            var observable = new EventRaisingClass();
            using var monitor = observable.Monitor();

            // Act
            observable.RaiseEventWithSpecificSender(observable);
            observable.RaiseEventWithSpecificSender(new object());

            // Assert
            var recording = monitor
                .Should()
                .Raise(nameof(observable.PropertyChanged))
                .WithSender(observable);

            recording.Should().ContainSingle().Which.Parameters[0].Should().BeSameAs(observable);
        }

        [Fact]
        public void When_constraints_are_specified_it_should_filter_the_events_based_on_those_constraints()
        {
            // Arrange
            var observable = new EventRaisingClass();
            using var monitor = observable.Monitor();

            // Act
            observable.RaiseEventWithSenderAndPropertyName("Foo");
            observable.RaiseEventWithSenderAndPropertyName("Boo");

            // Assert
            var recording = monitor
                .Should()
                .Raise(nameof(observable.PropertyChanged))
                .WithSender(observable)
                .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "Boo");

            recording
                .Should().ContainSingle("because we were expecting a specific property change")
                .Which.Parameters.Last().Should().BeOfType<PropertyChangedEventArgs>()
                .Which.PropertyName.Should().Be("Boo");
        }

        [Fact]
        public void When_events_are_raised_regardless_of_time_tick_it_should_return_by_invokation_order()
        {
            // Arrange
            var observable = new TestEventRaisingInOrder();
            var utcNow = 11.January(2022).At(12, 00).AsUtc();
            using var monitor = observable.Monitor(() => utcNow);

            // Act
            observable.RaiseAllEvents();

            // Assert
            monitor.OccurredEvents[0].EventName.Should().Be(nameof(TestEventRaisingInOrder.InterfaceEvent));
            monitor.OccurredEvents[0].Sequence.Should().Be(0);

            monitor.OccurredEvents[1].EventName.Should().Be(nameof(TestEventRaisingInOrder.Interface2Event));
            monitor.OccurredEvents[1].Sequence.Should().Be(1);

            monitor.OccurredEvents[2].EventName.Should().Be(nameof(TestEventRaisingInOrder.Interface3Event));
            monitor.OccurredEvents[2].Sequence.Should().Be(2);
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
    }

    public class ShouldRaisePropertyChanged
    {
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
                " to raise event \"PropertyChanged\" for property \"SomeProperty\" because the property was changed, but it did not*");
        }

        [Fact]
        public void When_a_property_agnostic_property_changed_event_for_was_not_raised_it_should_throw()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                monitor.Should().RaisePropertyChangeFor(null);
            };

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                " to raise event \"PropertyChanged\" for property <null>, but it did not*");
        }

        [Fact]
        public void
            When_the_property_changed_event_was_raised_for_the_wrong_property_it_should_throw_and_include_the_actual_properties_raised()
        {
            // Arrange
            var bar = new EventRaisingClass();
            using var monitor = bar.Monitor();
            bar.RaiseEventWithSenderAndPropertyName("OtherProperty1");
            bar.RaiseEventWithSenderAndPropertyName("OtherProperty2");
            bar.RaiseEventWithSenderAndPropertyName("OtherProperty2");

            // Act
            Action act = () => monitor.Should().RaisePropertyChangeFor(b => b.SomeProperty);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*property*SomeProperty*but*OtherProperty1*OtherProperty2*");
        }
    }

    public class ShouldNotRaisePropertyChanged
    {
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
    }

    public class PreconditionChecks
    {
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
                .WithParameterName("expression");
        }

        [Fact]
        public void Event_assertions_should_expose_the_monitor()
        {
            // Arrange
            var subject = new EventRaisingClass();
            using var monitor = subject.Monitor();

            // Act
            var exposedMonitor = monitor.Should().Monitor;

            // Assert
            ((object)exposedMonitor).Should().BeSameAs(monitor);
        }
    }

    public class Metadata
    {
        [Fact]
        public async Task When_monitoring_an_object_it_should_monitor_all_the_events_it_exposes()
        {
            // Arrange
            var eventSource = new ClassThatRaisesEventsItself();
            using var eventMonitor = eventSource.Monitor();

            // Act
            EventMetadata[] metadata = eventMonitor.MonitoredEvents;

            // Assert
            await metadata.Should().BeEquivalentToAsync(new[]
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
        public async Task When_monitoring_an_object_through_an_interface_it_should_monitor_only_the_events_it_exposes()
        {
            // Arrange
            var eventSource = new ClassThatRaisesEventsItself();
            using var monitor = eventSource.Monitor<IEventRaisingInterface>();

            // Act
            EventMetadata[] metadata = monitor.MonitoredEvents;

            // Assert
            await metadata.Should().BeEquivalentToAsync(new[]
            {
                new
                {
                    EventName = nameof(IEventRaisingInterface.InterfaceEvent),
                    HandlerType = typeof(EventHandler)
                }
            });
        }

#if NETFRAMEWORK // DefineDynamicAssembly is obsolete in .NET Core
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
            Action action = () => eventMonitor.GetRecordingFor("SomeEvent");

            // Assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("Not monitoring any events named \"SomeEvent\".");
        }

        private object CreateProxyObject()
        {
            Type baseType = typeof(EventRaisingClass);
            Type interfaceType = typeof(IEventRaisingInterface);

            AssemblyName assemblyName = new() { Name = baseType.Assembly.FullName + ".GeneratedForTest" };

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName,
                AssemblyBuilderAccess.Run);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
            string typeName = baseType.Name + "_GeneratedForTest";

            TypeBuilder typeBuilder =
                moduleBuilder.DefineType(typeName, TypeAttributes.Public, baseType, new[] { interfaceType });

            MethodBuilder addHandler = EmitAddRemoveEventHandler("add");
            typeBuilder.DefineMethodOverride(addHandler, interfaceType.GetMethod("add_InterfaceEvent"));
            MethodBuilder removeHandler = EmitAddRemoveEventHandler("remove");
            typeBuilder.DefineMethodOverride(removeHandler, interfaceType.GetMethod("remove_InterfaceEvent"));

            Type generatedType = typeBuilder.CreateType();
            return Activator.CreateInstance(generatedType);

            MethodBuilder EmitAddRemoveEventHandler(string methodName)
            {
                MethodBuilder method =
                    typeBuilder.DefineMethod($"{interfaceType.FullName}.{methodName}_InterfaceEvent",
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
            Action action = () => eventMonitor.GetRecordingFor("PropertyChanged");

            // Assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("Not monitoring any events named \"PropertyChanged\".");
        }

        [Fact]
        public async Task When_an_object_raises_two_events_it_should_provide_the_data_about_those_occurrences()
        {
            // Arrange
            DateTime utcNow = 17.September(2017).At(21, 00).AsUtc();

            var eventSource = new EventRaisingClass();
            using var monitor = eventSource.Monitor(() => utcNow);

            // Act
            eventSource.RaiseEventWithSenderAndPropertyName("theProperty");

            utcNow += 1.Hours();

            eventSource.RaiseNonConventionalEvent("first", 123, "third");

            // Assert
            await monitor.OccurredEvents.Should().BeEquivalentToAsync(new[]
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
    }

    public class WithArgs
    {
        [Fact]
        public void One_matching_argument_type_before_mismatching_types_passes()
        {
            // Arrange
            A a = new();
            using var aMonitor = a.Monitor();

            a.OnEvent(new B());
            a.OnEvent(new C());

            // Act / Assert
            IEventRecording filteredEvents = aMonitor.GetRecordingFor(nameof(A.Event)).WithArgs<B>();
            filteredEvents.Should().HaveCount(1);
        }

        [Fact]
        public void One_matching_argument_type_after_mismatching_types_passes()
        {
            // Arrange
            A a = new();
            using var aMonitor = a.Monitor();

            a.OnEvent(new C());
            a.OnEvent(new B());

            // Act / Assert
            IEventRecording filteredEvents = aMonitor.GetRecordingFor(nameof(A.Event)).WithArgs<B>();
            filteredEvents.Should().HaveCount(1);
        }

        [Fact]
        public void Throws_when_none_of_the_arguments_are_of_the_expected_type()
        {
            // Arrange
            A a = new();
            using var aMonitor = a.Monitor();

            a.OnEvent(new C());
            a.OnEvent(new C());

            // Act
            Action act = () => aMonitor.GetRecordingFor(nameof(A.Event)).WithArgs<B>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*event*argument*");
        }

        [Fact]
        public void One_matching_argument_type_anywhere_between_mismatching_types_passes()
        {
            // Arrange
            A a = new();
            using var aMonitor = a.Monitor();

            a.OnEvent(new C());
            a.OnEvent(new B());
            a.OnEvent(new C());

            // Act / Assert
            IEventRecording filteredEvents = aMonitor.GetRecordingFor(nameof(A.Event)).WithArgs<B>();
            filteredEvents.Should().HaveCount(1);
        }

        [Fact]
        public void One_matching_argument_type_anywhere_between_mismatching_types_with_parameters_passes()
        {
            // Arrange
            A a = new();
            using var aMonitor = a.Monitor();

            a.OnEvent(new C());
            a.OnEvent(new B());
            a.OnEvent(new C());

            // Act / Assert
            IEventRecording filteredEvents = aMonitor.GetRecordingFor(nameof(A.Event)).WithArgs<B>(_ => true);
            filteredEvents.Should().HaveCount(1);
        }

        [Fact]
        public void Mismatching_argument_types_with_one_parameter_matching_a_different_type_fails()
        {
            // Arrange
            A a = new();
            using var aMonitor = a.Monitor();

            a.OnEvent(new C());
            a.OnEvent(new C());

            // Act
            Action act = () => aMonitor.GetRecordingFor(nameof(A.Event)).WithArgs<B>(_ => true);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*event*argument*type*B*none*");
        }

        [Fact]
        public void Mismatching_argument_types_with_two_or_more_parameters_matching_a_different_type_fails()
        {
            // Arrange
            A a = new();
            using var aMonitor = a.Monitor();

            a.OnEvent(new C());
            a.OnEvent(new C());

            // Act
            Action act = () => aMonitor.GetRecordingFor(nameof(A.Event)).WithArgs<B>(_ => true, _ => false);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Expected*event*parameters*type*B*found*");
        }

        [Fact]
        public void One_matching_argument_type_with_two_or_more_parameters_matching_a_mismatching_type_fails()
        {
            // Arrange
            A a = new();
            using var aMonitor = a.Monitor();

            a.OnEvent(new C());
            a.OnEvent(new B());

            // Act
            Action act = () => aMonitor.GetRecordingFor(nameof(A.Event)).WithArgs<B>(_ => true, _ => false);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Expected*event*parameters*type*B*found*");
        }
    }

    public class A
    {
#pragma warning disable MA0046
        public event EventHandler<object> Event;
#pragma warning restore MA0046

        public void OnEvent(object o)
        {
            Event.Invoke(nameof(A), o);
        }
    }

    public class B
    {
    }

    public class C
    {
    }

    public class ClassThatRaisesEventsItself : IInheritsEventRaisingInterface
    {
#pragma warning disable RCS1159
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore RCS1159

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

    private class TestEventRaisingInOrder : IEventRaisingInterface, IEventRaisingInterface2, IEventRaisingInterface3
    {
        public event EventHandler Interface3Event;

        public event EventHandler Interface2Event;

        public event EventHandler InterfaceEvent;

        public void RaiseAllEvents()
        {
            InterfaceEvent?.Invoke(this, EventArgs.Empty);
            Interface2Event?.Invoke(this, EventArgs.Empty);
            Interface3Event?.Invoke(this, EventArgs.Empty);
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

    public interface IEventRaisingInterface3
    {
        event EventHandler Interface3Event;
    }

    public interface IInheritsEventRaisingInterface : IEventRaisingInterface
    {
    }

    public class EventRaisingClass : INotifyPropertyChanged
    {
        public string SomeProperty { get; set; }

        public int SomeOtherProperty { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (_, _) => { };

#pragma warning disable MA0046
        public event Action<string, int, string> NonConventionalEvent = (_, _, _) => { };
#pragma warning restore MA0046

        public void RaiseNonConventionalEvent(string first, int second, string third)
        {
            NonConventionalEvent.Invoke(first, second, third);
        }

        public void RaiseEventWithoutSender()
        {
#pragma warning disable AV1235, MA0091 // 'sender' is deliberately null
            PropertyChanged(null, new PropertyChangedEventArgs(""));
#pragma warning restore AV1235, MA0091
        }

        public void RaiseEventWithSender()
        {
            PropertyChanged(this, new PropertyChangedEventArgs(""));
        }

        public void RaiseEventWithSpecificSender(object sender)
        {
#pragma warning disable MA0091
            PropertyChanged(sender, new PropertyChangedEventArgs(""));
#pragma warning restore MA0091
        }

        public void RaiseEventWithSenderAndPropertyName(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
