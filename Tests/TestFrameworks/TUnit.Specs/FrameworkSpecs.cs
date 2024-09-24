using System;
using FluentAssertions;

namespace TUnit.Specs;

public class FrameworkSpecs
{
    [Test]
    public void When_tunit_is_used_it_should_throw_tunit_exceptions_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;

        exception.GetType()
            .FullName.Should()
            .ContainEquivalentOf("TUnit.Assertions.Exceptions.AssertionException");
    }
}
