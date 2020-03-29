using System;

using FluentAssertions.Collections;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class AndWhichConstraintSpecs
    {
        [Fact]
        public void When_many_objects_are_provided_accessing_which_should_throw_a_descriptive_exception()
        {
            // Arrange
            var continuation = new AndWhichConstraint<StringCollectionAssertions, string>(null, new[] { "hello", "world" });

            // Act
            Action act = () =>
            {
                var item = continuation.Which;
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "More than one object found.  FluentAssertions cannot determine which object is meant.*")
                .WithMessage("*Found objects:*\"hello\"*\"world\"");
        }
    }
}
