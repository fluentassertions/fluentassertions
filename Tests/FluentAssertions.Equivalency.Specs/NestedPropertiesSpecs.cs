using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class NestedPropertiesSpecs
{
    [Fact]
    public async Task When_all_the_properties_of_the_nested_objects_are_equal_it_should_succeed()
    {
        // Arrange
        var subject = new Root
        {
            Text = "Root",
            Level = new Level1 { Text = "Level1", Level = new Level2 { Text = "Level2" } }
        };

        var expected = new RootDto
        {
            Text = "Root",
            Level = new Level1Dto { Text = "Level1", Level = new Level2Dto { Text = "Level2" } }
        };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_the_expectation_contains_a_nested_null_it_should_properly_report_the_difference()
    {
        // Arrange
        var subject = new Root { Text = "Root", Level = new Level1 { Text = "Level1", Level = new Level2() } };

        var expected = new RootDto { Text = "Root", Level = new Level1Dto { Text = "Level1", Level = null } };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Expected*Level.Level*to be <null>, but found*Level2*Without automatic conversion*");
    }

    [Fact]
    public async Task
        When_not_all_the_properties_of_the_nested_objects_are_equal_but_nested_objects_are_excluded_it_should_succeed()
    {
        // Arrange
        var subject = new
        {
            Property = new ClassWithValueSemanticsOnSingleProperty
            {
                Key = "123",
                NestedProperty = "Should be ignored"
            }
        };

        var expected = new
        {
            Property = new ClassWithValueSemanticsOnSingleProperty
            {
                Key = "123",
                NestedProperty = "Should be ignored as well"
            }
        };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected,
            options => options.ExcludingNestedObjects());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_nested_objects_should_be_excluded_it_should_do_a_simple_equality_check_instead()
    {
        // Arrange
        var item = new Item { Child = new Item() };

        // Act
        Func<Task> act = () => item.Should().BeEquivalentToAsync(new Item(), options => options.ExcludingNestedObjects());

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*Item*null*");
    }

    public class Item
    {
        public Item Child { get; set; }
    }

    [Fact]
    public async Task When_not_all_the_properties_of_the_nested_objects_are_equal_it_should_throw()
    {
        // Arrange
        var subject = new Root { Text = "Root", Level = new Level1 { Text = "Level1" } };

        var expected = new RootDto { Text = "Root", Level = new Level1Dto { Text = "Level2" } };

        // Act
        Func<Task> act = () =>
            subject.Should().BeEquivalentToAsync(expected);

        // Assert
        (await act.Should().ThrowAsync<XunitException>()).Which.Message

            // Checking exception message exactly is against general guidelines
            // but in that case it was done on purpose, so that we have at least have a single
            // test confirming that whole mechanism of gathering description from
            // equivalency steps works.
            .Should().Match(
                @"Expected property subject.Level.Text to be ""Level2"", but ""Level1"" differs near ""1"" (index 5).*" +
                "With configuration:*" +
                "- Use declared types and members*" +
                "- Compare enums by value*" +
                "- Compare tuples by their properties*" +
                "- Compare anonymous types by their properties*" +
                "- Compare records by their members*" +
                "- Match member by name (or throw)*" +
                "- Be strict about the order of items in byte arrays*" +
                "- Without automatic conversion.*");
    }

    [Fact]
    public async Task When_the_actual_nested_object_is_null_it_should_throw()
    {
        // Arrange
        var subject = new Root { Text = "Root", Level = new Level1 { Text = "Level2" } };

        var expected = new RootDto { Text = "Root", Level = null };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act
            .Should().ThrowAsync<XunitException>()
            .WithMessage("Expected*Level*to be <null>*, but found*Level1*Level2*");
    }

    public class StringSubContainer
    {
        public string SubValue { get; set; }
    }

    public class StringContainer
    {
        public StringContainer(string mainValue, string subValue = null)
        {
            MainValue = mainValue;
            SubValues = new[] { new StringSubContainer { SubValue = subValue } };
        }

        public string MainValue { get; set; }

        public IList<StringSubContainer> SubValues { get; set; }
    }

    public class MyClass2
    {
        public StringContainer One { get; set; }

        public StringContainer Two { get; set; }
    }

    [Fact]
    public async Task When_deeply_nested_strings_dont_match_it_should_properly_report_the_mismatches()
    {
        // Arrange
        var expected = new[]
        {
            new MyClass2 { One = new StringContainer("EXPECTED", "EXPECTED"), Two = new StringContainer("CORRECT") },
            new MyClass2()
        };

        var actual = new[]
        {
            new MyClass2
            {
                One = new StringContainer("INCORRECT", "INCORRECT"), Two = new StringContainer("CORRECT")
            },
            new MyClass2()
        };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*EXPECTED*INCORRECT*EXPECTED*INCORRECT*");
    }

    [Fact]
    public async Task When_the_nested_object_property_is_null_it_should_throw()
    {
        // Arrange
        var subject = new Root { Text = "Root", Level = null };

        var expected = new RootDto { Text = "Root", Level = new Level1Dto { Text = "Level2" } };

        // Act
        Func<Task> act = () =>
            subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act
            .Should().ThrowAsync<XunitException>()
            .WithMessage("Expected property subject.Level*to be*Level1Dto*Level2*, but found <null>*");
    }

    [Fact]
    public async Task When_not_all_the_properties_of_the_nested_object_exist_on_the_expected_object_it_should_throw()
    {
        // Arrange
        var subject = new { Level = new { Text = "Level1", } };

        var expected = new { Level = new { Text = "Level1", OtherProperty = "OtherProperty" } };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected);

        // Assert
        await act
            .Should().ThrowAsync<XunitException>()
            .WithMessage("Expectation has property subject.Level.OtherProperty that the other object does not have*");
    }

    [Fact]
    public async Task When_all_the_shared_properties_of_the_nested_objects_are_equal_it_should_succeed()
    {
        // Arrange
        var subject = new { Level = new { Text = "Level1", Property = "Property" } };

        var expected = new { Level = new { Text = "Level1", OtherProperty = "OtherProperty" } };

        // Act
        Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected, options => options.ExcludingMissingMembers());

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_deeply_nested_properties_do_not_have_all_equal_values_it_should_throw()
    {
        // Arrange
        var root = new Root
        {
            Text = "Root",
            Level = new Level1 { Text = "Level1", Level = new Level2 { Text = "Level2" } }
        };

        var rootDto = new RootDto
        {
            Text = "Root",
            Level = new Level1Dto { Text = "Level1", Level = new Level2Dto { Text = "A wrong text value" } }
        };

        // Act
        Func<Task> act = () => root.Should().BeEquivalentToAsync(rootDto);

        // Assert
        await act
            .Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected*Level.Level.Text*to be *\"Level2\"*A wrong text value*");
    }

    [Fact]
    public async Task When_two_objects_have_the_same_nested_objects_it_should_not_throw()
    {
        // Arrange
        var c1 = new ClassOne();
        var c2 = new ClassOne();

        // Act
        Func<Task> act = () => c1.Should().BeEquivalentToAsync(c2);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_a_property_of_a_nested_object_doesnt_match_it_should_clearly_indicate_the_path()
    {
        // Arrange
        var c1 = new ClassOne();
        var c2 = new ClassOne();
        c2.RefOne.ValTwo = 2;

        // Act
        Func<Task> act = () => c1.Should().BeEquivalentToAsync(c2);

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("Expected property c1.RefOne.ValTwo to be 2, but found 3*");
    }

    [Fact]
    public async Task Should_support_nested_collections_containing_empty_objects()
    {
        // Arrange
        var orig = new[] { new OuterWithObject { MyProperty = new[] { new Inner() } } };

        var expectation = new[] { new OuterWithObject { MyProperty = new[] { new Inner() } } };

        // Act / Assert
        await orig.Should().BeEquivalentToAsync(expectation);
    }

    public class Inner
    {
    }

    public class OuterWithObject
    {
        public Inner[] MyProperty { get; set; }
    }
}
