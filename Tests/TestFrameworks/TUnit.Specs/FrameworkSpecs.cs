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

        // Don't reference the exception type explicitly like this: act.Should().Throw<AssertionException>()
        // It could cause this specs project to load the assembly containing the exception (this actually happens for xUnit)
        exception.GetType().FullName.Should().Be("TUnit.Assertions.Exceptions.AssertionException");
    }
}
