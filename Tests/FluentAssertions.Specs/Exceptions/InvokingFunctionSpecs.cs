using System;
using Xunit;

namespace FluentAssertionsAsync.Specs.Exceptions;

public class InvokingFunctionSpecs
{
    [Fact]
    public void Invoking_on_null_is_not_allowed()
    {
        // Arrange
        Does someClass = null;

        // Act
        Action act = () => someClass.Invoking(d => d.Return());

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
        Action act = () => someClass.Invoking((Func<Does, object>)null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("action");
    }
}
