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
            PropertyPath = "";
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
            get { return (PropertyPath.Length == 0); }
        }

        public Type CompileTimeType { get; set; }

        public string PropertyPath { get; set; }

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

                foreach (var selectionRule in config.SelectionRules)
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
                        "Expected " + PropertyPath + " to be {0}{reason}, but it contains a cyclic reference.",
                        Expectation);
            }
        }

        public StructuralEqualityContext CreateNestedContext(object subject, object expectation, string childPropertyName)
        {
            string propertyPath = (PropertyPath.Length > 0) ? PropertyPath + childPropertyName : childPropertyName;

            return new StructuralEqualityContext
            {
                Subject = subject,
                Expectation = expectation,
                config = config,
                PropertyPath = propertyPath,
                Reason = Reason,
                ReasonArgs = ReasonArgs,
                CompileTimeType = (subject != null) ? subject.GetType() : null,
                processedObjects = processedObjects.Concat(new[] {Subject}).ToList()
            };
        }

        public PropertyInfo FindMatchFor(PropertyInfo propertyInfo)
        {
            PropertyInfo matchingProperty = null;

            foreach (var rule in config.MatchingRules)
            {
                matchingProperty = rule.Match(propertyInfo, Expectation, PropertyPath);
                if (matchingProperty != null)
                {
                    break;
                }
            }

            return matchingProperty;
        }
    }
}