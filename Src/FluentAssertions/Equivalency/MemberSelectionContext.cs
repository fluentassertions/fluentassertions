using System;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides contextual information to an <see cref="IMemberSelectionRule"/>.
    /// </summary>
    public class MemberSelectionContext
    {
        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object and the type is not <see cref="object"/>,
        /// then it returns the same <see cref="System.Type"/> as the <see cref="IObjectInfo.RuntimeType"/> property does.
        /// </summary>
        public Type CompileTimeType { get; set; }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        public Type RuntimeType { get; set; }

        public IEquivalencyAssertionOptions Options { get; set; }
    }
}
