using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Types;

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains assertions for the <see cref="ParameterInfo"/> objects returned by the parent <see cref="ParameterInfoSelector"/>.
/// </summary>
[DebuggerNonUserCode]
public class ParameterInfoSelectorAssertions
{
    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public IEnumerable<ParameterInfo> SubjectParameters { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterInfoSelectorAssertions"/> class, for a number of <see cref="ParameterInfo"/> objects.
    /// </summary>
    /// <param name="parameters">The parameters to assert.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parameters"/> is <see langword="null"/>.</exception>
    public ParameterInfoSelectorAssertions(params ParameterInfo[] parameters)
    {
        Guard.ThrowIfArgumentIsNull(parameters);

        SubjectParameters = parameters;
    }

    /// <summary>
    /// Asserts that the selected parameters are decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<ParameterInfoSelectorAssertions> BeDecoratedWith<TAttribute>(string because = "",
        params object[] becauseArgs)
        where TAttribute : Attribute
    {
        ParameterInfo[] parametersWithoutAttribute = GetParametersWithout<TAttribute>();

        Execute.Assertion
            .ForCondition(parametersWithoutAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected all selected parameters to be decorated with {0}{reason}" +
                ", but the following parameters are not:" + Environment.NewLine +
                GetDescriptionsFor(parametersWithoutAttribute), typeof(TAttribute));

        return new AndConstraint<ParameterInfoSelectorAssertions>(this);
    }

    /// <summary>
    /// Asserts that the selected parameters are not decorated with the specified <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<ParameterInfoSelectorAssertions> NotBeDecoratedWith<TAttribute>(string because = "",
        params object[] becauseArgs)
        where TAttribute : Attribute
    {
        ParameterInfo[] parametersWithAttribute = GetParametersWith<TAttribute>();

        Execute.Assertion
            .ForCondition(parametersWithAttribute.Length == 0)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected all selected parameters not to be decorated with {0}{reason}" +
                ", but the following parameters are:" + Environment.NewLine +
                GetDescriptionsFor(parametersWithAttribute), typeof(TAttribute));

        return new AndConstraint<ParameterInfoSelectorAssertions>(this);
    }

    private ParameterInfo[] GetParametersWithout<TAttribute>()
        where TAttribute : Attribute
    {
        return SubjectParameters.Where(parameter => !parameter.IsDecoratedWith<TAttribute>()).ToArray();
    }

    private ParameterInfo[] GetParametersWith<TAttribute>()
        where TAttribute : Attribute
    {
        return SubjectParameters.Where(parameter => parameter.IsDecoratedWith<TAttribute>()).ToArray();
    }

    private static string GetDescriptionsFor(IEnumerable<ParameterInfo> parameters)
    {
        IEnumerable<string> descriptions = parameters.Select(parameter => ParameterInfoAssertions.GetDescriptionFor(parameter));

        return string.Join(Environment.NewLine, descriptions);
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
#pragma warning disable CA1822 // Do not change signature of a public member
    protected string Context => "parameter info";
#pragma warning restore CA1822

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
}
