using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Common;

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
            new AttributeBasedFormatter(),
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
            new ExpressionValueFormatter(),
            new ExceptionValueFormatter(),
            new EnumerableValueFormatter(),
            new DefaultValueFormatter(),
        };

        #endregion

        /// <summary>
        /// A list of objects responsible for formatting the objects represented by placeholders.
        /// </summary>
        public static IEnumerable<IValueFormatter> Formatters
        {
            get { return customFormatters.Concat(defaultFormatters); }
        }

        /// <summary>
        /// Returns a human-readable representation of a particular object.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="nestedPropertyLevel">
        ///     The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        ///     no <see cref="object.ToString()"/> override.
        /// </param>
        /// <param name="useLineBreaks">
        /// Indicates whether the formatter should use line breaks when the specific <see cref="IValueFormatter"/> supports it.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(object value, bool useLineBreaks = false, IList<object> processedObjects = null, int nestedPropertyLevel = 0)
        {
            Services.Initialize();

            if (processedObjects == null)
            {
                processedObjects = new List<object>();
            }

            const int MaxDepth = 15;
            if (nestedPropertyLevel > MaxDepth)
            {
                return "{Maximum recursion depth was reached...}";
            }

            IValueFormatter firstFormatterThatCanHandleValue = Formatters.First(f => f.CanHandle(value));
            return firstFormatterThatCanHandleValue.ToString(value, useLineBreaks, processedObjects, nestedPropertyLevel);
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
        /// Allows a platform-specific assembly to add formatters without affecting the ones added by callers of <see cref="AddFormatter"/>.
        /// </summary>
        /// <param name="formatters"></param>
        internal static void AddPlatformFormatters(IValueFormatter[] formatters)
        {
            foreach (var formatter in formatters)
            {
                if (!defaultFormatters.Contains(formatter))
                {
                    defaultFormatters.Insert(0, formatter);
                }
            }
        }
    }
}
