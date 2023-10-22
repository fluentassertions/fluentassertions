using System;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions.Specialized;
using Xunit;

namespace FluentAssertions.Specs.Specialized;

public class DelegateAssertionSpecs
{
    [Fact]
    public void When_injecting_a_null_extractor_it_should_throw()
    {
        // Arrange
        Action subject = () => { };

        // Act
        Func<ActionAssertions> act = () => new ActionAssertions(subject, extractor: null);

        // Act
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("extractor");
    }

    [Fact]
    public void When_injecting_a_null_clock_it_should_throw()
    {
        // Arrange
        Action subject = () => { };
        IExtractExceptions extractor = new AggregateExceptionExtractor();

        // Act
        Func<ActionAssertions> act = () => new ActionAssertions(subject, extractor, clock: null);

        // Act
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("clock");
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
