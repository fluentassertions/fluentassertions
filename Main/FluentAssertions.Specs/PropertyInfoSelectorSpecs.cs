using System;
using System.Collections.Generic;
using System.Reflection;

using FluentAssertions.Assertions;
using Internal.Main.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class PropertyInfoSelectorSpecs
    {
        [TestMethod]
        public void When_selecting_properties_from_types_in_an_assembly_it_should_return_the_applicable_properties()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<PropertyInfo> properties = assembly.Types()
                .ThatAreDecoratedWith<SomeAttribute>()
                .Properties();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            properties.Should()
                .HaveCount(2)
                .And.Contain(m => m.Name == "Property1")
                .And.Contain(m => m.Name == "Property2");
        }

        [TestMethod]
        public void When_selecting_properties_that_are_public_or_internal_it_should_return_only_the_applicable_properties()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(TestClassForPropertySelector);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<PropertyInfo> properties = type.Properties().ThatArePublicOrInternal.ToArray();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            const int PublicPropertyCount = 3;
            const int InternalcPropertyCount = 1;
            properties.Should().HaveCount(PublicPropertyCount + InternalcPropertyCount);
        }

        [TestMethod]
        public void When_selecting_properties_decorated_with_specific_attribute_it_should_return_only_the_applicable_properties()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(TestClassForPropertySelector);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreDecoratedWith<DummyPropertyAttribute>().ToArray();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            properties.Should().HaveCount(2);
        }

        [TestMethod]
        public void When_selecting_methods_that_return_a_specific_type_it_should_return_only_the_applicable_methods()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(TestClassForPropertySelector);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<PropertyInfo> properties = type.Properties().OfType<string>().ToArray();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            properties.Should().HaveCount(2);
        }

        [TestMethod]
        public void When_combining_filters_to_filter_methods_it_should_return_only_the_applicable_methods()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            Type type = typeof(TestClassForPropertySelector);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<PropertyInfo> properties = type.Properties()
                .ThatArePublicOrInternal
                .OfType<string>()
                .ThatAreDecoratedWith<DummyPropertyAttribute>()
                .ToArray();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            properties.Should().HaveCount(1);
        }
    }

    #region Internal classes used in unit tests

    internal class TestClassForPropertySelector
    {
        public virtual string PublicVirtualStringProperty { get; set; }

        [DummyProperty]
        public virtual string PublicVirtualStringPropertyWithAttribute { get; set; }

        public virtual int PublicVirtualIntPropertyWithPrivateSetter { get; private set; }

        internal virtual int InternalVirtualIntPropertyWithPrivateSetter { get; private set; }

        [DummyProperty]
        protected virtual int ProtectedVirtualIntPropertyWithAttribute { get; set; }

        private int PrivateIntProperty { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DummyPropertyAttribute : Attribute
    {
    }

    #endregion
}