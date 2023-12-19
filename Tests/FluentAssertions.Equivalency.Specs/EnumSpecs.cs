using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class EnumSpecs
{
    [Fact]
    public async Task When_asserting_the_same_enum_member_is_equivalent_it_should_succeed()
    {
        // Arrange
        object subject = EnumOne.One;
        object expectation = EnumOne.One;

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_the_actual_enum_value_is_null_it_should_report_that_properly()
    {
        // Arrange
        var subject = new { NullableEnum = (DayOfWeek?)null };

        var expectation = new { NullableEnum = DayOfWeek.Friday };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*5*null*");
    }

    [Fact]
    public async Task When_the_actual_enum_name_is_null_it_should_report_that_properly()
    {
        // Arrange
        var subject = new { NullableEnum = (DayOfWeek?)null };

        var expectation = new { NullableEnum = DayOfWeek.Friday };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation, o => o.ComparingEnumsByValue());

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*5*null*");
    }

    [Fact]
    public async Task When_asserting_different_enum_members_are_equivalent_it_should_fail()
    {
        // Arrange
        object subject = EnumOne.One;
        object expectation = EnumOne.Two;

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected *EnumOne.Two {value: 3}*but*EnumOne.One {value: 0}*");
    }

    [Fact]
    public async Task Comparing_collections_of_enums_by_value_includes_custom_message()
    {
        // Arrange
        var subject = new[] { EnumOne.One };
        var expectation = new[] { EnumOne.Two };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation, "some {0}", "reason");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected *EnumOne.Two {value: 3}*some reason*but*EnumOne.One {value: 0}*");
    }

    [Fact]
    public async Task Comparing_collections_of_enums_by_name_includes_custom_message()
    {
        // Arrange
        var subject = new[] { EnumOne.Two };
        var expectation = new[] { EnumFour.Three };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation, config => config.ComparingEnumsByName(),
            "some {0}", "reason");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*to equal EnumFour.Three {value: 3} by name*some reason*but found EnumOne.Two {value: 3}*");
    }

    [Fact]
    public async Task Comparing_collections_of_numerics_with_collections_of_enums_includes_custom_message()
    {
        // Arrange
        var actual = new[] { 1 };

        var expected = new[] { TestEnum.First };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, options => options.ComparingEnumsByValue(),
            "some {0}", "reason");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*some reason*");
    }

    [Fact]
    public async Task When_asserting_members_from_different_enum_types_are_equivalent_it_should_compare_by_value_by_default()
    {
        // Arrange
        var subject = new ClassWithEnumOne();
        var expectation = new ClassWithEnumTwo();

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_asserting_members_from_different_enum_types_are_equivalent_by_value_it_should_succeed()
    {
        // Arrange
        var subject = new ClassWithEnumOne { Enum = EnumOne.One };
        var expectation = new ClassWithEnumThree { Enum = EnumThree.ValueZero };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation, config => config.ComparingEnumsByValue());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_asserting_members_from_different_enum_types_are_equivalent_by_string_value_it_should_succeed()
    {
        // Arrange
        var subject = new ClassWithEnumOne { Enum = EnumOne.Two };

        var expectation = new ClassWithEnumThree { Enum = EnumThree.Two };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation, config => config.ComparingEnumsByName());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task
        When_asserting_members_from_different_enum_types_are_equivalent_by_value_but_comparing_by_name_it_should_throw()
    {
        // Arrange
        var subject = new ClassWithEnumOne { Enum = EnumOne.Two };
        var expectation = new ClassWithEnumFour { Enum = EnumFour.Three };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation, config => config.ComparingEnumsByName());

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*to equal EnumFour.Three {value: 3} by name, but found EnumOne.Two {value: 3}*");
    }

    [Fact]
    public async Task When_asserting_members_from_different_char_enum_types_are_equivalent_by_value_it_should_succeed()
    {
        // Arrange
        var subject = new ClassWithEnumCharOne { Enum = EnumCharOne.B };
        var expectation = new ClassWithEnumCharTwo { Enum = EnumCharTwo.ValueB };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expectation, config => config.ComparingEnumsByValue());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_asserting_enums_typed_as_object_are_equivalent_it_should_succeed()
    {
        // Arrange
        object e1 = EnumOne.One;
        object e2 = EnumOne.One;

        // Act
        Func<Task> act = () => e1.Should().BeEquivalentToAsync(e2);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_a_numeric_member_is_compared_with_an_enum_it_should_throw()
    {
        // Arrange
        var actual = new { Property = 1 };

        var expected = new { Property = TestEnum.First };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, options => options.ComparingEnumsByValue());

        // Assert
        await act.Should().ThrowAsync<XunitException>();
    }

    [Fact]
    public async Task When_a_string_member_is_compared_with_an_enum_it_should_throw()
    {
        // Arrange
        var actual = new { Property = "First" };

        var expected = new { Property = TestEnum.First };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, options => options.ComparingEnumsByName());

        // Assert
        await act.Should().ThrowAsync<XunitException>();
    }

    [Fact]
    public async Task When_null_enum_members_are_compared_by_name_it_should_succeed()
    {
        // Arrange
        var actual = new { Property = null as TestEnum? };

        var expected = new { Property = null as TestEnum? };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, options => options.ComparingEnumsByName());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_null_enum_members_are_compared_by_value_it_should_succeed()
    {
        // Arrange
        var actual = new { Property = null as TestEnum? };

        var expected = new { Property = null as TestEnum? };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, options => options.ComparingEnumsByValue());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_zero_and_null_enum_are_compared_by_value_it_should_throw()
    {
        // Arrange
        var actual = new { Property = (TestEnum)0 };

        var expected = new { Property = null as TestEnum? };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, options => options.ComparingEnumsByValue());

        // Assert
        await act.Should().ThrowAsync<XunitException>();
    }

    public enum TestEnum
    {
        First = 1
    }

    [Fact]
    public async Task When_subject_is_null_and_enum_has_some_value_it_should_throw()
    {
        // Arrange
        object subject = null;
        object expectedEnum = EnumULong.UInt64Max;

        // Act
        Func<Task> act = () =>
            subject.Should().BeEquivalentToAsync(expectedEnum, x => x.ComparingEnumsByName(), "comparing enums should throw");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected*to be equivalent to EnumULong.UInt64Max {value: 18446744073709551615} because comparing enums should throw, but found <null>*");
    }

    [Fact]
    public async Task When_expectation_is_null_and_subject_enum_has_some_value_it_should_throw_with_a_useful_message()
    {
        // Arrange
        object subjectEnum = EnumULong.UInt64Max;
        object expected = null;

        // Act
        Func<Task> act = () =>
            subjectEnum.Should().BeEquivalentToAsync(expected, x => x.ComparingEnumsByName(), "comparing enums should throw");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*to be <null> because comparing enums should throw, but found EnumULong.UInt64Max*");
    }

    [Fact]
    public async Task When_both_enums_are_equal_and_greater_than_max_long_it_should_not_throw()
    {
        // Arrange
        object enumOne = EnumULong.UInt64Max;
        object enumTwo = EnumULong.UInt64Max;

        // Act
        Func<Task> act = () => enumOne.Should().BeEquivalentToAsync(enumTwo);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_both_enums_are_equal_and_of_different_underlying_types_it_should_not_throw()
    {
        // Arrange
        object enumOne = EnumLong.Int64Max;
        object enumTwo = EnumULong.Int64Max;

        // Act
        Func<Task> act = () => enumOne.Should().BeEquivalentToAsync(enumTwo);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_both_enums_are_large_and_not_equal_it_should_throw()
    {
        // Arrange
        object subjectEnum = EnumLong.Int64LessOne;
        object expectedEnum = EnumULong.UInt64Max;

        // Act
        Func<Task> act = () => subjectEnum.Should().BeEquivalentToAsync(expectedEnum, "comparing enums should throw");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected subjectEnum*to equal EnumULong.UInt64Max {value: 18446744073709551615} by value because comparing enums should throw, but found EnumLong.Int64LessOne {value: 9223372036854775806}*");
    }
}
