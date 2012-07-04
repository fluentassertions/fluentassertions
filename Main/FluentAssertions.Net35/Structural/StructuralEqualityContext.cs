using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Structural
{
    internal class StructuralEqualityContext
    {
        private ComparisonConfiguration config = ComparisonConfiguration.Default;

        private IList<object> processedObjects = new List<object>();

        public StructuralEqualityContext()
        {
            FullPropertyPath = "";
        }

        public object Subject { get; set; }

        public object Expectation { get; set; }

        public string Reason { get; set; }

        public object[] ReasonArgs { get; set; }

        public ComparisonConfiguration Config
        {
            get { return config; }
        }

        public bool IsRoot
        {
            get { return (FullPropertyPath.Length == 0); }
        }

        public Type CompileTimeType { get; set; }

        /// <summary>
        /// Gets or sets the current property of the <see cref="Subject"/> that is being processed, or <c>null</c>
        /// if <see cref="IsRoot"/> is <c>true</c>.
        /// </summary>
        public PropertyInfo CurrentProperty { get; set; }

        /// <summary>
        /// Gets the full path from the root object until the current property, separated by dots.
        /// </summary>
        public string FullPropertyPath { get; set; }

        public bool ContainsCyclicReference
        {
            get { return processedObjects.Contains(Subject); }
        }

        public Verification Verification
        {
            get { return Execute.Verification.BecauseOf(Reason, ReasonArgs); }
        }

        public IEnumerable<PropertyInfo> SelectedProperties
        {
            get
            {
                IEnumerable<PropertyInfo> properties = new List<PropertyInfo>();

                foreach (ISelectionRule selectionRule in config.SelectionRules)
                {
                    properties = selectionRule.SelectProperties(properties, new TypeInfo
                    {
                        DeclaredType = CompileTimeType,
                        RuntimeType = Subject.GetType()
                    });
                }

                return properties;
            }
        }

        public void HandleCyclicReference()
        {
            if (config.CyclicReferenceHandling == CyclicReferenceHandling.ThrowException)
            {
                Execute.Verification
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith(
                        "Expected " + FullPropertyPath + " to be {0}{reason}, but it contains a cyclic reference.",
                        Expectation);
            }
        }

        public StructuralEqualityContext CreateNested(
            object subject, object expectation, 
            string memberType, string memberDescription)
        {
            return CreateNested(CurrentProperty, subject, expectation, memberType, memberDescription, "");
        }

        public StructuralEqualityContext CreateNested(
            PropertyInfo subjectProperty,
            object subject, object expectation,
            string memberType, string memberDescription, string separator)
        {
            string propertyPath = IsRoot ? memberType : FullPropertyPath + separator;

            return new StructuralEqualityContext
            {
                CurrentProperty = subjectProperty,
                Subject = subject,
                Expectation = expectation,
                config = config,
                FullPropertyPath = propertyPath + memberDescription,
                Reason = Reason,
                ReasonArgs = ReasonArgs,
                CompileTimeType = (subject != null) ? subject.GetType() : null,
                processedObjects = processedObjects.Concat(new[] {Subject}).ToList()
            };
        }

        public PropertyInfo FindMatchFor(PropertyInfo propertyInfo)
        {
            var query =
                from rule in config.MatchingRules
                let match = rule.Match(propertyInfo, Expectation, FullPropertyPath)
                where match != null
                select match;

            return query.FirstOrDefault();
        }
    }
}