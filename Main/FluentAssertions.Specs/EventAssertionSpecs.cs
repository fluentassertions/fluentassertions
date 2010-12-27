using System;
using System.ComponentModel;

using FluentAssertions.EventMonitoring;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class EventAssertionSpecs
    {
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
            subject.RaiseEvent();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldRaise("PropertyChanged");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        public class EventRaisingClass : INotifyPropertyChanged
        {
            public void RaiseEvent()
            {
                PropertyChanged(this, new PropertyChangedEventArgs(""));
            }
            
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }
    }
}