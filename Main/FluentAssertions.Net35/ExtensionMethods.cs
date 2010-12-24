using System;

namespace FluentAssertions
{
    internal static class ExtensionMethods
    {
        public static int IndexOfFirstMismatch(this string value, string expected)
        {
            for (int index = 0; index < value.Length; index++)
            {
                if ((index >= expected.Length) || (value[index] != expected[index]))
                {
                    return index;
                }
            }

            return -1;
        }

        public static string Mismatch(this string value, int index)
        {
            int length = Math.Min(value.Length - index, 3);

            return string.Format("'{0}' (index {1})", value.Substring(index, length), index);
        }
    }
}
