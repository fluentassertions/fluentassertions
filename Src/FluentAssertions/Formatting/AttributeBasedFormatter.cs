using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Formatting;

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
    /// <see langword="true"/> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanHandle(object value)
    {
        return IsScanningEnabled && value is not null && GetFormatter(value) is not null;
    }

    private static bool IsScanningEnabled
    {
        get { return Configuration.Current.ValueFormatterDetectionMode != ValueFormatterDetectionMode.Disabled; }
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        MethodInfo method = GetFormatter(value);

        object[] parameters = { value, formattedGraph };

        method.Invoke(null, parameters);
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
        while (valueType is not null);

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
            where type is not null
            from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
            where method.IsStatic
            where method.ReturnType == typeof(void)
            where method.IsDecoratedWithOrInherit<ValueFormatterAttribute>()
            let methodParameters = method.GetParameters()
            where methodParameters.Length == 2
            select new { Type = methodParameters[0].ParameterType, Method = method }
            into formatter
            group formatter by formatter.Type
            into formatterGroup
            select formatterGroup.First();

        return query.ToDictionary(f => f.Type, f => f.Method);
    }

    private static bool Applicable(Assembly assembly)
    {
        Configuration configuration = Configuration.Current;
        ValueFormatterDetectionMode mode = configuration.ValueFormatterDetectionMode;

        return mode == ValueFormatterDetectionMode.Scan || (
            mode == ValueFormatterDetectionMode.Specific &&
            assembly.FullName.Split(',')[0].Equals(configuration.ValueFormatterAssembly, StringComparison.OrdinalIgnoreCase));
    }
}
