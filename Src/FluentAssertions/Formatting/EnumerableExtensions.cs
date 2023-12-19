using System.Collections.Generic;
using System.Text;

namespace FluentAssertionsAsync.Formatting;

internal static class EnumerableExtensions
{
    internal static string JoinUsingWritingStyle<T>(this IEnumerable<T> items)
    {
        var buffer = new StringBuilder();

        T lastItem = default;
        bool first = true;

        foreach (var item in items)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(", ");
                }

                buffer.Append(lastItem);
            }

            lastItem = item;
        }

        if (buffer.Length > 0)
        {
            buffer.Append(" and ");
        }

        buffer.Append(lastItem);

        return buffer.ToString();
    }
}
