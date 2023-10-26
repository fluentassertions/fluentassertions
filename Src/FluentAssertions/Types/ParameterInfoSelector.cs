using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Types;

/// <summary>
/// Allows for fluent selection of parameters of a method through reflection.
/// </summary>
public class ParameterInfoSelector : IEnumerable<ParameterInfo>
{
    private IEnumerable<ParameterInfo> selectedParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterInfoSelector"/> class.
    /// </summary>
    /// <param name="method">The method from which to select parameters.</param>
    /// <exception cref="ArgumentNullException"><paramref name="method"/> is <see langword="null"/>.</exception>
    public ParameterInfoSelector(MethodInfo method)
        : this(new[] { method })
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterInfoSelector"/> class.
    /// </summary>
    /// <param name="methods">The methods from which to select parameters.</param>
    /// <exception cref="ArgumentNullException"><paramref name="methods"/> is or contains <see langword="null"/>.</exception>
    public ParameterInfoSelector(IEnumerable<MethodInfo> methods)
    {
        Guard.ThrowIfArgumentIsNull(methods);
        Guard.ThrowIfArgumentContainsNull(methods);

        selectedParameters = methods.SelectMany(method => method.GetParameters());
    }

    /// <summary>
    /// Only select the parameters that are input parameters.
    /// </summary>
    public ParameterInfoSelector ThatAreInput
    {
        get
        {
            selectedParameters = selectedParameters.Where(parameter => parameter.IsIn);
            return this;
        }
    }

    /// <summary>
    /// Only select the parameters that are not input parameters.
    /// </summary>
    public ParameterInfoSelector ThatAreNotInput
    {
        get
        {
            selectedParameters = selectedParameters.Where(parameter => !parameter.IsIn);
            return this;
        }
    }

    /// <summary>
    /// Only select the parameters that are output parameters.
    /// </summary>
    public ParameterInfoSelector ThatAreOutput
    {
        get
        {
            selectedParameters = selectedParameters.Where(parameter => parameter.IsOut);
            return this;
        }
    }

    /// <summary>
    /// Only select the parameters that are not output parameters.
    /// </summary>
    public ParameterInfoSelector ThatAreNotOutput
    {
        get
        {
            selectedParameters = selectedParameters.Where(parameter => !parameter.IsOut);
            return this;
        }
    }

    /// <summary>
    /// Only select the parameters that have a default value
    /// </summary>
    public ParameterInfoSelector ThatHaveDefaultValue
    {
        get
        {
            selectedParameters = selectedParameters.Where(parameter => parameter.HasDefaultValue);
            return this;
        }
    }

    /// <summary>
    /// Only select the parameters that do not have a default value
    /// </summary>
    public ParameterInfoSelector ThatDoNotHaveDefaultValue
    {
        get
        {
            selectedParameters = selectedParameters.Where(parameter => !parameter.HasDefaultValue);
            return this;
        }
    }

    /// <summary>
    /// Only select the parameters that are decorated with an attribute of the specified type.
    /// </summary>
    public ParameterInfoSelector ThatAreDecoratedWith<TAttribute>()
        where TAttribute : Attribute
    {
        selectedParameters = selectedParameters.Where(parameter => parameter.IsDecoratedWith<TAttribute>());
        return this;
    }

    /// <summary>
    /// Only select the parameters that are decorated with, or inherits from a parent class, an attribute of the specified type.
    /// </summary>
    public ParameterInfoSelector ThatAreDecoratedWithOrInherit<TAttribute>()
        where TAttribute : Attribute
    {
        selectedParameters = selectedParameters.Where(parameter => parameter.IsDecoratedWithOrInherit<TAttribute>());
        return this;
    }

    /// <summary>
    /// Only select the parameters that are not decorated with an attribute of the specified type.
    /// </summary>
    public ParameterInfoSelector ThatAreNotDecoratedWith<TAttribute>()
        where TAttribute : Attribute
    {
        selectedParameters = selectedParameters.Where(parameter => !parameter.IsDecoratedWith<TAttribute>());
        return this;
    }

    /// <summary>
    /// Only select the parameters that are not decorated with and does not inherit from a parent class an attribute of the specified type.
    /// </summary>
    public ParameterInfoSelector ThatAreNotDecoratedWithOrInherit<TAttribute>()
        where TAttribute : Attribute
    {
        selectedParameters = selectedParameters.Where(parameter => !parameter.IsDecoratedWithOrInherit<TAttribute>());
        return this;
    }

    /// <summary>
    /// Only select the parameters that return the specified type
    /// </summary>
    public ParameterInfoSelector OfType<TReturn>()
    {
        selectedParameters = selectedParameters.Where(parameter => parameter.ParameterType == typeof(TReturn));
        return this;
    }

    /// <summary>
    /// Only select the parameters that do not return the specified type
    /// </summary>
    public ParameterInfoSelector NotOfType<TReturn>()
    {
        selectedParameters = selectedParameters.Where(parameter => parameter.ParameterType != typeof(TReturn));
        return this;
    }

    /// <summary>
    /// Select the types of the parameters
    /// </summary>
    public TypeSelector Types()
    {
        var returnTypes = selectedParameters.Select(parameter => parameter.ParameterType);

        return new TypeSelector(returnTypes);
    }

    /// <summary>
    /// The resulting <see cref="ParameterInfo"/> objects.
    /// </summary>
    public ParameterInfo[] ToArray()
    {
        return selectedParameters.ToArray();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Collections.Generic.IEnumerator{T}"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public IEnumerator<ParameterInfo> GetEnumerator()
    {
        return selectedParameters.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
