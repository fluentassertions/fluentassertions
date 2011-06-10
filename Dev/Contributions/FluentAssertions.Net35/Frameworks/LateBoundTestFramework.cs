using System;
using System.Reflection;

namespace FluentAssertions.Frameworks
{
    internal abstract class LateBoundTestFramework : ITestFramework
    {
        private Assembly assembly;

        public void Throw(string message)
        {
            Type exceptionType = assembly.GetType(ExceptionFullName);
            if (exceptionType == null)
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
                try
                {
                    assembly = Assembly.Load(AssemblyName);
                    return (assembly != null);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        protected abstract string AssemblyName { get; }
        protected abstract string ExceptionFullName { get; }
    }
}