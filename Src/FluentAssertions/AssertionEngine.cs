using System;
using System.Linq;
using System.Reflection;
using FluentAssertions.Extensibility;
using JetBrains.Annotations;

namespace FluentAssertions;

public static class AssertionEngine
{
    private static readonly object Lockable = new();
    private static bool isInitialized;

    static AssertionEngine()
    {
        EnsureInitialized();
    }

    [PublicAPI]
    public static void ResetToDefaults()
    {
        isInitialized = false;
        EnsureInitialized();
    }

    internal static void EnsureInitialized()
    {
        if (isInitialized)
        {
            return;
        }

        lock (Lockable)
        {
            if (!isInitialized)
            {
                ExecuteCustomInitializers();
            }
        }
    }

    private static void ExecuteCustomInitializers()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var currentAssemblyName = currentAssembly.GetName();

        AssertionEngineInitializerAttribute[] attributes = [];

        try
        {
            attributes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => assembly != currentAssembly && !assembly.IsDynamic && !IsFramework(assembly))
                .Where(a => a.GetReferencedAssemblies().Any(r => r.FullName == currentAssemblyName.FullName))
                .SelectMany(a => a.GetCustomAttributes<AssertionEngineInitializerAttribute>())
                .ToArray();
        }
        catch
        {
            // Just ignore any exceptions that might happen while trying to find the attributes
        }

        foreach (var attribute in attributes)
        {
            try
            {
                attribute.Initialize();
            }
            catch
            {
                // Just ignore any exceptions that might happen while trying to find the attributes
            }
        }
    }

    private static bool IsFramework(Assembly assembly)
    {
#if NET6_0_OR_GREATER
        return assembly!.FullName?.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase) == true ||
            assembly.FullName?.StartsWith("System.", StringComparison.OrdinalIgnoreCase) == true;
#else
        return assembly.FullName.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase) ||
            assembly.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase);
#endif
    }
}
