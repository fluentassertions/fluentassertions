using System.IO;
using System.Reflection.Emit;
using System.Reflection;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;
using Formatter = FluentAssertions.Formatting.Formatter;

#if NET45
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace FluentAssertions.Specs
{
    public class EventAssertionSpecs
    {
        [Fact]
        public void When_asserting_an_event_raise_and_the_object_is_not_monitored_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaise("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>().WithMessage(
                "Object <FluentAssertions.Specs.EventAssertionSpecs+EventRaisingClass> is not being monitored for events or has already been garbage collected. " +
                "Use the MonitorEvents() extension method to start monitoring events.");
        }

        [Fact]
        public void When_asserting_that_an_event_was_not_raised_and_the_object_is_not_monitored_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldNotRaise("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>().WithMessage(
                "Object <FluentAssertions.Specs.EventAssertionSpecs+EventRaisingClass> is not being monitored for events or has already been garbage collected. " +
                "Use the MonitorEvents() extension method to start monitoring events.");
        }

        [Fact]
        public void When_asserting_an_event_that_doesnt_exist_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaise("NonExistingEvent");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>().WithMessage(
                "Not monitoring any events named \"NonExistingEvent\".");
        }

        [Fact]
        public void When_asserting_that_an_event_was_not_raised_and_it_doesnt_exist_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldNotRaise("NonExistingEvent");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>().WithMessage(
                "Not monitoring any events named \"NonExistingEvent\".");
        }

        [Fact]
        public void When_an_event_was_not_raised_it_should_throw_and_use_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaise("PropertyChanged", "{0} should cause the event to get raised", "Foo()");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                " to raise event \"PropertyChanged\" because Foo() should cause the event to get raised, but it did not.");
        }

        [Fact]
        public void When_the_expected_event_was_raised_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithoutSender();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaise("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_an_unexpected_event_was_raised_it_should_throw_and_use_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithoutSender();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldNotRaise("PropertyChanged", "{0} should cause the event to get raised", "Foo()");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected object " + Formatter.ToString(subject) +
                             " to not raise event \"PropertyChanged\" because Foo() should cause the event to get raised, but it did.");
        }

        [Fact]
        public void When_an_unexpected_event_was_not_raised_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldNotRaise("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_the_event_sender_is_not_the_expected_object_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithoutSender();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaise("PropertyChanged").WithSender(subject);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage("Expected sender " + Formatter.ToString(subject) +
                                                          ", but found <null>.");
        }

        [Fact]
        public void When_the_event_sender_is_the_expected_object_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithSender();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaise("PropertyChanged").WithSender(subject);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_the_event_parameters_dont_match_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithoutSender();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject
                .ShouldRaise("PropertyChanged")
                .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "SomeProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected at least one event with arguments matching (args.PropertyName == \"SomeProperty\"), but found none.");
        }

#if NET45
        [Fact]
        public void When_the_event_parameter_is_of_a_different_type_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject
                .ShouldRaise("PropertyChanged")
                .WithArgs<UnhandledExceptionEventArgs>(args => args.IsTerminating);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<ArgumentException>()
                .WithMessage("No argument of event PropertyChanged is of type <System.UnhandledExceptionEventArgs>.");
        }

#endif

        [Fact]
        public void When_the_event_parameters_do_match_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject
                .ShouldRaise("PropertyChanged")
                .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "SomeProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_running_in_parallel_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            Action<int> action = _ =>
            {
                EventRaisingClass subject = new EventRaisingClass();
                subject.MonitorEvents();
                subject.RaiseEventWithSender();
                subject.ShouldRaise("PropertyChanged");
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => Enumerable.Range(0, 1000)
                .AsParallel()
                .ForAll(action);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_a_monitored_class_event_has_fired_it_should_be_possible_to_reset_the_event_monitor()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            var eventMonitor = subject.MonitorEvents();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            eventMonitor.Reset();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.ShouldNotRaise("PropertyChanged");
        }

        [Fact]
        public void When_a_non_conventional_event_with_a_specific_argument_was_raised_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject
                .ShouldRaise("NonConventionalEvent")
                .WithArgs<string>(args => args == "third argument");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_a_non_conventional_event_with_many_specific_arguments_was_raised_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject
                .ShouldRaise("NonConventionalEvent")
                .WithArgs<string>(null, args => args == "third argument");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_a_non_conventional_event_with_a_specific_argument_was_not_raised_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            const int wrongArgument = 3;
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject
                .ShouldRaise("NonConventionalEvent")
                .WithArgs<int>(args => args == wrongArgument);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected at least one event with arguments matching (args == " + wrongArgument + "), but found none.");
        }

        [Fact]
        public void When_a_non_conventional_event_with_many_specific_arguments_was_not_raised_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            const string wrongArgument = "not a third argument";
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseNonConventionalEvent("first argument", 2, "third argument");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject
                .ShouldRaise("NonConventionalEvent")
                .WithArgs<string>(null, args => args == wrongArgument);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected at least one event with arguments matching \"(args == \\\"" + wrongArgument + "\\\")\", but found none.");
        }



        [Fact]
        public void When_trying_to_attach_to_notify_property_changed_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            var eventMonitor = subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => eventMonitor.Attach(typeof(INotifyPropertyChanged));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_trying_to_attach_to_something_other_than_notify_property_changed_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            var eventMonitor = subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => eventMonitor.Attach(typeof(EventRaisingClass));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NotSupportedException>()
                .WithMessage("Cannot monitor events of type \"EventRaisingClass\".");
        }


