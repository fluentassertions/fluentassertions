using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Types;

/// <summary>
/// Allows for fluent selection of properties of a type through reflection.
/// </summary>
public class PropertyInfoSelector : IEnumerable<PropertyInfo>
{
    private IEnumerable<PropertyInfo> selectedProperties;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyInfoSelector"/> class.
    /// </summary>
    /// <param name="type">The type from which to select properties.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public PropertyInfoSelector(Type type)
        : this(new[] { type })
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyInfoSelector"/> class.
    /// </summary>
    /// <param name="types">The types from which to select properties.</param>
    /// <exception cref="ArgumentNullException"><paramref name="types"/> is or contains <see langword="null"/>.</exception>
    public PropertyInfoSelector(IEnumerable<Type> types)
    {
        Guard.ThrowIfArgumentIsNull(types);
        Guard.ThrowIfArgumentContainsNull(types);

        selectedProperties = types.SelectMany(t => t
            .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
    }

    /// <summary>
    /// Only select the properties that have at least one public or internal accessor
    /// </summary>
    public PropertyInfoSelector ThatArePublicOrInternal
    {
        get
        {
            selectedProperties = selectedProperties.Where(property =>
            {
                return property.GetGetMethod(nonPublic: true) is { IsPublic: true } or { IsAssembly: true }
                    || property.GetSetMethod(nonPublic: true) is { IsPublic: true } or { IsAssembly: true };
            });

            return this;
        }
    }

    /// <summary>
    /// Only select the properties that are abstract
    /// </summary>
    public PropertyInfoSelector ThatAreAbstract
    {
        get
        {
            selectedProperties = selectedProperties.Where(property => property.IsAbstract());
            return this;
        }
    }

    /// <summary>
    /// Only select the properties that are not abstract
    /// </summary>
    public PropertyInfoSelector ThatAreNotAbstract
    {
        get
        {
            selectedProperties = selectedProperties.Where(property => !property.IsAbstract());
            return this;
        }
    }

    /// <summary>
    /// Only select the properties that are static
    /// </summary>
    public PropertyInfoSelector ThatAreStatic
    {
        get
        {
            selectedProperties = selectedProperties.Where(property => property.IsStatic());
            return this;
        }
    }

    /// <summary>
    /// Only select the properties that are not static
    /// </summary>
    public PropertyInfoSelector ThatAreNotStatic
    {
        get
        {
            selectedProperties = selectedProperties.Where(property => !property.IsStatic());
            return this;
        }
    }

    /// <summary>
    /// Only select the properties that are virtual
    /// </summary>
    public PropertyInfoSelector ThatAreVirtual
    {
        get
        {
            selectedProperties = selectedProperties.Where(property => property.IsVirtual());
            return this;
        }
    }

    /// <summary>
    /// Only select the properties that are not virtual
    /// </summary>
    public PropertyInfoSelector ThatAreNotVirtual
    {
        get
        {
            selectedProperties = selectedProperties.Where(property => !property.IsVirtual());
            return this;
        }
    }

    /// <summary>
    /// Only select the properties that are decorated with an attribute of the specified type.
    /// </summary>
    public PropertyInfoSelector ThatAreDecoratedWith<TAttribute>()
        where TAttribute : Attribute
    {
        selectedProperties = selectedProperties.Where(property => property.IsDecoratedWith<TAttribute>());
        return this;
    }

    /// <summary>
    /// Only select the properties that are decorated with, or inherits from a parent class, an attribute of the specified type.
    /// </summary>
    public PropertyInfoSelector ThatAreDecoratedWithOrInherit<TAttribute>()
        where TAttribute : Attribute
    {
        selectedProperties = selectedProperties.Where(property => property.IsDecoratedWithOrInherit<TAttribute>());
        return this;
    }

    /// <summary>
    /// Only select the properties that are not decorated with an attribute of the specified type.
    /// </summary>
    public PropertyInfoSelector ThatAreNotDecoratedWith<TAttribute>()
        where TAttribute : Attribute
    {
        selectedProperties = selectedProperties.Where(property => !property.IsDecoratedWith<TAttribute>());
        return this;
    }

    /// <summary>
    /// Only select the properties that are not decorated with and does not inherit from a parent class an attribute of the specified type.
    /// </summary>
    public PropertyInfoSelector ThatAreNotDecoratedWithOrInherit<TAttribute>()
        where TAttribute : Attribute
    {
        selectedProperties = selectedProperties.Where(property => !property.IsDecoratedWithOrInherit<TAttribute>());
        return this;
    }

    /// <summary>
    /// Only select the properties that return the specified type
    /// </summary>
    public PropertyInfoSelector OfType<TReturn>()
    {
        selectedProperties = selectedProperties.Where(property => property.PropertyType == typeof(TReturn));
        return this;
    }

    /// <summary>
    /// Only select the properties that do not return the specified type
    /// </summary>
    public PropertyInfoSelector NotOfType<TReturn>()
    {
        selectedProperties = selectedProperties.Where(property => property.PropertyType != typeof(TReturn));
        return this;
    }

    /// <summary>
    /// Select return types of the properties
    /// </summary>
    public TypeSelector ReturnTypes()
    {
        var returnTypes = selectedProperties.Select(property => property.PropertyType);

        return new TypeSelector(returnTypes);
    }

    /// <summary>
    /// The resulting <see cref="PropertyInfo"/> objects.
    /// </summary>
    public PropertyInfo[] ToArray()
    {
        return selectedProperties.ToArray();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Collections.Generic.IEnumerator{T}"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public IEnumerator<PropertyInfo> GetEnumerator()
    {
        return selectedProperties.GetEnumerator();
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
