using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Types;

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains assertions for the <see cref="PropertyInfo"/> objects returned by the parent <see cref="PropertyInfoSelector"/>.
/// </summary>
[DebuggerNonUserCode]
public class PropertyInfoSelectorAssertions
{
    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public IEnumerable<PropertyInfo> SubjectProperties { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyInfoSelectorAssertions"/> class, for a number of <see cref="PropertyInfo"/> objects.
    /// </summary>
    /// <param name="properties">The properties to assert.</param>
    /// <exception cref="ArgumentNullException"><paramref name="properties"/> is <see langword="null"/>.</exception>
    public PropertyInfoSelectorAssertions(params PropertyInfo[] properties)
    {
        Guard.ThrowIfArgumentIsNull(properties);

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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoSelectorAssertions> BeVirtual(string because = "", params object[] becauseArgs)
    {
        PropertyInfo[] nonVirtualProperties = GetAllNonVirtualPropertiesFromSelection();

        Execute.Assertion
            .ForCondition(nonVirtualProperties.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected all selected properties to be virtual{reason}, but the following properties are not virtual:" +
                Environment.NewLine + GetDescriptionsFor(nonVirtualProperties));

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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoSelectorAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
    {
        PropertyInfo[] virtualProperties = GetAllVirtualPropertiesFromSelection();

        Execute.Assertion
            .ForCondition(virtualProperties.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected all selected properties not to be virtual{reason}, but the following properties are virtual:" +
                Environment.NewLine + GetDescriptionsFor(virtualProperties));

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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoSelectorAssertions> BeWritable(string because = "", params object[] becauseArgs)
    {
        PropertyInfo[] readOnlyProperties = GetAllReadOnlyPropertiesFromSelection();

        Execute.Assertion
            .ForCondition(readOnlyProperties.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected all selected properties to have a setter{reason}, but the following properties do not:" +
                Environment.NewLine + GetDescriptionsFor(readOnlyProperties));

        return new AndConstraint<PropertyInfoSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected properties do not have a setter.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoSelectorAssertions> NotBeWritable(string because = "", params object[] becauseArgs)
    {
        PropertyInfo[] writableProperties = GetAllWritablePropertiesFromSelection();

        Execute.Assertion
            .ForCondition(writableProperties.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected selected properties to not have a setter{reason}, but the following properties do:" +
                Environment.NewLine + GetDescriptionsFor(writableProperties));

        return new AndConstraint<PropertyInfoSelectorAssertions>(this);
    }

    private PropertyInfo[] GetAllReadOnlyPropertiesFromSelection()
    {
        return SubjectProperties.Where(property => !property.CanWrite).ToArray();
    }

    private PropertyInfo[] GetAllWritablePropertiesFromSelection()
    {
        return SubjectProperties.Where(property => property.CanWrite).ToArray();
    }

    private PropertyInfo[] GetAllNonVirtualPropertiesFromSelection()
    {
        return SubjectProperties.Where(property => !property.IsVirtual()).ToArray();
    }

    private PropertyInfo[] GetAllVirtualPropertiesFromSelection()
    {
        return SubjectProperties.Where(property => property.IsVirtual()).ToArray();
    }

    /// <summary>
    /// Asserts that the selected properties are decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoSelectorAssertions> BeDecoratedWith<TAttribute>(string because = "",
        params object[] becauseArgs)
        where TAttribute : Attribute
    {
        PropertyInfo[] propertiesWithoutAttribute = GetPropertiesWithout<TAttribute>();

        Execute.Assertion
            .ForCondition(propertiesWithoutAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected all selected properties to be decorated with {0}{reason}" +
                ", but the following properties are not:" + Environment.NewLine +
                GetDescriptionsFor(propertiesWithoutAttribute), typeof(TAttribute));

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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<PropertyInfoSelectorAssertions> NotBeDecoratedWith<TAttribute>(string because = "",
        params object[] becauseArgs)
        where TAttribute : Attribute
    {
        PropertyInfo[] propertiesWithAttribute = GetPropertiesWith<TAttribute>();

        Execute.Assertion
            .ForCondition(propertiesWithAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected all selected properties not to be decorated with {0}{reason}" +
                ", but the following properties are:" + Environment.NewLine +
                GetDescriptionsFor(propertiesWithAttribute), typeof(TAttribute));

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
        IEnumerable<string> descriptions = properties.Select(property => PropertyInfoAssertions.GetDescriptionFor(property));

        return string.Join(Environment.NewLine, descriptions);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
#pragma warning disable CA1822 // Do not change signature of a public member
    protected string Context => "property info";
#pragma warning restore CA1822

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
}
