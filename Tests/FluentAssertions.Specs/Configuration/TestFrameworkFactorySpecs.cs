using System;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;
using TestFramework = FluentAssertions.Configuration.TestFramework;

namespace FluentAssertions.Specs.Configuration;

public class TestFrameworkFactorySpecs
{
    [Fact]
    public void When_running_xunit_test_implicitly_it_should_be_detected()
    {
        // Arrange
        var testFramework = TestFrameworkFactory.GetFramework(null);

        // Act
        Action act = () => testFramework.Throw("MyMessage");

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_running_xunit_test_explicitly_it_should_be_detected()
    {
        // Arrange
        var testFramework = TestFrameworkFactory.GetFramework(TestFramework.XUnit2);

        // Act
        Action act = () => testFramework.Throw("MyMessage");

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_running_test_with_unknown_test_framework_it_should_throw()
    {
        // Act
        Action act = () => TestFrameworkFactory.GetFramework((TestFramework)42);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*the test framework '42' but this is not supported*");
    }

    [Fact]
    public void When_running_test_with_late_bound_but_unavailable_test_framework_it_should_throw()
    {
        // Act
        Action act = () => TestFrameworkFactory.GetFramework(TestFramework.NUnit);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*test framework 'nunit' but the required assembly 'nunit.framework' could not be found*");
    }
}
