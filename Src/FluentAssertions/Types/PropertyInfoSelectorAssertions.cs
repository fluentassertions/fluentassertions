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
        public PropertyInfoSelectorAssertions(params PropertyInfo[] properties)
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
            PropertyInfo[] nonVirtualProperties = GetAllNonVirtualPropertiesFromSelection();

            string failureMessage =
                "Expected all selected properties to be virtual{reason}, but the following properties are not virtual:" +
                Environment.NewLine +
                GetDescriptionsFor(nonVirtualProperties);

            Execute.Assertion
                .ForCondition(!nonVirtualProperties.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected properties are not virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoSelectorAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
        {
            PropertyInfo[] virtualProperties = GetAllVirtualPropertiesFromSelection();

            string failureMessage =
                "Expected all selected properties not to be virtual{reason}, but the following properties are virtual:" +
                Environment.NewLine +
                GetDescriptionsFor(virtualProperties);

            Execute.Assertion
                .ForCondition(!virtualProperties.Any())
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
                "Expected all selected properties to have a setter{reason}, but the following properties do not:" +
                Environment.NewLine +
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
            IEnumerable<PropertyInfo> query =
                from property in SubjectProperties
                where !property.IsVirtual()
                select property;

            return query.ToArray();
        }

        private PropertyInfo[] GetAllVirtualPropertiesFromSelection()
        {
            IEnumerable<PropertyInfo> query =
                from property in SubjectProperties
                where property.IsVirtual()
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
            PropertyInfo[] propertiesWithoutAttribute = GetPropertiesWithout<TAttribute>();

            string failureMessage =
                "Expected all selected properties to be decorated with {0}{reason}, but the following properties are not:" +
                Environment.NewLine +
                GetDescriptionsFor(propertiesWithoutAttribute);

            Execute.Assertion
                .ForCondition(!propertiesWithoutAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage, typeof(TAttribute));

            return new AndConstraint<PropertyInfoSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected properties are not decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoSelectorAssertions> NotBeDecoratedWith<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            PropertyInfo[] propertiesWithAttribute = GetPropertiesWith<TAttribute>();

            string failureMessage =
                "Expected all selected properties not to be decorated with {0}{reason}, but the following properties are:" +
                Environment.NewLine +
                GetDescriptionsFor(propertiesWithAttribute);

            Execute.Assertion
                .ForCondition(!propertiesWithAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage, typeof(TAttribute));

            return new AndConstraint<PropertyInfoSelectorAssertions>(this);
        }

        private PropertyInfo[] GetPropertiesWithout<TAttribute>()
            where TAttribute : Attribute
        {
            return SubjectProperties.Where(property => !property.IsDecoratedWith<TAttribute>()).ToArray();
        }

        private PropertyInfo[] GetPropertiesWith<TAttribute>()
            where TAttribute : Attribute
        {
            return SubjectProperties.Where(property => property.IsDecoratedWith<TAttribute>()).ToArray();
        }

        private static string GetDescriptionsFor(IEnumerable<PropertyInfo> properties)
        {
            string[] descriptions = properties.Select(PropertyInfoAssertions.GetDescriptionFor).ToArray();
            return string.Join(Environment.NewLine, descriptions);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
#pragma warning disable CA1822 // Do not change signature of a public member
        protected string Context => "property info";
#pragma warning restore CA1822
    }
}
