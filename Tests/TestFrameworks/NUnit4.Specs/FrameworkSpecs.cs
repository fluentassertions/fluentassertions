using System;
using FluentAssertionsAsync;
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
        act.Should().Throw<AssertionException>();
    }
}
