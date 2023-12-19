using System;
using FluentAssertionsAsync;
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
        exception.GetType()
            .FullName.Should()
            .ContainEquivalentOf("NUnit.Framework.AssertionException");
    }
}
