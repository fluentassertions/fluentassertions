using System;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    /// <summary>
    /// Holds any global options that control the behavior of FluentAssertions.
    /// </summary>
    public static class AssertionOptions
    {
        private static EquivalencyAssertionOptions defaults = new();

        static AssertionOptions()
        {
            EquivalencyPlan = new EquivalencyPlan();
        }

        public static EquivalencyAssertionOptions<T> CloneDefaults<T>()
        {
            return new(defaults);
        }

        internal static TOptions CloneDefaults<T, TOptions>(Func<EquivalencyAssertionOptions, TOptions> predicate)
            where TOptions : EquivalencyAssertionOptions<T>
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate));

            return predicate(defaults);
        }

        /// <summary>
        /// Allows configuring the defaults used during a structural equivalency assertion.
        /// </summary>
        /// <param name="defaultsConfigurer">
        /// An action that is used to configure the defaults.
        /// </param>
        public static void AssertEquivalencyUsing(
            Func<EquivalencyAssertionOptions, EquivalencyAssertionOptions> defaultsConfigurer)
        {
            Guard.ThrowIfArgumentIsNull(defaultsConfigurer, nameof(defaultsConfigurer));

            defaults = defaultsConfigurer(defaults);
        }

        /// <summary>
        /// Represents a mutable plan consisting of steps that are executed while asserting a (collection of) object(s)
        /// is structurally equivalent to another (collection of) object(s).
        /// </summary>
        public static EquivalencyPlan EquivalencyPlan { get; }

        /// <summary>
        /// Gets the default formatting options used by the formatters in Fluent Assertions.
        /// </summary>
        public static FormattingOptions FormattingOptions { get; } = new();
    }
}
