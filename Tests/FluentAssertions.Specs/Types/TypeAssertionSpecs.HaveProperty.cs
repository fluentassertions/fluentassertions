using System;
using FluentAssertionsAsync.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]HaveProperty specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class HaveProperty
    {
        [Fact]
        public void When_asserting_a_type_has_a_property_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveProperty(typeof(string), "PrivateWriteProtectedReadProperty")
                    .Which.Should()
                    .BeWritable(CSharpAccessModifier.Private)
                    .And.BeReadable(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), "PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected String *ClassWithNoMembers.PublicProperty to exist *failure message*, but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_which_it_has_with_a_different_type_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveProperty(typeof(int), "PrivateWriteProtectedReadProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected String *.ClassWithMembers.PrivateWriteProtectedReadProperty to be of type System.Int32 " +
                    "*failure message*, but it is not.");
        }

        [Fact]
        public void When_subject_is_null_have_property_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), "PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected String type.PublicProperty to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_of_null_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(null, "PublicProperty");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("propertyType");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty(typeof(string), string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }

    public class HavePropertyOfT
    {
        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveProperty<string>("PrivateWriteProtectedReadProperty")
                    .Which.Should()
                    .BeWritable(CSharpAccessModifier.Private)
                    .And.BeReadable(CSharpAccessModifier.Protected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty<string>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_has_a_propertyOfT_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveProperty<string>(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }

    public class NotHaveProperty
    {
        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty("Property");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty("PrivateWriteProtectedReadProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected String *.ClassWithMembers.PrivateWriteProtectedReadProperty to not exist *failure message*" +
                    ", but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_property_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveProperty("PublicProperty", "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type.PublicProperty to not exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_with_a_null_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_a_property_with_an_empty_name_it_should_throw()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveProperty(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("name");
        }
    }
}
