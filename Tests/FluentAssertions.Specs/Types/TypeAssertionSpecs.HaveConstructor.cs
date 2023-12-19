using System;
using FluentAssertionsAsync.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]HaveConstructor specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class HaveConstructor
    {
        [Fact]
        public void When_asserting_a_type_has_a_constructor_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveConstructor(new[] { typeof(string) })
                    .Which.Should().HaveAccessModifier(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_constructor_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveConstructor(new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected constructor *ClassWithNoMembers(System.Int32, System.Type) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_constructor_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveConstructor(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected constructor type(System.String) to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_constructor_of_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveConstructor(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }
    }

    public class NotHaveConstructor
    {
        [Fact]
        public void When_asserting_a_type_does_not_have_a_constructor_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveConstructor(new[] { typeof(string) });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_constructor_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveConstructor(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected constructor *.ClassWithMembers(System.String) not to exist *failure message*, but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_constructor_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveConstructor(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected constructor type(System.String) not to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_constructor_of_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveConstructor(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }
    }
}
