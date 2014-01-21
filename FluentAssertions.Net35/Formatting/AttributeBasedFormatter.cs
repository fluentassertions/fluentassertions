using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    /// <summary>
    /// Specialized value formatter that looks for static methods in the caller's assembly marked with the 
    /// <see cref="ValueFormatterAttribute"/>.
    /// </summary>
    public class AttributeBasedFormatter : IValueFormatter
    {
        private MethodInfo[] formatters;
        private ValueFormatterDetectionMode detectionMode = ValueFormatterDetectionMode.Disabled;

        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return IsScanningEnabled && (value != null) && (GetFormatter(value) != null);
        }

        private static bool IsScanningEnabled
        {
            get { return (Configuration.Current.ValueFormatterDetectionMode != ValueFormatterDetectionMode.Disabled); }
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
            MethodInfo method = GetFormatter(value);
            return (string)method.Invoke(null, new[]
            {
                value
            });
        }

        private MethodInfo GetFormatter(object value)
        {
            return Formatters.FirstOrDefault(m => m.GetParameters().Single().ParameterType == value.GetType());
        }

        public MethodInfo[] Formatters
        {
            get
            {
                HandleValueFormatterDetectionModeChanges();

                return formatters ?? (formatters = FindCustomFormatters());
            }
        }

        private void HandleValueFormatterDetectionModeChanges()
        {
            if (detectionMode != Configuration.Current.ValueFormatterDetectionMode)
            {
                detectionMode = Configuration.Current.ValueFormatterDetectionMode;
                formatters = null;
            }
        }

        private MethodInfo[] FindCustomFormatters()
        {
            var query =
                from type in Services.Reflector.GetAllTypesFromAppDomain(Applicable)
                where type != null
                from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                where method.IsStatic
                where method.HasAttribute<ValueFormatterAttribute>()
                where method.GetParameters().Count() == 1
                select method;

            return query.ToArray();
        }

        private static bool Applicable(Assembly assembly)
        {
            var configuration = Configuration.Current;
            ValueFormatterDetectionMode mode = configuration.ValueFormatterDetectionMode;

            return ((mode == ValueFormatterDetectionMode.Scan) || (
                (mode == ValueFormatterDetectionMode.Specific) &&
                assembly.FullName.Split(',')[0].Equals(configuration.ValueFormatterAssembly, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
}