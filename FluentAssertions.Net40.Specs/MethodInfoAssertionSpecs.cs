using System;
using System.Reflection;

using FluentAssertions.Common;
using FluentAssertions.Types;

#if WINRT || WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class MethodInfoAssertionSpecs
    {
        [TestMethod]
        public void When_asserting_a_method_is_virtual_and_it_is_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsVirtual).GetMethodNamed("PublicVirtualDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeVirtual();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_method_is_virtual_but_it_is_not_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithNonVirtualPublicMethods).GetMethodNamed("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeVirtual("we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.PublicDoNothing" +
                    " to be virtual because we want to test the error message," +
                    " but it is not virtual.");
        }

        [TestMethod]
        public void When_asserting_a_method_is_decorated_with_attribute_and_it_is_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetMethodNamed("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_method_is_decorated_with_an_attribute_it_should_allow_chaining_assertions_on_it()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetMethodNamed("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>().Which.Filter.Should.NotBeEmpty();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_asserting_a_method_is_decorated_with_an_attribute_but_it_is_not_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetMethodNamed("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing to be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was not found.");
        }

        [TestMethod]
        public void When_asserting_a_method_is_decorated_with_attribute_matching_a_predicate_and_it_is_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetMethodNamed("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>(d => d.Filter);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_method_is_decorated_with_an_attribute_matching_a_predeicate_but_it_is_not_it_should_throw_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetMethodNamed("PublicDoNothing");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>(d => !d.Filter, "because we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing to be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was not found.");
        }
    }

    #region Internal classes used in unit tests

    internal class ClassWithAllMethodsVirtual
    {
        public virtual void PublicVirtualDoNothing()
        {
        }

        internal virtual void InternalVirtualDoNothing()
        {
        }

        protected virtual void ProtectedVirtualDoNothing()
        {
        }
    }

    internal interface IInterfaceWithPublicMethod
    {
        void PublicDoNothing();
    }

    internal class ClassWithNonVirtualPublicMethods : IInterfaceWithPublicMethod
    {
        public void PublicDoNothing()
        {
        }

        internal void InternalDoNothing()
        {
        }

        protected void ProtectedDoNothing()
        {
        }
    }

    internal class ClassWithAllMethodsDecoratedWithDummyAttribute
    {
        [DummyMethod(Filter = true)]
        public void PublicDoNothing()
        {
        }

        [DummyMethod]
        protected void ProtectedDoNothing()
        {
        }

        [DummyMethod]
        private void PrivateDoNothing()
        {
        }
    }

    internal class ClassWithMethodsThatAreNotDecoratedWithDummyAttribute
    {
        public void PublicDoNothing()
        {
        }

        protected void ProtectedDoNothing()
        {
        }

        private void PrivateDoNothing()
        {
        }
    }

    #endregion
}