using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace FluentAssertions.Frameworks
{
    internal static class AssertionHelper
    {
        #region Private Definitions

        private const string AppSettingKey = "FluentAssertions.TestFramework";

        private static readonly Dictionary<string, ITestFramework> frameworks = new Dictionary<string, ITestFramework>
        {
            {"nunit", new NUnitTestFramework()},
            {"xunit", new XUnitTestFramework()},
            {"mstest", new MSTestFramework()},
            {"mspec", new MSpecFramework()},
            {"fallback", new FallbackTestFramework()}
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
                    testFramework = FindStrategy();
                }

                return testFramework;
            }
        }

        private static ITestFramework FindStrategy()
        {
            ITestFramework detectedFramework = AttemptToDetectUsingAppSetting();

            if (detectedFramework == null)
            {
                detectedFramework = AttemptToDetectUsingAssemblyScanning();
            }

            if (detectedFramework == null)
            {
                FailWithIncorrectConfiguration();
            }

            return detectedFramework;
        }

        private static void FailWithIncorrectConfiguration()
        {
            string errorMessage =
                "Failed to detect the test framework. Make sure that the framework assembly is copied into the test run directory, or " +
                    "configure it explicitly in the <appSettings> section using key \"" + AppSettingKey +
                        "\" and one of the supported " +
                            " frameworks: " + string.Join(", ", frameworks.Keys.ToArray());

            throw new ConfigurationErrorsException(errorMessage);
        }

        private static ITestFramework AttemptToDetectUsingAppSetting()
        {
            string frameworkName = ConfigurationManager.AppSettings[AppSettingKey];

            if (!string.IsNullOrEmpty(frameworkName) && frameworks.ContainsKey(frameworkName.ToLower()))
            {
                ITestFramework framework = frameworks[frameworkName.ToLower()];
                if (!framework.IsAvailable)
                {
                    throw new Exception(
                        "FluentAssertions was configured to use " + frameworkName +
                            " but the required test framework assembly could not be found");
                }

                return framework;
            }

            return null;
        }

        private static ITestFramework AttemptToDetectUsingAssemblyScanning()
        {
            return frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
        }
    }
}