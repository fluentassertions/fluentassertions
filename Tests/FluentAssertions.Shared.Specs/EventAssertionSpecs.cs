using System;
using System.ComponentModel;
using System.Linq;
using FluentAssertions.Events;
using FluentAssertions.Formatting;

#if NET40 || NET45
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions.Execution;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Reflection;
#endif

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Reflection.Emit;
using System.Reflection;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class EventAssertionSpecs
    {
#if !WINRT && !SILVERLIGHT && !WINDOWS_PHONE_APP && !CORE_CLR

        #region Should(Not)Raise

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                "Type <" + subject.GetType().Name + "> does not expose an event named \"NonExistingEvent\".");
        }

        [TestMethod]
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
                "Type <" + subject.GetType().Name + "> does not expose an event named \"NonExistingEvent\".");
        }

        [TestMethod]
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
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                    " to raise event \"PropertyChanged\" because Foo() should cause the event to get raised, but it did not.");
        }

        [TestMethod]
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

        [TestMethod]
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
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected object " + Formatter.ToString(subject) +
                    " to not raise event \"PropertyChanged\" because Foo() should cause the event to get raised, but it did.");
        }

        [TestMethod]
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

        [TestMethod]
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
            act.ShouldThrow<AssertFailedException>().WithMessage("Expected sender " + Formatter.ToString(subject) +
                ", but found <null>.");
        }

        [TestMethod]
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

        [TestMethod]
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
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected at least one event with arguments matching (args.PropertyName == \"SomeProperty\"), but found none.");
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        #endregion

#endif

        #region Should(Not)RaisePropertyChanged events

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect object " + Formatter.ToString(subject) +
                    " to raise the \"PropertyChanged\" event for property \"SomeProperty\" because nothing happened, but it did.");
        }

        [TestMethod]
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
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                    " to raise event \"PropertyChanged\" for property \"SomeProperty\" because the property was changed, but it did not.");
        }

        [TestMethod]
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
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected object " + Formatter.ToString(subject) +
                    " to raise event \"PropertyChanged\" for property <null>, but it did not.");
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void When_monitoring_class_it_should_be_possible_to_obtain_a_recorder()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = new EventRaisingClass();
            eventSource.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var recorder = eventSource.GetRecorderForEvent("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            recorder.Should().NotBeNull();
            recorder.EventObject.Should().BeSameAs(eventSource);
            recorder.EventName.Should().Be("PropertyChanged");
        }

        [TestMethod]
        public void When_a_class_is_not_being_monitored_it_should_not_be_possible_to_get_a_recorder()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = new EventRaisingClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => eventSource.GetRecorderForEvent("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("*not being monitored*");
        }

        [TestMethod]
        public void When_no_recorder_exists_for_an_event_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = new EventRaisingClass();
            eventSource.MonitorEvents();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => eventSource.GetRecorderForEvent("SomeEvent");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("*not expose*SomeEvent*");
        }

        [TestMethod]
        public void When_monitoring_interface_of_a_class_it_should_be_possible_to_obtain_a_recorder()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = CreateProxyObject();
            eventSource.MonitorEvents<IEventRaisingInterface>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var recorder = eventSource.GetRecorderForEvent("InterfaceEvent");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            recorder.Should().NotBeNull();
            recorder.EventObject.Should().BeSameAs(eventSource);
            recorder.EventName.Should().Be("InterfaceEvent");
        }

        [TestMethod]
        public void When_monitoring_interface_of_a_class_and_no_recorder_exists_for_an_event_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = CreateProxyObject();
            eventSource.MonitorEvents<IEventRaisingInterface>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => eventSource.GetRecorderForEvent("SomeEvent");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("*not expose*SomeEvent*");
        }

        [TestMethod]
        public void When_no_recorder_exists_for_an_event_in_monitored_interface_of_a_class_but_exists_in_the_class_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var eventSource = CreateProxyObject();
            eventSource.MonitorEvents<IEventRaisingInterface>();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => eventSource.GetRecorderForEvent("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("*not expose*PropertyChanged*");
        }


        [TestMethod]
        public void When_a_monitored_class_in_not_referenced_anymore_it_should_be_garbage_collected()
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

        [TestMethod]
        public void When_a_monitored_class_in_not_referenced_anymore_it_should_be_garbage_collected_also_if_an_Event_passing_Sender_was_raised()
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

#if NET40 || NET45
        [TestMethod]
        public void When_the_fallback_assertion_exception_crosses_appdomain_boundaries_it_should_be_serializable()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            new AssertFailedException("").Should().BeBinarySerializable();
        }

        [TestMethod]
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
            Assert.AreEqual(exceptionToString, exception.ToString(), "exception.ToString()");
        }

#endif

        #endregion

        public class EventRaisingClass : INotifyPropertyChanged
        {
            public string SomeProperty { get; set; }
            public event PropertyChangedEventHandler PropertyChanged = delegate { };

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

        public interface IEventRaisingInterface
        {
            event EventHandler InterfaceEvent;
        }

        private object CreateProxyObject()
        {
            Type baseType = typeof(EventRaisingClass);
            Type interfaceType = typeof(IEventRaisingInterface);
            AssemblyName assemblyName = new AssemblyName() { Name = baseType.Assembly.FullName + ".GeneratedForTest" };
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, false);
            string typeName = baseType.Name + "_GeneratedForTest";
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public, baseType, new Type[] { interfaceType });

            Func<string, MethodBuilder> emitAddRemoveEventHandler = (methodName) =>
            {
                MethodBuilder method = typeBuilder.DefineMethod(string.Format("{0}.{1}_InterfaceEvent", interfaceType.FullName, methodName),
                    MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot);
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
    }
}