using System;
using FluentAssertions.Specialized;
using Xunit;

namespace FluentAssertions.Specs
{
    public class DelegateAssertionSpecs
    {
        [Fact]
        public void When_injecting_a_null_extractor_it_should_throw()
        {
            // Arrange
            Action subject = () => { };

            // Act
            Action act = () => new ActionAssertions(subject, extractor: null);

            // Act
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("extractor");
        }

        [Fact]
        public void When_injecting_a_null_clock_it_should_throw()
        {
            // Arrange
            Action subject = () => { };
            IExtractExceptions extractor = new AggregateExceptionExtractor();

            // Act
            Action act = () => new ActionAssertions(subject, extractor, clock: null);

            // Act
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("clock");
        }
    }
}
