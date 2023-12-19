using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NumericAssertionSpecs
{
    public class Match
    {
        [Fact]
        public void When_value_satisfies_predicate_it_should_not_throw()
        {
            // Arrange
            int value = 1;

            // Act / Assert
            value.Should().Match(o => o > 0);
        }

        [Fact]
        public void When_value_does_not_match_the_predicate_it_should_throw()
        {
            // Arrange
            int value = 1;

            // Act
            Action act = () => value.Should().Match(o => o == 0, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected value to match (o == 0) because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void When_value_is_matched_against_a_null_it_should_throw()
        {
            // Arrange
            int value = 1;

            // Act
            Action act = () => value.Should().Match(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }
    }
}
