using System;
using FluentAssertionsAsync.Execution;
using Xunit;

namespace FluentAssertionsAsync.Specs.Specialized;

public class DelegateAssertionSpecs
{
    [Fact]
    public void Null_clock_throws_exception()
    {
        // Arrange
        Func<int> subject = () => 1;

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

    public class ThrowExactly
    {
        [Fact]
        public void Does_not_continue_assertion_on_exact_exception_type()
        {
            // Arrange
            var a = () => { };

            // Act
            using var scope = new AssertionScope();
            a.Should().ThrowExactly<InvalidOperationException>();

            // Assert
            scope.Discard().Should().ContainSingle()
                .Which.Should().Match("*InvalidOperationException*no exception*");
        }
    }
}
