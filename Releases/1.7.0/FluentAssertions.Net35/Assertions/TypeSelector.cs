using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Allows for fluent filtering a list of types.
    /// </summary>
    public class TypeSelector : IEnumerable<Type>
    {
        private Type[] types;

        internal TypeSelector(IEnumerable<Type> types)
        {
            this.types = types.ToArray();
        }

        /// <summary>
        /// Determines whether a type is a subclass of another type, but NOT the same type.
        /// </summary>
        public TypeSelector ThatDeriveFrom<TBase>()
        {
            types = types.Where(type => type.IsSubclassOf(typeof(TBase))).ToArray();
            return this;
        }
        
        /// <summary>
        /// Determines whether a type implements an interface (but is not the interface itself).
        /// </summary>
        public TypeSelector ThatImplement<TInterface>()
        {
            types = types.Where(t => typeof (TInterface).IsAssignableFrom(t) && (t != typeof (TInterface))).ToArray();
            return this;
        }

        /// <summary>
        /// Determines whether a type is decorated with a particular attribute.
        /// </summary>
        public TypeSelector ThatAreDecoratedWith<TAttribute>()
        {
            types = types.Where(t => t.GetCustomAttributes(typeof(TAttribute), true).Length > 0).ToArray();
            return this;
        }

        /// <summary>
        /// Determines whether the namespace of type is exactly <paramref name="namespace"/>.
        /// </summary>
        public TypeSelector ThatAreInNamespace(string @namespace)
        {
            types = types.Where(t => t.Namespace == @namespace).ToArray();
            return this;
        }

        /// <summary>
        /// Determines whether the namespace of type is starts with <paramref name="namespace"/>.
        /// </summary>
        public TypeSelector ThatAreUnderNamespace(string @namespace)
        {
            types = types.Where(t => (t.Namespace != null) && t.Namespace.StartsWith(@namespace)).ToArray();
            return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Type> GetEnumerator()
        {
            return types.Cast<Type>().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}