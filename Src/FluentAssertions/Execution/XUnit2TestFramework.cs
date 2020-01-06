using System;
using System.Reflection;

namespace FluentAssertions.Execution
{
    internal class XUnit2TestFramework : ITestFramework
    {
        private Assembly assembly = null;

        public bool IsAvailable
        {
            get
            {
                try
                {
                    assembly = Assembly.Load(new AssemblyName("xunit.assert"));

                    return (assembly != null);
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Throw(string message)
        {
            Type exceptionType = assembly.GetType("Xunit.Sdk.XunitException");
            if (exceptionType is null)
            {
                throw new Exception("Failed to create the XUnit assertion type");
            }

            throw (Exception)Activator.CreateInstance(exceptionType, message);
        }
    }
}
