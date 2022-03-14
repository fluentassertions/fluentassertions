using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
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
                testFramework = DetectFramework();
            }

            testFramework.Throw(message);
        }

        private static ITestFramework DetectFramework()
        {
            ITestFramework detectedFramework = AttemptToDetectUsingAppSetting()
                ?? AttemptToDetectUsingDynamicScanning()
                ?? new FallbackTestFramework();

            return detectedFramework;
        }

        private static ITestFramework AttemptToDetectUsingAppSetting()
        {
            string frameworkName = Services.Configuration.TestFrameworkName;
            if (string.IsNullOrEmpty(frameworkName))
            {
                return null;
            }

            if (!Frameworks.TryGetValue(frameworkName, out ITestFramework framework))
            {
                string frameworks = string.Join(", ", Frameworks.Keys);
                var message = $"FluentAssertions was configured to use {frameworkName} but the requested test framework is not supported. " +
                    $"Please use one of the supported frameworks: {frameworks}";

                throw new Exception(message);
            }

            if (!framework.IsAvailable)
            {
                string frameworks = string.Join(", ", Frameworks.Keys);
                var message = framework is LateBoundTestFramework lateBoundTestFramework
                    ? $"FluentAssertions was configured to use {frameworkName} but the required test framework assembly {lateBoundTestFramework.AssemblyName} could not be found. " +
                        $"Please use one of the supported frameworks: {frameworks}"
                    : $"FluentAssertions was configured to use {frameworkName} but the required test framework could not be found. " +
                        $"Please use one of the supported frameworks: {frameworks}";

                throw new Exception(message);
            }

            return framework;
        }

        private static ITestFramework AttemptToDetectUsingDynamicScanning()
        {
            return Frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
        }
    }
}
