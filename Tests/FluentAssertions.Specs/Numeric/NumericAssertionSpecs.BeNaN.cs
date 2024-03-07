using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeNaN
    {
        [Fact]
        public void When_a_float_value_is_NaN_it_should_succeed()
        {
            // Arrange
            float actual = float.NaN;

            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(-1f)]
        [InlineData(0f)]
        [InlineData(1f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.Epsilon)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [Theory]
        public void When_a_float_value_is_not_NaN_it_should_fail(float actual)
        {
            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_float_value_is_not_NaN_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            float actual = 1;

            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected actual to be NaN, but found 1F.");
        }

        [Fact]
        public void When_a_float_value_is_returned_from_BeNaN_it_should_chain()
        {
            // Arrange
            float actual = float.NaN;

            // Act
            Action act = () => actual.Should().BeNaN()
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_double_value_is_NaN_it_should_succeed()
        {
            // Arrange
            double actual = double.NaN;

            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(-1d)]
        [InlineData(0d)]
        [InlineData(1d)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [Theory]
        public void When_a_double_value_is_not_NaN_it_should_fail(double actual)
        {
            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_double_value_is_not_NaN_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            double actual = 1;

            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected actual to be NaN, but found 1.0.");
        }

        [Fact]
        public void When_a_double_value_is_returned_from_BeNaN_it_should_chain()
        {
            // Arrange
            double actual = double.NaN;

            // Act
            Action act = () => actual.Should().BeNaN()
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_float_value_is_NaN_it_should_succeed()
        {
            // Arrange
            float? actual = float.NaN;

            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(null)]
        [InlineData(-1f)]
        [InlineData(0f)]
        [InlineData(1f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.Epsilon)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [Theory]
        public void When_a_nullable_float_value_is_not_NaN_it_should_fail(float? actual)
        {
            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_float_value_is_not_NaN_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            float? actual = 1;

            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected actual to be NaN, but found 1F.");
        }

        [Fact]
        public void When_a_nullable_float_value_is_returned_from_BeNaN_it_should_chain()
        {
            // Arrange
            float? actual = float.NaN;

            // Act
            Action act = () => actual.Should().BeNaN()
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_double_value_is_NaN_it_should_succeed()
        {
            // Arrange
            double? actual = double.NaN;

            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(null)]
        [InlineData(-1d)]
        [InlineData(0d)]
        [InlineData(1d)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [Theory]
        public void When_a_nullable_double_value_is_not_NaN_it_should_fail(double? actual)
        {
            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_double_value_is_not_NaN_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            double? actual = 1;

            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected actual to be NaN, but found 1.0.");
        }

        [Fact]
        public void When_a_nullable_double_value_is_returned_from_BeNaN_it_should_chain()
        {
            // Arrange
            double? actual = double.NaN;

            // Act
            Action act = () => actual.Should().BeNaN()
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class NotBeNaN
    {
        [InlineData(-1f)]
        [InlineData(0f)]
        [InlineData(1f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.Epsilon)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [Theory]
        public void When_a_float_value_is_not_NaN_it_should_succeed(float actual)
        {
            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_float_value_is_NaN_it_should_fail()
        {
            // Arrange
            float actual = float.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_float_value_is_NaN_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            float actual = float.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Did not expect actual to be NaN.");
        }

        [Fact]
        public void When_a_float_value_is_returned_from_NotBeNaN_it_should_chain()
        {
            // Arrange
            float actual = 1;

            // Act
            Action act = () => actual.Should().NotBeNaN()
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(-1d)]
        [InlineData(0d)]
        [InlineData(1d)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [Theory]
        public void When_a_double_value_is_not_NaN_it_should_succeed(double actual)
        {
            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_double_value_is_NaN_it_should_fail()
        {
            // Arrange
            double actual = double.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_double_value_is_NaN_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            double actual = double.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Did not expect actual to be NaN.");
        }

        [Fact]
        public void When_a_double_value_is_returned_from_NotBeNaN_it_should_chain()
        {
            // Arrange
            double actual = 1;

            // Act
            Action act = () => actual.Should().NotBeNaN()
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(null)]
        [InlineData(-1f)]
        [InlineData(0f)]
        [InlineData(1f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.Epsilon)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [Theory]
        public void When_a_nullable_float_value_is_not_NaN_it_should_succeed(float? actual)
        {
            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_float_value_is_NaN_it_should_fail()
        {
            // Arrange
            float? actual = float.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_float_value_is_NaN_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            float? actual = float.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Did not expect actual to be NaN.");
        }

        [Fact]
        public void When_a_nullable_float_value_is_returned_from_NotBeNaN_it_should_chain()
        {
            // Arrange
            float? actual = 1;

            // Act
            Action act = () => actual.Should().NotBeNaN()
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(null)]
        [InlineData(-1d)]
        [InlineData(0d)]
        [InlineData(1d)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData(double.Epsilon)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [Theory]
        public void When_a_nullable_double_value_is_not_NaN_it_should_succeed(double? actual)
        {
            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_double_value_is_NaN_it_should_fail()
        {
            // Arrange
            double? actual = double.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_double_value_is_NaN_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            double? actual = double.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Did not expect actual to be NaN.");
        }

        [Fact]
        public void When_a_nullable_double_value_is_returned_from_NotBeNaN_it_should_chain()
        {
            // Arrange
            double? actual = 1;

            // Act
            Action act = () => actual.Should().NotBeNaN()
                .And.Be(actual);

            // Assert
            act.Should().NotThrow();
        }
    }
}
