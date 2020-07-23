using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class AggregateExceptionAssertionSpecs
    {
        [Fact]
        public void When_the_expected_exception_is_wrapped_it_should_succeed()
        {
            // Arrange
            var exception = new AggregateException(
                new InvalidOperationException("Ignored"),
                new XunitException("Background"));

            // Act
            Action act = () => throw exception;

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Background");
        }

        [Fact]
        public void When_the_expected_exception_was_not_thrown_it_should_report_the_actual_exceptions()
        {
            // Arrange
            Action throwingOperation = () =>
            {
                throw new AggregateException(
                    new InvalidOperationException("You can't do this"),
                    new NullReferenceException("Found a null"));
            };

            // Act
            Action act = () => throwingOperation
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Something I expected");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*InvalidOperation*You can't do this*")
                .WithMessage("*NullReferenceException*Found a null*");
        }

        [Fact]
        public void When_no_exception_was_expected_it_should_report_the_actual_exceptions()
        {
            // Arrange
            Action throwingOperation = () =>
            {
                throw new AggregateException(
                    new InvalidOperationException("You can't do this"),
                    new NullReferenceException("Found a null"));
            };

            // Act
            Action act = () => throwingOperation.Should().NotThrow();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*InvalidOperation*You can't do this*")
                .WithMessage("*NullReferenceException*Found a null*");
        }
    }
}
