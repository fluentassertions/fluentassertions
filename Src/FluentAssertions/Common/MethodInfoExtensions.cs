﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentAssertions.Common;

internal static class MethodInfoExtensions
{
    /// <summary>
    /// A sum of all possible <see cref="MethodImplOptions"/>. It's needed to calculate what options were used when decorating with <see cref="MethodImplAttribute"/>.
    /// They are a subset of <see cref="MethodImplAttributes"/> which can be checked on a type and therefore this mask has to be applied to check only for options.
    /// </summary>
    private static readonly Lazy<int> ImplementationOptionsMask =
        new(() => Enum.GetValues(typeof(MethodImplOptions)).Cast<int>().Sum(optionValue => optionValue));

    internal static bool IsAsync(this MethodInfo methodInfo)
    {
        return methodInfo.IsDecoratedWith<AsyncStateMachineAttribute>();
    }

    internal static IEnumerable<TAttribute> GetMatchingAttributes<TAttribute>(this MemberInfo memberInfo,
        Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
        where TAttribute : Attribute
    {
        var customAttributes = memberInfo.GetCustomAttributes<TAttribute>(inherit: false).ToList();

        if (typeof(TAttribute) == typeof(MethodImplAttribute) && memberInfo is MethodBase methodBase)
        {
            (bool success, MethodImplAttribute methodImplAttribute) = RecreateMethodImplAttribute(methodBase);
            if (success)
            {
                customAttributes.Add(methodImplAttribute as TAttribute);
            }
        }

        return customAttributes
            .Where(isMatchingAttributePredicate.Compile());
    }

    internal static bool IsNonVirtual(this MethodInfo method)
    {
        return !method.IsVirtual || method.IsFinal;
    }

    private static (bool Success, MethodImplAttribute Attribute) RecreateMethodImplAttribute(MethodBase methodBase)
    {
        MethodImplAttributes implementationFlags = methodBase.MethodImplementationFlags;

        int implementationFlagsMatchingImplementationOptions =
            (int)implementationFlags & ImplementationOptionsMask.Value;

        MethodImplOptions implementationOptions = (MethodImplOptions)implementationFlagsMatchingImplementationOptions;

        if (implementationOptions != default)
        {
            return (true, new MethodImplAttribute(implementationOptions));
        }

        return (false, null);
    }
}
