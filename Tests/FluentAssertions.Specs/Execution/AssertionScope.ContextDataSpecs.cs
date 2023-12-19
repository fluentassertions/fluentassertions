using FluentAssertionsAsync.Execution;
using Xunit;

namespace FluentAssertionsAsync.Specs.Execution;

/// <content>
/// The chaining API specs.
/// </content>
public partial class AssertionScopeSpecs
{
    [Fact]
    public void Get_value_when_key_is_present()
    {
        // Arrange
        var scope = new AssertionScope();
        scope.AddNonReportable("SomeKey", "SomeValue");
        scope.AddNonReportable("SomeOtherKey", "SomeOtherValue");

        // Act
        var value = scope.Get<string>("SomeKey");

        // Assert
        value.Should().Be("SomeValue");
    }

    [Fact]
    public void Get_default_value_when_key_is_not_present()
    {
        // Arrange
        var scope = new AssertionScope();

        // Act
        var value = scope.Get<int>("SomeKey");

        // Assert
        value.Should().Be(0);
    }

    [Fact]
    public void Get_default_value_when_nullable_value_is_null()
    {
        // Arrange
        var scope = new AssertionScope();

        int? someValue = null;
        scope.AddNonReportable("SomeKey", someValue);

        // Act
        var value = scope.Get<int>("SomeKey");

        // Assert
        value.Should().Be(0);
    }

    [Fact]
    public void Value_should_be_of_requested_type()
    {
        // Arrange
        var scope = new AssertionScope();
        scope.AddNonReportable("SomeKey", "SomeValue");

        // Act
        var value = scope.Get<string>("SomeKey");

        // Assert
        value.Should().BeOfType<string>();
    }
}
