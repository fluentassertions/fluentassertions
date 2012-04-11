using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Structural
{
    internal class StructuralEqualityContext
    {
        private IList<string> includedProperties = new List<string>();
        private IList<string> excludedProperties = new List<string>();
        private IList<object> processedObjects = new List<object>();

        public StructuralEqualityContext()
        {
            PropertyPath = "";
            PropertySelection = PropertySelection.None;
        }

        public object Subject { get; set; }

        public object Expectation { get; set; }

        public string Reason { get; set; }

        public object[] ReasonArgs { get; set; }

        public bool IsRoot
        {
            get { return (PropertyPath.Length == 0); }
        }

        public PropertySelection PropertySelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it should continue comparing (collections of objects) that
        /// the <see cref="OtherObject"/> refers to.
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how cyclic references that are encountered while comparing (collections of)
        /// objects should be handled.
        /// </summary>
        public CyclicReferenceHandling CyclicReferenceHandling { get; set; }

        public Type CompileTimeType { get; set; }

        public string PropertyPath { get; set; }

        public IEnumerable<string> IncludedProperties
        {
            get { return includedProperties; }
        }

        public IList<string> ExcludedProperties
        {
            get { return excludedProperties; }
        }

        public bool ContainsCyclicReference
        {
            get { return processedObjects.Contains(Subject); }
        }

        public Verification Verification
        {
            get { return Execute.Verification.BecauseOf(Reason, ReasonArgs); }
        }

        public void ExcludeProperty(string propertyName)
        {
            excludedProperties.Add(propertyName);
        }

        public void IncludeProperty(string propertyName)
        {
            includedProperties.Add(propertyName);
        }

        public void HandleCyclicReference()
        {
            if (CyclicReferenceHandling == CyclicReferenceHandling.ThrowException)
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
                PropertyPath = propertyPath,
                Reason = Reason,
                ReasonArgs = ReasonArgs,
                Recursive = true,
                PropertySelection = PropertySelection,
                CompileTimeType = (subject != null) ? subject.GetType() : null,
                CyclicReferenceHandling = CyclicReferenceHandling,
                includedProperties = includedProperties,
                excludedProperties = excludedProperties,
                processedObjects = processedObjects.Concat(new[] {Subject}).ToList()
            };
        }
    }
}