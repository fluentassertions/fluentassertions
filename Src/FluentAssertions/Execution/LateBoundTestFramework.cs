using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FluentAssertionsAsync.Execution;

internal abstract class LateBoundTestFramework : ITestFramework
{
    private Assembly assembly;

    [DoesNotReturn]
    public void Throw(string message)
    {
        Type exceptionType = assembly.GetType(ExceptionFullName);

        if (exceptionType is null)
        {
            throw new NotSupportedException(
                $"Failed to create the assertion exception for the current test framework: \"{ExceptionFullName}, {assembly.FullName}\"");
        }

        throw (Exception)Activator.CreateInstance(exceptionType, message);
    }

    public bool IsAvailable
    {
        get
        {
            string prefix = AssemblyName + ",";

            assembly = Array.Find(AppDomain.CurrentDomain
                .GetAssemblies(), a => a.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            return assembly is not null;
        }
    }

    protected internal abstract string AssemblyName { get; }

    protected abstract string ExceptionFullName { get; }
}
