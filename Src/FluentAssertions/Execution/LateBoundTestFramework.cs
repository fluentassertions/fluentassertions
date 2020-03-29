using System;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Execution
{
    internal abstract class LateBoundTestFramework : ITestFramework
    {
        private Assembly assembly = null;

        public void Throw(string message)
        {
            Type exceptionType = assembly.GetType(ExceptionFullName);
            if (exceptionType is null)
            {
                throw new Exception(string.Format(
                    "Failed to create the assertion exception for the current test framework: \"{0}, {1}\"",
                    ExceptionFullName, assembly.FullName));
            }

            throw (Exception)Activator.CreateInstance(exceptionType, message);
        }

        public bool IsAvailable
        {
            get
            {
                string prefix = AssemblyName + ",";

                assembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a => a.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

                return (assembly != null);
            }
        }

        protected abstract string AssemblyName { get; }

        protected abstract string ExceptionFullName { get; }
    }
}
