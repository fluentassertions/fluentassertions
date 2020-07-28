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
            if (string.IsNullOrEmpty(frameworkName)
                || !Frameworks.TryGetValue(frameworkName, out ITestFramework framework))
            {
                return null;
            }

            if (!framework.IsAvailable)
            {
                throw new Exception(
                    "FluentAssertions was configured to use " + frameworkName +
                    " but the required test framework assembly could not be found");
            }

            return framework;
        }

        private static ITestFramework AttemptToDetectUsingDynamicScanning()
        {
            return Frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
        }
    }
}
