using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeNaN
    {
        [Fact]
        public void NaN_is_equal_to_NaN_when_its_a_float()
        {
            // Arrange
            float actual = float.NaN;

            // Act / Assert
            actual.Should().BeNaN();
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
        public void Should_fail_when_asserting_normal_float_value_to_be_NaN(float actual)
        {
            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_a_descriptive_message_when_asserting_normal_float_value_to_be_NaN()
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
        public void Should_chain_when_asserting_NaN_as_float()
        {
            // Arrange
            float actual = float.NaN;

            // Act / Assert
            actual.Should().BeNaN()
                .And.Be(actual);
        }

        [Fact]
        public void NaN_is_equal_to_NaN_when_its_a_double()
        {
            // Arrange
            double actual = double.NaN;

            // Act / Assert
            actual.Should().BeNaN();
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
        public void Should_fail_when_asserting_normal_double_value_to_be_NaN(double actual)
        {
            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_a_descriptive_message_when_asserting_normal_double_value_to_be_NaN()
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
        public void Should_chain_when_asserting_NaN_as_double()
        {
            // Arrange
            double actual = double.NaN;

            // Act / Assert
            actual.Should().BeNaN()
                .And.Be(actual);
        }

        [Fact]
        public void NaN_is_equal_to_NaN_when_its_a_nullable_float()
        {
            // Arrange
            float? actual = float.NaN;

            // Act / Assert
            actual.Should().BeNaN();
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
        public void Should_fail_when_asserting_nullable_normal_float_value_to_be_NaN(float? actual)
        {
            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_a_descriptive_message_when_asserting_nullable_normal_float_value_to_be_NaN()
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
        public void Should_chain_when_asserting_NaN_as_nullable_float()
        {
            // Arrange
            float? actual = float.NaN;

            // Act / Assert
            actual.Should().BeNaN()
                .And.Be(actual);
        }

        [Fact]
        public void NaN_is_equal_to_NaN_when_its_a_nullable_double()
        {
            // Arrange
            double? actual = double.NaN;

            // Act / Assert
            actual.Should().BeNaN();
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
        public void Should_fail_when_asserting_nullable_normal_double_value_to_be_NaN(double? actual)
        {
            // Act
            Action act = () => actual.Should().BeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_a_descriptive_message_when_asserting_nullable_normal_double_value_to_be_NaN()
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
        public void Should_chain_when_asserting_NaN_as_nullable_double()
        {
            // Arrange
            double? actual = double.NaN;

            // Act / Assert
            actual.Should().BeNaN()
                .And.Be(actual);
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
        public void Normal_float_is_never_equal_to_NaN(float actual)
        {
            // Act / Assert
            actual.Should().NotBeNaN();
        }

        [Fact]
        public void Should_fail_when_asserting_NaN_as_float()
        {
            // Arrange
            float actual = float.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_a_descriptive_message_when_asserting_NaN_as_float()
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
        public void Should_chain_when_asserting_normal_float_value()
        {
            // Arrange
            float actual = 1;

            // Act / Assert
            actual.Should().NotBeNaN()
                .And.Be(actual);
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
        public void Normal_double_is_never_equal_to_NaN(double actual)
        {
            // Act / Assert
            actual.Should().NotBeNaN();
        }

        [Fact]
        public void Should_fail_when_asserting_NaN_as_double()
        {
            // Arrange
            double actual = double.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_a_descriptive_message_when_asserting_NaN_as_double()
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
        public void Should_chain_when_asserting_normal_double_value()
        {
            // Arrange
            double actual = 1;

            // Act / Assert
            actual.Should().NotBeNaN()
                .And.Be(actual);
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
        public void Normal_nullable_float_is_never_equal_to_NaN(float? actual)
        {
            // Act / Assert
            actual.Should().NotBeNaN();
        }

        [Fact]
        public void Should_fail_when_asserting_NaN_as_nullable_float()
        {
            // Arrange
            float? actual = float.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_a_descriptive_message_when_asserting_NaN_as_nullable_float()
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
        public void Should_chain_when_asserting_normal_nullable_float_value()
        {
            // Arrange
            float? actual = 1;

            // Act / Assert
            actual.Should().NotBeNaN()
                .And.Be(actual);
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
        public void Normal_nullable_double_is_never_equal_to_NaN(double? actual)
        {
            // Act / Assert
            actual.Should().NotBeNaN();
        }

        [Fact]
        public void Should_fail_when_asserting_NaN_as_nullable_double()
        {
            // Arrange
            double? actual = double.NaN;

            // Act
            Action act = () => actual.Should().NotBeNaN();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_a_descriptive_message_when_asserting_NaN_as_nullable_double()
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
        public void Should_chain_when_asserting_normal_nullable_double_value()
        {
            // Arrange
            double? actual = 1;

            // Act / Assert
            actual.Should().NotBeNaN()
                .And.Be(actual);
        }
    }
}
