using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Formatting
{
    /// <summary>
    /// Provides services for formatting an object being used in an assertion in a human readable format.
    /// </summary>
    public static class Formatter
    {
        /// <summary>
        /// A list of objects responsible for formatting the objects represented by placeholders.
        /// </summary>
        public static readonly List<IValueFormatter> Formatters = new List<IValueFormatter>
        {
#if !WINRT
            new AttributeBasedFormatter(),
#endif
            new PropertyInfoFormatter(),
            new NullValueFormatter(),
            new GuidValueFormatter(),
            new DateTimeValueFormatter(),
            new TimeSpanValueFormatter(),
            new NumericValueFormatter(),
            new StringValueFormatter(),
            new ExpressionValueFormatter(),
            new EnumerableValueFormatter(),
            new XDocumentValueFormatter(),
            new XElementValueFormatter(),
            new XAttributeValueFormatter(),
            new ExceptionValueFormatter(),
            new DefaultValueFormatter(),
        };

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
            if (processedObjects == null)
            {
                processedObjects = new List<object>();
            }

            IValueFormatter firstFormatterThatCanHandleValue = Formatters.First(f => f.CanHandle(value));
            return firstFormatterThatCanHandleValue.ToString(value, useLineBreaks, processedObjects, nestedPropertyLevel);
        }

        /// <summary>
        /// Ensures a custom formatter is included in the chain, just before the default formatter is executed.
        /// </summary>
        public static void AddFormatter(IValueFormatter formatter)
        {
            if (!Formatters.Contains(formatter))
            {
                Formatters.Insert(Formatters.Count - 2, formatter);
            }
        }
    }
}
