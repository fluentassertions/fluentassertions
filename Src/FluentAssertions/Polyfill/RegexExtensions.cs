#if !NET8_0_OR_GREATER

// ReSharper disable once CheckNamespace
namespace System.Text.RegularExpressions;

/// <summary>
/// Since .NET 7 <see cref="Regex"/> has a Count method.
/// The overload here is polyfill for older target frameworks to avoid littering the code base with #ifs.
/// </summary>
internal static class RegexExtensions
{
    public static int Count(this Regex regex, string input) =>
        regex.Matches(input).Count;
}

#endif
