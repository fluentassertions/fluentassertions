using System;
using FluentAssertions;
using NUnit.Framework;

namespace NUnit3.Specs;

[TestFixture]
public class FrameworkSpecs
{
    [Test]
    public void When_nunit3_is_used_it_should_throw_nunit_exceptions_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;

        // Don't reference the exception type explicitly like this: act.Should().Throw<AssertionException>()
        // It could cause this specs project to load the assembly containing the exception (this actually happens for xUnit)
        exception.GetType().FullName.Should().Be("NUnit.Framework.AssertionException");
    }
}
