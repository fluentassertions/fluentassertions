#if NET6_0_OR_GREATER

using System;
using System.Text.Json.Nodes;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class JsonNodeSpecs
{
    [Fact]
    public void Supports_numeric_properties()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "number" : 1,
            "floatingpoint": 1.0
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            number = 2,
            floatingpoint = 2.0
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*$.number*2*1*$.floatingpoint*2.0*1.0*");
    }

    [Fact]
    public void Supports_string_properties()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "name": "John",
            "description": "A person"
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            name = "Jane",
            description = "Another person"
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*JSON property $.name*Jane*John*");
    }

    [Fact]
    public void Supports_boolean_properties()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "isActive": true,
            "isDeleted": false
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            isActive = false,
            isDeleted = true
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*$.isActive*False*True*$.isDeleted*True*False*");
    }

    [Fact]
    public void Supports_null_properties()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "nullValue": null,
            "notNull": "value"
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            nullValue = "not null",
            notNull = (string)null
        });

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected JSON property $.nullValue to be*not null*but found <null>*$.notNull to be <null>*value*");
    }

    [Fact]
    public void Supports_array_properties()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "numbers": [1, 2, 3],
            "strings": ["a", "b", "c"]
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            numbers = new[]
            {
                1,
                2,
                4
            },
            strings = new[]
            {
                "a",
                "b",
                "d"
            }
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*$.numbers[2]*4*3*$.strings[2]*d*c*");
    }

    [Fact]
    public void Supports_nested_object_properties()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "person": {
                "name": "John",
                "age": 25
            },
            "address": {
                "street": "Main St",
                "city": "New York"
            }
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            person = new
            {
                name = "Jane",
                age = 30
            },
            address = new
            {
                street = "Second St",
                city = "Boston"
            }
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*property $.person.name*Jane*John*");
    }

    [Fact]
    public void Supports_empty_objects()
    {
        // Arrange
        var node = JsonNode.Parse("{}");

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
        });

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("*No members were found for comparison*");
    }

    [Fact]
    public void Supports_empty_arrays()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "emptyArray": []
            }
            """);

        // Act & Assert
        node.Should().BeEquivalentTo(new
        {
            emptyArray = new int[0]
        });
    }

    [Fact]
    public void Supports_mixed_type_arrays()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "mixedArray": [1, "text", true, null]
            }
            """);

        // Act & Assert
        node.Should().BeEquivalentTo(new
        {
            mixedArray = new object[]
            {
                1,
                "text",
                true,
                null
            }
        });
    }

    [Fact]
    public void Supports_deeply_nested_structures()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "level1": {
                "level2": {
                    "level3": {
                        "value": "deep"
                    }
                }
            }
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            level1 = new
            {
                level2 = new
                {
                    level3 = new
                    {
                        value = "shallow"
                    }
                }
            }
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*$.level1.level2.level3.value*shallow*deep*");
    }

    [Fact]
    public void Can_detect_a_missing_property()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "existingProperty": "value"
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            existingProperty = "value",
            missingProperty = "missing"
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*missingProperty*");
    }

    [Fact]
    public void Can_ignore_a_missing_property()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "existingProperty": "value"
            }
            """);

        // Act
        var act = () => node.Should().BeEquivalentTo(new
        {
            existingProperty = "wrongvalue",
            missingProperty = "missing"
        }, options => options.ExcludingMissingMembers());

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*existingProperty*wrongvalue*")
            .Which.Message.Should().NotContain("missingProperty");
    }

    [Fact]
    public void Can_interpret_an_iso_date_as_a_local_datetime()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "date": "2025-09-11T21:17:00"
            }
            """);

        // Act / Assert
        node.Should().BeEquivalentTo(new
        {
            date = 11.September(2025).At(21, 17),
        });
    }

    [Fact]
    public void Can_interpret_an_iso_date_as_an_utc_datetime()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "date": "2025-09-11T21:17:00Z"
            }
            """);

        // Act / Assert
        node.Should().BeEquivalentTo(new
        {
            date = 11.September(2025).At(21, 17).AsUtc()
        });
    }

    [Fact]
    public void Casing_of_properties_must_match_by_default()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "existingProperty": "value"
            }
            """);

        // Act / Assert
        node.Should().BeEquivalentTo(new
        {
            existingProperty = "value",
        });
    }

    [Fact]
    public void Can_match_properties_even_if_the_casing_is_different()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "existingProperty": "value"
            }
            """);

        // Act / Assert
        node.Should().BeEquivalentTo(new
        {
            ExistingProperty = "value",
        }, options => options.IgnoringJsonPropertyCasing());
    }

    [Fact]
    public void Supports_extra_properties_in_json()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "expectedProperty": "value",
            "extraProperty": "extra"
            }
            """);

        // Act & Assert
        // This should pass as extra properties are typically ignored in equivalency
        node.Should().BeEquivalentTo(new
        {
            expectedProperty = "value"
        }, options => options.IgnoringJsonPropertyCasing());
    }

    [Fact]
    public void Supports_complex_mixed_scenario()
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

        // Act & Assert
        node.Should().BeEquivalentTo(new
        {
            id = 123,
            name = "Product",
            price = 99.99,
            isAvailable = true,
            tags = new[]
            {
                "electronics",
                "gadget"
            },
            metadata = new
            {
                createdBy = "admin",
                createdAt = (string)null,
                settings = new
                {
                    visible = true,
                    priority = 1
                }
            },
            variants = new object[0]
        });
    }

    [Fact]
    public void Can_assert_the_inequivalence_of_complex_mixed_scenario()
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
        var act = () => node.Should().NotBeEquivalentTo(new
        {
            id = 123,
            name = "Product",
            price = 99.99,
            isAvailable = true,
            tags = new[]
            {
                "electronics",
                "gadget"
            },
            metadata = new
            {
                createdBy = "admin",
                createdAt = (string)null,
                settings = new
                {
                    visible = true,
                    priority = 1
                }
            },
            variants = new object[0]
        });

        // Assert
        act.Should().Throw<XunitException>().WithMessage("Did not expect*metadata*but they are*");
    }

    [Fact]
    public void An_inequivalency_assertion_can_be_case_insensitive()
    {
        // Arrange
        var node = JsonNode.Parse(
            """
            {
            "existingProperty": "value"
            }
            """);

        // Act
        var act = () => node.Should().NotBeEquivalentTo(new
        {
            ExistingProperty = "value",
        }, options => options.IgnoringJsonPropertyCasing());

        // Assert
        act.Should().Throw<XunitException>().WithMessage("Did not expect*existingProperty*");
    }

    [Fact]
    public void Can_treat_a_json_array_as_a_collection()
    {
        // Arrange
        JsonArray array = JsonNode.Parse("[1, 2, 3]")!.AsArray();

        // Act & Assert
        array.Should().NotBeEmpty();
    }

    [Fact]
    public void Can_treat_a_null_json_array_as_a_collection()
    {
        // Arrange
        JsonArray array = null;

        // Act & Assert
        array.Should().BeNull();
    }

    [Fact]
    public void Can_assert_json_node_instances()
    {
        // Arrange
        JsonNode jsonNode = null;

        // Act & Assert
        jsonNode.Should().BeNull();
    }

    [Fact]
    public void Can_assert_json_value_instances()
    {
        // Arrange
        JsonValue jsonValue = null;

        // Act & Assert
        jsonValue.Should().BeNull();
    }

    [Fact]
    public void Can_assert_json_object_instances()
    {
        // Arrange
        JsonObject jsonObject = null;

        // Act & Assert
        jsonObject.Should().BeNull();
    }
}

#endif
