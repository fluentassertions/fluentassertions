using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Xml;

namespace FluentAssertions.Formatting
{
    /// <summary>
    /// Provides services for formatting an object being used in an assertion in a human readable format.
    /// </summary>
    public static class Formatter
    {
        #region Private Definitions

        private static readonly List<IValueFormatter> customFormatters = new List<IValueFormatter>();

        private static readonly List<IValueFormatter> defaultFormatters = new List<IValueFormatter>
        {
#if !NETSTANDARD1_3 && !NETSTANDARD1_6
            new XmlNodeFormatter(),
#endif
            new AttributeBasedFormatter(),
            new AggregateExceptionValueFormatter(),
            new XDocumentValueFormatter(),
            new XElementValueFormatter(),
            new XAttributeValueFormatter(),
            new PropertyInfoFormatter(),
            new NullValueFormatter(),
            new GuidValueFormatter(),
            new DateTimeOffsetValueFormatter(),
            new TimeSpanValueFormatter(),
            new Int32ValueFormatter(),
            new Int64ValueFormatter(),
            new DoubleValueFormatter(),
            new SingleValueFormatter(),
            new DecimalValueFormatter(),
            new ByteValueFormatter(),
            new UInt32ValueFormatter(),
            new UInt64ValueFormatter(),
            new Int16ValueFormatter(),
            new UInt16ValueFormatter(),
            new SByteValueFormatter(),
            new StringValueFormatter(),
            new TaskFormatter(),
            new ExpressionValueFormatter(),
            new ExceptionValueFormatter(),
            new MultidimensionalArrayFormatter(),
            new EnumerableValueFormatter(),
            new DefaultValueFormatter()
        };

        /// <summary>
        /// Is used to detect recursive calls by <see cref="IValueFormatter"/> implementations.
        /// </summary>
        [ThreadStatic]
        private static bool isReentry;

        #endregion

        /// <summary>
        /// A list of objects responsible for formatting the objects represented by placeholders.
        /// </summary>
        public static IEnumerable<IValueFormatter> Formatters => customFormatters.Concat(defaultFormatters);

        /// <summary>
        /// Returns a human-readable representation of a particular object.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="useLineBreaks">
        /// Indicates whether the formatter should use line breaks when the specific <see cref="IValueFormatter"/> supports it.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(object value, bool useLineBreaks = false)
        {
            try
            {
                if (isReentry)
                {
                    throw new InvalidOperationException(
                        $"Use the {nameof(FormatChild)} delegate inside a {nameof(IValueFormatter)} to recursively format children");
                }

                isReentry = true;

                var graph = new ObjectGraph(value);

                var context = new FormattingContext
                {
                    Depth = graph.Depth,
                    UseLineBreaks = useLineBreaks
                };

                return Format(value, context, (path, childValue) => FormatChild(path, childValue, useLineBreaks, graph));
            }
            finally
            {
                isReentry = false;
            }
        }

        private static string FormatChild(string path, object childValue, bool useLineBreaks, ObjectGraph graph)
        {
            try
            {
                Guard.ThrowIfArgumentIsNullOrEmpty(path, nameof(path), "Formatting a child value requires a path");

                if (!graph.TryPush(path, childValue))
                {
                    return $"{{Cyclic reference to type {childValue.GetType()} detected}}";
                }
                else if (graph.Depth > 5)
                {
                    return "{Maximum recursion depth was reached…}";
                }
                else
                {
                    var context = new FormattingContext
                    {
                        Depth = graph.Depth,
                        UseLineBreaks = useLineBreaks
                    };

                    return Format(childValue, context, (x, y) => FormatChild(x, y, useLineBreaks, graph));
                }
            }
            finally
            {
                graph.Pop();
            }
        }

        private static string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            IValueFormatter firstFormatterThatCanHandleValue = Formatters.First(f => f.CanHandle(value));
            return firstFormatterThatCanHandleValue.Format(value, context, formatChild);
        }

        /// <summary>
        /// Removes a custom formatter that was previously added though <see cref="AddFormatter"/>.
        /// </summary>
        public static void RemoveFormatter(IValueFormatter formatter)
        {
            if (customFormatters.Contains(formatter))
            {
                customFormatters.Remove(formatter);
            }
        }

        /// <summary>
        /// Ensures a custom formatter is included in the chain, just before the default formatter is executed.
        /// </summary>
        public static void AddFormatter(IValueFormatter formatter)
        {
            if (!customFormatters.Contains(formatter))
            {
                customFormatters.Insert(0, formatter);
            }
        }

        /// <summary>
        /// Tracks the objects that were formatted as well as the path in the object graph of
        /// that object.
        /// </summary>
        /// <remarks>
        /// Is used to detect the maximum recursion depth as well as cyclic references in the graph.
        /// </remarks>
        private class ObjectGraph
        {
            private readonly CyclicReferenceDetector tracker;
            private readonly Stack<string> pathStack;

            public ObjectGraph(object rootObject)
            {
                tracker = new CyclicReferenceDetector(CyclicReferenceHandling.Ignore);
                pathStack = new Stack<string>();
                TryPush("root", rootObject);
            }

            public bool TryPush(string path, object value)
            {
                pathStack.Push(path);

                string fullPath = GetFullPath();
                var reference = new ObjectReference(value, fullPath);
                return !tracker.IsCyclicReference(reference);
            }

            private string GetFullPath() => string.Join(".", pathStack.Reverse());

            public void Pop()
            {
                pathStack.Pop();
            }

            public int Depth => (pathStack.Count - 1);

            public override string ToString()
            {
                return string.Join(".", pathStack.Reverse().ToArray());
            }
        }
    }
}
