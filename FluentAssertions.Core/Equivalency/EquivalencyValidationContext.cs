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
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        ///   Gets the full path from the root object until the current property, separated by dots.
        /// </summary>
        public string PropertyPath { get; set; }

        /// <summary>
        ///   Gets a textual description of the current property based on the <see cref="PropertyPath" />.
        /// </summary>
        public string PropertyDescription { get; private set; }

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
                Type type = CompileTimeType;
                if (Subject != null)
                {
                    type = Subject.GetType();
                }
                else if (PropertyInfo != null)
                {
                    type = PropertyInfo.PropertyType;
                }

                return type;
            }
        }

        internal EquivalencyValidationContext CreateForNestedProperty(PropertyInfo nestedProperty,
            PropertyInfo matchingProperty)
        {
            object subject = nestedProperty.GetValue(Subject, null);
            object expectation = matchingProperty.GetValue(Expectation, null);

            return CreateNested(
                nestedProperty,
                subject,
                expectation,
                "property ",
                nestedProperty.Name,
                ".",
                nestedProperty.PropertyType);
        }

        public EquivalencyValidationContext CreateForCollectionItem<T>(int index, T subject, object expectation)
        {
            return CreateNested(
                PropertyInfo,
                subject,
                expectation,
                "item",
                "[" + index + "]",
                string.Empty,
                typeof(T));
        }

        public EquivalencyValidationContext CreateForDictionaryItem(object key, object subject, object expectation)
        {
            return CreateNested(
                PropertyInfo,
                subject,
                expectation,
                "pair",
                "[" + key + "]",
                string.Empty,
                GetDictionaryValueType(subject, CompileTimeType));
        }

        private Type GetDictionaryValueType(object value, Type compileTimeType)
        {
            if (!ReferenceEquals(value, null))
            {
                return value.GetType();
            }
            else if (compileTimeType.IsGenericType)
            {
                return compileTimeType.GetGenericArguments()[1];
            }
            else
            {
                return typeof(object);
            }
        }

        private EquivalencyValidationContext CreateNested(
            PropertyInfo subjectProperty, object subject, object expectation,
            string memberType, string memberDescription, string separator,
            Type compileTimeType)
        {
            string propertyPath = IsRoot ? memberType : PropertyDescription + separator;

            return new EquivalencyValidationContext
            {
                PropertyInfo = subjectProperty,
                Subject = subject,
                Expectation = expectation,
                PropertyPath = PropertyPath.Combine(memberDescription, separator),
                PropertyDescription = propertyPath + memberDescription,
                Reason = Reason,
                ReasonArgs = ReasonArgs,
                CompileTimeType = compileTimeType,
            };
        }
    }
}