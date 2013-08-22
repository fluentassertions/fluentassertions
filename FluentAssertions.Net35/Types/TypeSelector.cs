using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Allows for fluent filtering a list of types.
    /// </summary>
    public class TypeSelector : IEnumerable<Type>
    {
#if !WINRT
        private List<Type> types;
#else
        private List<TypeInfo> types;
#endif

        internal TypeSelector(
#if !WINRT
            IEnumerable<Type> types
#else
            IEnumerable<TypeInfo> types
#endif
            )
        {
            this.types = types.ToList();
        }


        /// <summary>
        /// The resulting <see cref="Type"/> objects.
        /// </summary>
        public Type[] ToArray()
        {
#if !WINRT            
            return types.ToArray();
#else
            return types.Select(t => t.AsType()).ToArray();
#endif
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
        /// Determines whether a type implements an interface (but is not the interface itself).
        /// </summary>
        public TypeSelector ThatImplement<TInterface>()
        {
            types = types.Where(t => 
#if !WINRT
                typeof (TInterface)
#else
                typeof(TInterface).GetTypeInfo()
#endif
                .IsAssignableFrom(t) && (t != typeof(TInterface)
#if WINRT
                .GetTypeInfo()
#endif       
                )).ToList();
            return this;
        }

        /// <summary>
        /// Determines whether a type is decorated with a particular attribute.
        /// </summary>
        public TypeSelector ThatAreDecoratedWith<TAttribute>()
        {
            types = types.Where(t => 
#if !WINRT
                t.GetCustomAttributes(typeof(TAttribute), true).Length > 0
#else
                t.GetCustomAttributes(typeof(TAttribute), true).Any()
#endif

                ).ToList();
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
        /// Determines whether the namespace of type is starts with <paramref name="namespace"/>.
        /// </summary>
        public TypeSelector ThatAreUnderNamespace(string @namespace)
        {
            types = types.Where(t => (t.Namespace != null) && t.Namespace.StartsWith(@namespace)).ToList();
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
#if !WINRT
            return types.GetEnumerator();
#else
            return types.Select(t => t.AsType()).AsEnumerable().GetEnumerator();
#endif
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