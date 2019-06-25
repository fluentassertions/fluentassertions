using System;
using System.Reflection;

namespace FluentAssertions.Execution
{
    internal class NSpec1Framework : ITestFramework
    {
        private Assembly assembly = null;

        public bool IsAvailable
        {
            get
            {
                try
                {
                    assembly = Assembly.Load(new AssemblyName("nspec"));

                    if (assembly is null)
                    {
                        return false;
                    }

                    int majorVersion = assembly.GetName().Version.Major;

                    return majorVersion == 1;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Throw(string message)
        {
            Type exceptionType = assembly.GetType("NSpec.Domain.ExampleFailureException");
            if (exceptionType is null)
            {
                throw new Exception("Failed to create the NSpec assertion type");
            }

            // v1 ctor requires an innerException: https://github.com/nspec/NSpec/blob/v1.0.7/NSpec/Domain/ExampleFailureException.cs
            throw (Exception)Activator.CreateInstance(exceptionType, message, null);
        }
    }
}
