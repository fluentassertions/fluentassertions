using System;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Extensibility;
using JetBrains.Annotations;

namespace FluentAssertionsAsync.Common;

/// <summary>
/// Maintains the framework-specific services.
/// </summary>
public static class Services
{
    private static readonly object Lockable = new();
    private static Configuration configuration;
    private static bool isInitialized;

    static Services()
    {
        EnsureInitialized();
    }

    public static IConfigurationStore ConfigurationStore { get; set; }

    public static Configuration Configuration
    {
        get
        {
            lock (Lockable)
            {
                return configuration ??= new Configuration(ConfigurationStore);
            }
        }
    }

    public static Action<string> ThrowException { get; set; }

    public static IReflector Reflector { get; set; }

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

                Reflector = new FullFrameworkReflector();
#if NETFRAMEWORK || NET6_0_OR_GREATER
                ConfigurationStore = new ConfigurationStoreExceptionInterceptor(new AppSettingsConfigurationStore());
#else
                ConfigurationStore = new NullConfigurationStore();
#endif
                ThrowException = new TestFrameworkProvider(Configuration).Throw;

                isInitialized = true;
            }
        }
    }

    private static void ExecuteCustomInitializers()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var currentAssemblyName = currentAssembly.GetName();

        var attributes = Array.Empty<AssertionEngineInitializerAttribute>();

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
