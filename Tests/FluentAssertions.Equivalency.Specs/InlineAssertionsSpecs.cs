using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class InlineAssertionsSpecs
{
    [Fact]
    public void The_predicate_must_be_matched()
    {
        // Arrange
        var actual = new
        {
            Name = "John",
            Age = 30
        };

        var expectation = new
        {
            Name = "John",
            Age = Value.ThatMatches<int>(age => age < 30)
        };

        // Act
        var act = () => actual.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Age*30*");
    }
}
