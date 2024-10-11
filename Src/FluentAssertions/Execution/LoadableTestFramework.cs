using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FluentAssertions.Execution;

/// <summary>
/// This attempts to load the assembly into the App Domain.
/// If it isn't loadable, then IsAvailable will return false
/// </summary>
internal abstract class LoadableTestFramework : ITestFramework
{
    private Assembly assembly;

    public bool IsAvailable
    {
        get
        {
            try
            {
                // For netfx the assembly is not in AppDomain by default, so we can't just scan AppDomain.CurrentDomain
                assembly = Assembly.Load(new AssemblyName(AssemblyName));

                return assembly is not null;
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
        Type exceptionType = assembly.GetType(ExceptionFullName)
            ?? throw new NotSupportedException($"Failed to create {ExceptionFullName}");

        throw (Exception)Activator.CreateInstance(exceptionType, message);
    }

    protected internal abstract string AssemblyName { get; }

    protected abstract string ExceptionFullName { get; }
}
