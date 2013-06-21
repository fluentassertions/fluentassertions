using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class EquivalencyValidationContext : IEquivalencyValidationContext
    {
        #region Private Definitions

        private IList<object> processedObjects = new List<object>();

        #endregion

        public EquivalencyValidationContext(IEquivalencyAssertionOptions config)
        {
            Config = config;
            PropertyDescription = "";
            PropertyPath = "";
        }

        public IEquivalencyAssertionOptions Config { get; private set; }

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
        public object Subject { get; internal set; }

        /// <summary>
        /// Gets the property of the <see cref="IEquivalencyValidationContext.Expectation" /> that was matched against the <see
        /// cref="ISubjectInfo.PropertyInfo" />, 
        /// or <c>null</c> if <see cref="IEquivalencyValidationContext.IsRoot" /> is <c>true</c>.
        /// </summary>
        public PropertyInfo MatchingExpectationProperty { get; private set; }

        /// <summary>
        /// Gets the value of the <see cref="IEquivalencyValidationContext.MatchingExpectationProperty" />.
        /// </summary>
        public object Expectation { get; internal set; }

        /// <summary>
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        public string Reason { get; internal set; }

        /// <summary>
        ///   Zero or more objects to format using the placeholders in <see cref="IEquivalencyValidationContext.Reason" />.
        /// </summary>
        public object[] ReasonArgs { get; internal set; }

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
                else
                {
                    // Default
                }

                return type;
            }
        }

        internal bool ContainsCyclicReference
        {
            get { return processedObjects.Contains(Subject); }
        }

        internal IEnumerable<PropertyInfo> SelectedProperties
        {
            get
            {
                IEnumerable<PropertyInfo> properties = new List<PropertyInfo>();

                foreach (ISelectionRule selectionRule in Config.SelectionRules)
                {
                    properties = selectionRule.SelectProperties(properties, this);
                }

                return properties;
            }
        }

        internal void HandleCyclicReference()
        {
            if (Config.CyclicReferenceHandling == CyclicReferenceHandling.ThrowException)
            {
                Execute.Verification
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith(
                        "Expected " + PropertyDescription + " to be {0}{reason}, but it contains a cyclic reference.",
                        Expectation);
            }
        }

        internal EquivalencyValidationContext CreateForNestedProperty(PropertyInfo nestedProperty)
        {
            EquivalencyValidationContext nestedContext = null;

            var matchingProperty = FindMatchFor(nestedProperty);
            if (matchingProperty != null)
            {
                var subject = nestedProperty.GetValue(Subject, null);
                var expectation = matchingProperty.GetValue(Expectation, null);

                nestedContext = CreateNested(nestedProperty, subject, matchingProperty, expectation, "property ",
                    nestedProperty.Name, ".");
            }

            return nestedContext;
        }

        private PropertyInfo FindMatchFor(PropertyInfo propertyInfo)
        {
            var query =
                from rule in Config.MatchingRules
                let match = rule.Match(propertyInfo, Expectation, PropertyDescription)
                where match != null
                select match;

            return query.FirstOrDefault();
        }

        public EquivalencyValidationContext CreateForCollectionItem(int index, object subject, object expectation)
        {
            return CreateNested(PropertyInfo, subject, MatchingExpectationProperty, expectation, "item", "[" + index + "]", "");
        }

        public EquivalencyValidationContext CreateForDictionaryItem(object key, object subject, object expectation)
        {
            return CreateNested(PropertyInfo, subject, MatchingExpectationProperty, expectation, "pair", "[" + key + "]", "");
        }

        private EquivalencyValidationContext CreateNested(
            PropertyInfo subjectProperty, object subject,
            PropertyInfo matchingProperty, object expectation,
            string memberType, string memberDescription, string separator)
        {
            string propertyPath = IsRoot ? memberType : PropertyDescription + separator;

            return new EquivalencyValidationContext(Config)
            {
                PropertyInfo = subjectProperty,
                MatchingExpectationProperty = matchingProperty,
                Subject = subject,
                Expectation = expectation,
                PropertyPath = PropertyPath.Combine(memberDescription, separator),
                PropertyDescription = propertyPath + memberDescription,
                Reason = Reason,
                ReasonArgs = ReasonArgs,
                CompileTimeType = (subject != null) ? subject.GetType() : null,
                processedObjects = processedObjects.Concat(new[] { Subject }).ToList()
            };
        }
    }
}