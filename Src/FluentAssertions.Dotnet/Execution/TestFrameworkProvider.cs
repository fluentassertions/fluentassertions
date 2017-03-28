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
            { "xunit2", new XUnit2TestFramework()},
            { "nunit-pcl", new NUnitPclTestFramework() },
            
            
            { "xunit", new XUnitTestFramework() },
            { "mspec", new MSpecFramework() },
            { "mbunit", new MbUnitTestFramework() },
            { "gallio", new GallioTestFramework() },            
            { "mstest", new MSTestFramework() },
            { "mstestv2", new MSTestFrameworkV2() },
            { "mstest-rt", new MSTestFrameworkRT() },

            { "fallback", new FallbackTestFramework() }
        };

        private static ITestFramework testFramework;

        #endregion

        public static void Throw(string message)
        {
            if (testFramework == null)
            {
                testFramework = FindStrategy();
            }

            testFramework.Throw(message);
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