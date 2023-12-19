using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Formatting;

public class EnumerableValueFormatter : IValueFormatter
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
        return value is IEnumerable;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        IEnumerable<object> collection = ((IEnumerable)value).Cast<object>();

        using var iterator = new Iterator<object>(collection, MaxItems);

        var iteratorGraph = formattedGraph.KeepOnSingleLineAsLongAsPossible();
        FormattedObjectGraph.PossibleMultilineFragment separatingCommaGraph = null;

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

            separatingCommaGraph?.InsertLineOrFragment(", ");
            separatingCommaGraph = formattedGraph.KeepOnSingleLineAsLongAsPossible();

            // We cannot know whether or not the enumerable will take up more than one line of
            // output until we have formatted the first item. So we format the first item, then
            // go back and insert the enumerable's opening brace in the correct place depending
            // on whether that first item was all on one line or not.
            if (iterator.IsLast)
            {
                iteratorGraph.AddStartingLineOrFragment("{");
                iteratorGraph.AddLineOrFragment("}");
            }
        }

        if (iterator.IsEmpty)
        {
            iteratorGraph.AddFragment("{empty}");
        }
    }
}
