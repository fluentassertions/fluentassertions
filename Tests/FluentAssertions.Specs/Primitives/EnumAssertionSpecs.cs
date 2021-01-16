using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives
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
        public void When_null_enum_does_not_have_the_expected_flag_it_should_fail()
        {
            // Arrange
            TestEnum? someObject = null;

            // Act
            Action act = () => someObject.Should().HaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_enum_does_not_have_specified_flag_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act
            Action act = () => someObject.Should().HaveFlag(TestEnum.Three, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*have flag TestEnum.Three {value: 4}*because we want to test the failure message*but found TestEnum.One|Two {value: 3}.");
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
            Action act = () => someObject.Should().NotHaveFlag(TestEnum.Two, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the enum to not have flag TestEnum.Two {value: 2}*because we want to test the failure message*");
        }

        [Fact]
        public void When_null_enum_does_not_have_the_expected_flag_it_should_not_fail()
        {
            // Arrange
            TestEnum? someObject = null;

            // Act
            Action act = () => someObject.Should().NotHaveFlag(TestEnum.Three);

            // Assert
            act.Should().NotThrow();
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

        #region Be / NotBe

        [Fact]
        public void When_enums_are_equal_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(MyEnum.One, MyEnum.One)]
        public void When_nullable_enums_are_equal_it_should_succeed(MyEnum? subject, MyEnum? expected)
        {
            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_null_enum_and_an_enum_are_unequal_it_should_throw()
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
        public void When_enums_are_unequal_it_should_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().Be(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Theory]
        [InlineData(null, MyEnum.One)]
        [InlineData(MyEnum.One, null)]
        [InlineData(MyEnum.One, MyEnum.Two)]
        public void When_nullable_enums_are_equal_it_should_throw(MyEnum? subject, MyEnum? expected)
        {
            // Act
            Action act = () => subject.Should().Be(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_enums_are_unequal_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_null_enum_and_an_enum_are_unequal_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = null;
            MyEnum expected = MyEnum.Two;

            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(null, MyEnum.One)]
        [InlineData(MyEnum.One, null)]
        [InlineData(MyEnum.One, MyEnum.Two)]
        public void When_nullable_enums_are_unequal_it_should_succeed(MyEnum? subject, MyEnum? expected)
        {
            // Act
            Action act = () => subject.Should().NotBe(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_enums_are_equal_it_should_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnum expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().NotBe(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(MyEnum.One, MyEnum.One)]
        public void When_nullable_enums_are_unequal_it_should_throw(MyEnum? subject, MyEnum? expected)
        {
            // Act
            Action act = () => subject.Should().NotBe(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        public enum MyEnum
        {
            One = 1,
            Two = 2
        }

        #endregion

        #region HaveValue / NotHaveValue

        [Fact]
        public void When_enum_has_the_expected_value_it_should_succeed()
        {
            // Arrange
            TestEnum someObject = TestEnum.One;

            // Act / Assert
            someObject.Should().HaveValue(1);
        }

        [Fact]
        public void When_null_enum_does_not_have_the_expected_value_it_should_fail()
        {
            // Arrange
            TestEnum? someObject = null;

            // Act
            Action act = () => someObject.Should().HaveValue(3);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_enum_does_not_have_specified_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum someObject = TestEnum.One;

            // Act
            Action act = () => someObject.Should().HaveValue(3, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*have value 3*because we want to test the failure message*but found*");
        }

        [Fact]
        public void When_enum_does_not_have_the_unexpected_value_it_should_succeed()
        {
            // Arrange
            TestEnum someObject = TestEnum.One;

            // Act / Assert
            someObject.Should().NotHaveValue(3);
        }

        [Fact]
        public void When_enum_does_have_specified_value_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum someObject = TestEnum.One;

            // Act
            Action act = () => someObject.Should().NotHaveValue(1, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the enum to not have value 1*because we want to test the failure message*");
        }

        [Fact]
        public void When_null_enum_does_not_have_the_expected_value_it_should_not_fail()
        {
            // Arrange
            TestEnum? someObject = null;

            // Act
            Action act = () => someObject.Should().NotHaveValue(3);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region HaveSameValueAs / NotHaveSameValueAs

        [Fact]
        public void When_enums_have_equal_values_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnumOtherName expected = MyEnumOtherName.OtherOne;

            // Act
            Action act = () => subject.Should().HaveSameValueAs(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_enums_have_equal_values_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnumOtherName expected = MyEnumOtherName.OtherOne;

            // Act
            Action act = () => subject.Should().HaveSameValueAs(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_enums_have_equal_values_it_should_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnumOtherName expected = MyEnumOtherName.OtherTwo;

            // Act
            Action act = () => subject.Should().HaveSameValueAs(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Theory]
        [InlineData(null, MyEnumOtherName.OtherOne)]
        [InlineData(MyEnum.One, MyEnumOtherName.OtherTwo)]
        public void When_nullable_enums_have_equal_values_it_should_throw(MyEnum? subject, MyEnumOtherName expected)
        {
            // Act
            Action act = () => subject.Should().HaveSameValueAs(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_enum_have_unequal_values_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnumOtherName expected = MyEnumOtherName.OtherTwo;

            // Act
            Action act = () => subject.Should().NotHaveSameValueAs(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(null, MyEnumOtherName.OtherOne)]
        [InlineData(MyEnum.One, MyEnumOtherName.OtherTwo)]
        public void When_nullable_enums_have_unequal_values_it_should_succeed(MyEnum? subject, MyEnumOtherName expected)
        {
            // Act
            Action act = () => subject.Should().NotHaveSameValueAs(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_enums_have_unequal_values_it_should_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnumOtherName expected = MyEnumOtherName.OtherOne;

            // Act
            Action act = () => subject.Should().NotHaveSameValueAs(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_nullable_enums_have_unequal_values_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnumOtherName expected = MyEnumOtherName.OtherOne;

            // Act
            Action act = () => subject.Should().NotHaveSameValueAs(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        public enum MyEnumOtherName
        {
            OtherOne = 1,
            OtherTwo = 2
        }

        #endregion

        #region HaveSameNameAs / NotHaveSameNameAs

        [Fact]
        public void When_enums_have_equal_names_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnumOtherValue expected = MyEnumOtherValue.One;

            // Act
            Action act = () => subject.Should().HaveSameNameAs(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_enums_have_equal_names_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnumOtherValue expected = MyEnumOtherValue.One;

            // Act
            Action act = () => subject.Should().HaveSameNameAs(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_enums_have_equal_names_it_should_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnumOtherValue expected = MyEnumOtherValue.Two;

            // Act
            Action act = () => subject.Should().HaveSameNameAs(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Theory]
        [InlineData(null, MyEnumOtherValue.One)]
        [InlineData(MyEnum.One, MyEnumOtherValue.Two)]
        public void When_nullable_enums_have_equal_names_it_should_throw(MyEnum? subject, MyEnumOtherValue expected)
        {
            // Act
            Action act = () => subject.Should().HaveSameNameAs(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_senum_have_unequal_names_it_should_succeed()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnumOtherValue expected = MyEnumOtherValue.Two;

            // Act
            Action act = () => subject.Should().NotHaveSameNameAs(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(null, MyEnumOtherValue.One)]
        [InlineData(MyEnum.One, MyEnumOtherValue.Two)]
        public void When_nullable_enums_have_unequal_names_it_should_succeed(MyEnum? subject, MyEnumOtherValue expected)
        {
            // Act
            Action act = () => subject.Should().NotHaveSameNameAs(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_enums_have_unequal_names_it_should_throw()
        {
            // Arrange
            MyEnum subject = MyEnum.One;
            MyEnumOtherValue expected = MyEnumOtherValue.One;

            // Act
            Action act = () => subject.Should().NotHaveSameNameAs(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_nullable_enums_have_unequal_names_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;
            MyEnumOtherValue expected = MyEnumOtherValue.One;

            // Act
            Action act = () => subject.Should().NotHaveSameNameAs(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        public enum MyEnumOtherValue
        {
            One = 11,
            Two = 22
        }

        #endregion

        #region BeNull / NotBeNull

        [Fact]
        public void When_nullable_enum_has_value_it_should_be_chainable()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;

            // Act
            Action act = () => subject.Should().HaveValue()
                .Which.Should().Be(MyEnum.One);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_enum_is_not_null_it_should_be_chainable()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;

            // Act
            Action act = () => subject.Should().NotBeNull()
                .Which.Should().Be(MyEnum.One);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_enum_does_not_have_value_it_should_throw()
        {
            // Arrange
            MyEnum? subject = null;

            // Act
            Action act = () => subject.Should().HaveValue("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_nullable_enum_is_null_it_should_throw()
        {
            // Arrange
            MyEnum? subject = null;

            // Act
            Action act = () => subject.Should().NotBeNull("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_nullable_enum_does_not_have_value_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = null;

            // Act
            Action act = () => subject.Should().NotHaveValue();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_enum_is_null_it_should_succeed()
        {
            // Arrange
            MyEnum? subject = null;

            // Act
            Action act = () => subject.Should().BeNull();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_nullable_enum_has_value_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;

            // Act
            Action act = () => subject.Should().NotHaveValue("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        [Fact]
        public void When_nullable_enum_is_not_null_it_should_throw()
        {
            // Arrange
            MyEnum? subject = MyEnum.One;

            // Act
            Action act = () => subject.Should().BeNull("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*because we want to test the failure message*");
        }

        #endregion
    }
}
