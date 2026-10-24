#if NET8_0_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using Xunit;

namespace FluentAssertions.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class Should
    {
        [Fact]
        public void A_half_gets_numeric_assertions()
        {
            // Arrange
            var subject = (Half)1.5;

            // Act / Assert
            NumericAssertions<Half> assertions = subject.Should();
            assertions.BePositive().And.BeGreaterThan((Half)1);
        }

        [Fact]
        public void A_nullable_half_gets_nullable_numeric_assertions()
        {
            // Arrange
            Half? subject = (Half)1.5;

            // Act / Assert
            NullableNumericAssertions<Half> assertions = subject.Should();
            assertions.HaveValue().And.Be((Half)1.5);
        }

        [Fact]
        public void A_native_float_gets_numeric_assertions()
        {
            // Arrange
            var subject = (NFloat)3.14;

            // Act / Assert
            NumericAssertions<NFloat> assertions = subject.Should();
            assertions.BePositive();
        }

        [Fact]
        public void A_double_still_resolves_to_the_dedicated_double_assertions()
        {
            // Arrange
            double subject = 1.5;

            // Act
            object assertions = subject.Should();

            // Assert
            assertions.Should().BeOfType<DoubleAssertions>();
        }

        [Fact]
        public void A_char_still_gets_comparable_assertions()
        {
            // Arrange
            char subject = 'a';

            // Act / Assert
            ComparableTypeAssertions<char> assertions = subject.Should();
            assertions.Subject.Should().Be('a');
        }

        [Fact]
        public void A_nullable_char_still_gets_object_assertions()
        {
            // Arrange
            char? subject = 'a';

            // Act / Assert
            ObjectAssertions assertions = subject.Should();
            assertions.Subject.Should().Be('a');
        }

        [Fact]
        public void A_big_integer_still_gets_comparable_assertions()
        {
            // Arrange
            var subject = new BigInteger(42);

            // Act / Assert
            ComparableTypeAssertions<BigInteger> assertions = subject.Should();
            assertions.BeRankedEquallyTo(subject);
        }

        [Fact]
        public void An_int128_still_gets_comparable_assertions()
        {
            // Arrange
            Int128 subject = 42;

            // Act / Assert
            ComparableTypeAssertions<Int128> assertions = subject.Should();
            assertions.BeRankedEquallyTo(subject);
        }

        [Fact]
        public void A_uint128_still_gets_comparable_assertions()
        {
            // Arrange
            UInt128 subject = 42;

            // Act / Assert
            ComparableTypeAssertions<UInt128> assertions = subject.Should();
            assertions.BeRankedEquallyTo(subject);
        }

        [Fact]
        public void A_native_int_still_gets_comparable_assertions()
        {
            // Arrange
            nint subject = 42;

            // Act / Assert
            ComparableTypeAssertions<nint> assertions = subject.Should();
            assertions.BeRankedEquallyTo(subject);
        }

        [Fact]
        public void A_native_unsigned_int_still_gets_comparable_assertions()
        {
            // Arrange
            nuint subject = 42;

            // Act / Assert
            ComparableTypeAssertions<nuint> assertions = subject.Should();
            assertions.BeRankedEquallyTo(subject);
        }

        [Fact]
        public void Nullable_big_integers_and_native_ints_still_get_object_assertions()
        {
            // Arrange
            BigInteger? bigInteger = 42;
            Int128? int128 = 42;
            UInt128? uint128 = 42;
            nint? nativeInt = 42;
            nuint? nativeUnsignedInt = 42;

            // Act
            ObjectAssertions bigIntegerAssertions = bigInteger.Should();
            ObjectAssertions int128Assertions = int128.Should();
            ObjectAssertions uint128Assertions = uint128.Should();
            ObjectAssertions nativeIntAssertions = nativeInt.Should();
            ObjectAssertions nativeUnsignedIntAssertions = nativeUnsignedInt.Should();

            // Assert
            bigIntegerAssertions.Subject.Should().Be(new BigInteger(42));
            int128Assertions.Subject.Should().Be((Int128)42);
            uint128Assertions.Subject.Should().Be((UInt128)42);
            nativeIntAssertions.Subject.Should().Be((nint)42);
            nativeUnsignedIntAssertions.Subject.Should().Be((nuint)42);
        }
    }
}
#endif
