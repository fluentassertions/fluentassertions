using System;
using System.Reflection;
using FluentAssertions.Common;
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
        #region Return

        [TestMethod]
        public void When_asserting_an_int_method_returns_int_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().Return(typeof(int));

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_int_method_returns_string_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().Return(typeof(string), "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected the return type of method IntMethod to be System.String  because we want to test the " +
                             "error message, but it is \"System.Int32\".");
        }

        [TestMethod]
        public void When_asserting_a_void_method_returns_string_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("VoidMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().Return(typeof(string), "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected the return type of method VoidMethod to be System.String  because we want to test the " +
                             "error message, but it is \"System.Void\".");
        }

        #endregion

        #region ReturnVoid

        [TestMethod]
        public void When_asserting_a_void_method_returns_void_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("VoidMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().ReturnVoid();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_int_method_returns_void_it_fails_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().ReturnVoid("because we want to test the error message {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected the return type of method IntMethod to be void  because we want to test the error message " +
                             "message, but it is \"System.Int32\".");
        }

        #endregion

        #region HaveAccessModifier

        [TestMethod]
        public void When_asserting_a_private_member_is_private_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("PrivateMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.Private);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_private_member_is_not_private_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("PrivateMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.Protected, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method PrivateMethod to be Protected because we want to test the error message, but it is " +
                             "Private.");
        }

        [TestMethod]
        public void When_asserting_a_protected_member_is_protected_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("ProtectedSetProperty");

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
                setMethod.Should().HaveAccessModifier(CSharpAccessModifier.Protected);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_protected_member_is_not_protected_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("ProtectedSetProperty");

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
                    .HaveAccessModifier(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method set_ProtectedSetProperty to be Public because we want to test the error message, but it" +
                             " is Protected.");
        }

        [TestMethod]
        public void When_asserting_a_public_member_is_public_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("PublicGetProperty");

            MethodInfo getMethod;

#if NETFX_CORE || WINRT
            getMethod = propertyInfo.GetMethod;
#else
            getMethod = propertyInfo.GetGetMethod(true);
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                getMethod.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_public_member_is_not_public_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("PublicGetProperty");

            MethodInfo getMethod;

#if NETFX_CORE || WINRT
            getMethod = propertyInfo.GetMethod;
#else
            getMethod = propertyInfo.GetGetMethod(true);
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                getMethod
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method get_PublicGetProperty to be Internal because we want to test the error message, but it" +
                             " is Public.");
        }

        [TestMethod]
        public void When_asserting_an_internal_member_is_internal_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("InternalMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.Internal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_an_internal_member_is_not_internal_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("InternalMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal, "because we want to test the" +
                                                                                                " error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method InternalMethod to be ProtectedInternal because we want to test the error message, but" +
                             " it is Internal.");
        }

        [TestMethod]
        public void When_asserting_a_protected_internal_member_is_protected_internal_it_succeeds()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("ProtectedInternalMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_asserting_a_protected_internal_member_is_not_protected_internal_it_throws_with_a_useful_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("InternalMethod");

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.Private, "we want to test the error {0}", "message");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected method InternalMethod to be Private because we want to test the error message, but it is " +
                             "Internal.");
        }

        #endregion
    }

    #region Internal classes used in unit tests

    class TestClass
    {
        public void VoidMethod() { }

        public int IntMethod() { return 0; }

        private void PrivateMethod() {}

        public string PublicGetProperty { get; private set; }

        protected string ProtectedSetProperty { private get; set; }

        internal string InternalMethod() { return null; }

        protected internal void ProtectedInternalMethod() { }
    }

    #endregion
}
