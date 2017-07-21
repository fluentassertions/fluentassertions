#region

using System;
using FluentAssertions.Common;
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

        static AssertionOptions()
        {
            EquivalencySteps = new EquivalencyStepCollection();
        }

        public static EquivalencyAssertionOptions<T> CloneDefaults<T>()
        {
            return new EquivalencyAssertionOptions<T>(defaults);
        }

        /// <summary>
        /// Defines a predicate with which the <see cref="EquivalencyValidator"/> determines if it should process 
        /// an object's properties or not. 
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> if the object should be treated as a value type and its <see cref="object.Equals(object)"/>
        /// must be used during a structural equivalency check.
        /// </returns>
        public static Func<Type, bool> IsValueType = type => 
            (type.Namespace == typeof (int).Namespace) && 
            !type.IsSameOrInherits(typeof(Exception));

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

        /// <summary>
        /// Represents a mutable collection of steps that are executed while asserting a (collection of) object(s) 
        /// is structurally equivalent to another (collection of) object(s).
        /// </summary>
        public static EquivalencyStepCollection EquivalencySteps { get; private set; }
    }
}