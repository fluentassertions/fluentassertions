using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Execution
{
    internal class NSpecFramework : ITestFramework
    {
        private Assembly assembly;

        public bool IsAvailable
        {
            get
            {
                assembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a => a.FullName.StartsWith("nspec,", StringComparison.OrdinalIgnoreCase));

                if (assembly is null)
                {
                    return false;
                }

                int majorVersion = assembly.GetName().Version.Major;

                return majorVersion >= 2;
            }
        }

        [DoesNotReturn]
        public void Throw(string message)
        {
            Type exceptionType = assembly.GetType("NSpec.Domain.AssertionException");
            if (exceptionType is null)
            {
                throw new Exception("Failed to create the NSpec assertion type");
            }

            throw (Exception)Activator.CreateInstance(exceptionType, message);
        }
    }
}
