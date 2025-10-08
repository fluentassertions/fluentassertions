using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Configuration;

namespace FluentAssertions.Execution;

/// <summary>
/// Determines the test framework, either by scanning the current app domain for known test framework assemblies or by
/// passing the framework name directly.
/// </summary>
internal static class TestFrameworkFactory
{
    private static readonly Dictionary<TestFramework, ITestFramework> Frameworks = new()
    {
        [TestFramework.MSpec] = new MSpecFramework(),
        [TestFramework.NUnit] = new NUnitTestFramework(),
        [TestFramework.MsTest] = new MSTestFrameworkV2(),
        [TestFramework.MsTest4] = new MSTestFrameworkV4(),

        // Keep TUnitFramework and XUnitTestFramework last as they use a try/catch approach
        [TestFramework.TUnit] = new TUnitFramework(),
        [TestFramework.XUnit2] = new XUnitTestFramework("xunit.assert"),
        [TestFramework.XUnit3] = new XUnitTestFramework("xunit.v3.assert"),
    };

    public static ITestFramework GetFramework(TestFramework? testFrameWork)
    {
        ITestFramework framework = null;

        if (testFrameWork is not null)
        {
            framework = AttemptToDetectUsingSetting((TestFramework)testFrameWork);
        }

        framework ??= AttemptToDetectUsingDynamicScanning();

        return framework ?? new FallbackTestFramework();
    }

    private static ITestFramework AttemptToDetectUsingSetting(TestFramework framework)
    {
        if (!Frameworks.TryGetValue(framework, out ITestFramework implementation))
        {
            string frameworks = string.Join(", ", Frameworks.Keys);
            var message =
                $"FluentAssertions was configured to use the test framework '{framework}' but this is not supported. " +
                $"Please use one of the supported frameworks: {frameworks}.";

            throw new InvalidOperationException(message);
        }

        if (!implementation.IsAvailable)
        {
            string frameworks = string.Join(", ", Frameworks.Keys);

            var innerMessage = implementation is LateBoundTestFramework lateBoundTestFramework
                ? $"the required assembly '{lateBoundTestFramework.AssemblyName}' could not be found"
                : "it could not be found";

            var message =
                $"FluentAssertions was configured to use the test framework '{framework}' but {innerMessage}. " +
                $"Please use one of the supported frameworks: {frameworks}.";

            throw new InvalidOperationException(message);
        }

        return implementation;
    }

    private static ITestFramework AttemptToDetectUsingDynamicScanning()
    {
        return Frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
    }
}
