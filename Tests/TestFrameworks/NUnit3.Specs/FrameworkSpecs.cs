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
        act.Should().Throw<AssertionException>();
    }
}
