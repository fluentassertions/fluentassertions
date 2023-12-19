using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Types;

/// <content>
/// The [Not]HaveImplicitConversionOperator specs.
/// </content>
public partial class TypeAssertionSpecs
{
    public class HaveImplicitConversionOperator
    {
        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operator_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(int);

            // Act
            Action act = () =>
                type.Should()
                    .HaveImplicitConversionOperator(sourceType, targetType)
                    .Which.Should()
                    .NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operator_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(string);

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator(
                    sourceType, targetType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_implicit_conversion_operator_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator(
                    typeof(TypeWithConversionOperators), typeof(string), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operator_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator(null, typeof(string));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("sourceType");
        }

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operator_to_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator(typeof(TypeWithConversionOperators), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("targetType");
        }
    }

    public class HaveImplicitConversionOperatorOfT
    {
        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operatorOfT_which_it_does_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should()
                    .HaveImplicitConversionOperator<TypeWithConversionOperators, int>()
                    .Which.Should()
                    .NotBeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_has_an_implicit_conversion_operatorOfT_which_it_does_not_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator<TypeWithConversionOperators, string>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but it does not.");
        }

        [Fact]
        public void When_subject_is_null_have_implicit_conversion_operatorOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().HaveImplicitConversionOperator<TypeWithConversionOperators, string>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to exist *failure message*" +
                    ", but type is <null>.");
        }
    }

    public class NotHaveImplicitConversionOperator
    {
        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operator_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(string);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveImplicitConversionOperator(sourceType, targetType);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operator_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);
            var sourceType = typeof(TypeWithConversionOperators);
            var targetType = typeof(int);

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator(
                    sourceType, targetType, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.Int32(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_implicit_conversion_operator_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator(
                    typeof(TypeWithConversionOperators), typeof(string), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but type is <null>.");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operator_from_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator(null, typeof(string));

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("sourceType");
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operator_to_null_it_should_throw()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator(typeof(TypeWithConversionOperators), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("targetType");
        }
    }

    public class NotHaveImplicitConversionOperatorOfT
    {
        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operatorOfT_which_it_does_not_it_succeeds()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should()
                    .NotHaveImplicitConversionOperator<TypeWithConversionOperators, string>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_type_does_not_have_an_implicit_conversion_operatorOfT_which_it_does_it_fails()
        {
            // Arrange
            var type = typeof(TypeWithConversionOperators);

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator<TypeWithConversionOperators, int>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.Int32(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but it does.");
        }

        [Fact]
        public void When_subject_is_null_not_have_implicit_conversion_operatorOfT_should_fail()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () =>
                type.Should().NotHaveImplicitConversionOperator<TypeWithConversionOperators, string>(
                    "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected public static implicit *.String(*.TypeWithConversionOperators) to not exist *failure message*" +
                    ", but type is <null>.");
        }
    }
}
