using System;

namespace FluentAssertions.Equivalency
{
    public static class EquivalencyAssertionOptionsExtentions
    {
        /// <summary>
        ///     Returns either the Runtime or Compile-time  the type of the subject based on the configuration's UseRuntimeTyping
        ///     setting.
        /// </summary>
        public static Type GetSubjectType(this IEquivalencyAssertionOptions config, ISubjectInfo context)
        {
            bool useRuntimeType = ShouldUseRuntimeType(config);

            return useRuntimeType ? context.RuntimeType : context.CompileTimeType;
        }

        private static bool ShouldUseRuntimeType(IEquivalencyAssertionOptions config)
        {
            return config.UseRuntimeTyping;
        }
    }
}