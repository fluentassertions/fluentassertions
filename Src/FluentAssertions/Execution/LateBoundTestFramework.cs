using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertions.Execution;

internal abstract class LateBoundTestFramework : ITestFramework
{
    private readonly bool loadAssembly;
    private Func<string, Exception> exceptionFactory;

    protected LateBoundTestFramework(bool loadAssembly = false)
    {
        this.loadAssembly = loadAssembly;
        exceptionFactory = _ => throw new InvalidOperationException($"{nameof(IsAvailable)} must be called first.");
    }

    [DoesNotReturn]
    public void Throw(string message) => throw exceptionFactory(message);

    public bool IsAvailable
    {
        get
        {
            var assembly = GetAssembly();
            var exceptionType = assembly?.GetType(ExceptionFullName);
            exceptionFactory = exceptionType != null
                ? GetExceptionFactory(exceptionType)
                : _ => throw new InvalidOperationException($"{GetType().Name} is not available");
            return exceptionType is not null;
        }
    }

    private Assembly GetAssembly()
    {
        var assembly = Array.Find(AppDomain.CurrentDomain.GetAssemblies(), a => a.GetName().Name == AssemblyName);
        if (assembly is null && loadAssembly)
        {
            try
            {
                return Assembly.Load(new AssemblyName(AssemblyName));
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        return assembly;
    }

    private static Func<string, Exception> GetExceptionFactory(Type exceptionType)
    {
        var constructor = exceptionType.GetConstructor([typeof(string)])
            ?? throw new MissingMemberException(exceptionType.FullName, ".ctor");
        var parameter = Expression.Parameter(typeof(string), "m");
        var expression = Expression.Lambda<Func<string, Exception>>(Expression.New(constructor, parameter), parameter);

        try
        {
            return expression.Compile();
        }
        catch
        {
            return message => (Exception)Activator.CreateInstance(exceptionType, message);
        }
    }

    protected internal abstract string AssemblyName { get; }

    protected abstract string ExceptionFullName { get; }
}
