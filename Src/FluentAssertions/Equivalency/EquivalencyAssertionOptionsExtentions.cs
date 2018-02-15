using System;

namespace FluentAssertions.Equivalency
{
    public static class EquivalencyAssertionOptionsExtentions
    {
        /// <summary>
        ///     Returns either the run-time or compile-time type of the subject based on the options provided by the caller.
        /// </summary>
        public static Type GetExpectationType(this IEquivalencyAssertionOptions config, IMemberInfo context)
        {
            return config.UseRuntimeTyping ? context.RuntimeType : context.CompileTimeType;
        }
    }
}
