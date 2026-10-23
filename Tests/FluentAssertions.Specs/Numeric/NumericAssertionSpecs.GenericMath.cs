#if NET8_0_OR_GREATER
using System;
using System.Numerics;
using FluentAssertions.Numeric;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class GenericMath
    {
        [Fact]
        public void Half_values_are_supported()
        {
            // Arrange
            var subject = (Half)1.5;

            // Act / Assert
            subject.Should().BePositive().And.BeGreaterThan((Half)1);
        }

        [Fact]
        public void Nullable_half_values_are_supported()
        {
            // Arrange
            Half? subject = (Half)1.5;

            // Act / Assert
            subject.Should().HaveValue().And.Be((Half)1.5);
        }

        [Fact]
        public void Big_integers_are_supported()
        {
            // Arrange
            var subject = new BigInteger(42);

            // Act / Assert
            subject.Should().Be(42);
        }

        [Fact]
        public void Half_within_precision_approximates_the_expected_value()
        {
            // Arrange
            var subject = (Half)1.5;

            // Act / Assert
            subject.Should().BeApproximately((Half)1.5, (Half)0.01);
        }

        [Fact]
        public void Half_outside_precision_does_not_approximate_the_expected_value()
        {
            // Arrange
            var subject = (Half)1.5;

            // Act
            Action act = () => subject.Should().BeApproximately((Half)2, (Half)0.1);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to approximate 2 +/- *, but * differed by *");
        }

        [Fact]
        public void Half_infinity_approximates_the_same_infinity()
        {
            // Arrange
            var subject = Half.PositiveInfinity;

            // Act / Assert
            subject.Should().BeApproximately(Half.PositiveInfinity, (Half)0.1);
        }

        [Fact]
        public void Half_infinity_does_not_approximate_a_finite_value()
        {
            // Arrange
            var subject = Half.PositiveInfinity;

            // Act
            Action act = () => subject.Should().BeApproximately((Half)1, (Half)0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Approximating_NaN_is_not_possible()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().BeApproximately(Half.NaN, (Half)0.1);

            // Assert
            act.Should().Throw<ArgumentException>().WithParameterName("expectedValue");
        }

        [Fact]
        public void A_negative_precision_is_not_allowed()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().BeApproximately((Half)1, (Half)(-0.1));

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("precision");
        }

        [Fact]
        public void Half_outside_precision_does_not_approximate_the_unexpected_value()
        {
            // Arrange
            var subject = (Half)1.5;

            // Act / Assert
            subject.Should().NotBeApproximately((Half)2, (Half)0.1);
        }

        [Fact]
        public void Half_within_precision_fails_NotBeApproximately()
        {
            // Arrange
            var subject = (Half)1.5;

            // Act
            Action act = () => subject.Should().NotBeApproximately((Half)1.5, (Half)0.1);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to not approximate * +/- *, but * only differed by *");
        }

        [Fact]
        public void Not_approximating_NaN_is_not_possible()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().NotBeApproximately(Half.NaN, (Half)0.1);

            // Assert
            act.Should().Throw<ArgumentException>().WithParameterName("unexpectedValue");
        }

        [Fact]
        public void Existing_overloads_still_take_precedence()
        {
            // Arrange
            double subject = 1.5;

            // Act / Assert
            NumericAssertions<double> assertions = subject.Should();
            assertions.BeApproximately(1.5, 0.01);
        }
    }
}
#endif
