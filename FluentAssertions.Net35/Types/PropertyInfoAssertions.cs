using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains assertions for the <see cref="PropertyInfo"/> objects returned by the parent <see cref="PropertyInfoSelector"/>.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInfoAssertions : ReferenceTypeAssertions<PropertyInfo, PropertyInfoAssertions>
    {
        private readonly bool isAssertingSingleProperty;

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IEnumerable<PropertyInfo> SubjectProperties { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfoAssertions"/> class, for a single <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property">The property to assert.</param>
        public PropertyInfoAssertions(PropertyInfo property)
        {
            isAssertingSingleProperty = true;
            SubjectProperties = new[] { property };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfoAssertions"/> class, for a number of <see cref="PropertyInfo"/> objects.
        /// </summary>
        /// <param name="properties">The properties to assert.</param>
        public PropertyInfoAssertions(IEnumerable<PropertyInfo> properties)
        {
            SubjectProperties = properties;
        }

        /// <summary>
        /// Asserts that the selected properties are virtual.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeVirtual(string reason = "", params object[] reasonArgs)
        {
            IEnumerable<PropertyInfo> nonVirtualProperties = GetAllNonVirtualPropertiesFromSelection();

            string failureMessage = isAssertingSingleProperty
                ? "Expected property " + GetDescriptionsFor(new[] { SubjectProperties.Single() }) +
                    " to be virtual{reason}, but it is not virtual."
                : "Expected all selected properties to be virtual{reason}, but the following properties are not virtual:\r\n" +
                    GetDescriptionsFor(nonVirtualProperties);

            Execute.Assertion
                .ForCondition(!nonVirtualProperties.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected properties have a setter.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeWritable(string reason = "", params object[] reasonArgs)
        {
            foreach (var property in SubjectProperties)
            {
                Execute.Assertion
                       .ForCondition(property.CanWrite)
                       .BecauseOf(reason, reasonArgs)
                       .FailWith("Expected {context:property} {0} to have a setter{reason}.", property);
            }

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        private PropertyInfo[] GetAllNonVirtualPropertiesFromSelection()
        {
            var query = 
                from property in SubjectProperties
#if !WINRT
                let getter = property.GetGetMethod(true)
#else
                let getter = property.GetMethod
#endif
                where !getter.IsVirtual || getter.IsFinal
                select property;

            return query.ToArray();
        }

        /// <summary>
        /// Asserts that the selected methods are decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeDecoratedWith<TAttribute>(string reason = "", params object[] reasonArgs)
        {
            IEnumerable<PropertyInfo> propertiesWithoutAttribute = GetPropertiesWithout<TAttribute>();

            string failureMessage = isAssertingSingleProperty
                ? "Expected property " + GetDescriptionsFor(new[] { SubjectProperties.Single() }) +
                    " to be decorated with {0}{reason}, but that attribute was not found."
                : "Expected all selected properties to be decorated with {0}{reason}, but the following properties are not:\r\n" +
                    GetDescriptionsFor(propertiesWithoutAttribute);

            Execute.Assertion
                .ForCondition(!propertiesWithoutAttribute.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage, typeof(TAttribute));

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        private PropertyInfo[] GetPropertiesWithout<TAttribute>()
        {
            return SubjectProperties.Where(property => !IsDecoratedWith<TAttribute>(property)).ToArray();
        }

        private static bool IsDecoratedWith<TAttribute>(PropertyInfo property)
        {
            return property.GetCustomAttributes(false).OfType<TAttribute>().Any();
        }

        private static string GetDescriptionsFor(IEnumerable<PropertyInfo> properties)
        {
            return string.Join(Environment.NewLine, properties.Select(GetDescriptionFor).ToArray());
        }

        private static string GetDescriptionFor(PropertyInfo property)
        {
            string propTypeName;
#if !WINRT
            propTypeName = property.PropertyType.Name;
#else
            propTypeName = property.PropertyType.GetTypeInfo().Name;
#endif
            return string.Format("{0} {1}.{2}", propTypeName, property.DeclaringType, property.Name);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "property info"; }
        }
    }
}
