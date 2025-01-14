using System;
using FluentAssertions;
using NUnit.Framework;

namespace NUnit4.Specs;

[TestFixture]
public class FrameworkSpecs
{
    [Test]
    public void Throw_nunit_framework_exception_for_nunit4_tests()
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
