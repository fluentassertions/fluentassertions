using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting;

public class EnumerableValueFormatter : IValueFormatter
{
    /// <summary>
    /// The number of items to include when formatting this object.
    /// </summary>
    /// <remarks>The default value is 32.</remarks>
    protected virtual int MaxItems { get; } = 32;

    /// <summary>
    /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which to create a <see cref="string"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public virtual bool CanHandle(object value)
    {
        return value is IEnumerable;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        IEnumerable<object> collection = ((IEnumerable)value).Cast<object>();

        using var iterator = new Iterator<object>(collection, MaxItems);

        var iteratorGraph = formattedGraph.StartPossibleMultilineFragment();

        while (iterator.MoveNext())
        {
            if (!iterator.HasReachedMaxItems)
            {
                formatChild(iterator.Index.ToString(CultureInfo.InvariantCulture), iterator.Current, formattedGraph);
            }
            else
            {
                using IDisposable _ = formattedGraph.WithIndentation();
                string moreItemsMessage = value is ICollection c ? $"…{c.Count - MaxItems} more…" : "…more…";
                iteratorGraph.AddLineOrFragment(moreItemsMessage);
            }

            if (iterator.IsFirst)
            {
                iteratorGraph.AddFragmentAtStart("{");
            }

            if (iterator.IsLast)
            {
                iteratorGraph.AddLineOrFragment("}");
            }
            else
            {
                iteratorGraph.AddFragmentAtEndOfLine(", ");
            }
        }

        if (iterator.IsEmpty)
        {
            iteratorGraph.AddFragment("{empty}");
        }
    }
}
