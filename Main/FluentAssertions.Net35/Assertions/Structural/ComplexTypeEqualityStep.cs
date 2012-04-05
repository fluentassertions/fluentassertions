using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if WINRT
using System.Reflection.RuntimeExtensions;
#endif

namespace FluentAssertions.Assertions.Structure
{
    internal class ComplexTypeEqualityStep : IStructuralEqualityStep
    {
#if !WINRT
        private const BindingFlags PublicPropertiesFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
#endif

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
            if (IsComplexType(context.Subject) && (context.IsRoot || context.Recursive))
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

        private static bool IsComplexType(object expectedValue)
        {
            return HasProperties(expectedValue) && (expectedValue.GetType().Namespace != typeof(int).Namespace);
        }

        private static bool HasProperties(object expectedValue)
        {
            return (expectedValue != null) && expectedValue.GetType()
#if !WINRT
                .GetProperties(PublicPropertiesFlag)
#else
                .GetRuntimeProperties().Where(p => !p.GetMethod.IsStatic)
#endif
                .Any();
        }

        private IEnumerable<PropertyInfo> DeterminePropertiesToInclude(StructuralEqualityContext context)
        {
            IEnumerable<PropertyInfo> properties;

            if (context.PropertySelection == PropertySelection.AllRuntimePublic)
            {
                properties = GetNonPrivateProperties(context.Subject.GetType());
            }
            else if ((context.PropertySelection == PropertySelection.AllCompileTimePublic) ||
                (context.PropertySelection == PropertySelection.OnlyShared))
            {
                properties = GetNonPrivateProperties(context.CompileTimeType);
            }
            else if (context.PropertySelection == PropertySelection.None)
            {
                properties = GetNonPrivateProperties(context.Subject.GetType(), context.IncludedProperties);
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

        private IEnumerable<PropertyInfo> GetNonPrivateProperties(Type typeToReflect,
            IEnumerable<string> explicitProperties = null)
        {
            var query =
#if !WINRT
                from propertyInfo in typeToReflect.GetProperties(PublicPropertiesFlag)
                let getMethod = propertyInfo.GetGetMethod(true)
                where (getMethod != null) && !getMethod.IsPrivate
#else
                from propertyInfo in typeToReflect.GetRuntimeProperties()
                let getMethod = propertyInfo.GetMethod
                where (getMethod != null) && !getMethod.IsPrivate && !getMethod.IsStatic


#endif
                where (explicitProperties == null) || explicitProperties.Contains(propertyInfo.Name)
                select propertyInfo;

            return query.ToList();
        }


        private PropertyInfo FindPropertyFrom(StructuralEqualityContext context, string propertyName)
        {
            PropertyInfo compareeProperty =
#if !WINRT
                context.Expectation.GetType().GetProperties(PublicPropertiesFlag)
#else
                context.Expectation.GetType().GetRuntimeProperties().Where(p => !p.GetMethod.IsStatic)
#endif
                    .SingleOrDefault(pi => pi.Name == propertyName);

            if ((context.PropertySelection != PropertySelection.OnlyShared) && (compareeProperty == null))
            {
                string path = (context.PropertyPath.Length > 0) ? context.PropertyPath + "." + propertyName : "property " + propertyName;

                FluentAssertions.Execute.Verification.FailWith(
                    "Subject has " + path + " that the other object does not have.");
            }

            return compareeProperty;
        }
    }
}