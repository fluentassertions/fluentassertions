using System;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using Xunit;

using static FluentAssertions.FluentActions;

namespace FluentAssertions.Specs.Execution;

public class TestFrameworkProviderTests
{
    [Fact]
    public void When_running_xunit_test_implicitly_it_should_be_detected()
    {
        var configuration = new Configuration(new TestConfigurationStore());
        var result = TestFrameworkProvider.DetectFramework(configuration);

        result.IsAvailable.Should().BeTrue();
        result.Should().BeOfType<XUnit2TestFramework>();
    }

    [Fact]
    public void When_running_xunit_test_explicitly_it_should_be_detected()
    {
        var configuration = new Configuration(new TestConfigurationStore())
        {
            TestFrameworkName = "xunit2"
        };
        var result = TestFrameworkProvider.DetectFramework(configuration);

        result.IsAvailable.Should().BeTrue();
        result.Should().BeOfType<XUnit2TestFramework>();
    }

    [Fact]
    public void When_running_test_with_unknown_test_framework_it_should_throw()
    {
        var configuration = new Configuration(new TestConfigurationStore())
        {
            TestFrameworkName = "foo"
        };

        Invoking(() => TestFrameworkProvider.AttemptToDetectUsingAppSetting(configuration))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("FluentAssertions was configured to use the test framework 'foo' but this is not supported."
                         + " Please use one of the supported frameworks: mspec, nspec3, nunit, mstestv2, xunit2.");
    }

    [Fact]
    public void When_running_test_with_direct_bound_but_unavailable_test_framework_it_should_throw()
    {
        var configuration = new Configuration(new TestConfigurationStore())
        {
            TestFrameworkName = "nspec3"
        };

        Invoking(() => TestFrameworkProvider.AttemptToDetectUsingAppSetting(configuration))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("FluentAssertions was configured to use the test framework 'nspec3' but it could not be found."
                         + " Please use one of the supported frameworks: mspec, nspec3, nunit, mstestv2, xunit2.");
    }

    [Fact]
    public void When_running_test_with_late_bound_but_unavailable_test_framework_it_should_throw()
    {
        var configuration = new Configuration(new TestConfigurationStore())
        {
            TestFrameworkName = "nunit"
        };

        Invoking(() => TestFrameworkProvider.AttemptToDetectUsingAppSetting(configuration))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("FluentAssertions was configured to use the test framework 'nunit' but the required assembly 'nunit.framework' could not be found."
                         + " Please use one of the supported frameworks: mspec, nspec3, nunit, mstestv2, xunit2.");
    }

    private sealed class TestConfigurationStore : IConfigurationStore
    {
        string IConfigurationStore.GetSetting(string name) => string.Empty;
    }
}
