#if NET6_0_OR_GREATER

using System.Text.Json.Nodes;
using FluentAssertions.Formatting;
using Xunit;
using Formatter = FluentAssertions.Formatting.Formatter;

namespace FluentAssertions.Specs.Formatting;

public class JsonFormatterSpecs
{
    [Fact]
    public void Can_format_a_json_node()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
                "id": 123,
                "name": "Product",
                "price": 99.99,
                "isAvailable": true,
                "tags": ["electronics", "gadget"],
                "metadata": {
                    "createdBy": "admin",
                    "createdAt": null,
                    "settings": {
                        "visible": true,
                        "priority": 1
                    }
                },
                "variants": []
            }
            """);

        // Act
        string result = Formatter.ToString(node);

        // Assert
        result.Should().Be(
            """
            {"id":123,"name":"Product","price":99.99,"isAvailable":true,"tags":["electronics","gadget"],"metadata":{"createdBy":"admin","createdAt":null,"settings":{"visible":true,"priority":1}},"variants":[]}
            """);
    }

    [Fact]
    public void Can_format_a_json_node_using_line_breaks()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
                "id": 123,
                "name": "Product",
                "price": 99.99,
                "isAvailable": true,
                "tags": ["electronics", "gadget"],
                "metadata": {
                    "createdBy": "admin",
                    "createdAt": null,
                    "settings": {
                        "visible": true,
                        "priority": 1
                    }
                },
                "variants": []
            }
            """);

        // Act
        string result = Formatter.ToString(node, new FormattingOptions
        {
            UseLineBreaks = true
        });

        // Assert
        result.Should().Be(
            """
            {
              "id": 123,
              "name": "Product",
              "price": 99.99,
              "isAvailable": true,
              "tags": [
                "electronics",
                "gadget"
              ],
              "metadata": {
                "createdBy": "admin",
                "createdAt": null,
                "settings": {
                  "visible": true,
                  "priority": 1
                }
              },
              "variants": []
            }
            """);
    }
}

#endif
