using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    public class Match
    {
        [Fact]
        public void When_nullable_value_satisfies_predicate_it_should_not_throw()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act / Assert
            nullableInteger.Should().Match(o => o.HasValue);
        }

        [Fact]
        public void When_nullable_value_does_not_match_the_predicate_it_should_throw()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () =>
                nullableInteger.Should().Match(o => !o.HasValue, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to match Not(o.HasValue) because we want to test the failure message, but found 1.");
        }

        [Fact]
        public void When_nullable_value_is_matched_against_a_null_it_should_throw()
        {
            // Arrange
            int? nullableInteger = 1;

            // Act
            Action act = () => nullableInteger.Should().Match(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }
    }
}
