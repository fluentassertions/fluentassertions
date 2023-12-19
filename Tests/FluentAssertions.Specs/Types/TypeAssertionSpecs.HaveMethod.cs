using System;
using FluentAssertionsAsync.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]HaveMethod specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class HaveMethod
    {
        [Fact]
        public void When_asserting_a_type_has_a_method_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveMethod("VoidMethod", new Type[] { })
                    .Which.Should()
                    .HaveAccessModifier(CSharpAccessModifier.Private)
                    .And.ReturnVoid();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod(
                    "NonExistentMethod", new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method *ClassWithNoMembers.NonExistentMethod(*.Int32, *.Type) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_with_different_parameter_types_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod(
                    "VoidMethod", new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method *.ClassWithMembers.VoidMethod(*.Int32, *.Type) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_method_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveMethod("Name", new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method type.Name(System.String) to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod(null, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod(string.Empty, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_method_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveMethod("Name", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }
    }

    public class NotHaveMethod
    {
        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("NonExistentMethod", new Type[] { });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_which_it_has_with_different_parameter_types_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("VoidMethod", new[] { typeof(int) });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_that_method_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("VoidMethod", new Type[] { }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method Void *.ClassWithMembers.VoidMethod() to not exist *failure message*, but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_method_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("Name", new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method type.Name(System.String) to not exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod(null, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod(string.Empty, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_method_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveMethod("Name", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }
    }
}
