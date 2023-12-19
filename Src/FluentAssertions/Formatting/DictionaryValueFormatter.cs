using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Formatting;

public class DictionaryValueFormatter : IValueFormatter
{
    /// <summary>
    /// The number of items to include when formatting this object.
    /// </summary>
    /// <remarks>The default value is 32.</remarks>
    protected virtual int MaxItems => 32;

    /// <summary>
    /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which to create a <see cref="string"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public virtual bool CanHandle(object value)
    {
        return value is IDictionary;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        int startCount = formattedGraph.LineCount;
        IEnumerable<KeyValuePair<object, object>> collection = AsEnumerable((IDictionary)value);

        using var iterator = new Iterator<KeyValuePair<object, object>>(collection, MaxItems);

        while (iterator.MoveNext())
        {
            if (iterator.IsFirst)
            {
                formattedGraph.AddFragment("{");
            }

            if (!iterator.HasReachedMaxItems)
            {
                var index = iterator.Index.ToString(CultureInfo.InvariantCulture);
                formattedGraph.AddFragment("[");
                formatChild(index + ".Key", iterator.Current.Key, formattedGraph);
                formattedGraph.AddFragment("] = ");
                formatChild(index + ".Value", iterator.Current.Value, formattedGraph);
            }
            else
            {
                using IDisposable _ = formattedGraph.WithIndentation();
                string moreItemsMessage = $"…{collection.Count() - MaxItems} more…";
                AddLineOrFragment(formattedGraph, startCount, moreItemsMessage);
            }

            if (iterator.IsLast)
            {
                AddLineOrFragment(formattedGraph, startCount, "}");
            }
            else
            {
                formattedGraph.AddFragment(", ");
            }
        }

        if (iterator.IsEmpty)
        {
            formattedGraph.AddFragment("{empty}");
        }
    }

    private static void AddLineOrFragment(FormattedObjectGraph formattedGraph, int startCount, string fragment)
    {
        if (formattedGraph.LineCount > (startCount + 1))
        {
            formattedGraph.AddLine(fragment);
        }
        else
        {
            formattedGraph.AddFragment(fragment);
        }
    }

    private static IEnumerable<KeyValuePair<object, object>> AsEnumerable(IDictionary dictionary)
    {
        IDictionaryEnumerator iterator = dictionary.GetEnumerator();

        using (iterator as IDisposable)
        {
            while (iterator.MoveNext())
            {
                yield return new KeyValuePair<object, object>(iterator.Key, iterator.Value);
            }
        }
    }
}