#region Should(Not)RaisePropertyChanged events

        [Fact]
        public void When_a_property_changed_event_was_raised_for_the_expected_property_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");
            subject.RaiseEventWithSenderAndPropertyName("SomeOtherProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaisePropertyChangeFor(x => x.SomeProperty);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_an_expected_property_changed_event_was_raised_for_all_properties_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithSenderAndPropertyName(null);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaisePropertyChangeFor(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_a_property_changed_event_was_raised_by_monitored_class_it_should_be_possible_to_reset_the_event_monitor()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            var eventMonitor = subject.MonitorEvents();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            eventMonitor.Reset();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.ShouldNotRaisePropertyChangeFor(e => e.SomeProperty);
        }

        [Fact]
        public void When_a_property_changed_event_for_an_unexpected_property_was_raised_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithSenderAndPropertyName("SomeProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldNotRaisePropertyChangeFor(x => x.SomeProperty, "nothing happened");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Did not expect object " + Formatter.ToString(subject) +
                " to raise the \"PropertyChanged\" event for property \"SomeProperty\" because nothing happened, but it did.");
        }

        [Fact]
        public void When_a_property_changed_event_for_a_specific_property_was_not_raised_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaisePropertyChangeFor(x => x.SomeProperty, "the property was changed");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                " to raise event \"PropertyChanged\" for property \"SomeProperty\" because the property was changed, but it did not.");
        }

        [Fact]
        public void When_a_property_agnostic_property_changed_event_for_was_not_raised_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaisePropertyChangeFor(null);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                " to raise event \"PropertyChanged\" for property <null>, but it did not.");
        }

        [Fact]
        public void When_a_property_changed_event_for_another_than_the_unexpected_property_was_raised_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            subject.MonitorEvents();
            subject.RaiseEventWithSenderAndPropertyName("SomeOtherProperty");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldNotRaisePropertyChangeFor(x => x.SomeProperty);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

#endregion

#region General Checks

        [Fact]
        public void When_monitoring_a_null_object_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            EventRaisingClass subject = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NullReferenceException>().WithMessage("Cannot monitor the events of a <null> object.");
        }

        [Fact]
        public void When_monitoring_class_it_should_be_possible_to_obtain_a_recorder()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = new EventRaisingClass();
            var eventMonitor = eventSource.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var recorder = eventMonitor.GetEventRecorder("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            recorder.Should().NotBeNull();
            recorder.EventObject.Should().BeSameAs(eventSource);
            recorder.EventName.Should().Be("PropertyChanged");
        }

        [Fact]
        public void When_monitoring_class_requesting_to_monitor_again_should_return_same_monitor()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = new EventRaisingClass();
            var eventMonitor = eventSource.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var newMonitor = eventSource.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            newMonitor.Should().BeSameAs(eventMonitor);
        }

        [Fact]
        public void When_no_recorder_exists_for_an_event_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = new EventRaisingClass();
            var eventMonitor = eventSource.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => eventMonitor.GetEventRecorder("SomeEvent");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Not monitoring any events named \"SomeEvent\".");
        }

        [Fact]
        public void When_a_monitored_class_is_not_referenced_anymore_it_should_be_garbage_collected()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            var referenceToSubject = new WeakReference(subject);
            subject.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            subject = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            referenceToSubject.IsAlive.Should().BeFalse();
        }

        [Fact]
        public void
            When_a_monitored_class_is_not_referenced_anymore_it_should_be_garbage_collected_also_if_an_Event_passing_Sender_was_raised
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new EventRaisingClass();
            var referenceToSubject = new WeakReference(subject);
            subject.MonitorEvents();
            subject.RaiseEventWithSender();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            subject = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            referenceToSubject.IsAlive.Should().BeFalse();
        }

