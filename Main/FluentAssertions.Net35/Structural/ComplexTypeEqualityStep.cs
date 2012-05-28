using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

#if WINRT
using System.Reflection.RuntimeExtensions;
#endif

namespace FluentAssertions.Structural
{
    internal class ComplexTypeEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(StructuralEqualityContext context)
        {
            return (context.Subject != null) &&
                context.Subject.GetType().IsComplexType() && (context.IsRoot || context.Config.Recurse);
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
            IEnumerable<PropertyInfo> selectedProperties = context.SelectedProperties.ToArray();
            if (!selectedProperties.Any())
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            foreach (PropertyInfo propertyInfo in selectedProperties)
            {
                AssertPropertyEquality(context, parent, propertyInfo);
            }

            return true;
        }

        private void AssertPropertyEquality(StructuralEqualityContext context, IStructuralEqualityValidator parent,
            PropertyInfo propertyInfo)
        {
            PropertyInfo matchingPropertyInfo = context.FindMatchFor(propertyInfo);
            if (matchingPropertyInfo != null)
            {
                object expectation = matchingPropertyInfo.GetValue(context.Expectation, null);
                object subject = propertyInfo.GetValue(context.Subject, null);

                parent.AssertEquality(context.CreateNestedContext(subject, expectation,
                    !context.IsRoot ? "." + propertyInfo.Name : "property " + propertyInfo.Name));
            }
        }
    }

    public interface ISelectionRule
    {
        IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info);
    }

    public interface IMatchingRule
    {
        PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath);
    }

    public class TypeInfo
    {
        public Type DeclaredType { get; set; }
        public Type RuntimeType { get; set; }
    }

    public class AllRuntimePublicPropertiesSelectionRule : ISelectionRule
    {
        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            return info.RuntimeType.GetNonPrivateProperties();
        }
    }

    public class AllDeclaredPublicPropertiesSelectionRule : ISelectionRule
    {
        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            return info.DeclaredType.GetNonPrivateProperties();
        }
    }

    public class IgnorePropertySelectionRule : ISelectionRule
    {
        private readonly PropertyInfo propertyInfo;

        public IgnorePropertySelectionRule(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            return properties.Where(pi => pi != propertyInfo).ToArray();
        }
    }

    public class IncludePropertySelectionRule : ISelectionRule
    {
        private readonly PropertyInfo propertyInfo;

        public IncludePropertySelectionRule(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> properties, TypeInfo info)
        {
            var props = new List<PropertyInfo>(properties);

            if (!props.Contains(propertyInfo))
            {
                props.Add(propertyInfo);
            }

            return props;
        }
    }
    
    public class MustMatchByNameRule : IMatchingRule
    {
        public PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath)
        {
            PropertyInfo compareeProperty = expectation.FindProperty(subjectProperty.Name);

            if (compareeProperty == null)
            {
                string path = (propertyPath.Length > 0) ? propertyPath + "." : "property ";

                Execute.Verification.FailWith(
                    "Subject has " + path + subjectProperty.Name + " that the other object does not have.");
            }

            return compareeProperty;
        }
    }

    public class TryMatchByNameRule : IMatchingRule
    {
        public PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath)
        {
            return expectation.FindProperty(subjectProperty.Name);
        }
    }
}