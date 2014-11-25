using System;
using System.Linq;

namespace FluentAssertions.Equivalency
{
    internal static class EquivalencyAssertionOptionsExtentions
    {
        internal static Type GetSubjectType(this IEquivalencyAssertionOptions config, IEquivalencyValidationContext context)
        {
            bool useRuntimeType = ShouldUseRuntimeType(config);

            return useRuntimeType ? context.RuntimeType : context.CompileTimeType;
        }

        private static bool ShouldUseRuntimeType(IEquivalencyAssertionOptions config)
        {
            return config.SelectionRules.Any(selectionRule => selectionRule is AllRuntimePublicPropertiesSelectionRule);
        }
    }
}