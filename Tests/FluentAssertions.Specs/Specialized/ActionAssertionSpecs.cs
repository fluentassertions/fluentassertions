using System;
using Xunit;

namespace FluentAssertionsAsync.Specs.Specialized;

public class ActionAssertionSpecs
{
    [Fact]
    public void Null_clock_throws_exception()
    {
        // Arrange
        Action subject = () => { };

        // Act
        var act = void () => subject.Should(clock: null).NotThrow();

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("clock");
    }

    public class Throw
    {
        [Fact]
        public void Allow_additional_assertions_on_return_value()
        {
            // Arrange
            var exception = new Exception("foo");
            Action subject = () => throw exception;

            // Act / Assert
            subject.Should().Throw<Exception>()
                .Which.Message.Should().Be("foo");
        }
    }

    public class NotThrow
    {
        [Fact]
        public void Allow_additional_assertions_on_return_value()
        {
            // Arrange
            Action subject = () => { };

            // Act / Assert
            subject.Should().NotThrow()
                .And.NotBeNull();
        }
    }
}
