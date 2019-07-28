using System;
using System.Linq;
using System.Reflection;

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

            if ((assertionFailureBuilderType is null) || (assertionHelperType is null) || (testContextType is null))
            {
                throw new Exception(string.Format(
                    "Failed to create the assertion exception for the current test framework: \"{0}\"",
                    assembly.FullName));
            }

            object testContext = testContextType
                .GetRuntimeProperty("CurrentContext")
                .GetValue(null, null);

            if (testContext != null)
            {
                MethodInfo method = testContextType.GetRuntimeMethod("IncrementAssertCount", new Type[0]);
                method.Invoke(testContext, null);
            }

            object assertionFailureBuilder = Activator.CreateInstance(assertionFailureBuilderType, message);
            object assertionFailure;
            MethodInfo setMessageMethod = assertionFailureBuilderType.GetRuntimeMethod("SetMessage", new[] { typeof(string) });
            setMessageMethod.Invoke(assertionFailureBuilder, new object[] { message });

            MethodInfo toAssertionFailureMethod = assertionFailureBuilderType.GetRuntimeMethod("ToAssertionFailure", new Type[0]);
            assertionFailure = toAssertionFailureMethod.Invoke(assertionFailureBuilder, null);
            try
            {
                MethodInfo failMethod = assertionHelperType.GetRuntimeMethods().First(m => m.Name == "Fail");
                failMethod.Invoke(null, new[] { assertionFailure });
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
#if NETSTANDARD1_3 || NETSTANDARD1_6
                // For .NET Standard < 2.0, we need to attempt to load the assembly
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

        private static string AssemblyName => "Gallio";
    }
}
