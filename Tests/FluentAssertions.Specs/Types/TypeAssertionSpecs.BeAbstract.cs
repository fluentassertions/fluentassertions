using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]BeAbstract specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class BeAbstract
    {
        [Fact]
        public void When_type_is_abstract_it_succeeds()
        {
            // Arrange / Act / Assert
            typeof(Abstract).Should().BeAbstract();
        }

        [Theory]
        [InlineData(typeof(ClassWithoutMembers), "Expected type *.ClassWithoutMembers to be abstract.")]
        [InlineData(typeof(Sealed), "Expected type *.Sealed to be abstract.")]
        [InlineData(typeof(Static), "Expected type *.Static to be abstract.")]
        public void When_type_is_not_abstract_it_fails(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().BeAbstract();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_type_is_not_abstract_it_fails_with_a_meaningful_message()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () => type.Should().BeAbstract("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.ClassWithoutMembers to be abstract *failure message*.");
        }

        [Theory]
        [InlineData(typeof(IDummyInterface), "*.IDummyInterface must be a class.")]
        [InlineData(typeof(Struct), "*.Struct must be a class.")]
        [InlineData(typeof(ExampleDelegate), "*.ExampleDelegate must be a class.")]
        public void When_type_is_not_valid_for_BeAbstract_it_throws_exception(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().BeAbstract();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_subject_is_null_be_abstract_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().BeAbstract("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type to be abstract *failure message*, but type is <null>.");
        }
    }

    public class NotBeAbstract
    {
        [Theory]
        [InlineData(typeof(ClassWithoutMembers))]
        [InlineData(typeof(Sealed))]
        [InlineData(typeof(Static))]
        public void When_type_is_not_abstract_it_succeeds(Type type)
        {
            // Arrange / Act / Assert
            type.Should().NotBeAbstract();
        }

        [Fact]
        public void When_type_is_abstract_it_fails()
        {
            // Arrange
            var type = typeof(Abstract);

            // Act
            Action act = () => type.Should().NotBeAbstract();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.Abstract not to be abstract.");
        }

        [Fact]
        public void When_type_is_abstract_it_fails_with_a_meaningful_message()
        {
            // Arrange
            var type = typeof(Abstract);

            // Act
            Action act = () => type.Should().NotBeAbstract("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type *.Abstract not to be abstract *failure message*.");
        }

        [Theory]
        [InlineData(typeof(IDummyInterface), "*.IDummyInterface must be a class.")]
        [InlineData(typeof(Struct), "*.Struct must be a class.")]
        [InlineData(typeof(ExampleDelegate), "*.ExampleDelegate must be a class.")]
        public void When_type_is_not_valid_for_NotBeAbstract_it_throws_exception(Type type, string exceptionMessage)
        {
            // Act
            Action act = () => type.Should().NotBeAbstract();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(exceptionMessage);
        }

        [Fact]
        public void When_subject_is_null_not_be_abstract_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotBeAbstract("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type not to be abstract *failure message*, but type is <null>.");
        }
    }
}
