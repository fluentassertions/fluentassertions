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
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Returns <c>true</c> if this step finalizes the comparison task, returns <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met, or if it detects mismatching data.
        /// </remarks>
        public bool Execute(StructuralEqualityContext context, IStructuralEqualityValidator parent)
        {
            if ((context.Subject != null) && context.Subject.GetType().IsComplexType() && (context.IsRoot || context.Recursive))
            {
                foreach (PropertyInfo propertyInfo in DeterminePropertiesToInclude(context))
                {
                    object subject = propertyInfo.GetValue(context.Subject, null);

                    PropertyInfo expectationProperty = FindPropertyFrom(context, propertyInfo.Name);
                    if (expectationProperty != null)
                    {
                        object expectation = expectationProperty.GetValue(context.Expectation, null);

                        parent.AssertEquality(context.CreateNested(subject, expectation,
                            !context.IsRoot ? "." + propertyInfo.Name : "property " + propertyInfo.Name));
                    }
                }

                return true;
            }

            return false;
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

            if ((context.PropertySelection != PropertySelection.OnlyShared) && (compareeProperty == null))
            {
                string path = (context.PropertyPath.Length > 0)
                    ? context.PropertyPath + "." + propertyName
                    : "property " + propertyName;

                FluentAssertions.Execute.Verification.FailWith(
                    "Subject has " + path + " that the other object does not have.");
            }

            return compareeProperty;
        }
    }
}