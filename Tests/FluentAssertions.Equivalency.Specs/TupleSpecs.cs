using System;
using Xunit;

namespace FluentAssertions.Equivalency.Specs
{
    public class TupleSpecs
    {
        [Fact]
        public void When_a_nested_member_is_a_tuple_it_should_compare_its_property_for_equivalence()
        {
            // Arrange
            var actual = new { Tuple = (new[] { "string1" }, new[] { "string2" }) };

            var expected = new { Tuple = (new[] { "string1" }, new[] { "string2" }) };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_tuple_is_compared_it_should_compare_its_components()
        {
            // Arrange
            var actual = new Tuple<string, bool, int[]>("Hello", true, new int[] { 3, 2, 1 });
            var expected = new Tuple<string, bool, int[]>("Hello", true, new int[] { 1, 2, 3 });

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }
    }
}
