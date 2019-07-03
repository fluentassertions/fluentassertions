using System;
using System.Reflection;
using FluentAssertions.Localization;

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
                throw new Exception(Resources.Assertion_FailedToCreateXUnitAssertionType);
            }

            throw (Exception)Activator.CreateInstance(exceptionType, message);
        }
    }
}
