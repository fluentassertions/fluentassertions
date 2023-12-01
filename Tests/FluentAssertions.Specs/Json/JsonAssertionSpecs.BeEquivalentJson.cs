#if NET6_0_OR_GREATER
#nullable enable
using System;
using Xunit;

namespace FluentAssertions.Specs.Json;

public class JsonAssertionSpecs
{
    public class BeEquivalentJsonTo
    {
        [Fact]
        public void When_json_is_the_the_same_string_it_should_validate()
        {
            // Arrange
            string actual = """
            {
                "a": 1,
                "b": 2
            }
            """;
            actual.Should().BeJsonEquivalentTo(actual);
        }

        [Fact]
        public void When_json_has_different_indentation_it_should_validate()
        {
            // Arrange
            string actual = """
            {
                "a": 1,
                "b": 2
            }
            """;
            string expected = """{"a":1,"b":2}""";
            actual.Should().BeJsonEquivalentTo(expected);
        }

        [Fact]
        public void When_property_order_is_different_it_should_throw()
        {
            // Arrange
            string actual = """
            {
                "a": 1,
                "b": 2,
            }
            """;
            string expected = """
            {
                "b": 2,
                "a": 1
            }
            """;
            var action = () => actual.Should().BeJsonEquivalentTo(expected);
            action.Should().Throw<Exception>().Which.Message.Should().Be("""
                 Expected actual to match expected json, but found diff
                 "
                 PropertyName mismatch (validate strict order)
                             { "a": 1, "b": 2, } (diff at index 6)
                             { "b": 2, "a": 1 } (diff at index 6)
                               ^
                 "
                 """);
        }
    }
}
#endif
