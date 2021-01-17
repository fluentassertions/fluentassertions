using System.Globalization;

namespace FluentAssertions.Equivalency
{
    internal static class EquivalencyValidationContextExtensions
    {
        public static IEquivalencyValidationContext AsCollectionItem<T>(this IEquivalencyValidationContext context,
            int index, object subject, T expectation) =>
            context.AsCollectionItem(index.ToString(CultureInfo.InvariantCulture), subject, expectation);
    }
}
