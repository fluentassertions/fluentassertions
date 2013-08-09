using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public class ExceptionValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is Exception;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="useLineBreaks"> </param>
        /// <param name="processedObjects">
        /// A collection of objects that 
        /// </param>
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, bool useLineBreaks, IList<object> processedObjects = null, int nestedPropertyLevel = 0)
        {
            var exception = (Exception)value;

            var builder = new StringBuilder();
            builder.AppendFormat("{0} with message \"{1}\"\n", exception.GetType().FullName, exception.Message);

            if (exception.StackTrace != null)
            {
                foreach (string line in exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    builder.AppendLine("  " + line);
                }
            }

            return builder.ToString();
        }
    }
}
