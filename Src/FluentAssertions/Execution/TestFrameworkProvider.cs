//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace FluentAssertions.Execution
//{
//    internal static class TestFrameworkProvider
//    {
//        #region Private Definitions

//        private const string AppSettingKey = "FluentAssertions.TestFramework";

//        private static readonly Dictionary<string, ITestFramework> frameworks = new Dictionary<string, ITestFramework>
//        {
//            { "nunit", new NUnitTestFramework() },
//            { "xunit", new XUnitTestFramework() },
//            { "mspec", new MSpecFramework() },
//            { "mbunit", new MbUnitTestFramework() },
//            { "gallio", new GallioTestFramework() },
//            { "mstest", new MSTestFramework() },
//#if NET45
//            { "xunit2", new XUnit2TestFramework()},
//            { "mstestv2", new MSTestFrameworkV2() },
//#endif
//            { "fallback", new FallbackTestFramework() }
//        };

//        private static ITestFramework testFramework;

//        #endregion

//        public static void Throw(string message)
//        {
//            if (testFramework == null)
//            {
//                testFramework = DetectFramework();
//            }

//            testFramework.Throw(message);
//        }

//        private static ITestFramework DetectFramework()
//        {
//            ITestFramework detectedFramework = null;

//            detectedFramework = AttemptToDetectUsingAppSetting();
//            if (detectedFramework == null)
//            {
//                detectedFramework = AttemptToDetectUsingAssemblyScanning();
//            }

//            if (detectedFramework == null)
//            {
//                FailWithIncorrectConfiguration();
//            }

//            return detectedFramework;
//        }

//        private static void FailWithIncorrectConfiguration()
//        {
//            string errorMessage =
//                "Failed to detect the test framework. Make sure that the framework assembly is copied into the test run directory"
//                + ", or configure it explicitly in the <appSettings> section using key \"" + AppSettingKey +
//                "\" and one of the supported " +
//                " frameworks: " + string.Join(", ", frameworks.Keys.ToArray());

//            throw new ConfigurationErrorsException(errorMessage);
//        }

//        private static ITestFramework AttemptToDetectUsingAppSetting()
//        {
//            string frameworkName = ConfigurationManager.AppSettings[AppSettingKey];

//            if (!string.IsNullOrEmpty(frameworkName) && frameworks.ContainsKey(frameworkName.ToLower()))
//            {
//                ITestFramework framework = frameworks[frameworkName.ToLower()];
//                if (!framework.IsAvailable)
//                {
//                    throw new Exception(
//                        "FluentAssertions was configured to use " + frameworkName +
//                        " but the required test framework assembly could not be found");
//                }

//                return framework;
//            }

//            return null;
//        }

//        private static ITestFramework AttemptToDetectUsingAssemblyScanning()
//        {
//            return frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
//        }
//    }
//}