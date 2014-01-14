using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Types
{
    [DebuggerNonUserCode]
    public class MethodInfoAssertions :
        ReferenceTypeAssertions<MethodInfo, MethodInfoAssertions>
    {
        public MethodInfoAssertions(MethodInfo methodInfo)
        {
            this.Subject = methodInfo;
        }

        protected override string Context
        {
            get { return "method"; }
        }

        public AndConstraint<MethodInfoAssertions> BeDecoratedWith<TAttribute>(
            Func<TAttribute, bool> isMatchingAttributePredicate,
            string reason = "", params object[] reasonArgs)
        {
            string failureMessage = "Expected method " +
                                    GetDescriptionFor(Subject) +
                                    " to be decorated with {0}{reason}, but that attribute was not found.";

            Execute.Assertion
                .ForCondition(IsDecoratedWith(Subject,
                    isMatchingAttributePredicate))
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage, typeof (TAttribute));

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        public AndConstraint<MethodInfoAssertions> BeDecoratedWith<TAttribute>(
            string reason = "", params object[] reasonArgs)
        {
            return BeDecoratedWith<TAttribute>(attr => true, reason, reasonArgs);
        }

        public AndConstraint<MethodInfoAssertions> BeVirtual(
            string reason = "", params object[] reasonArgs)
        {
            string failureMessage = "Expected method " +
                                    GetDescriptionFor(Subject) +
                                    " to be virtual{reason}, but it is not virtual.";

            Execute.Assertion
                .ForCondition(!IsNonVirtual(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage);

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        internal static string GetDescriptionFor(MethodInfo method)
        {
            string returnTypeName = method.ReturnType.Name;

            return String.Format("{0} {1}.{2}", returnTypeName,
                method.DeclaringType, method.Name);
        }

        internal static bool IsDecoratedWith<TAttribute>(MethodInfo method, Func<TAttribute, bool> isMatchingPredicate)
        {
            return method.GetCustomAttributes(false).OfType<TAttribute>().Any(isMatchingPredicate);
        }

        internal static bool IsNonVirtual(MethodInfo method)
        {
            return !method.IsVirtual || method.IsFinal;
        }
    }
}
