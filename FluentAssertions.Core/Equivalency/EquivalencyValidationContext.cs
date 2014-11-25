using System;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    public class EquivalencyValidationContext : IEquivalencyValidationContext
    {
        public EquivalencyValidationContext()
        {
            PropertyDescription = "";
            PropertyPath = "";
        }

        /// <summary>
        /// Gets the <see cref="ISubjectInfo.PropertyInfo" /> of the property that returned the current object, or 
        /// <c>null</c> if the current object represents the root object.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        ///   Gets the full path from the root object until the current property, separated by dots.
        /// </summary>
        public string PropertyPath { get; set; }

        /// <summary>
        ///   Gets a textual description of the current property based on the <see cref="PropertyPath" />.
        /// </summary>
        public string PropertyDescription { get; set; }

        /// <summary>
        /// Gets the value of the <see cref="ISubjectInfo.PropertyInfo" />
        /// </summary>
        public object Subject { get; set; }

        /// <summary>
        /// Gets the value of the <see cref="IEquivalencyValidationContext.MatchingExpectationProperty" />.
        /// </summary>
        public object Expectation { get; set; }

        /// <summary>
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        ///   Zero or more objects to format using the placeholders in <see cref="IEquivalencyValidationContext.Reason" />.
        /// </summary>
        public object[] ReasonArgs { get; set; }

        /// <summary>
        ///   Gets a value indicating whether the current context represents the root of the object graph.
        /// </summary>
        public bool IsRoot
        {
            get { return (PropertyDescription.Length == 0); }
        }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object, then it returns the 
        /// same <see cref="Type"/> as the <see cref="ISubjectInfo.RuntimeType"/> property does.
        /// </summary>
        public Type CompileTimeType { get; set; }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        public Type RuntimeType
        {
            get
            {
                if (Subject != null)
                {
                    return Subject.GetType();
                }

                if (PropertyInfo != null)
                {
                    return PropertyInfo.PropertyType;
                }

                return CompileTimeType;
            }
        }
    }

    internal static class EquivalencyValidationContextExtentions
    {
        internal static EquivalencyValidationContext CreateForNestedProperty(
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

        public static EquivalencyValidationContext CreateForCollectionItem<T>(
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

        public static EquivalencyValidationContext CreateForDictionaryItem<TKey, TValue>(
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

        private static EquivalencyValidationContext CreateNested(
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