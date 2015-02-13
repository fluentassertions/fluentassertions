using System;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    internal static class IEquivalencyValidationContextExtentions
    {
        internal static IEquivalencyValidationContext CreateForNestedMember(
            this IEquivalencyValidationContext equivalencyValidationContext,
            SelectedMemberInfo nestedMember,
            SelectedMemberInfo matchingProperty)
        {
            object subject = nestedMember.GetValue(equivalencyValidationContext.Subject, null);
            object expectation = matchingProperty.GetValue(equivalencyValidationContext.Expectation, null);

            return CreateNested(
                equivalencyValidationContext,
                nestedMember,
                subject,
                expectation,
                "member ",
                nestedMember.Name,
                ".",
                nestedMember.MemberType);
        }

        internal static IEquivalencyValidationContext CreateForCollectionItem<T>(
            this IEquivalencyValidationContext equivalencyValidationContext,
            int index,
            T subject,
            object expectation)
        {
            return CreateNested(
                equivalencyValidationContext,
                equivalencyValidationContext.SelectedMemberInfo,
                subject,
                expectation,
                "item",
                "[" + index + "]",
                String.Empty,
                typeof(T));
        }

        internal static IEquivalencyValidationContext CreateForDictionaryItem<TKey, TValue>(
            this IEquivalencyValidationContext equivalencyValidationContext,
            TKey key,
            TValue subject,
            object expectation)
        {
            return CreateNested(
                equivalencyValidationContext,
                equivalencyValidationContext.SelectedMemberInfo,
                subject,
                expectation,
                "pair",
                "[" + key + "]",
                String.Empty,
                typeof(TValue));
        }

        internal static IEquivalencyValidationContext CreateWithDifferentSubject(
            this IEquivalencyValidationContext context,
            object subject,
            Type compileTimeType)
        {
            return new EquivalencyValidationContext
                       {
                           CompileTimeType = compileTimeType,
                           Expectation = context.Expectation,
                           SelectedMemberDescription = context.SelectedMemberDescription,
                           SelectedMemberInfo = context.SelectedMemberInfo,
                           SelectedMemberPath = context.SelectedMemberPath,
                           Reason = context.Reason,
                           ReasonArgs = context.ReasonArgs,
                           Subject = subject
                       };
        }

        private static IEquivalencyValidationContext CreateNested(
            this IEquivalencyValidationContext equivalencyValidationContext,
            SelectedMemberInfo subjectProperty,
            object subject,
            object expectation,
            string memberType,
            string memberDescription,
            string separator,
            Type compileTimeType)
        {
            string propertyPath = equivalencyValidationContext.IsRoot
                                      ? memberType
                                      : equivalencyValidationContext.SelectedMemberDescription + separator;

            return new EquivalencyValidationContext
                       {
                           SelectedMemberInfo = subjectProperty,
                           Subject = subject,
                           Expectation = expectation,
                           SelectedMemberPath =
                               equivalencyValidationContext.SelectedMemberPath.Combine(
                                   memberDescription,
                                   separator),
                           SelectedMemberDescription = propertyPath + memberDescription,
                           Reason = equivalencyValidationContext.Reason,
                           ReasonArgs = equivalencyValidationContext.ReasonArgs,
                           CompileTimeType = compileTimeType,
                       };
        }
    }
}