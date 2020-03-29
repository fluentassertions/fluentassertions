using System;
using System.Reflection;
using FluentAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class MethodBaseAssertionSpecs
    {
        #region Return

        [Fact]
        public void When_asserting_an_int_method_returns_int_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().Return(typeof(int));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_int_method_returns_string_it_fails_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().Return(typeof(string), "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the return type of method IntMethod to be System.String because we want to test the " +
                             "error message, but it is \"System.Int32\".");
        }

        [Fact]
        public void When_asserting_a_void_method_returns_string_it_fails_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("VoidMethod");

            // Act
            Action act = () =>
                methodInfo.Should().Return(typeof(string), "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the return type of method VoidMethod to be System.String because we want to test the " +
                             "error message, but it is \"System.Void\".");
        }

        #endregion

        #region NotReturn

        [Fact]
        public void When_asserting_an_int_method_does_not_return_string_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotReturn(typeof(string));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_int_method_does_not_return_int_it_fails_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotReturn(typeof(int), "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the return type of*IntMethod*not to be System.Int32*because we want to test the " +
                             "error message, but it is.");
        }

        #endregion

        #region ReturnOfT

        [Fact]
        public void When_asserting_an_int_method_returnsOfT_int_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().Return<int>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_int_method_returnsOfT_string_it_fails_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().Return<string>("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the return type of method IntMethod to be System.String because we want to test the " +
                             "error message, but it is \"System.Int32\".");
        }

        #endregion

        #region NotReturnOfT

        [Fact]
        public void When_asserting_an_int_method_does_not_returnsOfT_string_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotReturn<string>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_int_method_does_not_returnsOfT_int_it_fails_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotReturn<int>("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the return type of*IntMethod*not to be System.Int32*because we want to test the " +
                             "error message, but it is.");
        }

        #endregion

        #region ReturnVoid

        [Fact]
        public void When_asserting_a_void_method_returns_void_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("VoidMethod");

            // Act
            Action act = () =>
                methodInfo.Should().ReturnVoid();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_int_method_returns_void_it_fails_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().ReturnVoid("because we want to test the error message {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the return type of method IntMethod to be void because we want to test the error message " +
                             "message, but it is \"System.Int32\".");
        }

        #endregion

        #region NotReturnVoid

        [Fact]
        public void When_asserting_an_int_method_does_not_return_void_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("IntMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotReturnVoid();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_void_method_does_not_return_void_it_fails_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("VoidMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotReturnVoid("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the return type of*VoidMethod*not to be void*because we want to test the error message*");
        }

        #endregion

        #region HaveAccessModifier

        [Fact]
        public void When_asserting_a_private_member_is_private_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("PrivateMethod");

            // Act
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_private_protected_member_is_private_protected_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("PrivateProtectedMethod");

            // Act
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.PrivateProtected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_private_member_is_protected_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("PrivateMethod");

            // Act
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.Protected, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method PrivateMethod to be Protected because we want to test the error message, but it is " +
                             "Private.");
        }

        [Fact]
        public void When_asserting_a_protected_member_is_protected_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("ProtectedSetProperty");

            MethodInfo setMethod = propertyInfo.SetMethod;

            // Act
            Action act = () =>
                setMethod.Should().HaveAccessModifier(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_protected_member_is_public_it_throws_with_a_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("ProtectedSetProperty");

            MethodInfo setMethod = propertyInfo.SetMethod;

            // Act
            Action act = () =>
                setMethod
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method set_ProtectedSetProperty to be Public because we want to test the error message, but it" +
                             " is Protected.");
        }

        [Fact]
        public void When_asserting_a_public_member_is_public_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("PublicGetProperty");

            MethodInfo getMethod = propertyInfo.GetMethod;

            // Act
            Action act = () =>
                getMethod.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_public_member_is_internal_it_throws_with_a_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("PublicGetProperty");

            MethodInfo getMethod = propertyInfo.GetMethod;

            // Act
            Action act = () =>
                getMethod
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method get_PublicGetProperty to be Internal because we want to test the error message, but it" +
                             " is Public.");
        }

        [Fact]
        public void When_asserting_an_internal_member_is_internal_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("InternalMethod");

            // Act
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.Internal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_internal_member_is_protectedInternal_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("InternalMethod");

            // Act
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal, "because we want to test the" +
                                                                                                " error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method InternalMethod to be ProtectedInternal because we want to test the error message, but" +
                             " it is Internal.");
        }

        [Fact]
        public void When_asserting_a_protected_internal_member_is_protected_internal_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("ProtectedInternalMethod");

            // Act
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_protected_internal_member_is_private_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("ProtectedInternalMethod");

            // Act
            Action act = () =>
                methodInfo.Should().HaveAccessModifier(CSharpAccessModifier.Private, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method ProtectedInternalMethod to be Private because we want to test the error message, but it is " +
                             "ProtectedInternal.");
        }

        #endregion

        #region NotHaveAccessModifier

        [Fact]
        public void When_asserting_a_private_member_is_not_protected_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("PrivateMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotHaveAccessModifier(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_private_member_is_not_private_protected_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("PrivateMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotHaveAccessModifier(CSharpAccessModifier.PrivateProtected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_private_member_is_not_private_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("PrivateMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotHaveAccessModifier(CSharpAccessModifier.Private, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method PrivateMethod not to be Private*because we want to test the error message*");
        }

        [Fact]
        public void When_asserting_a_protected_member_is_not_internal_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("ProtectedSetProperty");
            MethodInfo setMethod = propertyInfo.SetMethod;

            // Act
            Action act = () =>
                setMethod.Should().NotHaveAccessModifier(CSharpAccessModifier.Internal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_protected_member_is_not_protected_it_throws_with_a_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("ProtectedSetProperty");
            MethodInfo setMethod = propertyInfo.SetMethod;

            // Act
            Action act = () =>
                setMethod
                    .Should()
                    .NotHaveAccessModifier(CSharpAccessModifier.Protected, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method set_ProtectedSetProperty not to be Protected*because we want to test the error message*");
        }

        [Fact]
        public void When_asserting_a_public_member_is_not_private_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("PublicGetProperty");
            MethodInfo getMethod = propertyInfo.GetMethod;

            // Act
            Action act = () =>
                getMethod.Should().NotHaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_private_protected_member_is_not_private_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("PublicGetPrivateProtectedSet");
            MethodInfo setMethod = propertyInfo.SetMethod;

            // Act
            Action act = () =>
                setMethod.Should().NotHaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_public_member_is_not_public_it_throws_with_a_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(TestClass).GetPropertyByName("PublicGetProperty");
            MethodInfo getMethod = propertyInfo.GetMethod;

            // Act
            Action act = () =>
                getMethod
                    .Should()
                    .NotHaveAccessModifier(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method get_PublicGetProperty not to be Public*because we want to test the error message*");
        }

        [Fact]
        public void When_asserting_an_internal_member_is_not_protectedInternal_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("InternalMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotHaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_internal_member_is_not_internal_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("InternalMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotHaveAccessModifier(CSharpAccessModifier.Internal, "because we want to test the" +
                                                                                                " error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method InternalMethod not to be Internal*because we want to test the error message*");
        }

        [Fact]
        public void When_asserting_a_protected_internal_member_is_not_public_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("ProtectedInternalMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotHaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_protected_internal_member_is_not_protected_internal_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(TestClass).GetParameterlessMethod("ProtectedInternalMethod");

            // Act
            Action act = () =>
                methodInfo.Should().NotHaveAccessModifier(CSharpAccessModifier.ProtectedInternal, "we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method ProtectedInternalMethod not to be ProtectedInternal*because we want to test the error message*");
        }

        #endregion
    }

    #region Internal classes used in unit tests

    internal class TestClass
    {
        public void VoidMethod() { }

        public int IntMethod() { return 0; }

        private void PrivateMethod() { }

        public string PublicGetProperty { get; private set; }

        protected string ProtectedSetProperty { private get; set; }

        public string PublicGetPrivateProtectedSet { get; private protected set; }

        internal string InternalMethod() { return null; }

        protected internal void ProtectedInternalMethod() { }

        private protected void PrivateProtectedMethod() { }
    }

    #endregion
}
