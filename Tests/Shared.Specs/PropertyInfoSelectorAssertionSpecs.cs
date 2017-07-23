using System;

using FluentAssertions.Types;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class PropertyInfoSelectorAssertionSpecs
    {
        [Fact]
        public void When_asserting_properties_are_virtual_and_they_are_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithAllPropertiesVirtual));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfoSelector.Should().BeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_properties_are_virtual_but_non_virtual_properties_are_found_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithNonVirtualPublicProperties));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfoSelector.Should().BeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>();
        }

        [Fact]
        public void
            When_asserting_properties_are_virtual_but_non_virtual_properties_are_found_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithNonVirtualPublicProperties));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfoSelector.Should().BeVirtual("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
               .WithMessage("Expected all selected properties" +
                   " to be virtual because we want to test the error message," +
                   " but the following properties are not virtual:\r\n" +
                   "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.PublicNonVirtualProperty\r\n" +
                   "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.InternalNonVirtualProperty\r\n" +
                   "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.ProtectedNonVirtualProperty");
        }

        [Fact]
        public void When_asserting_properties_are_decorated_with_attribute_and_they_are_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfoSelector.Should().BeDecoratedWith<DummyPropertyAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [Fact]
        public void When_asserting_properties_are_decorated_with_attribute_and_they_are_not_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute))
                .ThatArePublicOrInternal;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfoSelector.Should().BeDecoratedWith<DummyPropertyAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>();
        }

        [Fact]
        public void
            When_asserting_properties_are_decorated_with_attribute_and_they_are_not_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfoSelector.Should()
                                    .BeDecoratedWith<DummyPropertyAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage("Expected all selected properties to be decorated with" +
                   " FluentAssertions.Specs.DummyPropertyAttribute because we want to test the error message," +
                   " but the following properties are not:\r\n" +
                   "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.PublicProperty\r\n" +
                   "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.InternalProperty\r\n" +
                   "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.ProtectedProperty");
        }

        [Fact]
        public void When_a_read_only_property_is_expected_to_be_writable_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithReadOnlyProperties));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfoSelector.Should().BeWritable("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage(
                    "Expected all selected properties to have a setter because we want to test the error message, " +
                    "but the following properties do not:\r\n" +
                    "String FluentAssertions.Specs.ClassWithReadOnlyProperties.ReadOnlyProperty\r\n" +
                    "String FluentAssertions.Specs.ClassWithReadOnlyProperties.ReadOnlyProperty2");
        }

        [Fact]
        public void When_writeable_properties_are_expected_to_be_writable_it_should_not_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithOnlyWritableProperties));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfoSelector.Should().BeWritable();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }
    }

    #region Internal classes used in unit tests

    internal class ClassWithAllPropertiesVirtual
    {
        public virtual string PublicVirtualProperty { set { } }

        internal virtual string InternalVirtualProperty { get { return null; } }

        protected virtual string ProtectedVirtualProperty { get; set; }
    }

    internal interface IInterfaceWithProperty
    {
        string PublicNonVirtualProperty { get; set; }
    }

    internal class ClassWithNonVirtualPublicProperties : IInterfaceWithProperty
    {
        public string PublicNonVirtualProperty { get; set; }

        internal string InternalNonVirtualProperty { get; set; }

        protected string ProtectedNonVirtualProperty { get; set; }
    }

    internal class ClassWithReadOnlyProperties
    {
        public string ReadOnlyProperty { get { return ""; } }
        public string ReadOnlyProperty2 { get { return ""; } }
        public string ReadWriteProperty { get { return ""; } set { } }
    }

    internal class ClassWithOnlyWritableProperties
    {
        public string ReadWriteProperty { set { } }
    }

    internal class ClassWithAllPropertiesDecoratedWithDummyAttribute
    {
        [DummyProperty("Value")]
        public string PublicProperty { get; set; }

        [DummyProperty("Value")]
        [DummyProperty("OtherValue")]
        public string PublicPropertyWithSameAttributeTwice { get; set; }

        [DummyProperty]
        internal string InternalProperty { get; set; }

        [DummyProperty]
        protected string ProtectedProperty { get; set; }
    }

    internal class ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute
    {
        public string PublicProperty { get; set; }

        internal string InternalProperty { get; set; }

        protected string ProtectedProperty { get; set; }
    }

    #endregion
}