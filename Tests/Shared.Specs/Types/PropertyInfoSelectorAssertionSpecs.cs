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
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithAllPropertiesVirtual));

            // Act
            Action act = () =>
                propertyInfoSelector.Should().BeVirtual();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_properties_are_virtual_but_non_virtual_properties_are_found_it_should_throw()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithNonVirtualPublicProperties));

            // Act
            Action act = () =>
                propertyInfoSelector.Should().BeVirtual();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_properties_are_virtual_but_non_virtual_properties_are_found_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithNonVirtualPublicProperties));

            // Act
            Action act = () =>
                propertyInfoSelector.Should().BeVirtual("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected all selected properties" +
                   " to be virtual because we want to test the error message," +
                   " but the following properties are not virtual:*" +
                   "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.PublicNonVirtualProperty*" +
                   "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.InternalNonVirtualProperty*" +
                   "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.ProtectedNonVirtualProperty");
        }

        [Fact]
        public void When_asserting_properties_are_not_virtual_and_they_are_not_it_should_succeed()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithNonVirtualPublicProperties));

            // Act
            Action act = () =>
                propertyInfoSelector.Should().NotBeVirtual();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_properties_are_not_virtual_but_virtual_properties_are_found_it_should_throw()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithAllPropertiesVirtual));

            // Act
            Action act = () =>
                propertyInfoSelector.Should().NotBeVirtual();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_properties_are_not_virtual_but_virtual_properties_are_found_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithAllPropertiesVirtual));

            // Act
            Action act = () =>
                propertyInfoSelector.Should().NotBeVirtual("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected all selected properties" +
                   " not to be virtual because we want to test the error message," +
                   " but the following properties are virtual*" +
                   "*ClassWithAllPropertiesVirtual.PublicVirtualProperty" +
                   "*ClassWithAllPropertiesVirtual.InternalVirtualProperty" +
                   "*ClassWithAllPropertiesVirtual.ProtectedVirtualProperty");
        }

        [Fact]
        public void When_asserting_properties_are_decorated_with_attribute_and_they_are_it_should_succeed()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                propertyInfoSelector.Should().BeDecoratedWith<DummyPropertyAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_properties_are_decorated_with_attribute_and_they_are_not_it_should_throw()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute))
                .ThatArePublicOrInternal;

            // Act
            Action act = () =>
                propertyInfoSelector.Should().BeDecoratedWith<DummyPropertyAttribute>();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_properties_are_decorated_with_attribute_and_they_are_not_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                propertyInfoSelector.Should()
                                    .BeDecoratedWith<DummyPropertyAttribute>("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected properties to be decorated with" +
                   " FluentAssertions.Specs.DummyPropertyAttribute because we want to test the error message," +
                   " but the following properties are not:*" +
                   "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.PublicProperty*" +
                   "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.InternalProperty*" +
                   "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.ProtectedProperty");
        }

        [Fact]
        public void When_asserting_properties_are_not_decorated_with_attribute_and_they_are_not_it_should_succeed()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                propertyInfoSelector.Should().NotBeDecoratedWith<DummyPropertyAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_properties_are_not_decorated_with_attribute_and_they_are_it_should_throw()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute))
                .ThatArePublicOrInternal;

            // Act
            Action act = () =>
                propertyInfoSelector.Should().NotBeDecoratedWith<DummyPropertyAttribute>();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_asserting_properties_are_not_decorated_with_attribute_and_they_are_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute));

            // Act
            Action act = () =>
                propertyInfoSelector.Should()
                                    .NotBeDecoratedWith<DummyPropertyAttribute>("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected all selected properties not to be decorated*" +
                   "DummyPropertyAttribute*" +
                   "because we want to test the error message*" +
                   "ClassWithAllPropertiesDecoratedWithDummyAttribute.PublicProperty*" +
                   "ClassWithAllPropertiesDecoratedWithDummyAttribute.InternalProperty*" +
                   "ClassWithAllPropertiesDecoratedWithDummyAttribute.ProtectedProperty*");
        }

        [Fact]
        public void When_a_read_only_property_is_expected_to_be_writable_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithReadOnlyProperties));

            // Act
            Action action = () => propertyInfoSelector.Should().BeWritable("because we want to test the error {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected all selected properties to have a setter because we want to test the error message, " +
                    "but the following properties do not:*" +
                    "String FluentAssertions.Specs.ClassWithReadOnlyProperties.ReadOnlyProperty*" +
                    "String FluentAssertions.Specs.ClassWithReadOnlyProperties.ReadOnlyProperty2");
        }

        [Fact]
        public void When_writable_properties_are_expected_to_be_writable_it_should_not_throw()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithOnlyWritableProperties));

            // Act
            Action action = () => propertyInfoSelector.Should().BeWritable();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_a_writable_property_is_expected_to_be_read_only_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithWritableProperties));

            // Act
            Action action = () => propertyInfoSelector.Should().NotBeWritable("because we want to test the error {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected selected properties to not have a setter because we want to test the error message, " +
                    "but the following properties do:*" +
                    "String FluentAssertions.Specs.ClassWithWritableProperties.ReadWriteProperty*" +
                    "String FluentAssertions.Specs.ClassWithWritableProperties.ReadWriteProperty2");
        }

        [Fact]
        public void When_read_only_properties_are_expected_to_not_be_writable_it_should_not_throw()
        {
            // Arrange
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithOnlyReadOnlyProperties));

            // Act
            Action action = () => propertyInfoSelector.Should().NotBeWritable();

            // Assert
            action.Should().NotThrow();
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

    internal class ClassWithWritableProperties
    {
        public string ReadOnlyProperty { get { return ""; } }
        public string ReadWriteProperty { get { return ""; } set { } }
        public string ReadWriteProperty2 { get { return ""; } set { } }
    }

    internal class ClassWithOnlyWritableProperties
    {
        public string ReadWriteProperty { set { } }
    }

    internal class ClassWithOnlyReadOnlyProperties
    {
        public string ReadOnlyProperty { get { return ""; } }
        public string ReadOnlyProperty2 { get { return ""; } }
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
