using System;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Execution;

public class TestFrameworkProviderTests
{
    [Fact]
    public void When_running_xunit_test_implicitly_it_should_be_detected()
    {
        // Arrange
        var configuration = new Configuration(new TestConfigurationStore());
        var testFrameworkProvider = new TestFrameworkProvider(configuration);

        // Act
        Action act = () => testFrameworkProvider.Throw("MyMessage");

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_running_xunit_test_explicitly_it_should_be_detected()
    {
        // Arrange
        var configuration = new Configuration(new TestConfigurationStore())
        {
            TestFrameworkName = "xunit2"
        };

        var testFrameworkProvider = new TestFrameworkProvider(configuration);

        // Act
        Action act = () => testFrameworkProvider.Throw("MyMessage");

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_running_test_with_unknown_test_framework_it_should_throw()
    {
        // Arrange
        var configuration = new Configuration(new TestConfigurationStore())
        {
            TestFrameworkName = "foo"
        };

        var testFrameworkProvider = new TestFrameworkProvider(configuration);

        // Act
        Action act = () => testFrameworkProvider.Throw("MyMessage");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*the test framework 'foo' but this is not supported*");
    }

    [Fact]
    public void When_running_test_with_late_bound_but_unavailable_test_framework_it_should_throw()
    {
        // Arrange
        var configuration = new Configuration(new TestConfigurationStore())
        {
            TestFrameworkName = "nunit"
        };

        var testFrameworkProvider = new TestFrameworkProvider(configuration);

        // Act
        Action act = () => testFrameworkProvider.Throw("MyMessage");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*test framework 'nunit' but the required assembly 'nunit.framework' could not be found*");
    }

    private sealed class TestConfigurationStore : IConfigurationStore
    {
        string IConfigurationStore.GetSetting(string name) => string.Empty;
    }
}
