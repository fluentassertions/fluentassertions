using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
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
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanHandle(object value)
        {
            return value is IEnumerable;
        }

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            int startCount = formattedGraph.LineCount;
            IEnumerable<object> collection = ((IEnumerable)value).Cast<object>();

            using var iterator = new Iterator<object>(collection, MaxItems);
            while (iterator.MoveNext())
            {
                if (iterator.IsFirst)
                {
                    formattedGraph.AddFragment("{");
                }

                if (!iterator.HasReachedMaxItems)
                {
                    formatChild(iterator.Index.ToString(CultureInfo.InvariantCulture), iterator.Current, formattedGraph);
                }
                else
                {
                    using IDisposable _ = formattedGraph.WithIndentation();
                    string moreItemsMessage = value is ICollection c ? $"…{c.Count - MaxItems} more…" : "…more…";
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
    }
}
