using System;
using Xunit;

namespace FluentAssertionsAsync.Specs.Exceptions;

public class InvokingActionSpecs
{
    [Fact]
    public void Invoking_on_null_is_not_allowed()
    {
        // Arrange
        Does someClass = null;

        // Act
        Action act = () => someClass.Invoking(d => d.Do());

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("subject");
    }

    [Fact]
    public void Invoking_with_null_is_not_allowed()
    {
        // Arrange
        Does someClass = Does.NotThrow();

        // Act
        Action act = () => someClass.Invoking(null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("action");
    }
}
