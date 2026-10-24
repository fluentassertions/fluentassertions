using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class BeApproximately
    {
        [Fact]
        public void When_approximating_a_float_with_a_negative_precision_it_should_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, -0.1F);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_float_is_not_approximating_a_range_it_should_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.001F, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to approximate *3.14* +/- *0.001* because rockets will crash otherwise, but *3.1415927* differed by *0.001592*");
        }

        [Fact]
        public void When_float_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9F)]
        [InlineData(11F)]
        [Theory]
        public void When_float_is_approximating_a_value_on_boundaries_it_should_not_throw(float value)
        {
            // Act
            Action act = () => value.Should().BeApproximately(10F, 1F);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9F)]
        [InlineData(11F)]
        [Theory]
        public void When_float_is_not_approximating_a_value_on_boundaries_it_should_throw(float value)
        {
            // Act
            Action act = () => value.Should().BeApproximately(10F, 0.9F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_approximating_a_float_towards_nan_it_should_not_throw()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_approximating_positive_infinity_float_towards_positive_infinity_it_should_not_throw()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(float.PositiveInfinity, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_negative_infinity_float_towards_negative_infinity_it_should_not_throw()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(float.NegativeInfinity, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_float_is_not_approximating_positive_infinity_it_should_throw()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(float.MaxValue, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_float_is_not_approximating_negative_infinity_it_should_throw()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(float.MinValue, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void NaN_can_never_be_close_to_any_float()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().BeApproximately(float.MinValue, 0.1F);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*NaN*");
        }

        [Fact]
        public void A_float_can_never_be_close_to_NaN()
        {
            // Arrange
            float value = float.MinValue;

            // Act
            Action act = () => value.Should().BeApproximately(float.NaN, 0.1F);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*NaN*");
        }

        [Fact]
        public void When_a_nullable_float_has_no_value_it_should_throw()
        {
            // Arrange
            float? value = null;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.001F);

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to approximate*3.14* +/-*0.001*, but it was <null>.");
        }

        [Fact]
        public void When_approximating_a_double_with_a_negative_precision_it_should_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().BeApproximately(3.14, -0.1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_double_is_not_approximating_a_range_it_should_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().BeApproximately(3.14, 0.001, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to approximate 3.14 +/- 0.001 because rockets will crash otherwise, but 3.1415927 differed by 0.001592*");
        }

        [Fact]
        public void When_double_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().BeApproximately(3.14, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_a_double_towards_nan_it_should_not_throw()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().BeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_approximating_positive_infinity_double_towards_positive_infinity_it_should_not_throw()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(double.PositiveInfinity, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_negative_infinity_double_towards_negative_infinity_it_should_not_throw()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(double.NegativeInfinity, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_double_is_not_approximating_positive_infinity_it_should_throw()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(double.MaxValue, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_double_is_not_approximating_negative_infinity_it_should_throw()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            Action act = () => value.Should().BeApproximately(double.MinValue, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [InlineData(9D)]
        [InlineData(11D)]
        [Theory]
        public void When_double_is_approximating_a_value_on_boundaries_it_should_not_throw(double value)
        {
            // Act
            Action act = () => value.Should().BeApproximately(10D, 1D);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9D)]
        [InlineData(11D)]
        [Theory]
        public void When_double_is_not_approximating_a_value_on_boundaries_it_should_throw(double value)
        {
            // Act
            Action act = () => value.Should().BeApproximately(10D, 0.9D);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void NaN_can_never_be_close_to_any_double()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().BeApproximately(double.MinValue, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void A_double_can_never_be_close_to_NaN()
        {
            // Arrange
            double value = double.MinValue;

            // Act
            Action act = () => value.Should().BeApproximately(double.NaN, 0.1F);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void When_approximating_a_decimal_with_a_negative_precision_it_should_throw()
        {
            // Arrange
            decimal value = 3.1415927M;

            // Act
            Action act = () => value.Should().BeApproximately(3.14m, -0.1m);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_range_it_should_throw()
        {
            // Arrange
            decimal value = 3.5011m;

            // Act
            Action act = () => value.Should().BeApproximately(3.5m, 0.001m, "rockets will crash otherwise");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected value to approximate*3.5* +/-*0.001* because rockets will crash otherwise, but *3.5011* differed by*0.0011*");
        }

        [Fact]
        public void When_decimal_is_indeed_approximating_a_value_it_should_not_throw()
        {
            // Arrange
            decimal value = 3.5011m;

            // Act
            Action act = () => value.Should().BeApproximately(3.5m, 0.01m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_approximating_a_value_on_lower_boundary_it_should_not_throw()
        {
            // Act
            decimal value = 9m;

            // Act
            Action act = () => value.Should().BeApproximately(10m, 1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_approximating_a_value_on_upper_boundary_it_should_not_throw()
        {
            // Act
            decimal value = 11m;

            // Act
            Action act = () => value.Should().BeApproximately(10m, 1m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_on_lower_boundary_it_should_throw()
        {
            // Act
            decimal value = 9m;

            // Act
            Action act = () => value.Should().BeApproximately(10m, 0.9m);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_on_upper_boundary_it_should_throw()
        {
            // Act
            decimal value = 11m;

            // Act
            Action act = () => value.Should().BeApproximately(10m, 0.9m);

            // Assert
            act.Should().Throw<XunitException>();
        }

#if NET8_0_OR_GREATER
        [Fact]
        public void Succeeds_for_a_half_within_the_precision()
        {
            // Arrange
            var subject = (Half)1.5;

            // Act / Assert
            subject.Should().BeApproximately((Half)1.55, (Half)0.1);
        }

        [Fact]
        public void The_half_must_be_within_the_precision()
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
        public void Succeeds_for_positive_infinity_when_the_expected_value_is_positive_infinity()
        {
            // Arrange
            var subject = Half.PositiveInfinity;

            // Act / Assert
            subject.Should().BeApproximately(Half.PositiveInfinity, (Half)0.1);
        }

        [Fact]
        public void Succeeds_for_negative_infinity_when_the_expected_value_is_negative_infinity()
        {
            // Arrange
            var subject = Half.NegativeInfinity;

            // Act / Assert
            subject.Should().BeApproximately(Half.NegativeInfinity, (Half)0.1);
        }

        [Fact]
        public void A_finite_half_does_not_approximate_positive_infinity()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().BeApproximately(Half.PositiveInfinity, (Half)0.1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*differed by NaN*");
        }

        [Fact]
        public void A_finite_half_does_not_approximate_negative_infinity()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().BeApproximately(Half.NegativeInfinity, (Half)0.1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*differed by NaN*");
        }

        [Fact]
        public void Positive_infinity_does_not_approximate_a_finite_half()
        {
            // Arrange
            var subject = Half.PositiveInfinity;

            // Act
            Action act = () => subject.Should().BeApproximately((Half)1, (Half)0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void A_half_cannot_approximate_NaN()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().BeApproximately(Half.NaN, (Half)0.1);

            // Assert
            act.Should().Throw<ArgumentException>().WithParameterName("expectedValue");
        }

        [Fact]
        public void A_negative_precision_is_not_allowed_for_a_half()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().BeApproximately((Half)1, (Half)(-0.1));

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("precision");
        }
#endif
    }

    public class NotBeApproximately
    {
        [Fact]
        public void When_not_approximating_a_float_with_a_negative_precision_it_should_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, -0.1F);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_float_is_approximating_a_range_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.1F, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to not approximate *3.14* +/- *0.1* because rockets will crash otherwise, but *3.1415927* only differed by *0.001592*");
        }

        [Fact]
        public void When_float_is_not_approximating_a_value_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            float value = 3.1415927F;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.001F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_a_float_towards_nan_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_not_approximating_a_float_towards_positive_infinity_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.MaxValue, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_not_approximating_a_float_towards_negative_infinity_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.MinValue, 0.1F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_approximating_positive_infinity_float_towards_positive_infinity_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.PositiveInfinity, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_not_approximating_negative_infinity_float_towards_negative_infinity_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.NegativeInfinity, 0.1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [InlineData(9F)]
        [InlineData(11F)]
        [Theory]
        public void When_float_is_not_approximating_a_value_on_boundaries_it_should_not_throw(float value)
        {
            // Act
            Action act = () => value.Should().NotBeApproximately(10F, 0.9F);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9F)]
        [InlineData(11F)]
        [Theory]
        public void When_float_is_approximating_a_value_on_boundaries_it_should_throw(float value)
        {
            // Act
            Action act = () => value.Should().NotBeApproximately(10F, 1F);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_float_has_no_value_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            float? value = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14F, 0.001F);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void NaN_can_never_be_close_to_any_float()
        {
            // Arrange
            float value = float.NaN;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.MinValue, 0.1F);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*NaN*");
        }

        [Fact]
        public void A_float_can_never_be_close_to_NaN()
        {
            // Arrange
            float value = float.MinValue;

            // Act
            Action act = () => value.Should().NotBeApproximately(float.NaN, 0.1F);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*NaN*");
        }

        [Fact]
        public void When_not_approximating_a_double_with_a_negative_precision_it_should_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, -0.1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_double_is_approximating_a_range_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.1, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to not approximate *3.14* +/- *0.1* because rockets will crash otherwise, but *3.1415927* only differed by *0.001592*");
        }

        [Fact]
        public void When_double_is_not_approximating_a_value_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            double value = 3.1415927;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.001);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_approximating_a_double_towards_nan_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_not_approximating_a_double_towards_positive_infinity_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.MaxValue, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_not_approximating_a_double_towards_negative_infinity_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.MinValue, 0.1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_approximating_positive_infinity_double_towards_positive_infinity_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.PositiveInfinity, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void
            When_not_approximating_negative_infinity_double_towards_negative_infinity_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.NegativeInfinity, 0.1);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_nullable_double_has_no_value_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            double? value = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14, 0.001);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9D)]
        [InlineData(11D)]
        [Theory]
        public void When_double_is_not_approximating_a_value_on_boundaries_it_should_not_throw(double value)
        {
            // Act
            Action act = () => value.Should().NotBeApproximately(10D, 0.9D);

            // Assert
            act.Should().NotThrow();
        }

        [InlineData(9D)]
        [InlineData(11D)]
        [Theory]
        public void When_double_is_approximating_a_value_on_boundaries_it_should_throw(double value)
        {
            // Act
            Action act = () => value.Should().NotBeApproximately(10D, 1D);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void NaN_can_never_be_close_to_any_double()
        {
            // Arrange
            double value = double.NaN;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.MinValue, 0.1F);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*NaN*");
        }

        [Fact]
        public void A_double_can_never_be_close_to_NaN()
        {
            // Arrange
            double value = double.MinValue;

            // Act
            Action act = () => value.Should().NotBeApproximately(double.NaN, 0.1F);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("*NaN*");
        }

        [Fact]
        public void When_not_approximating_a_decimal_with_a_negative_precision_it_should_throw()
        {
            // Arrange
            decimal value = 3.1415927m;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.14m, -0.1m);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("precision")
                .WithMessage("*must be non-negative*");
        }

        [Fact]
        public void When_decimal_is_approximating_a_range_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            decimal value = 3.5011m;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.5m, 0.1m, "rockets will crash otherwise");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to not approximate *3.5* +/- *0.1* because rockets will crash otherwise, but *3.5011* only differed by *0.0011*");
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_and_should_not_approximate_it_should_not_throw()
        {
            // Arrange
            decimal value = 3.5011m;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.5m, 0.001m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_decimal_has_no_value_and_should_not_approximate_it_should_throw()
        {
            // Arrange
            decimal? value = null;

            // Act
            Action act = () => value.Should().NotBeApproximately(3.5m, 0.001m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_on_lower_boundary_it_should_not_throw()
        {
            // Act
            decimal value = 9m;

            // Act
            Action act = () => value.Should().NotBeApproximately(10m, 0.9m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_not_approximating_a_value_on_upper_boundary_it_should_not_throw()
        {
            // Act
            decimal value = 11m;

            // Act
            Action act = () => value.Should().NotBeApproximately(10m, 0.9m);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_decimal_is_approximating_a_value_on_lower_boundary_it_should_throw()
        {
            // Act
            decimal value = 9m;

            // Act
            Action act = () => value.Should().NotBeApproximately(10m, 1m);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_decimal_is_approximating_a_value_on_upper_boundary_it_should_throw()
        {
            // Act
            decimal value = 11m;

            // Act
            Action act = () => value.Should().NotBeApproximately(10m, 1m);

            // Assert
            act.Should().Throw<XunitException>();
        }

#if NET8_0_OR_GREATER
        [Fact]
        public void Succeeds_for_a_half_outside_the_precision()
        {
            // Arrange
            var subject = (Half)1.5;

            // Act / Assert
            subject.Should().NotBeApproximately((Half)2, (Half)0.1);
        }

        [Fact]
        public void The_half_must_not_be_within_the_precision()
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
        public void Succeeds_for_a_finite_half_when_the_unexpected_value_is_positive_infinity()
        {
            // Arrange
            var subject = (Half)1;

            // Act / Assert
            subject.Should().NotBeApproximately(Half.PositiveInfinity, (Half)0.1);
        }

        [Fact]
        public void Succeeds_for_a_finite_half_when_the_unexpected_value_is_negative_infinity()
        {
            // Arrange
            var subject = (Half)1;

            // Act / Assert
            subject.Should().NotBeApproximately(Half.NegativeInfinity, (Half)0.1);
        }

        [Fact]
        public void Positive_infinity_is_not_allowed_when_the_unexpected_value_is_positive_infinity()
        {
            // Arrange
            var subject = Half.PositiveInfinity;

            // Act
            Action act = () => subject.Should().NotBeApproximately(Half.PositiveInfinity, (Half)0.1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*only differed by NaN*");
        }

        [Fact]
        public void Negative_infinity_is_not_allowed_when_the_unexpected_value_is_negative_infinity()
        {
            // Arrange
            var subject = Half.NegativeInfinity;

            // Act
            Action act = () => subject.Should().NotBeApproximately(Half.NegativeInfinity, (Half)0.1);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*only differed by NaN*");
        }

        [Fact]
        public void A_half_cannot_be_compared_to_an_unexpected_NaN()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().NotBeApproximately(Half.NaN, (Half)0.1);

            // Assert
            act.Should().Throw<ArgumentException>().WithParameterName("unexpectedValue");
        }

        [Fact]
        public void A_negative_precision_is_not_allowed_for_an_unexpected_half()
        {
            // Arrange
            var subject = (Half)1;

            // Act
            Action act = () => subject.Should().NotBeApproximately((Half)1, (Half)(-0.1));

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("precision");
        }
#endif
    }
}
