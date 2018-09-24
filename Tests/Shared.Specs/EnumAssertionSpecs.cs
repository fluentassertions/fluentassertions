using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public enum EnumULong : ulong
    {
        Int64Max = Int64.MaxValue,
        UInt64LessOne = UInt64.MaxValue - 1,
        UInt64Max = UInt64.MaxValue
    }

    public enum EnumLong : long
    {
        Int64Max = Int64.MaxValue,
        Int64LessOne = Int64.MaxValue - 1
    }

    public class EnumAssertionSpecs
    {
        [Fact]
        public void When_both_enums_are_equal_and_greater_than_max_long_it_should_not_throw()
        {
            // Arrange
            var enumOne = EnumULong.UInt64Max;
            var enumTwo = EnumULong.UInt64Max;

            // Act
            Action act = () => enumOne.Should().BeEquivalentTo(enumTwo);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_both_enums_are_equal_and_of_different_underlying_types_it_should_not_throw()
        {
            // Arrange
            var enumOne = EnumLong.Int64Max;
            var enumTwo = EnumULong.Int64Max;

            // Act
            Action act = () => enumOne.Should().BeEquivalentTo(enumTwo);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_both_enums_are_large_and_not_equal_it_should_throw()
        {
            // Arrange
            var subjectEnum = EnumLong.Int64LessOne;
            var expectedEnum = EnumULong.UInt64Max;

            // Act
            Action act = () => subjectEnum.Should().BeEquivalentTo(expectedEnum, "comparing enums should throw");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected enum to be \"EnumULong.UInt64Max({(UInt64)EnumULong.UInt64Max})\" because comparing enums should throw, but found \"EnumLong.Int64LessOne({(Int64)EnumLong.Int64LessOne})\"*");
        }
    }
}
