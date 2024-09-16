using System;
using FluentAssertions;
using TUnit.Assertions.Exceptions;

namespace TUnit.Specs;

public class FrameworkSpecs
{
    [Test]
    public void When_tunit_is_used_it_should_throw_tunit_exceptions_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        act.Should().Throw<AssertionException>();
    }
}
