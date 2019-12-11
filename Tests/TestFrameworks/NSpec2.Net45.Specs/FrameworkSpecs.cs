using System;
using FluentAssertions;
using NSpec;

namespace NSpec2.Specs
{
    public class FrameworkSpecs : nspec
    {
        // NSpec requires tests to start with "it" or "specify", see https://github.com/nspec/NSpec/blob/master/sln/src/NSpec/DefaultConventions.cs
        public void It_should_throw_nspec2_exceptions_for_assertion_failures_when_nspec2_is_used()
        {
            // Act
            Action act = () => 0.Should().Be(1);

            // Assert
            Exception exception = act.Should().Throw<Exception>().Which;
            exception.GetType().FullName.Should().ContainEquivalentOf("NSpec.Domain.AssertionException");
        }
    }
}
