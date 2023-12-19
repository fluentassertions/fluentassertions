using System;
using FluentAssertionsAsync;
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
        exception.GetType()
            .FullName.Should()
            .ContainEquivalentOf("Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException");
    }
}
