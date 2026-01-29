using FluentAssertions.Configuration;

namespace FluentAssertions;

/// <summary>
/// Provides access to the global configuration and options to customize the behavior of FluentAssertions.
/// </summary>
[System.Diagnostics.StackTraceHidden]
public static class AssertionConfiguration
{
    public static GlobalConfiguration Current => AssertionEngine.Configuration;
}
