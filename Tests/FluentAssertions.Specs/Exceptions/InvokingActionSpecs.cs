using System;
using Xunit;

namespace FluentAssertions.Specs.Exceptions;

public class InvokingActionSpecs
{
    [Fact]
    public void Invoking_on_null_is_not_allowed()
    {
        // Arrange
        Action someClass = null;

        // Act
        Action act = () => someClass.Invoking(d => d());

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("subject");
    }

    [Fact]
    public void Invoking_with_null_is_not_allowed()
    {
        // Arrange
        Action someClass = () => { };

        // Act
        Action act = () => someClass.Invoking(null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("action");
    }
}
