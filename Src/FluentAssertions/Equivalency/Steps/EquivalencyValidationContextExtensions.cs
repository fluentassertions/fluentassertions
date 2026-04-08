using System.Globalization;

namespace FluentAssertions.Equivalency.Steps;

[System.Diagnostics.StackTraceHidden]
internal static class EquivalencyValidationContextExtensions
{
    // Pre-cached string representations of common collection indices to avoid repeated allocations.
    private static readonly string[] CachedIndexStrings = InitializeCachedIndexStrings(1024);

    private static string[] InitializeCachedIndexStrings(int count)
    {
        var result = new string[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = i.ToString(CultureInfo.InvariantCulture);
        }

        return result;
    }

    public static IEquivalencyValidationContext AsCollectionItem<TItem>(this IEquivalencyValidationContext context,
        int index) =>
        context.AsCollectionItem<TItem>(index < CachedIndexStrings.Length
            ? CachedIndexStrings[index]
            : index.ToString(CultureInfo.InvariantCulture));
}

