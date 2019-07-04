using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

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
            IEnumerable<PropertyInfo> nonVirtualProperties = GetAllNonVirtualPropertiesFromSelection();

            Execute.Assertion
                .ForCondition(!nonVirtualProperties.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.PropertyInfo_ExpectedAllSelectedPropertiesToBeVirtualButTheFollowingPropertiesAreNotX0X1Format,
                    Environment.NewLine, GetFormattedDescriptionsFor(nonVirtualProperties));

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
            IEnumerable<PropertyInfo> virtualProperties = GetAllVirtualPropertiesFromSelection();

            Execute.Assertion
                .ForCondition(!virtualProperties.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.PropertyInfo_ExpectedAllSelectedPropertiesNotToBeVirtualButTheFollowingPropertiesAreX0X1Format,
                    Environment.NewLine, GetFormattedDescriptionsFor(virtualProperties));

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

            Execute.Assertion
                .ForCondition(!readOnlyProperties.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.PropertyInfo_ExpectedAllSelectedPropertiesToHaveASetterButTheFollowingPropertiesDoNotX0X1Format,
                    Environment.NewLine, GetFormattedDescriptionsFor(readOnlyProperties));

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

        private PropertyInfo[] GetAllVirtualPropertiesFromSelection()
        {
            var query =
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
            IEnumerable<PropertyInfo> propertiesWithoutAttribute = GetPropertiesWithout<TAttribute>();

            Execute.Assertion
                .ForCondition(!propertiesWithoutAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.PropertyInfo_ExpectedAllSelectedPropertiesToBeDecoratedWithX0ButFollowingPropertiesAreNotX1X2Format,
                    typeof(TAttribute), Environment.NewLine, GetFormattedDescriptionsFor(propertiesWithoutAttribute));

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
            IEnumerable<PropertyInfo> propertiesWithAttribute = GetPropertiesWith<TAttribute>();

            Execute.Assertion
                .ForCondition(!propertiesWithAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.PropertyInfo_ExpectedAllSelectedPropertiesNotToBeDecoratedWithX0ButFollowingPropertiesAreX1X2Format,
                    typeof(TAttribute), Environment.NewLine, GetFormattedDescriptionsFor(propertiesWithAttribute));

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

        private static AlreadyFormattedString GetFormattedDescriptionsFor(IEnumerable<PropertyInfo> properties)
        {
            string[] descriptions = properties.Select(PropertyInfoAssertions.GetDescriptionFor).ToArray();
            return string.Join(Environment.NewLine, descriptions).ToAlreadyFormattedString();
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected string Context => "property info";
    }
}
