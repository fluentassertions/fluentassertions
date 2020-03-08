using System;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Extensions;
using FluentAssertions.Specialized;

namespace FluentAssertions
{
    /// <summary>
    /// Holds any global options that control the behavior of FluentAssertions.
    /// </summary>
    public static class AssertionOptions
    {
        private static EquivalencyAssertionOptions defaults = new EquivalencyAssertionOptions();

        static AssertionOptions()
        {
            EquivalencySteps = new EquivalencyStepCollection();
            TaskTimeout = 30.Seconds();
        }

        public static EquivalencyAssertionOptions<T> CloneDefaults<T>()
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
            Guard.ThrowIfArgumentIsNull(defaultsConfigurer, nameof(defaultsConfigurer));

            defaults = defaultsConfigurer(defaults);
        }

        /// <summary>
        /// Represents a mutable collection of steps that are executed while asserting a (collection of) object(s)
        /// is structurally equivalent to another (collection of) object(s).
        /// </summary>
        public static EquivalencyStepCollection EquivalencySteps { get; }

        /// <summary>
        /// Gets or sets the timeout to wait for <see cref="Task"/> operations to complete.
        /// </summary>
        /// <remarks>
        /// The option is used by
        /// <list type="bullet">
        /// <item><see cref="TaskCompletionSourceAssertions{T}"/></item>
        /// </list>
        /// </remarks>
        public static TimeSpan TaskTimeout { get; set; }
    }
}
