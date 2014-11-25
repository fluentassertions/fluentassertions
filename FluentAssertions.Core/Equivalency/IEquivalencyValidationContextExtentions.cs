using System;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    internal static class IEquivalencyValidationContextExtentions
    {
        internal static IEquivalencyValidationContext CreateForNestedProperty(
            this IEquivalencyValidationContext equivalencyValidationContext,
            PropertyInfo nestedProperty,
            PropertyInfo matchingProperty)
        {
            object subject = nestedProperty.GetValue(equivalencyValidationContext.Subject, null);
            object expectation = matchingProperty.GetValue(equivalencyValidationContext.Expectation, null);

            return CreateNested(
                equivalencyValidationContext,
                nestedProperty,
                subject,
                expectation,
                "property ",
                nestedProperty.Name,
                ".",
                nestedProperty.PropertyType);
        }

        public static IEquivalencyValidationContext CreateForCollectionItem<T>(
            this IEquivalencyValidationContext equivalencyValidationContext,
            int index,
            T subject,
            object expectation)
        {
            return CreateNested(
                equivalencyValidationContext,
                equivalencyValidationContext.PropertyInfo,
                subject,
                expectation,
                "item",
                "[" + index + "]",
                String.Empty,
                typeof(T));
        }

        public static IEquivalencyValidationContext CreateForDictionaryItem<TKey, TValue>(
            this IEquivalencyValidationContext equivalencyValidationContext,
            TKey key,
            TValue subject,
            object expectation)
        {
            return CreateNested(
                equivalencyValidationContext,
                equivalencyValidationContext.PropertyInfo,
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
                           PropertyDescription = context.PropertyDescription,
                           PropertyInfo = context.PropertyInfo,
                           PropertyPath = context.PropertyPath,
                           Reason = context.Reason,
                           ReasonArgs = context.ReasonArgs,
                           Subject = subject
                       };
        }

        private static IEquivalencyValidationContext CreateNested(
            this IEquivalencyValidationContext equivalencyValidationContext,
            PropertyInfo subjectProperty,
            object subject,
            object expectation,
            string memberType,
            string memberDescription,
            string separator,
            Type compileTimeType)
        {
            string propertyPath = equivalencyValidationContext.IsRoot
                                      ? memberType
                                      : equivalencyValidationContext.PropertyDescription + separator;

            return new EquivalencyValidationContext
                       {
                           PropertyInfo = subjectProperty,
                           Subject = subject,
                           Expectation = expectation,
                           PropertyPath =
                               equivalencyValidationContext.PropertyPath.Combine(
                                   memberDescription,
                                   separator),
                           PropertyDescription = propertyPath + memberDescription,
                           Reason = equivalencyValidationContext.Reason,
                           ReasonArgs = equivalencyValidationContext.ReasonArgs,
                           CompileTimeType = compileTimeType,
                       };
        }
    }
}