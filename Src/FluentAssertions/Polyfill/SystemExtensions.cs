#if NET47 || NETSTANDARD2_0

// ReSharper disable once CheckNamespace
namespace System;

internal static class SystemExtensions
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.string.indexof?view=netframework-4.8#System_String_IndexOf_System_Char_
    public static int IndexOf(this string str, char c, StringComparison _) =>
        str.IndexOf(c);

    // https://docs.microsoft.com/en-us/dotnet/api/system.string.replace?view=netframework-4.8#System_String_Replace_System_String_System_String_
    public static string Replace(this string str, string oldValue, string newValue, StringComparison _) =>
        str.Replace(oldValue, newValue);

    // https://docs.microsoft.com/en-us/dotnet/api/system.string.indexof?view=netframework-4.8#System_String_IndexOf_System_String_System_StringComparison_
    public static bool Contains(this string str, string value, StringComparison comparison) =>
        str.IndexOf(value, comparison) != -1;

    public static bool Contains(this string str, char value, StringComparison comparison) =>
        str.IndexOf(value, comparison) != -1;

    // https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/String.Comparison.cs,1014
    public static bool StartsWith(this string str, char value) =>
        str.Length != 0 && str[0] == value;

    public static string[] Split(this string str, char separator, StringSplitOptions options = StringSplitOptions.None) =>
        str.Split([separator], options);

    public static string[] Split(this string str, string separator, StringSplitOptions options = StringSplitOptions.None) =>
        str.Split([separator], options);
}

#endif
