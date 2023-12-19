using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Types;

/// <summary>
/// Allows for fluent filtering a list of types.
/// </summary>
public class TypeSelector : IEnumerable<Type>
{
    private List<Type> types;

    public TypeSelector(Type type)
        : this(new[] { type })
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeSelector"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="types"/> is or contains <see langword="null"/>.</exception>
    public TypeSelector(IEnumerable<Type> types)
    {
        Guard.ThrowIfArgumentIsNull(types);
        Guard.ThrowIfArgumentContainsNull(types);

        this.types = types.ToList();
    }

    /// <summary>
    /// The resulting <see cref="System.Type"/> objects.
    /// </summary>
    public Type[] ToArray()
    {
        return types.ToArray();
    }

    /// <summary>
    /// Determines whether a type is a subclass of another type, but NOT the same type.
    /// </summary>
    public TypeSelector ThatDeriveFrom<TBase>()
    {
        types = types.Where(type => type.IsSubclassOf(typeof(TBase))).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether a type is not a subclass of another type.
    /// </summary>
    public TypeSelector ThatDoNotDeriveFrom<TBase>()
    {
        types = types.Where(type => !type.IsSubclassOf(typeof(TBase))).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether a type implements an interface (but is not the interface itself).
    /// </summary>
    public TypeSelector ThatImplement<TInterface>()
    {
        types = types
            .Where(t => typeof(TInterface).IsAssignableFrom(t) && t != typeof(TInterface))
            .ToList();

        return this;
    }

    /// <summary>
    /// Determines whether a type does not implement an interface (but is not the interface itself).
    /// </summary>
    public TypeSelector ThatDoNotImplement<TInterface>()
    {
        types = types
            .Where(t => !typeof(TInterface).IsAssignableFrom(t) && t != typeof(TInterface))
            .ToList();

        return this;
    }

    /// <summary>
    /// Determines whether a type is decorated with a particular attribute.
    /// </summary>
    public TypeSelector ThatAreDecoratedWith<TAttribute>()
        where TAttribute : Attribute
    {
        types = types
            .Where(t => t.IsDecoratedWith<TAttribute>())
            .ToList();

        return this;
    }

    /// <summary>
    /// Determines whether a type is decorated with, or inherits from a parent class, a particular attribute.
    /// </summary>
    public TypeSelector ThatAreDecoratedWithOrInherit<TAttribute>()
        where TAttribute : Attribute
    {
        types = types
            .Where(t => t.IsDecoratedWithOrInherit<TAttribute>())
            .ToList();

        return this;
    }

    /// <summary>
    /// Determines whether a type is not decorated with a particular attribute.
    /// </summary>
    public TypeSelector ThatAreNotDecoratedWith<TAttribute>()
        where TAttribute : Attribute
    {
        types = types
            .Where(t => !t.IsDecoratedWith<TAttribute>())
            .ToList();

        return this;
    }

    /// <summary>
    /// Determines whether a type is not decorated with and does not inherit from a parent class, a particular attribute.
    /// </summary>
    public TypeSelector ThatAreNotDecoratedWithOrInherit<TAttribute>()
        where TAttribute : Attribute
    {
        types = types
            .Where(t => !t.IsDecoratedWithOrInherit<TAttribute>())
            .ToList();

        return this;
    }

    /// <summary>
    /// Determines whether the namespace of type is exactly <paramref name="namespace"/>.
    /// </summary>
    public TypeSelector ThatAreInNamespace(string @namespace)
    {
        types = types.Where(t => t.Namespace == @namespace).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether the namespace of type is exactly not <paramref name="namespace"/>.
    /// </summary>
    public TypeSelector ThatAreNotInNamespace(string @namespace)
    {
        types = types.Where(t => t.Namespace != @namespace).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether the namespace of type starts with <paramref name="namespace"/>.
    /// </summary>
    public TypeSelector ThatAreUnderNamespace(string @namespace)
    {
        types = types.Where(t => t.IsUnderNamespace(@namespace)).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether the namespace of type does not start with <paramref name="namespace"/>.
    /// </summary>
    public TypeSelector ThatAreNotUnderNamespace(string @namespace)
    {
        types = types.Where(t => !t.IsUnderNamespace(@namespace)).ToList();
        return this;
    }

    /// <summary>
    /// Filters and returns the types that are value types
    /// </summary>
    public TypeSelector ThatAreValueTypes()
    {
        types = types.Where(t => t.IsValueType).ToList();
        return this;
    }

    /// <summary>
    /// Filters and returns the types that are not value types
    /// </summary>
    public TypeSelector ThatAreNotValueTypes()
    {
        types = types.Where(t => !t.IsValueType).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether the type is a class
    /// </summary>
    public TypeSelector ThatAreClasses()
    {
        types = types.Where(t => t.IsClass).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether the type is not a class
    /// </summary>
    public TypeSelector ThatAreNotClasses()
    {
        types = types.Where(t => !t.IsClass).ToList();
        return this;
    }

    /// <summary>
    /// Filters and returns the types that are abstract
    /// </summary>
    public TypeSelector ThatAreAbstract()
    {
        types = types.Where(t => t.IsCSharpAbstract()).ToList();
        return this;
    }

    /// <summary>
    /// Filters and returns the types that are not abstract
    /// </summary>
    public TypeSelector ThatAreNotAbstract()
    {
        types = types.Where(t => !t.IsCSharpAbstract()).ToList();
        return this;
    }

    /// <summary>
    /// Filters and returns the types that are sealed
    /// </summary>
    public TypeSelector ThatAreSealed()
    {
        types = types.Where(t => t.IsSealed).ToList();
        return this;
    }

    /// <summary>
    /// Filters and returns the types that are not sealed
    /// </summary>
    public TypeSelector ThatAreNotSealed()
    {
        types = types.Where(t => !t.IsSealed).ToList();
        return this;
    }

    /// <summary>
    /// Filters and returns only the types that are interfaces
    /// </summary>
    public TypeSelector ThatAreInterfaces()
    {
        types = types.Where(t => t.IsInterface).ToList();
        return this;
    }

    /// <summary>
    /// Filters and returns only the types that are not interfaces
    /// </summary>
    public TypeSelector ThatAreNotInterfaces()
    {
        types = types.Where(t => !t.IsInterface).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether the type is static
    /// </summary>
    public TypeSelector ThatAreStatic()
    {
        types = types.Where(t => t.IsCSharpStatic()).ToList();
        return this;
    }

    /// <summary>
    /// Determines whether the type is not static
    /// </summary>
    public TypeSelector ThatAreNotStatic()
    {
        types = types.Where(t => !t.IsCSharpStatic()).ToList();
        return this;
    }

    /// <summary>
    /// Allows to filter the types with the <paramref name="predicate"/> passed
    /// </summary>
    public TypeSelector ThatSatisfy(Func<Type, bool> predicate)
    {
        types = types.Where(predicate).ToList();
        return this;
    }

    /// <summary>
    /// Returns T for the types which are <see cref="Task{T}"/> or <see cref="ValueTask{T}"/>; the type itself otherwise
    /// </summary>
    public TypeSelector UnwrapTaskTypes()
    {
        types = types.ConvertAll(type =>
        {
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Task<>) || genericTypeDefinition == typeof(ValueTask<>))
                {
                    return type.GetGenericArguments().Single();
                }
            }

            return type == typeof(Task) || type == typeof(ValueTask) ? typeof(void) : type;
        });

        return this;
    }

    /// <summary>
    /// Returns T for the types which are <see cref="IEnumerable{T}"/> or implement the <see cref="IEnumerable{T}"/>; the type itself otherwise
    /// </summary>
    public TypeSelector UnwrapEnumerableTypes()
    {
        var unwrappedTypes = new List<Type>();

        foreach (Type type in types)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                unwrappedTypes.Add(type.GetGenericArguments().Single());
            }
            else
            {
                var iEnumerableImplementations = type
                    .GetInterfaces()
                    .Where(iType => iType.IsGenericType && iType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(ied => ied.GetGenericArguments().Single())
                    .ToList();

                if (iEnumerableImplementations.Count > 0)
                {
                    unwrappedTypes.AddRange(iEnumerableImplementations);
                }
                else
                {
                    unwrappedTypes.Add(type);
                }
            }
        }

        types = unwrappedTypes;
        return this;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Collections.Generic.IEnumerator{T}"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public IEnumerator<Type> GetEnumerator()
    {
        return types.GetEnumerator();
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
