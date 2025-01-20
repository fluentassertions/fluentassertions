using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Configuration;
using FluentAssertions.Execution;
using FluentAssertions.Extensibility;
using JetBrains.Annotations;

namespace FluentAssertions;

/// <summary>
/// Represents the run-time configuration of the assertion library.
/// </summary>
public static class AssertionEngine
{
    private static readonly object Lockable = new();
    private static ITestFramework testFramework;
    private static bool isInitialized;

    static AssertionEngine()
    {
        EnsureInitialized();
    }

    /// <summary>
    /// Gets or sets the run-time test framework used for throwing assertion exceptions.
    /// </summary>
    public static ITestFramework TestFramework
    {
        get
        {
            if (testFramework is not null)
            {
                return testFramework;
            }

            lock (Lockable)
            {
#pragma warning disable CA1508
                if (testFramework is null)
#pragma warning restore CA1508
                {
                    testFramework = TestFrameworkFactory.GetFramework(Configuration.TestFramework);
                }
            }

            return testFramework;
        }
        set => testFramework = value;
    }

    /// <summary>
    /// Provides access to the global configuration and options to customize the behavior of FluentAssertions.
    /// </summary>
    public static GlobalConfiguration Configuration { get; private set; } = new();

    /// <summary>
    /// Resets the configuration to its default state and forces the engine to reinitialize the next time it is used.
    /// </summary>
    [PublicAPI]
    public static void ResetToDefaults()
    {
        isInitialized = false;
        Configuration = new GlobalConfiguration();
        testFramework = null;
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

                if (!License.Accepted)
                {
                    const string softWarning =
                        """
                             Warning:
                             The component "Fluent Assertions" is governed by the rules defined in the Xceed License Agreement and
                             the Xceed Fluent Assertions Community License. You may use Fluent Assertions free of charge for
                             non-commercial use only. An active subscription is required to use Fluent Assertions for commercial use.

                             Please contact Xceed Sales mailto:sales@xceed.com to acquire a subscription at a very low cost.

                             A paid commercial license supports the development and continued increasing support of
                             Fluent Assertions users under both commercial and community licenses. Help us
                             keep Fluent Assertions at the forefront of unit testing.

                             For more information, visit https://xceed.com/products/unit-testing/fluent-assertions/
                        """;

                    Console.WriteLine(softWarning);
                    Trace.WriteLine(softWarning);
                }

                isInitialized = true;
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
