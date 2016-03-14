using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoSelectorAssertions> BeVirtual(string because = "", params object[] becauseArgs)
        {
            IEnumerable<PropertyInfo> nonVirtualProperties = GetAllNonVirtualPropertiesFromSelection();

            string failureMessage =
                "Expected all selected properties to be virtual{reason}, but the following properties are not virtual:\r\n" +
                GetDescriptionsFor(nonVirtualProperties);

            Execute.Assertion
                .ForCondition(!nonVirtualProperties.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected properties have a setter.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoSelectorAssertions> BeWritable(string because = "", params object[] becauseArgs)
        {
            PropertyInfo[] readOnlyProperties = GetAllReadOnlyPropertiesFromSelection();

            string failureMessage =
                "Expected all selected properties to have a setter{reason}, but the following properties do not:\r\n" +
                GetDescriptionsFor(readOnlyProperties);

            Execute.Assertion
                .ForCondition(!readOnlyProperties.Any())
                .BecauseOf(because, becauseArgs)
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
                where !property.IsVirtual()
                select property;

            return query.ToArray();
        }

        /// <summary>
        /// Asserts that the selected properties are decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoSelectorAssertions> BeDecoratedWith<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            IEnumerable<PropertyInfo> propertiesWithoutAttribute = GetPropertiesWithout<TAttribute>();

            string failureMessage =
                "Expected all selected properties to be decorated with {0}{reason}, but the following properties are not:\r\n" +
                GetDescriptionsFor(propertiesWithoutAttribute);

            Execute.Assertion
                .ForCondition(!propertiesWithoutAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage, typeof(TAttribute));

            return new AndConstraint<PropertyInfoSelectorAssertions>(this);
        }

        private PropertyInfo[] GetPropertiesWithout<TAttribute>()
            where TAttribute : Attribute
        {
            return SubjectProperties.Where(property => !property.IsDecoratedWith<TAttribute>()).ToArray();
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
