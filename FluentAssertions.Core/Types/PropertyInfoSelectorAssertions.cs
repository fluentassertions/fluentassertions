using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains assertions for the <see cref="PropertyInfo"/> objects returned by the parent <see cref="PropertyInfoSelector"/>.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInfoSelectorAssertions
    {
        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IEnumerable<PropertyInfo> SubjectProperties { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfoSelectorAssertions"/> class, for a number of <see cref="PropertyInfo"/> objects.
        /// </summary>
        /// <param name="properties">The properties to assert.</param>
        public PropertyInfoSelectorAssertions(IEnumerable<PropertyInfo> properties)
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
        public AndConstraint<PropertyInfoSelectorAssertions> BeVirtual(string reason = "", params object[] reasonArgs)
        {
            IEnumerable<PropertyInfo> nonVirtualProperties = GetAllNonVirtualPropertiesFromSelection();

            string failureMessage =
                "Expected all selected properties to be virtual{reason}, but the following properties are not virtual:\r\n" +
                GetDescriptionsFor(nonVirtualProperties);

            Execute.Assertion
                .ForCondition(!nonVirtualProperties.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoSelectorAssertions>(this);
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
        public AndConstraint<PropertyInfoSelectorAssertions> BeWritable(string reason = "", params object[] reasonArgs)
        {
            PropertyInfo[] readOnlyProperties = GetAllReadOnlyPropertiesFromSelection();

            string failureMessage =
                "Expected all selected properties to have a setter{reason}, but the following properties do not:\r\n" +
                GetDescriptionsFor(readOnlyProperties);

            Execute.Assertion
                .ForCondition(!readOnlyProperties.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoSelectorAssertions>(this);
        }

        private PropertyInfo[] GetAllReadOnlyPropertiesFromSelection()
        {
            return SubjectProperties.Where(property => !property.CanWrite).ToArray();
        }

        private PropertyInfo[] GetAllNonVirtualPropertiesFromSelection()
        {
            var query =
                from property in SubjectProperties
                where PropertyInfoAssertions.IsGetterNonVirtual(property)
                select property;

            return query.ToArray();
        }

        /// <summary>
        /// Asserts that the selected properties are decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<PropertyInfoSelectorAssertions> BeDecoratedWith<TAttribute>(string reason = "", params object[] reasonArgs)
            where TAttribute : Attribute
        {
            IEnumerable<PropertyInfo> propertiesWithoutAttribute = GetPropertiesWithout<TAttribute>();

            string failureMessage =
                "Expected all selected properties to be decorated with {0}{reason}, but the following properties are not:\r\n" +
                GetDescriptionsFor(propertiesWithoutAttribute);

            Execute.Assertion
                .ForCondition(!propertiesWithoutAttribute.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage, typeof(TAttribute));

            return new AndConstraint<PropertyInfoSelectorAssertions>(this);
        }

        private PropertyInfo[] GetPropertiesWithout<TAttribute>()
            where TAttribute : Attribute
        {
            return SubjectProperties.Where(property => !PropertyInfoAssertions.IsDecoratedWith<TAttribute>(property)).ToArray();
        }

        private static string GetDescriptionsFor(IEnumerable<PropertyInfo> properties)
        {
            return string.Join(Environment.NewLine, properties.Select(PropertyInfoAssertions.GetDescriptionFor).ToArray());
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected string Context
        {
            get { return "property info"; }
        }
    }
}
