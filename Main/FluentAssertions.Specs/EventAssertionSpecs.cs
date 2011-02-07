using System;
using System.ComponentModel;

using FluentAssertions.EventMonitoring;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class EventAssertionSpecs
    {
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
                "Object <" + subject +
                    "> is not being monitored for events. Use the MonitorEvents() extension method to start monitoring events.");
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
                "Object <" + subject +
                    "> is not being monitored for events. Use the MonitorEvents() extension method to start monitoring events.");
        }

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
                "Object <" + subject + "> does not expose an event named \"NonExistingEvent\".");
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
                "Object <" + subject + "> does not expose an event named \"NonExistingEvent\".");
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
                "Expected object <" + subject + "> to raise event \"PropertyChanged\" because Foo() should cause the event to get raised, but it did not.");
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
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected object <" + subject + "> to not raise event \"PropertyChanged\" because Foo() should cause the event to get raised, but it did.");
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
            act.ShouldThrow<AssertFailedException>().WithMessage("Expected sender <" + subject + ">, but found <null>.");
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
                .WithMessage("Expected at least one event with arguments matching (args.PropertyName == \"SomeProperty\"), but found none.");
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

        #endregion

        #region Should(Not)RaisePropertyChanged events

        [TestMethod]
        public void When_a_property_changed_event_was_raised_for_the_right_property_it_should_not_throw()
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
                "Did not expect object <" + subject + "> to raise the \"PropertyChanged\" event for property \"SomeProperty\" because nothing happened, but it did.");
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
                "Expected object <" + subject + "> to raise event \"PropertyChanged\" because the property was changed, but it did not.");
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

        internal class EventRaisingClass : INotifyPropertyChanged
        {
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

            public string SomeProperty { get; set; }
        }
    }
}