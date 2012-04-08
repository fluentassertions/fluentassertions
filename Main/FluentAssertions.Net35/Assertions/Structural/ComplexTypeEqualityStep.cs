using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;

#if WINRT
using System.Reflection.RuntimeExtensions;
#endif

namespace FluentAssertions.Assertions.Structural
{
    internal class ComplexTypeEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(StructuralEqualityContext context)
        {
            return (context.Subject != null) && 
                context.Subject.GetType().IsComplexType() && (context.IsRoot || context.Recursive);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(StructuralEqualityContext context, IStructuralEqualityValidator parent)
        {
            foreach (PropertyInfo propertyInfo in DeterminePropertiesToInclude(context))
            {
                AssertPropertyEquality(context, parent, propertyInfo);
            }

            return true;
        }

        private void AssertPropertyEquality(StructuralEqualityContext context, IStructuralEqualityValidator parent,
            PropertyInfo propertyInfo)
        {
            PropertyInfo expectationProperty = FindPropertyFrom(context, propertyInfo.Name);
            if (expectationProperty != null)
            {
                object expectation = expectationProperty.GetValue(context.Expectation, null);
                object subject = propertyInfo.GetValue(context.Subject, null);

                parent.AssertEquality(context.CreateNestedContext(subject, expectation,
                    !context.IsRoot ? "." + propertyInfo.Name : "property " + propertyInfo.Name));
            }
        }

        private IEnumerable<PropertyInfo> DeterminePropertiesToInclude(StructuralEqualityContext context)
        {
            IEnumerable<PropertyInfo> properties;

            if (context.PropertySelection == PropertySelection.AllRuntimePublic)
            {
                properties = context.Subject.GetType().GetNonPrivateProperties();
            }
            else if ((context.PropertySelection == PropertySelection.AllCompileTimePublic) ||
                (context.PropertySelection == PropertySelection.OnlyShared))
            {
                properties = context.CompileTimeType.GetNonPrivateProperties();
            }
            else if (context.PropertySelection == PropertySelection.None)
            {
                properties = context.Subject.GetType().GetNonPrivateProperties(context.IncludedProperties);
            }
            else
            {
                throw new InvalidOperationException("Unknown PropertySelection value " + context.PropertySelection);
            }

            properties = properties.Where(pi => !context.ExcludedProperties.Contains(pi.Name)).ToArray();
            if ((context.PropertySelection != PropertySelection.OnlyShared) && !properties.Any())
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            return properties.ToArray();
        }

        private PropertyInfo FindPropertyFrom(StructuralEqualityContext context, string propertyName)
        {
            PropertyInfo compareeProperty = context.Expectation.FindProperty(propertyName);

            if ((compareeProperty == null) && (context.PropertySelection != PropertySelection.OnlyShared))
            {
                string path = (context.PropertyPath.Length > 0) ? context.PropertyPath + "." : "property " ;

                context.Verification.FailWith(
                    "Subject has " + path + propertyName + " that the other object does not have.");
            }

            return compareeProperty;
        }
    }
}