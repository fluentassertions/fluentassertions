using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FluentAssertions.Execution;

/// <summary>
/// Implements the xUnit (version 2 and 3) test framework adapter.
/// </summary>
internal class XUnitTestFramework(string assemblyName) : ITestFramework
{
    private Type exceptionType;

    public bool IsAvailable
    {
        get
        {
            try
            {
                // For netfx the assembly is not in AppDomain by default, so we can't just scan AppDomain.CurrentDomain
                exceptionType = Assembly.Load(new AssemblyName(assemblyName)).GetType("Xunit.Sdk.XunitException");

                return exceptionType is not null;
            }
            catch
            {
                return false;
            }
        }
    }

    [DoesNotReturn]
    public void Throw(string message)
    {
        throw (Exception)Activator.CreateInstance(exceptionType, message);
    }
}
