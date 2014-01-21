using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Execution
{
    internal static class TestFrameworkProvider
    {
        #region Private Definitions

        private static readonly Dictionary<string, ITestFramework> frameworks = new Dictionary<string, ITestFramework>
        {
            { "nunit-pcl", new NUnitPclTestFramework() },
            { "mstest", new MSTestFramework() }
        };

        private static ITestFramework testFramework;

        #endregion

        public static ITestFramework TestFramework
        {
            get
            {
                if (testFramework == null)
                {
                    testFramework = FindStrategy();
                }

                return testFramework;
            }
        }

        private static ITestFramework FindStrategy()
        {
            ITestFramework detectedFramework = null;
            detectedFramework = AttemptToDetectUsingAssemblyScanning();

            if (detectedFramework == null)
            {
                FailWithIncorrectConfiguration();
            }

            return detectedFramework;
        }

        private static void FailWithIncorrectConfiguration()
        {
            throw new InvalidOperationException(
                "Failed to detect the test framework. Make sure that the framework assembly is copied into the test run directory");
        }

        private static ITestFramework AttemptToDetectUsingAssemblyScanning()
        {
            return frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
        }
    }
}