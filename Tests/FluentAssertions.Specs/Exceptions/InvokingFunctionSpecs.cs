using System;
using Xunit;

namespace FluentAssertions.Specs.Exceptions;

public class InvokingFunctionSpecs
{
    [Fact]
    public void Invoking_on_null_is_not_allowed()
    {
        // Arrange
        Func<int> someClass = null;

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
        Action act = () => someClass.Invoking((Func<Action, object>)null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("action");
    }
}
