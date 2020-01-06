using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentAssertions.Common
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// A sum of all possible <see cref="MethodImplOptions"/>. It's needed to calculate what options were used when decorating with <see cref="MethodImplAttribute"/>.
        /// They are a subset of <see cref="MethodImplAttributes"/> which can be checked on a type and therefore this mask has to be applied to check only for options.
        /// </summary>
        private static readonly Lazy<int> ImplementationOptionsMask =
            new Lazy<int>(() => Enum.GetValues(typeof(MethodImplOptions)).Cast<int>().Sum(x => x));

        internal static bool IsAsync(this MethodInfo methodInfo)
        {
            return methodInfo.GetMatchingAttributes<Attribute>(a => a.GetType().FullName.Equals("System.Runtime.CompilerServices.AsyncStateMachineAttribute")).Any();
        }

        internal static IEnumerable<TAttribute> GetMatchingAttributes<TAttribute>(this MemberInfo memberInfo, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
            where TAttribute : Attribute
        {
            var customAttributes = memberInfo.GetCustomAttributes(
                typeof(TAttribute), false)
                .Cast<TAttribute>()
                .ToList();

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

        private static (bool success, MethodImplAttribute attribute) RecreateMethodImplAttribute(MethodBase methodBase)
        {
            MethodImplAttributes implementationFlags = methodBase.MethodImplementationFlags;

            int implementationFlagsMatchingImplementationOptions =
                (int)implementationFlags & ImplementationOptionsMask.Value;

            MethodImplOptions implementationOptions =
                (MethodImplOptions)
                implementationFlagsMatchingImplementationOptions;

            if (implementationOptions != 0)
            {
                return (true, new MethodImplAttribute(implementationOptions));
            }

            return (false, null);
        }
    }
}
