#if NET6_0_OR_GREATER

using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Nodes;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Specialized;

public class JsonNodeSpecs
{
    public class HaveProperty
    {
        [Fact]
        public void Can_ensure_a_property_exists()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("{ \"name\": \"John\" }");

            // Act
            var act = () => jsonNode.Should().HaveProperty("code", "that is what we expect");

            // Assert
            act.Should().Throw<XunitException>("Expected jsonNode to have property \"code\" because that is what we expect");
        }

        [Fact]
        public void Can_continue_with_the_value_of_a_property()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("{ \"name\": \"John\" }");

            // Act
            var subject = jsonNode.Should().HaveProperty("name").Which;

            // Assert
            subject.ToString().Should().Be("John");
        }

        [Fact]
        public void Can_ensure_a_property_does_not_exist()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("{ \"name\": \"John\" }");

            // Act & Assert
            jsonNode.Should().NotHaveProperty("code");
        }

        [Fact]
        public void Fails_for_an_unexpected_property()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("{ \"name\": \"John\" }");

            // Act
            var act = () => jsonNode.Should().NotHaveProperty("name", "because we expect it not to exist");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect jsonNode to have property \"name\" because we expect it not to exist.");
        }

        [Fact]
        public void Cannot_check_for_a_property_on_a_null_node()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act
            var act = () => jsonNode.Should().HaveProperty("name");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Cannot assert the existence of a property on a <null> JSON node*");
        }

        [Fact]
        public void Cannot_check_for_the_absence_of_property_on_a_null_node()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act
            var act = () => jsonNode.Should().NotHaveProperty("name");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Cannot assert the existence of a property on a <null> JSON node*");
        }
    }

    public class BeArray
    {
        [Fact]
        public void Can_ensure_a_json_fragment_is_an_array()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("[1, 2, 3]");

            // Act
            IEnumerable<JsonNode> items = jsonNode.Should().BeAnArray().Which;

            // Assert
            items.Should().HaveCount(3);
        }

        [Fact]
        public void Fails_for_a_non_array_json_fragment()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("{ \"name\": \"John\" }");

            // Act
            var act = () => jsonNode.Should().BeAnArray("we expect it to be an array");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected jsonNode to be an array because we expect it to be an array, but* is not.");
        }

        [Fact]
        public void Can_ensure_json_is_not_an_array()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("{ \"name\": \"John\" }");

            // Act & Assert
            jsonNode.Should().NotBeAnArray();
        }

        [Fact]
        public void Fails_for_an_unexpected_array_fragment()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("[1, 2, 3]");

            // Act
            var act = () => jsonNode.Should().NotBeAnArray("we expect it not to be an array");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect jsonNode to be an array because we expect it not to be an array, but*1*is.");
        }

        [Fact]
        public void A_null_node_cannot_be_an_array()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act
            var act = () => jsonNode.Should().BeAnArray();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected jsonNode to be an array*");
        }

        [Fact]
        public void A_null_node_cannot_not_be_an_array()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act & Assert
            jsonNode.Should().NotBeAnArray();
        }
    }

    public class BeNumeric
    {
        [Fact]
        public void The_minimal_double_value_is_numeric()
        {
            // Arrange
            var jsonNode = JsonNode.Parse(double.MinValue.ToString(CultureInfo.InvariantCulture));

            // Act & Assert
            jsonNode.Should().BeNumeric();
        }

        [Fact]
        public void The_maximum_double_value_is_numeric()
        {
            // Arrange
            var jsonNode = JsonNode.Parse(double.MaxValue.ToString(CultureInfo.InvariantCulture));

            // Act & Assert
            jsonNode.Should().BeNumeric();
        }

        [Fact]
        public void The_minimal_long_value_is_numeric()
        {
            // Arrange
            var jsonNode = JsonNode.Parse(long.MinValue.ToString(CultureInfo.InvariantCulture));

            // Act & Assert
            jsonNode.Should().BeNumeric();
        }

        [Fact]
        public void The_maximum_long_value_is_numeric()
        {
            // Arrange
            var jsonNode = JsonNode.Parse(long.MaxValue.ToString(CultureInfo.InvariantCulture));

            // Act & Assert
            jsonNode.Should().BeNumeric();
        }

        [Fact]
        public void The_maximum_unsigned_long_value_is_numeric()
        {
            // Arrange
            var jsonNode = JsonNode.Parse(ulong.MaxValue.ToString(CultureInfo.InvariantCulture));

            // Act & Assert
            jsonNode.Should().BeNumeric();
        }

        [Fact]
        public void Can_return_the_actual_value()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("42");

            // Act & Assert
            jsonNode.Should().BeNumeric().Which.Should().Be("42");
        }

        [Fact]
        public void Can_return_the_actual_numeric_value_as_a_specific_type()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("42");

            // Act & Assert
            jsonNode.Should().BeNumeric<int>().Which.Should().Be(42);
        }

        [Fact]
        public void Includes_the_reason_for_something_other_than_a_number()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"not a number\"");

            // Act
            var act = () => jsonNode.Should().BeNumeric("we expect an int");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected jsonNode to be a numeric value because we expect an int, but \"not a number\" is not.");
        }

        [Fact]
        public void Can_ensure_a_string_is_not_a_number()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"not a number\"");

            // Act & Assert
            jsonNode.Should().NotBeNumeric();
        }

        [Fact]
        public void Can_ensure_a_ulong_is_a_number()
        {
            // Arrange
            var jsonNode = JsonNode.Parse(ulong.MaxValue.ToString(CultureInfo.InvariantCulture));

            // Act
            var act = () => jsonNode.Should().NotBeNumeric();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Can_ensure_a_double_is_a_number()
        {
            // Arrange
            var jsonNode = JsonNode.Parse(double.MaxValue.ToString(CultureInfo.InvariantCulture));

            // Act
            var act = () => jsonNode.Should().NotBeNumeric();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Includes_a_reason_for_an_unexpected_number()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("42");

            // Act
            var act = () => jsonNode.Should().NotBeNumeric("we expect something else");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect jsonNode to be a numeric value because we expect something else, but 42 is.");
        }

        [Fact]
        public void Fails_while_asserting_a_null_json_is_a_number()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act
            var act = () => jsonNode.Should().BeNumeric();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected jsonNode to be a numeric value*");
        }

        [Fact]
        public void A_null_node_is_not_a_number()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act & Assert
            jsonNode.Should().NotBeNumeric();
        }
    }

    public class BeLocalDate
    {
        [Fact]
        public void Can_ensure_json_is_a_local_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00\"");

            // Act & Assert
            jsonNode.Should().BeLocalDate().Which.Should().Be(11.September(2025).At(21, 17).AsLocal());
        }

        [Fact]
        public void Fails_for_something_that_is_not_a_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"not a date\"");

            // Act
            var act = () => jsonNode.Should().BeLocalDate("we expect a local date");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected jsonNode to be a local date because we expect a local date, but \"not a date\" is not.");
        }

        [Fact]
        public void A_utc_date_is_not_a_local_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00Z\"");

            // Act
            var act = () => jsonNode.Should().BeLocalDate("we don't expect UTC");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected jsonNode to be a local date because we don't expect UTC, but*17:00Z*is not.");
        }

        [Fact]
        public void A_null_node_is_not_a_local_date()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act
            var act = () => jsonNode.Should().BeLocalDate();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected jsonNode to be a local date*");
        }

        [Fact]
        public void An_arbitrary_string_is_not_a_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"not a date\"");

            // Act & Assert
            jsonNode.Should().NotBeLocalDate();
        }

        [Fact]
        public void Fails_for_a_local_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00\"");

            // Act
            var act = () => jsonNode.Should().NotBeLocalDate("we expect something else");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect jsonNode to be a local date because we expect something else, but*21:17* is.");
        }

        [Fact]
        public void A_utc_date_is_not_a_local_date_as_expected()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00Z\"");

            // Act & Assert
            jsonNode.Should().NotBeLocalDate("we expect something else");
        }

        [Fact]
        public void A_null_node_is_correctly_identified_as_not_a_local_date()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act & Assert
            jsonNode.Should().NotBeLocalDate();
        }
    }

    public class BeUtcDate
    {
        [Fact]
        public void Can_ensure_json_is_a_utc_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00Z\"");

            // Act & Assert
            jsonNode.Should().BeUtcDate().Which.Should().Be(11.September(2025).At(21, 17).AsUtc());
        }

        [Fact]
        public void Fails_for_something_that_is_not_a_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"not a date\"");

            // Act
            var act = () => jsonNode.Should().BeUtcDate("we expect that");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected jsonNode to be a UTC date because we expect that, but \"not a date\" is not.");
        }

        [Fact]
        public void A_local_date_is_not_an_utc_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00\"");

            // Act
            var act = () => jsonNode.Should().BeUtcDate("we don't expect local");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected jsonNode to be a UTC date because we don't expect local, but*17:00*is not.");
        }

        [Fact]
        public void A_null_node_is_not_a_local_date()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act
            var act = () => jsonNode.Should().BeUtcDate();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected jsonNode to be a UTC date, but*null*is not.");
        }

        [Fact]
        public void An_arbitrary_string_is_not_a_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"not a date\"");

            // Act & Assert
            jsonNode.Should().NotBeUtcDate();
        }

        [Fact]
        public void Fails_for_a_utc_date()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00Z\"");

            // Act
            var act = () => jsonNode.Should().NotBeUtcDate("we expect something else");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect jsonNode to be a UTC date because we expect something else, but*21:17* is.");
        }

        [Fact]
        public void A_local_date_is_not_a_utc_date_as_expected()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"2025-09-11T21:17:00\"");

            // Act & Assert
            jsonNode.Should().NotBeUtcDate("we expect something else");
        }

        [Fact]
        public void A_null_node_is_correctly_identified_as_not_a_UTC_date()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act & Assert
            jsonNode.Should().NotBeUtcDate();
        }
    }

    public class BeBool
    {
        [Fact]
        public void Can_ensure_the_json_fragment_is_a_bool()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("true");

            // Act & Assert
            jsonNode.Should().BeBool().Which.Should().BeTrue();
        }

        [Fact]
        public void Fails_for_a_json_fragment_that_is_not_a_bool()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"not a boolean\"");

            // Act
            var act = () => jsonNode.Should().BeBool("we expect a boolean");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected jsonNode to be a boolean because we expect a boolean, but \"not a boolean\" is not.");
        }

        [Fact]
        public void Null_is_not_a_json_fragment()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act
            var act = () => jsonNode.Should().BeBool();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected jsonNode to be a boolean, but <null> is not.");
        }

        [Fact]
        public void Can_ensure_json_is_not_a_bool()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"not a boolean\"");

            // Act & Assert
            jsonNode.Should().NotBeBool();
        }

        [Fact]
        public void Fails_for_a_json_fragment_is_a_bool_after_all()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("true");

            // Act
            var act = () => jsonNode.Should().NotBeBool("we are expected something else");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect jsonNode to be a boolean because we are expected something else, but*true* is.");
        }

        [Fact]
        public void Null_is_not_a_bool_as_expected()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act & Assert
            jsonNode.Should().NotBeBool();
        }
    }

    public class BeString
    {
        [Fact]
        public void Can_ensure_json_is_string()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"Hello World\"");

            // Act & Assert
            jsonNode.Should().BeString().Which.Should().Be("Hello World");
        }

        [Fact]
        public void Fails_for_anything_but_a_string()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("42");

            // Act
            var act = () => jsonNode.Should().BeString("we expect a string");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected jsonNode to be a string because we expect a string, but 42 is not.");
        }

        [Fact]
        public void A_null_value_is_not_a_json_string()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act
            var act = () => jsonNode.Should().BeString();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected jsonNode to be a string*");
        }

        [Fact]
        public void Can_ensure_json_is_not_string()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("42");

            // Act & Assert
            jsonNode.Should().NotBeString();
        }

        [Fact]
        public void Fails_for_a_string_that_is_not_expected()
        {
            // Arrange
            var jsonNode = JsonNode.Parse("\"Hello World\"");

            // Act
            var act = () => jsonNode.Should().NotBeString("we expect something else");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect jsonNode to be a string because we expect something else, but*Hello World*is.");
        }

        [Fact]
        public void A_null_value_is_not_a_string_as_expected()
        {
            // Arrange
            JsonNode jsonNode = null;

            // Act & Assert
            jsonNode.Should().NotBeString();
        }
    }
}

#endif
