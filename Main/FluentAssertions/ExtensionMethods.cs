namespace FluentAssertions
{
    internal static class ExtensionMethods
    {
        public static int IndexOfFirstMismatch(this string value, string expected)
        {
            for (int index = 0; index < value.Length; index++)
            {
                if (value[index] != expected[index])
                {
                    return index;
                }
            }

            return -1;
        }
    }
}
