using System;
using System.Reflection;
using FluentAssertionsAsync.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]HaveAccessModifier specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class HaveAccessModifier
    {
        [Fact]
        public void When_asserting_a_public_type_is_public_it_succeeds()
        {
            // Arrange
            Type type = typeof(IPublicInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_public_member_is_internal_it_throws()
        {
            // Arrange
            Type type = typeof(IPublicInterface);

            // Act
            Action act = () =>
                type
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type IPublicInterface to be Internal *failure message*, but it is Public.");
        }

        [Fact]
        public void An_internal_class_has_an_internal_access_modifier()
        {
            // Arrange
            Type type = typeof(InternalClass);

            // Act / Assert
            type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);
        }

        [Fact]
        public void An_internal_interface_has_an_internal_access_modifier()
        {
            // Arrange
            Type type = typeof(IInternalInterface);

            // Act / Assert
            type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);
        }

        [Fact]
        public void An_internal_struct_has_an_internal_access_modifier()
        {
            // Arrange
            Type type = typeof(InternalStruct);

            // Act / Assert
            type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);
        }

        [Fact]
        public void An_internal_enum_has_an_internal_access_modifier()
        {
            // Arrange
            Type type = typeof(InternalEnum);

            // Act / Assert
            type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);
        }

        [Fact]
        public void An_internal_class_does_not_have_a_protected_internal_modifier()
        {
            // Arrange
            Type type = typeof(InternalClass);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalClass to be ProtectedInternal *failure message*, but it is Internal.");
        }

        [Fact]
        public void An_internal_interface_does_not_have_a_protected_internal_modifier()
        {
            // Arrange
            Type type = typeof(IInternalInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type IInternalInterface to be ProtectedInternal *failure message*, but it is Internal.");
        }

        [Fact]
        public void An_internal_struct_does_not_have_a_protected_internal_modifier()
        {
            // Arrange
            Type type = typeof(InternalStruct);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalStruct to be ProtectedInternal *failure message*, but it is Internal.");
        }

        [Fact]
        public void An_internal_enum_does_not_have_a_protected_internal_modifier()
        {
            // Arrange
            Type type = typeof(InternalEnum);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalEnum to be ProtectedInternal *failure message*, but it is Internal.");
        }

        [Fact]
        public void When_asserting_a_nested_private_type_is_private_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_private_type_is_protected_it_throws()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Protected, "we want to test the failure {0}",
                    "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type PrivateClass to be Protected *failure message*, but it is Private.");
        }

        [Fact]
        public void When_asserting_a_nested_protected_type_is_protected_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_protected_type_is_public_it_throws()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type ProtectedEnum to be Public, but it is Protected.");
        }

        [Fact]
        public void When_asserting_a_nested_public_type_is_public_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.IPublicInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_public_member_is_internal_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.IPublicInterface);

            // Act
            Action act = () =>
                type
                    .Should()
                    .HaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type IPublicInterface to be Internal *failure message*, but it is Public.");
        }

        [Fact]
        public void When_asserting_a_nested_internal_type_is_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.InternalClass);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Internal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_internal_type_is_protected_internal_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.InternalClass);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalClass to be ProtectedInternal *failure message*, but it is Internal.");
        }

        [Fact]
        public void When_asserting_a_nested_protected_internal_member_is_protected_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.IProtectedInternalInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_protected_internal_member_is_private_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.IProtectedInternalInterface);

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Private, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type IProtectedInternalInterface to be Private *failure message*, but it is ProtectedInternal.");
        }

        [Fact]
        public void When_subject_is_null_have_access_modifier_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be Public *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_access_modifier_with_an_invalid_enum_value_it_should_throw()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveAccessModifier((CSharpAccessModifier)int.MaxValue);

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithParameterName("accessModifier");
        }
    }

    public class NotHaveAccessModifier
    {
        [Fact]
        public void When_asserting_a_public_type_is_not_private_it_succeeds()
        {
            // Arrange
            Type type = typeof(IPublicInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_public_member_is_not_public_it_throws()
        {
            // Arrange
            Type type = typeof(IPublicInterface);

            // Act
            Action act = () =>
                type
                    .Should()
                    .NotHaveAccessModifier(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type IPublicInterface not to be Public *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_an_internal_type_is_not_protected_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(InternalClass);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_internal_type_is_not_internal_it_throws()
        {
            // Arrange
            Type type = typeof(InternalClass);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the failure {0}",
                    "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalClass not to be Internal *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_private_type_is_not_protected_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_private_type_is_not_private_it_throws()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("PrivateClass", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Private, "we want to test the failure {0}",
                    "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type PrivateClass not to be Private *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_protected_type_is_not_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Internal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_protected_type_is_not_protected_it_throws()
        {
            // Arrange
            Type type = typeof(Nested).GetNestedType("ProtectedEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Protected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type ProtectedEnum not to be Protected, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_public_type_is_not_private_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.IPublicInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_public_member_is_not_public_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.IPublicInterface);

            // Act
            Action act = () =>
                type
                    .Should()
                    .NotHaveAccessModifier(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type IPublicInterface not to be Public *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_internal_type_is_not_protected_internal_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.InternalClass);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.ProtectedInternal);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_internal_type_is_not_internal_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.InternalClass);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Internal, "we want to test the failure {0}",
                    "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type InternalClass not to be Internal *failure message*, but it is.");
        }

        [Fact]
        public void When_asserting_a_nested_protected_internal_member_is_not_public_it_succeeds()
        {
            // Arrange
            Type type = typeof(Nested.IProtectedInternalInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Public);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_nested_protected_internal_member_is_not_protected_internal_it_throws()
        {
            // Arrange
            Type type = typeof(Nested.IProtectedInternalInterface);

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(
                    CSharpAccessModifier.ProtectedInternal, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type IProtectedInternalInterface not to be ProtectedInternal *failure message*, but it is.");
        }

        [Fact]
        public void When_subject_is_null_not_have_access_modifier_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier(CSharpAccessModifier.Public, "we want to test the failure {0}",
                    "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type not to be Public *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_access_modifier_with_an_invalid_enum_value_it_should_throw()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveAccessModifier((CSharpAccessModifier)int.MaxValue);

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithParameterName("accessModifier");
        }
    }
}
