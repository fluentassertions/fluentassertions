#if !NET6_0_OR_GREATER
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System;

internal static class StringExtensions
{
    extension(string)
    {
        public static string Create(IFormatProvider _, FormattableString formattable) => FormattableString.Invariant(formattable);
    }
}
#endif
