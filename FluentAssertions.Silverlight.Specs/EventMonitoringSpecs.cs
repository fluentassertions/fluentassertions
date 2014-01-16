using System;
using System.ComponentModel;

using FluentAssertions.Events;
using FluentAssertions.Primitives;

#if WINRT || WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Silverlight.Specs
{
    [TestClass]
    public class EventMonitoringSpecs
    {
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
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                "Expected object*EventRaisingClass*to raise event*PropertyChanged*for*SomeProperty*because the property was changed, but it did not.",
                ComparisonMode.Wildcard);
        }
        
        [TestMethod]
        public void When_a_property_changed_event_for_a_specific_property_was_raised_but_without_sender_it_should_throw()
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
                .ShouldRaisePropertyChangeFor(x => x.SomeProperty, "the property was changed")
                .WithSender(subject);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected sender*EventRaisingClass*, but found <null>.", ComparisonMode.Wildcard);
        }

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

        public class EventRaisingClass : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };

            public void RaiseEventWithoutSender()
            {
                PropertyChanged(null, new PropertyChangedEventArgs("SomeProperty"));
            }

            public void RaiseEventWithSender()
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SomeProperty"));
            }

            public void RaiseEventWithSenderAndPropertyName(string propertyName)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            public string SomeProperty { get; set; }
        }
    }
}