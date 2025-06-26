using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace FluentAssertions.Execution;

internal abstract class LateBoundTestFramework : ITestFramework
{
    private Func<string, Exception> exceptionFactory =
        _ => throw new InvalidOperationException($"{nameof(IsAvailable)} must be called first.");

    /// <summary>
    /// When set to <see langword="true"/>, the assembly specified by the <see cref="AssemblyName"/> property will
    /// be dynamically loaded if it is not already loaded in the application domain.
    /// When set to <see langword="false"/>, the framework will not attempt to load the assembly dynamically.
    /// </summary>
    protected bool LoadAssembly { get; init; }

    [DoesNotReturn]
    public void Throw(string message) => throw exceptionFactory(message);

    public bool IsAvailable
    {
        get
        {
            var assembly = FindExceptionAssembly();
            var exceptionType = assembly?.GetType(ExceptionFullName);

            exceptionFactory = exceptionType != null
                ? message => (Exception)Activator.CreateInstance(exceptionType, message)
                : _ => throw new InvalidOperationException($"{GetType().Name} is not available");

            return exceptionType is not null;
        }
    }

    private Assembly FindExceptionAssembly()
    {
        var assembly = Array.Find(AppDomain.CurrentDomain.GetAssemblies(), a => a.GetName().Name == AssemblyName);

        if (assembly is null && LoadAssembly)
        {
            try
            {
                return Assembly.Load(new AssemblyName(AssemblyName));
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (FileLoadException)
            {
                return null;
            }
        }

        return assembly;
    }

    protected internal abstract string AssemblyName { get; }

    protected abstract string ExceptionFullName { get; }
}
