using System;
using System.Reflection;


#if !OLD_MSTEST
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
        public void When_a_virtual_property_is_expected_to_be_virtual_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesVirtual).GetRuntimeProperty("PublicVirtualProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesVirtual).GetProperty("PublicVirtualProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_non_virtual_property_is_expected_to_be_virtual_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithNonVirtualPublicProperties).GetRuntimeProperty("PublicNonVirtualProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithNonVirtualPublicProperties).GetProperty("PublicNonVirtualProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeVirtual("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
               .WithMessage(
                   "Expected property String FluentAssertions.Specs.ClassWithNonVirtualPublicProperties.PublicNonVirtualProperty" +
                       " to be virtual because we want to test the error message," +
                       " but it is not virtual.");
        }

        [TestMethod]
        public void When_asserting_a_property_is_decorated_with_attribute_and_it_is_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetProperty("PublicProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }        
        
        [TestMethod]
        public void When_a_property_is_decorated_with_an_attribute_it_should_allow_chaining_assertions()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetProperty("PublicProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>().Which.Value.Should().Be("OtherValue");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage("Expected*OtherValue*Value*");
        }
        
        [TestMethod]
        public void When_a_property_is_decorated_with_an_attribute_and_multiple_attributes_match_continuation_using_the_matched_value_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicPropertyWithSameAttributeTwice");
#else
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetProperty("PublicPropertyWithSameAttributeTwice");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>().Which.Value.Should().Be("OtherValue");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_a_property_is_decorated_with_attribute_and_it_is_not_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");
#else
            PropertyInfo propertyInfo =
                typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute).GetProperty("PublicProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>("because we want to test the error message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
               .WithMessage("Expected property String " +
                   "FluentAssertions.Specs.ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.PublicProperty to be decorated with " +
                   "FluentAssertions.Specs.DummyPropertyAttribute because we want to test the error message, but that attribute was not found.");
        }

        [TestMethod]
        public void When_a_read_only_property_is_expected_to_be_writable_it_should_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadOnlyProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetProperty("ReadOnlyProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeWritable("that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected property ReadOnlyProperty to have a setter because that's required.");
        }
        
        [TestMethod]
        public void When_a_read_write_property_is_expected_to_be_writable_it_should_not_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if WINRT || WINDOWS_PHONE_APP
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadWriteProperty");
#else
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetProperty("ReadWriteProperty");
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action action = () => propertyInfo.Should().BeWritable("that's required");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            action.ShouldNotThrow();
        }
    }
}