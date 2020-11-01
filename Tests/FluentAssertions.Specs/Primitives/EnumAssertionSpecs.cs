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
        #region HaveFlag / NotHaveFlag

        [Fact]
        public void When_enum_has_the_expected_flag_it_should_succeed()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act / Assert
            someObject.Should().HaveFlag(TestEnum.One);
        }

        [Fact]
        public void When_a_nullable_enum_has_the_expected_flag_it_should_succeed()
        {
            // Arrange
            TestEnum? someObject = TestEnum.One | TestEnum.Two;

            // Act / Assert
            someObject.Should().HaveFlag(TestEnum.Two);
        }

        [Fact]
        public void When_enum_is_null_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            Enum someObject = null;

            // Act
            Action act = () => someObject.Should().HaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*but found <null>*");
        }

        [Fact]
        public void When_nullable_enum_is_null_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum? someObject = null;

            // Act
            Action act = () => someObject.Should().HaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*but found <null>*");
        }

        [Fact]
        public void When_enum_does_not_have_specified_flag_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act
            Action act = () => someObject.Should().HaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("The enum was expected to have flag Three but found One, Two.");
        }

        [Fact]
        public void When_enum_does_not_have_the_unexpected_flag_it_should_succeed()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act / Assert
            someObject.Should().NotHaveFlag(TestEnum.Three);
        }

        [Fact]
        public void When_enum_does_have_specified_flag_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act
            Action act = () => someObject.Should().NotHaveFlag(TestEnum.Two);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Did not expect the enum to have flag Two.");
        }

        [Fact]
        public void When_enum_should_not_have_flag_but_is_null_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            Enum someObject = null;

            // Act
            Action act = () => someObject.Should().NotHaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*but found <null>*");
        }

        [Fact]
        public void When_nullable_enum_should_not_have_flag_but_is_null_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum? someObject = null;

            // Act
            Action act = () => someObject.Should().NotHaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*but found <null>*");
        }

        [Flags]
        public enum TestEnum
        {
            None = 0,
            One = 1,
            Two = 2,
            Three = 4
        }

        [Flags]
        public enum OtherEnum
        {
            Default,
            First,
            Second
        }

        #endregion

        #region BeEquivalentTo

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
                .WithMessage($"Expected subjectEnum*to equal EnumULong.UInt64Max({(ulong)EnumULong.UInt64Max}) by value because comparing enums should throw, but found EnumLong.Int64LessOne({(long)EnumLong.Int64LessOne})*");
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

        #endregion

        #region Be / NotBe

        [Fact]
        public void When_comparing_a_null_enum_against_a_null_enum_for_equality_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = null;
            MyEnum? expected = null;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_an_enum_for_equality_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_an_enum_against_a_nullable_enum_for_equality_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum? expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_a_nullable_enum_for_equality_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum? expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_an_enum_for_equality_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_an_enum_against_a_nullable_enum_for_equality_it_should_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum? expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_a_nullable_enum_for_equality_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum? expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_a_null_enum_against_a_nullable_enum_for_equality_it_should_throw()
        {
            // Arrange
            MyEnum? subject = null;
            MyEnum? expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_a_null_enum_against_an_enum_for_equality_it_should_throw()
        {
            // Arrange
            MyEnum? subject = null;
            MyEnum expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_a_null_enum_for_equality_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum? expected = null;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_an_enum_against_a_null_enum_for_equality_it_should_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum? expected = null;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_a_null_enum_against_a_null_enum_for_inequality_it_should_throw()
        {
            // Arrange
            MyEnum? subject = null;
            MyEnum? expected = null;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_an_enum_for_inequality_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_an_enum_against_a_nullable_enum_for_inequality_it_should_throww()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum? expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_a_nullable_enum_for_inequality_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum? expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_an_enum_for_inequality_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_an_enum_against_a_nullable_enum_for_inequality_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum? expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_a_nullable_enum_for_inequality_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum? expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_a_null_enum_against_a_nullable_enum_for_inequality_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = null;
            MyEnum? expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_a_null_enum_against_an_enum_for_inequality_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = null;
            MyEnum expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_a_nullable_enum_against_a_null_enum_for_inequality_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnum? expected = null;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_comparing_an_enum_against_a_null_enum_for_inequality_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum? expected = null;

            // Act
            Action act = () => subject.Should().NotBe(expected);

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
            One = 1,
            Two = 2
        }

        #endregion
    }
}
