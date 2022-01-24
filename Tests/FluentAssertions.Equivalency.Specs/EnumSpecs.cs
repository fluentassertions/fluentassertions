using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class EnumSpecs
    {
        [Fact]
        public void When_asserting_the_same_enum_member_is_equivalent_it_should_succeed()
        {
            // Arrange
            object subject = EnumOne.One;
            object expectation = EnumOne.One;

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_the_actual_enum_value_is_null_it_should_report_that_properly()
        {
            // Arrange
            var subject = new { NullableEnum = (DayOfWeek?)null };

            var expectation = new { NullableEnum = DayOfWeek.Friday };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*5*null*");
        }

        [Fact]
        public void When_the_actual_enum_name_is_null_it_should_report_that_properly()
        {
            // Arrange
            var subject = new { NullableEnum = (DayOfWeek?)null };

            var expectation = new { NullableEnum = DayOfWeek.Friday };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, o => o.ComparingEnumsByValue());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*5*null*");
        }

        [Fact]
        public void When_asserting_different_enum_members_are_equivalent_it_should_fail()
        {
            // Arrange
            object subject = EnumOne.One;
            object expectation = EnumOne.Two;

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected *EnumOne.Two {value: 3}*but*EnumOne.One {value: 0}*");
        }

        [Fact]
        public void When_asserting_members_from_different_enum_types_are_equivalent_it_should_compare_by_value_by_default()
        {
            // Arrange
            var subject = new ClassWithEnumOne();
            var expectation = new ClassWithEnumTwo();

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_members_from_different_enum_types_are_equivalent_by_value_it_should_succeed()
        {
            // Arrange
            var subject = new ClassWithEnumOne { Enum = EnumOne.One };
            var expectation = new ClassWithEnumThree { Enum = EnumThree.ValueZero };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, config => config.ComparingEnumsByValue());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_members_from_different_enum_types_are_equivalent_by_string_value_it_should_succeed()
        {
            // Arrange
            var subject = new ClassWithEnumOne { Enum = EnumOne.Two };
            var expectation = new ClassWithEnumThree() { Enum = EnumThree.Two };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, config => config.ComparingEnumsByName());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_asserting_members_from_different_enum_types_are_equivalent_by_value_but_comparing_by_name_it_should_throw()
        {
            // Arrange
            var subject = new ClassWithEnumOne { Enum = EnumOne.Two };
            var expectation = new ClassWithEnumFour { Enum = EnumFour.Three };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, config => config.ComparingEnumsByName());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*to equal EnumFour.Three {value: 3} by name, but found EnumOne.Two {value: 3}*");
        }

        [Fact]
        public void When_asserting_members_from_different_char_enum_types_are_equivalent_by_value_it_should_succeed()
        {
            // Arrange
            var subject = new ClassWithEnumCharOne { Enum = EnumCharOne.B };
            var expectation = new ClassWithEnumCharTwo { Enum = EnumCharTwo.ValueB };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, config => config.ComparingEnumsByValue());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_enums_typed_as_object_are_equivalent_it_should_succeed()
        {
            // Arrange
            object e1 = EnumOne.One;
            object e2 = EnumOne.One;

            // Act
            Action act = () => e1.Should().BeEquivalentTo(e2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_numeric_member_is_compared_with_an_enum_it_should_throw()
        {
            // Arrange
            var actual = new { Property = 1 };

            var expected = new { Property = TestEnum.First };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected, options => options.ComparingEnumsByValue());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_string_member_is_compared_with_an_enum_it_should_throw()
        {
            // Arrange
            var actual = new { Property = "First" };

            var expected = new { Property = TestEnum.First };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected, options => options.ComparingEnumsByName());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_null_enum_members_are_compared_by_name_it_should_succeed()
        {
            // Arrange
            var actual = new { Property = null as TestEnum? };

            var expected = new { Property = null as TestEnum? };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected, options => options.ComparingEnumsByName());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_null_enum_members_are_compared_by_value_it_should_succeed()
        {
            // Arrange
            var actual = new { Property = null as TestEnum? };

            var expected = new { Property = null as TestEnum? };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected, options => options.ComparingEnumsByValue());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_zero_and_null_enum_are_compared_by_value_it_should_throw()
        {
            // Arrange
            var actual = new { Property = (TestEnum)0 };

            var expected = new { Property = null as TestEnum? };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected, options => options.ComparingEnumsByValue());

            // Assert
            act.Should().Throw<XunitException>();
        }

        public enum TestEnum
        {
            First = 1
        }

        [Fact]
        public void When_subject_is_null_and_enum_has_some_value_it_should_throw()
        {
            // Arrange
            object subject = null;
            object expectedEnum = EnumULong.UInt64Max;

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(expectedEnum, x => x.ComparingEnumsByName(), "comparing enums should throw");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected*to be equivalent to EnumULong.UInt64Max {value: 18446744073709551615} because comparing enums should throw, but found <null>*");
        }

        [Fact]
        public void When_expectation_is_null_and_subject_enum_has_some_value_it_should_throw_with_a_useful_message()
        {
            // Arrange
            object subjectEnum = EnumULong.UInt64Max;
            object expected = null;

            // Act
            Action act = () =>
                subjectEnum.Should().BeEquivalentTo(expected, x => x.ComparingEnumsByName(), "comparing enums should throw");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*to be <null> because comparing enums should throw, but found EnumULong.UInt64Max*");
        }

        [Fact]
        public void When_both_enums_are_equal_and_greater_than_max_long_it_should_not_throw()
        {
            // Arrange
            object enumOne = EnumULong.UInt64Max;
            object enumTwo = EnumULong.UInt64Max;

            // Act
            Action act = () => enumOne.Should().BeEquivalentTo(enumTwo);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_both_enums_are_equal_and_of_different_underlying_types_it_should_not_throw()
        {
            // Arrange
            object enumOne = EnumLong.Int64Max;
            object enumTwo = EnumULong.Int64Max;

            // Act
            Action act = () => enumOne.Should().BeEquivalentTo(enumTwo);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_both_enums_are_large_and_not_equal_it_should_throw()
        {
            // Arrange
            object subjectEnum = EnumLong.Int64LessOne;
            object expectedEnum = EnumULong.UInt64Max;

            // Act
            Action act = () => subjectEnum.Should().BeEquivalentTo(expectedEnum, "comparing enums should throw");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected subjectEnum*to equal EnumULong.UInt64Max {value: 18446744073709551615} by value because comparing enums should throw, but found EnumLong.Int64LessOne {value: 9223372036854775806}*");
        }
    }
}
