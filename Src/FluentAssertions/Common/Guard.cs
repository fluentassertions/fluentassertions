using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace FluentAssertionsAsync.Common;

internal static class Guard
{
    public static void ThrowIfArgumentIsNull<T>([ValidatedNotNull][NoEnumeration] T obj,
        [CallerArgumentExpression(nameof(obj))]
        string paramName = "")
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    public static void ThrowIfArgumentIsNull<T>([ValidatedNotNull][NoEnumeration] T obj, string paramName, string message)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName, message);
        }
    }

    public static void ThrowIfArgumentIsNullOrEmpty([ValidatedNotNull] string str,
        [CallerArgumentExpression(nameof(str))]
        string paramName = "")
    {
        if (string.IsNullOrEmpty(str))
        {
            ThrowIfArgumentIsNull(str, paramName);
            throw new ArgumentException("The value cannot be an empty string.", paramName);
        }
    }

    public static void ThrowIfArgumentIsNullOrEmpty([ValidatedNotNull] string str, string paramName, string message)
    {
        if (string.IsNullOrEmpty(str))
        {
            ThrowIfArgumentIsNull(str, paramName, message);
            throw new ArgumentException(message, paramName);
        }
    }

    public static void ThrowIfArgumentIsOutOfRange<T>(T value, [CallerArgumentExpression(nameof(value))] string paramName = "")
        where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
        {
            throw new ArgumentOutOfRangeException(paramName);
        }
    }

    public static void ThrowIfArgumentContainsNull<T>(IEnumerable<T> values,
        [CallerArgumentExpression(nameof(values))]
        string paramName = "")
    {
        if (values.Any(t => t is null))
        {
            throw new ArgumentNullException(paramName, "Collection contains a null value");
        }
    }

    public static void ThrowIfArgumentIsEmpty<T>(IEnumerable<T> values, string paramName, string message)
    {
        if (!values.Any())
        {
            throw new ArgumentException(message, paramName);
        }
    }

    public static void ThrowIfArgumentIsEmpty(string str, string paramName, string message)
    {
        if (str.Length == 0)
        {
            throw new ArgumentException(message, paramName);
        }
    }

    public static void ThrowIfArgumentIsNegative(TimeSpan timeSpan,
        [CallerArgumentExpression(nameof(timeSpan))]
        string paramName = "")
    {
        if (timeSpan < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(paramName, "The value must be non-negative.");
        }
    }

    public static void ThrowIfArgumentIsNegative(float value, [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "The value must be non-negative.");
        }
    }

    public static void ThrowIfArgumentIsNegative(double value, [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "The value must be non-negative.");
        }
    }

    public static void ThrowIfArgumentIsNegative(decimal value, [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "The value must be non-negative.");
        }
    }

    /// <summary>
    /// Workaround to make dotnet_code_quality.null_check_validation_methods work
    /// https://github.com/dotnet/roslyn-analyzers/issues/3451#issuecomment-606690452
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    private sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
