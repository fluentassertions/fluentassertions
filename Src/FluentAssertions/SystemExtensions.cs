#if NET47 || NETSTANDARD2_0
namespace System
{
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
    }
}
#endif