#if NET45

        private object CreateProxyObject()
        {
            Type baseType = typeof(EventRaisingClass);
            Type interfaceType = typeof(IEventRaisingInterface);
            AssemblyName assemblyName = new AssemblyName {Name = baseType.Assembly.FullName + ".GeneratedForTest"};
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName,
                AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
            string typeName = baseType.Name + "_GeneratedForTest";
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public, baseType, new[] {interfaceType});

            Func<string, MethodBuilder> emitAddRemoveEventHandler = methodName =>
            {
                MethodBuilder method =
                    typeBuilder.DefineMethod(string.Format("{0}.{1}_InterfaceEvent", interfaceType.FullName, methodName),
                        MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig |
                        MethodAttributes.NewSlot);
                method.SetReturnType(typeof(void));
                method.SetParameters(typeof(EventHandler));
                ILGenerator gen = method.GetILGenerator();
                gen.Emit(OpCodes.Ret);
                return method;
            };
            MethodBuilder addHandler = emitAddRemoveEventHandler("add");
            typeBuilder.DefineMethodOverride(addHandler, interfaceType.GetMethod("add_InterfaceEvent"));
            MethodBuilder removeHandler = emitAddRemoveEventHandler("remove");
            typeBuilder.DefineMethodOverride(removeHandler, interfaceType.GetMethod("remove_InterfaceEvent"));

            Type generatedType = typeBuilder.CreateType();
            return Activator.CreateInstance(generatedType);
        }

        [Fact]
        public void When_the_fallback_assertion_exception_crosses_appdomain_boundaries_it_should_be_serializable()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            new AssertionFailedException("").Should().BeBinarySerializable();
        }

        [Fact]
        public void When_the_fallback_assertion_exception_crosses_appdomain_boundaries_it_should_be_deserializable()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            Exception exception = new AssertionFailedException("Message");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------

            // Save the full ToString() value, including the exception message and stack trace.
            string exceptionToString = exception.ToString();

            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new SimpleBinder(exception.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // "Save" object state
                formatter.Serialize(memoryStream, exception);

                // Re-use the same stream for de-serialization
                memoryStream.Seek(0, 0);

                // Replace the original exception with de-serialized one
                exception = (AssertionFailedException)formatter.Deserialize(memoryStream);
            }

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------

            // Double-check that the exception message and stack trace (owned by the base Exception) are preserved
            exceptionToString.Should().Be(exception.ToString(), "exception.ToString()");
        }

        private class SimpleBinder : SerializationBinder
        {
            private Type type;

            public SimpleBinder(Type type)
            {
                this.type = type;
            }

            public override Type BindToType(string assemblyName, string typeName)
            {
                if ((type.FullName == typeName) && (type.Assembly.FullName == assemblyName))
                {
                    return type;
                }
                else
                {
                    return null;
                }
            }
        }

        [Fact]
        public void When_monitoring_interface_of_a_class_it_should_be_possible_to_obtain_a_recorder()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = CreateProxyObject();
            var eventMonitor = eventSource.MonitorEvents<IEventRaisingInterface>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var recorder = eventMonitor.GetEventRecorder("InterfaceEvent");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            recorder.Should().NotBeNull();
            recorder.EventObject.Should().BeSameAs(eventSource);
            recorder.EventName.Should().Be("InterfaceEvent");
        }

        [Fact]
        public void When_monitoring_interface_of_a_class_and_no_recorder_exists_for_an_event_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = CreateProxyObject();
            var eventMonitor = eventSource.MonitorEvents<IEventRaisingInterface>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => eventMonitor.GetEventRecorder("SomeEvent");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Not monitoring any events named \"SomeEvent\".");
        }

        [Fact]
        public void When_no_recorder_exists_for_an_event_in_monitored_interface_of_a_class_but_exists_in_the_class_it_should_throw
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = CreateProxyObject();
            var eventMonitor = eventSource.MonitorEvents<IEventRaisingInterface>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => eventMonitor.GetEventRecorder("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Not monitoring any events named \"PropertyChanged\".");
        }

        [Fact]
        public void When_trying_to_monitor_events_of_unimplemented_interface_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = CreateProxyObject();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => eventSource.MonitorEvents<IEventRaisingInterface2>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<TargetException>()
                .WithMessage("*not match target type*");
        }
#endif

        [Fact]
        public void When_monitoring_more_than_one_event_on_a_class_it_should_be_possible_to_reset_just_one()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new TestEventRaising();
            var eventRecorder = subject.MonitorEvents<IEventRaisingInterface>().GetEventRecorder("InterfaceEvent");
            subject.MonitorEvents<IEventRaisingInterface2>();
            subject.RaiseBothEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            eventRecorder.Reset();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.ShouldNotRaise("InterfaceEvent");
            subject.ShouldRaise("Interface2Event");
        }

        [Fact]
        public void When_monitoring_a_class_it_should_be_possible_to_attach_to_additional_interfaces_on_the_same_object()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //----------------------------------------------------------------------------------------------------------
            var subject = new TestEventRaising();
            subject.MonitorEvents<IEventRaisingInterface>();
            subject.MonitorEvents<IEventRaisingInterface2>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            subject.RaiseBothEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            subject.ShouldRaise("InterfaceEvent");
            subject.ShouldRaise("Interface2Event");
        }

        public class TestEventRaising : IEventRaisingInterface, IEventRaisingInterface2
        {
            public event EventHandler InterfaceEvent;
            public event EventHandler Interface2Event;

            public void RaiseBothEvents()
            {
                InterfaceEvent?.Invoke(this, new EventArgs());
                Interface2Event?.Invoke(this, new EventArgs());
            }
        }


#endregion

        public interface IEventRaisingInterface
        {
            event EventHandler InterfaceEvent;
        }

        public interface IEventRaisingInterface2
        {
            event EventHandler Interface2Event;
        }

        public class EventRaisingClass : INotifyPropertyChanged
        {
            public string SomeProperty { get; set; }

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