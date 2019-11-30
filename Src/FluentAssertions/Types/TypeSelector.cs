using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Types
{
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

        public TypeSelector(IEnumerable<Type> types)
        {
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
            types = types.Where(type => type.GetTypeInfo().IsSubclassOf(typeof(TBase))).ToList();
            return this;
        }

        /// <summary>
        /// Determines whether a type is not a subclass of another type.
        /// </summary>
        public TypeSelector ThatDoNotDeriveFrom<TBase>()
        {
            types = types.Where(type => !type.GetTypeInfo().IsSubclassOf(typeof(TBase))).ToList();
            return this;
        }

        /// <summary>
        /// Determines whether a type implements an interface (but is not the interface itself).
        /// </summary>
        public TypeSelector ThatImplement<TInterface>()
        {
            types = types.Where(t =>
                typeof(TInterface)
                    .IsAssignableFrom(t) && (t != typeof(TInterface)
                        )).ToList();
            return this;
        }

        /// <summary>
        /// Determines whether a type does not implement an interface (but is not the interface itself).
        /// </summary>
        public TypeSelector ThatDoNotImplement<TInterface>()
        {
            types = types.Where(t =>
                !typeof(TInterface)
                    .IsAssignableFrom(t) && (t != typeof(TInterface)
                        )).ToList();
            return this;
        }

        /// <summary>
        /// Determines whether a type is decorated with a particular attribute.
        /// </summary>
        public TypeSelector ThatAreDecoratedWith<TAttribute>()
            where TAttribute : Attribute
        {
            types = types

                .Where(t => t.GetTypeInfo().IsDecoratedWith<TAttribute>())
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

                .Where(t => t.GetTypeInfo().IsDecoratedWithOrInherit<TAttribute>())
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

                .Where(t => !t.GetTypeInfo().IsDecoratedWith<TAttribute>())
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

                .Where(t => !t.GetTypeInfo().IsDecoratedWithOrInherit<TAttribute>())
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
}
