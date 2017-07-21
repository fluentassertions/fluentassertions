using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertions.Common
{
    public static class MethodInfoExtensions
    {
        internal static bool IsAsync(this MethodInfo methodInfo)
        {
            return methodInfo.GetMatchingAttributes<Attribute>(a => a.GetType().FullName.Equals("System.Runtime.CompilerServices.AsyncStateMachineAttribute")).Any();
        }

        internal static IEnumerable<TAttribute> GetMatchingAttributes<TAttribute>(this MemberInfo memberInfo, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate) where TAttribute : Attribute
        {
            return memberInfo.GetCustomAttributes(
                typeof(TAttribute), false)
                .Cast<TAttribute>()
                .Where(isMatchingAttributePredicate.Compile());
        }

        internal static bool IsNonVirtual(this MethodInfo method)
        {
            return !method.IsVirtual || method.IsFinal;
        }
    }
}
