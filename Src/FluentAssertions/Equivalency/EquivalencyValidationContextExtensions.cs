using System;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    internal static class EquivalencyValidationContextExtensions
    {
        internal static IEquivalencyValidationContext CreateForNestedMember(this IEquivalencyValidationContext context,
            SelectedMemberInfo nestedMember, SelectedMemberInfo matchingProperty)
        {
            string memberDescription = nestedMember.Name;
            string propertyPath = (context.SelectedMemberDescription.Length == 0) ? "member " : context.SelectedMemberDescription + ".";

            return new EquivalencyValidationContext
            {
                SelectedMemberInfo = nestedMember,
                Subject = matchingProperty.GetValue(context.Subject, null),
                Expectation = nestedMember.GetValue(context.Expectation, null),
                SelectedMemberPath = context.SelectedMemberPath.Combine(memberDescription, "."),
                SelectedMemberDescription = propertyPath + memberDescription,
                Because = context.Because,
                BecauseArgs = context.BecauseArgs,
                CompileTimeType = nestedMember.MemberType,
                RootIsCollection = context.RootIsCollection,
                Tracer = context.Tracer
            };
        }

        internal static IEquivalencyValidationContext CreateForCollectionItem<T>(this IEquivalencyValidationContext context,
            string index, object subject, T expectation)
        {
            string memberDescription = "[" + index + "]";
            string propertyPath = (context.SelectedMemberDescription.Length == 0) ? "item" : context.SelectedMemberDescription + string.Empty;

            return new EquivalencyValidationContext
            {
                SelectedMemberInfo = context.SelectedMemberInfo,
                Subject = subject,
                Expectation = expectation,
                SelectedMemberPath = context.SelectedMemberPath.Combine(memberDescription, string.Empty),
                SelectedMemberDescription = propertyPath + memberDescription,
                Because = context.Because,
                BecauseArgs = context.BecauseArgs,
                CompileTimeType = typeof(T),
                RootIsCollection = context.RootIsCollection,
                Tracer = context.Tracer
            };
        }

        internal static IEquivalencyValidationContext CreateForDictionaryItem<TKey, TExpectation>(
            this IEquivalencyValidationContext context,
            TKey key,
            object subject,
            TExpectation expectation)
        {
            string memberDescription = "[" + key + "]";
            string propertyPath = (context.SelectedMemberDescription.Length == 0) ? "pair" : context.SelectedMemberDescription + string.Empty;

            return new EquivalencyValidationContext
            {
                SelectedMemberInfo = context.SelectedMemberInfo,
                Subject = subject,
                Expectation = expectation,
                SelectedMemberPath = context.SelectedMemberPath.Combine(memberDescription, string.Empty),
                SelectedMemberDescription = propertyPath + memberDescription,
                Because = context.Because,
                BecauseArgs = context.BecauseArgs,
                CompileTimeType = typeof(TExpectation),
                RootIsCollection = context.RootIsCollection,
                Tracer = context.Tracer
            };
        }

        internal static IEquivalencyValidationContext CreateWithDifferentSubject(this IEquivalencyValidationContext context,
            object convertedSubject, Type expectationType)
        {
            return new EquivalencyValidationContext
            {
                CompileTimeType = expectationType,
                Expectation = context.Expectation,
                SelectedMemberDescription = context.SelectedMemberDescription,
                SelectedMemberInfo = context.SelectedMemberInfo,
                SelectedMemberPath = context.SelectedMemberPath,
                Because = context.Because,
                BecauseArgs = context.BecauseArgs,
                Subject = convertedSubject,
                Tracer = context.Tracer
            };
        }
    }
}
