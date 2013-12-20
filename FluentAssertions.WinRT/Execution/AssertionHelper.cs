using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Execution
{
    internal static class AssertionHelper
    {
        #region Private Definitions

        private static readonly Dictionary<string, ITestFramework> frameworks = new Dictionary<string, ITestFramework>
        {
            { "nunit-pcl", new NUnitPclTestFramework() },
            { "mstest", new MSTestFramework() }
        };

        private static ITestFramework testFramework;

        #endregion

        public static void Throw(string message)
        {
            TestFramework.Throw(message);
        }

        private static ITestFramework TestFramework
        {
            get
            {
                if (testFramework == null)
                {
                    testFramework = DetectFramework();
                }

                return testFramework;
            }
        }

        private static ITestFramework DetectFramework()
        {
            ITestFramework detectedFramework = AttemptToDetectUsingAssemblyScanning();

            if (detectedFramework == null)
            {
                throw new InvalidOperationException(
                    "Failed to detect the test framework. Make sure that the framework assembly is copied into the test run directory");
            }

            return detectedFramework;
        }

        private static ITestFramework AttemptToDetectUsingAssemblyScanning()
        {
            return frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
        }
    }
}