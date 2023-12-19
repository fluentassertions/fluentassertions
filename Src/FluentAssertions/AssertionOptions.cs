using System;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Equivalency;
using FluentAssertionsAsync.Formatting;

namespace FluentAssertionsAsync;

/// <summary>
/// Holds any global options that control the behavior of FluentAssertions.
/// </summary>
public static class AssertionOptions
{
    private static EquivalencyOptions defaults = new();

    static AssertionOptions()
    {
        EquivalencyPlan = new EquivalencyPlan();
        Services.EnsureInitialized();
    }

    /// <summary>
    /// Creates a clone of the default options and allows the caller to modify them.
    /// </summary>
    public static EquivalencyOptions<T> CloneDefaults<T>()
    {
        return new EquivalencyOptions<T>(defaults);
    }

    /// <summary>
    /// Allows configuring the defaults used during a structural equivalency assertion.
    /// </summary>
    /// <remarks>
    /// This method is not thread-safe and should not be invoked from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    /// <param name="defaultsConfigurer">
    /// An action that is used to configure the defaults.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="defaultsConfigurer"/> is <see langword="null"/>.</exception>
    public static void AssertEquivalencyUsing(
        Func<EquivalencyOptions, EquivalencyOptions> defaultsConfigurer)
    {
        Guard.ThrowIfArgumentIsNull(defaultsConfigurer);

        defaults = defaultsConfigurer(defaults);
    }

    /// <summary>
    /// Represents a mutable plan consisting of steps that are executed while asserting a (collection of) object(s)
    /// is structurally equivalent to another (collection of) object(s).
    /// </summary>
    /// <remarks>
    /// Members on this property are not thread-safe and should not be invoked from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public static EquivalencyPlan EquivalencyPlan { get; }

    /// <summary>
    /// Gets the default formatting options used by the formatters in Fluent Assertions.
    /// </summary>
    /// <remarks>
    /// Members on this property should not be invoked from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public static FormattingOptions FormattingOptions { get; } = new();
}
