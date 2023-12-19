using System;
using System.Threading.Tasks;
using Xunit;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class TupleSpecs
{
    [Fact]
    public async Task When_a_nested_member_is_a_tuple_it_should_compare_its_property_for_equivalence()
    {
        // Arrange
        var actual = new { Tuple = (new[] { "string1" }, new[] { "string2" }) };

        var expected = new { Tuple = (new[] { "string1" }, new[] { "string2" }) };

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_a_tuple_is_compared_it_should_compare_its_components()
    {
        // Arrange
        var actual = new Tuple<string, bool, int[]>("Hello", true, new[] { 3, 2, 1 });
        var expected = new Tuple<string, bool, int[]>("Hello", true, new[] { 1, 2, 3 });

        // Act
        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        // Assert
        await act.Should().NotThrowAsync();
    }
}
