using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
    internal static class TestFrameworkProvider
    {
        #region Private Definitions

        private static readonly Dictionary<string, ITestFramework> Frameworks = new Dictionary<string, ITestFramework>(StringComparer.OrdinalIgnoreCase)
        {
            ["mspec"] = new MSpecFramework(),
            ["nspec3"] = new NSpecFramework(),
            ["nunit"] = new NUnitTestFramework(),
            ["mstestv2"] = new MSTestFrameworkV2(),
            ["xunit2"] = new XUnit2TestFramework()
        };

        private static ITestFramework testFramework;

        #endregion

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
                var message = string.Format("FluentAssertions was configured to use {0} but the requested test framework is not supported. " +
                    "Please use one of the supported frameworks: {1}", frameworkName, string.Join(", ", Frameworks.Keys));

                throw new Exception(message);
            }

            if (!framework.IsAvailable)
            {
                var message = framework is LateBoundTestFramework lateBoundTestFramework
                    ? string.Format("FluentAssertions was configured to use {0} but the required test framework assembly {1} could not be found. " +
                        "Please use one of the supported frameworks: {2}", frameworkName, lateBoundTestFramework.AssemblyName, string.Join(", ", Frameworks.Keys))
                    : string.Format("FluentAssertions was configured to use {0} but the required test framework could not be found. " +
                        "Please use one of the supported frameworks: {1}", frameworkName, string.Join(", ", Frameworks.Keys));

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
