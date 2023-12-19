using System;
using FluentAssertionsAsync.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]HaveIndexer specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class HaveIndexer
    {
        [Fact]
        public void When_asserting_a_type_has_an_indexer_which_it_does_then_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should()
                    .HaveIndexer(typeof(string), new[] { typeof(string) })
                    .Which.Should()
                    .BeWritable(CSharpAccessModifier.Internal)
                    .And.BeReadable(CSharpAccessModifier.Private);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_indexer_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithNoMembers);

            // Act
            Action act = () =>
                type.Should().HaveIndexer(
                    typeof(string), new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected String *ClassWithNoMembers[System.Int32, System.Type] to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_indexer_with_different_parameters_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveIndexer(
                    typeof(string), new[] { typeof(int), typeof(Type) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected String *.ClassWithMembers[System.Int32, System.Type] to exist *failure message*, but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_indexer_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveIndexer(typeof(string), new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected String type[System.String] to exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_indexer_for_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveIndexer(null, new[] { typeof(string) });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("indexerType");
        }

        [Fact]
        public void When_asserting_a_type_has_an_indexer_with_a_null_parameter_type_list_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().HaveIndexer(typeof(string), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }
    }

    public class NotHaveIndexer
    {
        [Fact]
        public void When_asserting_a_type_does_not_have_an_indexer_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(ClassWithoutMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveIndexer(new[] { typeof(string) });

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_indexer_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveIndexer(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected indexer *.ClassWithMembers[System.String] to not exist *failure message*, but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_indexer_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveIndexer(new[] { typeof(string) }, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected indexer type[System.String] to not exist *failure message*, but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_indexer_for_null_it_should_throw()
        {
            // Arrange
            Type type = typeof(ClassWithMembers);

            // Act
            Action act = () =>
                type.Should().NotHaveIndexer(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("parameterTypes");
        }
    }
}
