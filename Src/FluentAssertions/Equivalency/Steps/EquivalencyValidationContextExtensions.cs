using System.Globalization;

namespace FluentAssertions.Equivalency.Steps
{
    internal static class EquivalencyValidationContextExtensions
    {
        public static IEquivalencyValidationContext AsCollectionItem<TItem>(this IEquivalencyValidationContext context,
            int index) =>
            context.AsCollectionItem<TItem>(index.ToString(CultureInfo.InvariantCulture));
    }
}
