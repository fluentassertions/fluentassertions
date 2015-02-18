using System;

namespace FluentAssertions.Equivalency
{
    internal static class EquivalencyAssertionOptionsExtentions
    {
        internal static Type GetSubjectType(this IEquivalencyAssertionOptions config, ISubjectInfo context)
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