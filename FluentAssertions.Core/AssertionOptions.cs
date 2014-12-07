#region

using System;
using FluentAssertions.Equivalency;

#endregion

namespace FluentAssertions
{
    /// <summary>
    /// Holds any global options that control the behavior of FluentAssertions.
    /// </summary>
    public static class AssertionOptions
    {
        private static EquivalencyAssertionOptions defaults = new EquivalencyAssertionOptions();

        internal static EquivalencyAssertionOptions<T> CloneDefaults<T>()
        {
            return new EquivalencyAssertionOptions<T>(defaults);
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
            defaults = defaultsConfigurer(defaults);
        }
    }
}