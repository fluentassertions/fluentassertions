using System;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;

namespace FluentAssertions
{
    public static class AssertionOptions
    {
        private static readonly EquivalencyAssertionOptions defaults = new EquivalencyAssertionOptions();

        private static IConfigurationStore Store
        {
            get { return Services.ConfigurationStore; }
        }

        internal static EquivalencyAssertionOptions<T> CreateEquivalencyDefaults<T>()
        {
            return new EquivalencyAssertionOptions<T>(defaults);
        }

        /// <summary>
        /// Allows configuring the defaults used during a structural equivalency assertion.
        /// </summary>
        /// <param name="defaultsConfigurer">
        /// An action that is used to configure the defaults.
        /// </param>
        public static void AssertEquivalencyUsing(Action<EquivalencyAssertionOptions> defaultsConfigurer)
        {
            defaultsConfigurer(defaults);
        }
    }
}