using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public enum EnumULong : ulong
    {
        Int64Max = long.MaxValue,
        UInt64LessOne = ulong.MaxValue - 1,
        UInt64Max = ulong.MaxValue
    }

    public enum EnumLong : long
    {
        Int64Max = long.MaxValue,
        Int64LessOne = long.MaxValue - 1
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
                .WithMessage($"Expected subjectEnum to equal EnumULong.UInt64Max({(ulong)EnumULong.UInt64Max}) by value because comparing enums should throw, but found EnumLong.Int64LessOne({(long)EnumLong.Int64LessOne})*");
        }

        [Fact]
        public void When_subject_is_null_and_enum_has_some_value_it_should_throw()
        {
            // Arrange
            object subject = null;
            object expectedEnum = EnumULong.UInt64Max;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => subject.Should().BeEquivalentTo(expectedEnum, x => x.ComparingEnumsByName(), "comparing enums should throw");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"Expected*to equal EnumULong.UInt64Max({(ulong)EnumULong.UInt64Max}) by name because comparing enums should throw, but found null*");
        }

        [Fact]
        public void When_expectation_is_null_and_subject_enum_has_some_value_it_should_throw_with_a_useful_message()
        {
            // Arrange
            object subjectEnum = EnumULong.UInt64Max;
            object expected = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => subjectEnum.Should().BeEquivalentTo(expected, x => x.ComparingEnumsByName(), "comparing enums should throw");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*to be <null> because comparing enums should throw, but found UInt64Max*");
        }

        // TODO: should probably fail in 6.0, see #1204
        [Fact]
        public void When_comparing_an_enum_and_a_numeric_for_equality_it_should_not_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            object expected = 1;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().NotThrow();
        }

        // TODO: should probably fail in 6.0, see #1204
        [Fact]
        public void When_comparing_a_numeric_and_an_enum_for_equality_it_should_not_throw()
        {
            // Arrange
            object subject = 1;
            MyEnum expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().NotThrow();
        }

        private enum MyEnum
        {
            One = 1
        }
    }
}
