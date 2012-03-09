using System;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Frameworks
{
    internal class GallioTestFramework : ITestFramework
    {
        private Assembly assembly;

        /// <summary>
        /// Throws a framework-specific exception to indicate a failing unit test.
        /// </summary>
        public void Throw(string message)
        {
            Type assertionFailureBuilderType = assembly.GetType("Gallio.Framework.Assertions.AssertionFailureBuilder");
            Type assertionHelperType = assembly.GetType("Gallio.Framework.Assertions.AssertionHelper");
            Type testContextType = assembly.GetType("Gallio.Framework.TestContext");

            if ((assertionFailureBuilderType == null) || (assertionHelperType == null) || (testContextType == null))
            {
                throw new Exception(string.Format(
                    "Failed to create the assertion exception for the current test framework: \"{0}\"",
                    assembly.FullName));
            }

            object testContext = testContextType.GetProperty("CurrentContext").GetValue(null, null);

            if (testContext != null)
            {
                testContextType.InvokeMember("IncrementAssertCount", BindingFlags.InvokeMethod, null, testContext, null);
            }

            object assertionFailureBuilder = Activator.CreateInstance(assertionFailureBuilderType, message);
            assertionFailureBuilderType.InvokeMember("SetMessage", BindingFlags.InvokeMethod, null,
                assertionFailureBuilder, new object[] {message});
            object assertionFailure = assertionFailureBuilderType.InvokeMember("ToAssertionFailure",
                BindingFlags.InvokeMethod, null, assertionFailureBuilder, null);

            try
            {
                assertionHelperType.InvokeMember("Fail", BindingFlags.InvokeMethod, null, null, new[] {assertionFailure});
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the corresponding test framework is currently available.
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                assembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == AssemblyName);

                return (assembly != null);
            }
        }

        private string AssemblyName
        {
            get { return "Gallio"; }
        }
    }
}