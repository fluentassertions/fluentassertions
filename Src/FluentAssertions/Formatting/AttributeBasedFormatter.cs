using System;
using System.Collections.Generic;
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
        private Dictionary<Type, MethodInfo> formatters;
        private ValueFormatterDetectionMode detectionMode = ValueFormatterDetectionMode.Disabled;

        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="string"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return IsScanningEnabled && (value != null) && (GetFormatter(value) != null);
        }

        private static bool IsScanningEnabled
        {
            get { return Configuration.Current.ValueFormatterDetectionMode != ValueFormatterDetectionMode.Disabled; }
        }

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            MethodInfo method = GetFormatter(value);

            object[] parameters = new[] { value };

            return (string)method.Invoke(null, parameters);
        }

        private MethodInfo GetFormatter(object value)
        {
            Type valueType = value.GetType();
            do
            {
                if (Formatters.TryGetValue(valueType, out var formatter))
                {
                    return formatter;
                }

                valueType = valueType.BaseType;
            }
            while (valueType != null);

            return null;
        }

        private Dictionary<Type, MethodInfo> Formatters
        {
            get
            {
                HandleValueFormatterDetectionModeChanges();

                return formatters ??= FindCustomFormatters();
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

        private static Dictionary<Type, MethodInfo> FindCustomFormatters()
        {
            var query =
                from type in Services.Reflector.GetAllTypesFromAppDomain(Applicable)
                where type != null
                from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                where method.IsStatic
                where method.IsDecoratedWithOrInherit<ValueFormatterAttribute>()
                where method.GetParameters().Length == 1
                select new { Type = method.GetParameters().Single().ParameterType, Method = method } into formatter
                group formatter by formatter.Type into formatterGroup
                select formatterGroup.First();

            return query.ToDictionary(f => f.Type, f => f.Method);
        }

        private static bool Applicable(Assembly assembly)
        {
            Configuration configuration = Configuration.Current;
            ValueFormatterDetectionMode mode = configuration.ValueFormatterDetectionMode;

            return (mode == ValueFormatterDetectionMode.Scan) || (
                (mode == ValueFormatterDetectionMode.Specific) &&
                assembly.FullName.Split(',')[0].Equals(configuration.ValueFormatterAssembly, StringComparison.OrdinalIgnoreCase));
        }
    }
}
