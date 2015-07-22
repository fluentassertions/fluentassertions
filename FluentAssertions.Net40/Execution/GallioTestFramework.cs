using System;
using System.Linq;
using System.Reflection;

#if WINRT 
using System.Reflection.RuntimeExtensions;
#endif

namespace FluentAssertions.Execution
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

            object testContext = testContextType
#if !WINRT
                .GetProperty("CurrentContext")
#else
                .GetRuntimeProperty("CurrentContext")
#endif
                .GetValue(null, null);

            if (testContext != null)
            {
#if !WINRT && !CORE_CLR
                testContextType.InvokeMember("IncrementAssertCount", BindingFlags.InvokeMethod, null, testContext, null);
#else
                var method = testContextType.GetRuntimeMethod("IncrementAssertCount", new Type[0]);
                method.Invoke(testContext, null);
#endif
            }

            object assertionFailureBuilder = Activator.CreateInstance(assertionFailureBuilderType, message);
            object assertionFailure;
#if !WINRT && !CORE_CLR
            assertionFailureBuilderType.InvokeMember("SetMessage", BindingFlags.InvokeMethod, null,
                assertionFailureBuilder, new object[] {message});
            assertionFailure = assertionFailureBuilderType.InvokeMember("ToAssertionFailure",
                BindingFlags.InvokeMethod, null, assertionFailureBuilder, null);
#else
            var setMessageMethod = assertionFailureBuilderType.GetRuntimeMethod("SetMessage", new [] {typeof(string)});
            setMessageMethod.Invoke(assertionFailureBuilder, new object[] {message});

            var toAssertionFailureMethod = assertionFailureBuilderType.GetRuntimeMethod("ToAssertionFailure", new Type[0]);
            assertionFailure = toAssertionFailureMethod.Invoke(assertionFailureBuilder, null);
#endif
            try
            {
#if !WINRT && !CORE_CLR
                assertionHelperType.InvokeMember("Fail", BindingFlags.InvokeMethod, null, null, new[] {assertionFailure});
#else
                var failMethod = assertionHelperType.GetRuntimeMethods().First(m => m.Name == "Fail");
                failMethod.Invoke(null, new[] {assertionFailure});
#endif
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
#if CORE_CLR
                // For CoreCLR, we need to attempt to load the assembly
                try
                {
                    assembly = Assembly.Load(new AssemblyName(AssemblyName) { Version = new Version(0, 0, 0, 0) });
                    return assembly != null;
                }
                catch
                {
                    return false;
                }
#else
                assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name.Equals(AssemblyName, StringComparison.InvariantCultureIgnoreCase));

                return (assembly != null);
#endif
            }
        }

        private string AssemblyName
        {
            get { return "Gallio"; }
        }
    }
}