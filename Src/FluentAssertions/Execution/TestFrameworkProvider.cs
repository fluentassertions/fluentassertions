using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

/// <summary>
/// Implements a wrapper around all supported test frameworks to throw the correct assertion exception.
/// </summary>
internal static class TestFrameworkProvider
{
    #region Private Definitions

    private static readonly Dictionary<string, ITestFramework> Frameworks = new(StringComparer.OrdinalIgnoreCase)
    {
        ["mspec"] = new MSpecFramework(),
        ["nspec3"] = new NSpecFramework(),
        ["nunit"] = new NUnitTestFramework(),
        ["mstestv2"] = new MSTestFrameworkV2(),
        ["xunit2"] = new XUnit2TestFramework() // Keep this the last one as it uses a try/catch approach
    };

    private static ITestFramework testFramework;

    #endregion

    [DoesNotReturn]
    public static void Throw(string message)
    {
        if (testFramework is null)
        {
            testFramework = DetectFramework(Services.Configuration);
        }

        testFramework.Throw(message);
    }

    internal static ITestFramework DetectFramework(Configuration configuration)
    {
        ITestFramework detectedFramework = AttemptToDetectUsingAppSetting(configuration)
            ?? AttemptToDetectUsingDynamicScanning()
            ?? new FallbackTestFramework();

        return detectedFramework;
    }

    internal static ITestFramework AttemptToDetectUsingAppSetting(Configuration configuration)
    {
        string frameworkName = configuration.TestFrameworkName;
        if (string.IsNullOrEmpty(frameworkName))
        {
            return null;
        }

        if (!Frameworks.TryGetValue(frameworkName, out ITestFramework framework))
        {
            string frameworks = string.Join(", ", Frameworks.Keys);
            var message = $"FluentAssertions was configured to use the test framework '{frameworkName}' but this is not supported. " +
                $"Please use one of the supported frameworks: {frameworks}.";

            throw new InvalidOperationException(message);
        }

        if (!framework.IsAvailable)
        {
            string frameworks = string.Join(", ", Frameworks.Keys);
            var innerMessage = framework is LateBoundTestFramework lateBoundTestFramework
                ? $"the required assembly '{lateBoundTestFramework.AssemblyName}' could not be found"
                : "it could not be found";
            var message =
                $"FluentAssertions was configured to use the test framework '{frameworkName}' but {innerMessage}. " +
                $"Please use one of the supported frameworks: {frameworks}.";

            throw new InvalidOperationException(message);
        }

        return framework;
    }

    private static ITestFramework AttemptToDetectUsingDynamicScanning()
    {
        return Frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
    }
}
