using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestV2.Specs;

[TestClass]
public class FrameworkSpecs
{
    [TestMethod]
    public void When_mstestv2_is_used_it_should_throw_mstest_exceptions_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;

        // Don't reference the exception type explicitly like this: act.Should().Throw<AssertFailedException>()
        // It could cause this specs project to load the assembly containing the exception (this actually happens for xUnit)
        exception.GetType().FullName.Should().Be("Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException");
    }
}
