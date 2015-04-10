using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Types;
#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class MethodBaseAssertionSpecs
    {
        [TestMethod]
        public void When_asserting_a_private_member_is_private_it_should_not_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetMethodNamed("PrivateMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifiers.Private);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_private_member_is_not_private_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetMethodNamed("PrivateMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifiers.Protected, "because we want to test the failure");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method PrivateMethod to be Protected because we want to test the failure, but it is " +
                             "Private.");
        }
        [TestMethod]
        public void When_asserting_a_protected_member_is_protected_it_should_not_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyNamed("ProtectedSetProperty");

            MethodInfo setMethod;

#if NETFX_CORE || WINRT
            setMethod = propertyInfo.SetMethod;
#else
            setMethod = propertyInfo.GetSetMethod(true);
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                setMethod.Should().HaveAccessModifier(CSharpAccessModifiers.Protected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_protected_member_is_not_protected_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyNamed("ProtectedSetProperty");

            MethodInfo setMethod;

#if NETFX_CORE || WINRT
            setMethod = propertyInfo.SetMethod;
#else
            setMethod = propertyInfo.GetSetMethod(true);
#endif
            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                setMethod
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifiers.Public, "because we want to test the failure");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method set_ProtectedSetProperty to be Public because we want to test the failure, but it" +
                             " is Protected.");
        }
        [TestMethod]
        public void When_asserting_a_public_member_is_public_it_should_not_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyNamed("PublicGetProperty");

            MethodInfo getMethod;

#if NETFX_CORE || WINRT
            getMethod = propertyInfo.SetMethod;
#else
            getMethod = propertyInfo.GetSetMethod(true);
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                getMethod.Should().HaveAccessModifier(CSharpAccessModifiers.Public);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_public_member_is_not_public_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyNamed("PublicGetProperty");

            MethodInfo getMethod;

#if NETFX_CORE || WINRT
            getMethod = propertyInfo.SetMethod;
#else
            getMethod = propertyInfo.GetSetMethod(true);
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                getMethod
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifiers.Internal, "because we want to test the failure");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method get_PublicGetProperty to be Internal because we want to test the failure, but it" +
                             " is Public.");
        }
        [TestMethod]
        public void When_asserting_an_internal_member_is_internal_it_should_not_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetMethodNamed("InternalMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifiers.Internal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_internal_member_is_not_internal_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetMethodNamed("InternalMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifiers.ProtectedInternal, "because we want to test the" +
                                                                                                " failure");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method InternalMethod to be ProtectedInternal because we want to test the failure, but" +
                             " it is Internal.");
        }
        [TestMethod]
        public void When_asserting_a_protected_internal_member_is_protected_internal_it_should_not_throw()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetMethodNamed("ProtectedInternalMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifiers.ProtectedInternal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_protected_internal_member_is_not_protected_internal_it_should_throw_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetMethodNamed("InternalMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifiers.Private, "because we want to test the failure");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method InternalMethod to be Private because we want to test the failure, but it is " +
                             "Internal.");
        }
    }

    #region Internal classes used in unit tests

    class TestClass
    {
        private void PrivateMethod() {}

        public string PublicGetProperty { get; private set; }

        protected string ProtectedSetProperty { private get; set; }

        internal void InternalMethod() {}

        protected internal void ProtectedInternalMethod() { }
    }

    #endregion
}
