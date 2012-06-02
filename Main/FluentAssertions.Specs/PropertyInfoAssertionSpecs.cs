using System;

using FluentAssertions.Types;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class PropertyInfoAssertionSpecs
    {
        [TestMethod]
        public void When_asserting_properties_are_virtual_and_they_are_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof (ClassWithAllPropertiesVirtual));

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

        [TestMethod]
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
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_properties_are_virtual_but_non_virtual_properties_are_found_it_should_throw_with_descriptive_message()
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
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected all selected properties" +
                    " to be virtual because we want to test the error message," +
                        " but the following properties are not virtual:\r\n" +
                            "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.PublicVirtualProperty\r\n" +
                                "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.InternalVirtualProperty\r\n" +
                                    "String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.ProtectedVirtualProperty");
        }

        [TestMethod]
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

        [TestMethod]
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
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_properties_are_decorated_with_attribute_and_they_are_not_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var propertyInfoSelector = new PropertyInfoSelector(typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute));

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfoSelector.Should().BeDecoratedWith<DummyPropertyAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected all selected properties to be decorated with" +
                    " FluentAssertions.Specs.DummyPropertyAttribute because we want to test the error message," +
                        " but the following properties are not:\r\n" +
                            "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.PublicVirtualProperty\r\n" +
                                "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.InternalVirtualProperty\r\n" +
                                    "String FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.ProtectedVirtualProperty");
        }
    }

    #region Internal classes used in unit tests

    internal class ClassWithAllPropertiesVirtual
    {
        public virtual string PublicVirtualProperty { get; set; }

        internal virtual string InternalVirtualProperty { get; set; }

        protected virtual string ProtectedVirtualProperty { get; set; }
    }

    internal interface IInterfaceWithProperty
    {
        string PublicVirtualProperty { get; set; }
    }

    internal class ClassWithNonVirtualPublicProperties : IInterfaceWithProperty
    {
        public string PublicVirtualProperty { get; set; }

        internal string InternalVirtualProperty { get; set; }

        protected string ProtectedVirtualProperty { get; set; }
    }

    internal class ClassWithAllPropertiesDecoratedWithDummyAttribute
    {
        [DummyProperty]
        public string PublicVirtualProperty { get; set; }

        [DummyProperty]
        internal string InternalVirtualProperty { get; set; }

        [DummyProperty]
        protected string ProtectedVirtualProperty { get; set; }
    }

    internal class ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute
    {
        public string PublicVirtualProperty { get; set; }

        internal string InternalVirtualProperty { get; set; }

        protected string ProtectedVirtualProperty { get; set; }
    }

    #endregion
}