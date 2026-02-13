#if NET47 || NETSTANDARD2_0 || NETSTANDARD2_1

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System.Text;

/// <summary>
/// Since net6.0 StringBuilder has additional overloads taking an AppendInterpolatedStringHandler
/// and optionally an IFormatProvider.
/// The overload here is polyfill for older target frameworks to avoid littering the code base with #ifs
/// in order to silence analyzers about depending on the current culture instead of an invariant culture.
/// </summary>
internal static class StringBuilderExtensions
{
    public static StringBuilder Append(this StringBuilder stringBuilder, IFormatProvider provider, FormattableString formattable) =>
        stringBuilder.Append(string.Create(provider, formattable));

    public static StringBuilder AppendLine(this StringBuilder stringBuilder, IFormatProvider provider, FormattableString formattable) =>
        stringBuilder.AppendLine(string.Create(provider, formattable));

#if NET47 || NETSTANDARD2_0
    public static StringBuilder AppendJoin<T>(this StringBuilder stringBuilder, string separator, IEnumerable<T> values) =>
        stringBuilder.Append(string.Join(separator, values));
#endif
}

#endif
