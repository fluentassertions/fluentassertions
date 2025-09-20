using System;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Exceptions;

public class ExceptionMessageSpecs
{
    [Fact]
    public void Can_assert_the_exception_message_does_not_include_a_specific_string()
    {
        // Arrange
        Action throwException = () => throw new InvalidOperationException("Something bad happened");

        // Act
        Action act = () => throwException.Should()
            .Throw<InvalidOperationException>()
            .WithoutMessage("*bad*")
            .Which.Should().Be(typeof(InvalidOperationException));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Did not expect the exception message to match the equivalent of \"*bad*\", but \"Something bad happened\" matches.");
    }

    [Fact]
    public void Only_check_the_message_of_an_actual_exception()
    {
        // Arrange
        Action dontThrowAtAll = () => { };

        // Act
        var act = () =>
        {
            using var _ = new AssertionScope();

            return dontThrowAtAll.Should()
                .Throw<InvalidOperationException>()
                .WithoutMessage("*bad*");
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected*System.InvalidOperationException*no exception*");
    }

    [Fact]
    public async Task Can_assert_an_async_exception_message_does_not_include_a_specific_string()
    {
        // Arrange
        Func<Task> throwsAsync = () => throw new AggregateException(new ArgumentException("That was wrong."));

        // Act
        var act = () => throwsAsync.Should().ThrowAsync<ArgumentException>().WithoutMessage("That was wrong.");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Did not expect the exception message to match the equivalent of \"*wrong*\", but \"That was wrong.\" matches.");
    }
}
