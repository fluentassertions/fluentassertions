using System;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

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
    public class HaveFlag
    {
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
                .WithMessage(
                    "Expected*have flag TestEnum.Three {value: 4}*because we want to test the failure message*but found TestEnum.One|Two {value: 3}.");
        }
    }

    public class NotHaveFlag
    {
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
                .WithMessage(
                    "Expected*someObject*to not have flag TestEnum.Two {value: 2}*because we want to test the failure message*");
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

    public class Be
    {
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
                .WithMessage("*subject*because we want to test the failure message*");
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
    }

    public class NotBe
    {
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
    }

    public enum MyEnum
    {
        One = 1,
        Two = 2
    }

    public class HaveValue
    {
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
    }

    public class NotHaveValue
    {
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
                .WithMessage("Expected*someObject*to not have value 1*because we want to test the failure message*");
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
    }

    public class HaveSameValueAs
    {
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
    }

    public class NotHaveSameValueAs
    {
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
    }

    public enum MyEnumOtherName
    {
        OtherOne = 1,
        OtherTwo = 2
    }

    public class HaveSameNameAs
    {
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
    }

    public class NotHaveSameNameAs
    {
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
    }

    public enum MyEnumOtherValue
    {
        One = 11,
        Two = 22
    }

    public class BeNull
    {
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
    }

    public class NotBeNull
    {
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
    }

    public class Match
    {
        [Fact]
        public void An_enum_matching_the_predicate_should_not_throw()
        {
            // Arrange
            BindingFlags flags = BindingFlags.Public;

            // Act / Assert
            flags.Should().Match(x => x == BindingFlags.Public);
        }

        [Fact]
        public void An_enum_not_matching_the_predicate_should_throw_with_the_predicate_in_the_message()
        {
            // Arrange
            BindingFlags flags = BindingFlags.Public;

            // Act
            Action act = () => flags.Should().Match(x => x == BindingFlags.Static, "that's what we need");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*Static*because that's what we need*found*Public*");
        }

        [Fact]
        public void An_enum_cannot_be_compared_with_a_null_predicate()
        {
            // Act
            Action act = () => BindingFlags.Public.Should().Match(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*null*predicate*");
        }
    }

    public class BeOneOf
    {
        [Fact]
        public void An_enum_that_is_one_of_the_expected_values_should_not_throw()
        {
            // Arrange
            BindingFlags flags = BindingFlags.Public;

            // Act / Assert
            flags.Should().BeOneOf(BindingFlags.Public, BindingFlags.ExactBinding);
        }

        [Fact]
        public void Throws_when_the_enums_is_not_one_of_the_expected_enums()
        {
            // Arrange
            BindingFlags flags = BindingFlags.DeclaredOnly;

            // Act / Assert
            Action act = () =>
                flags.Should().BeOneOf(new[] { BindingFlags.Public, BindingFlags.ExactBinding }, "that's what we need");

            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected*Public*ExactBinding*because that's what we need*found*DeclaredOnly*");
        }

        [Fact]
        public void An_enum_cannot_be_part_of_an_empty_list()
        {
            // Arrange
            BindingFlags flags = BindingFlags.DeclaredOnly;

            // Act / Assert
            Action act = () => flags.Should().BeOneOf(Array.Empty<BindingFlags>());

            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Cannot*empty list of enums*");
        }

        [Fact]
        public void An_enum_cannot_be_part_of_a_null_list()
        {
            // Arrange
            BindingFlags flags = BindingFlags.DeclaredOnly;

            // Act / Assert
            Action act = () => flags.Should().BeOneOf(null);

            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Cannot*null list of enums*");
        }
    }

    public class BeDefined
    {
        [Fact]
        public void A_valid_entry_of_an_enum_is_defined()
        {
            // Arrange
            var dayOfWeek = DayOfWeek.Monday;

            // Act / Assert
            dayOfWeek.Should().BeDefined();
        }

        [Fact]
        public void If_a_value_casted_to_an_enum_type_and_it_does_not_exist_in_the_enum_it_throws()
        {
            // Arrange
            var dayOfWeek = (DayOfWeek)999;

            // Act
            Action act = () => dayOfWeek.Should().BeDefined("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected *to be defined in*failure message*, but it is not*");
        }

        [Fact]
        public void A_null_entry_of_an_enum_throws()
        {
            // Arrange
            MyEnum? subject = null;

            // Act
            Action act = () => subject.Should().BeDefined();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected *to be defined in*, but found <null>.");
        }
    }

    public class NotBeDefined
    {
        [Fact]
        public void An_invalid_entry_of_an_enum_is_not_defined_passes()
        {
            // Arrange
            var dayOfWeek = (DayOfWeek)999;

            // Act / Assert
            dayOfWeek.Should().NotBeDefined();
        }

        [Fact]
        public void A_valid_entry_of_an_enum_is_not_defined_fails()
        {
            // Arrange
            var dayOfWeek = DayOfWeek.Monday;

            // Act
            Action act = () => dayOfWeek.Should().NotBeDefined();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect*to be defined in*, but it is.");
        }

        [Fact]
        public void A_null_value_of_an_enum_is_not_defined_and_throws()
        {
            // Arrange
            MyEnum? subject = null;

            // Act
            Action act = () => subject.Should().NotBeDefined();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect *to be defined in*, but found <null>.");
        }
    }

    public class Miscellaneous
    {
        [Fact]
        public void Should_throw_a_helpful_error_when_accidentally_using_equals()
        {
            // Arrange
            MyEnum? subject = null;

            // Act
            var action = () => subject.Should().Equals(null);

            // Assert
            action.Should().Throw<NotSupportedException>()
                .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
        }
    }
}
