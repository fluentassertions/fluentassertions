using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Allows for fluent selection of methods of a type through reflection.
    /// </summary>
    public class MethodInfoSelector : IEnumerable<MethodInfo>
    {
        private IEnumerable<MethodInfo> selectedMethods = new List<MethodInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfoSelector"/> class.
        /// </summary>
        /// <param name="type">The type from which to select methods.</param>
        public MethodInfoSelector(Type type)
            : this(new[] { type })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfoSelector"/> class.
        /// </summary>
        /// <param name="types">The types from which to select methods.</param>
        public MethodInfoSelector(IEnumerable<Type> types)
        {
            selectedMethods = types.SelectMany(t => t
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => !HasSpecialName(method)));
        }

        /// <summary>
        /// Only select the methods that are public or internal.
        /// </summary>
        public MethodInfoSelector ThatArePublicOrInternal
        {
            get
            {
                selectedMethods = selectedMethods.Where(method => method.IsPublic || method.IsAssembly);
                return this;
            }
        }

        /// <summary>
        /// Only select the methods without a return value
        /// </summary>
        public MethodInfoSelector ThatReturnVoid
        {
            get
            {
                selectedMethods = selectedMethods.Where(method => method.ReturnType == typeof(void));
                return this;
            }
        }

        /// <summary>
        /// Only select the methods with a return value
        /// </summary>
        public MethodInfoSelector ThatDoNotReturnVoid
        {
            get
            {
                selectedMethods = selectedMethods.Where(method => method.ReturnType != typeof(void));
                return this;
            }
        }

        /// <summary>
        /// Only select the methods that return the specified type
        /// </summary>
        public MethodInfoSelector ThatReturn<TReturn>()
        {
            selectedMethods = selectedMethods.Where(method => method.ReturnType == typeof(TReturn));
            return this;
        }

        /// <summary>
        /// Only select the methods that do not return the specified type
        /// </summary>
        public MethodInfoSelector ThatDoNotReturn<TReturn>()
        {
            selectedMethods = selectedMethods.Where(method => method.ReturnType != typeof(TReturn));
            return this;
        }

        /// <summary>
        /// Only select the methods that are decorated with an attribute of the specified type.
        /// </summary>
        public MethodInfoSelector ThatAreDecoratedWith<TAttribute>()
            where TAttribute : Attribute
        {
            selectedMethods = selectedMethods.Where(method => method.IsDecoratedWith<TAttribute>());
            return this;
        }

        /// <summary>
        /// Only select the methods that are decorated with, or inherits from a parent class, an attribute of the specified type.
        /// </summary>
        public MethodInfoSelector ThatAreDecoratedWithOrInherit<TAttribute>()
            where TAttribute : Attribute
        {
            selectedMethods = selectedMethods.Where(method => method.IsDecoratedWithOrInherit<TAttribute>());
            return this;
        }

        /// <summary>
        /// Only select the methods that are not decorated with an attribute of the specified type.
        /// </summary>
        public MethodInfoSelector ThatAreNotDecoratedWith<TAttribute>()
            where TAttribute : Attribute
        {
            selectedMethods = selectedMethods.Where(method => !method.IsDecoratedWith<TAttribute>());
            return this;
        }

        /// <summary>
        /// Only select the methods that are not decorated with and does not inherit from a parent class, an attribute of the specified type.
        /// </summary>
        public MethodInfoSelector ThatAreNotDecoratedWithOrInherit<TAttribute>()
            where TAttribute : Attribute
        {
            selectedMethods = selectedMethods.Where(method => !method.IsDecoratedWithOrInherit<TAttribute>());
            return this;
        }

        /// <summary>
        /// The resulting <see cref="MethodInfo"/> objects.
        /// </summary>
        public MethodInfo[] ToArray()
        {
            return selectedMethods.ToArray();
        }

        /// <summary>
        /// Determines whether the specified method has a special name (like properties and events).
        /// </summary>
        private bool HasSpecialName(MethodInfo method)
        {
            return (method.Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<MethodInfo> GetEnumerator()
        {
            return selectedMethods.GetEnumerator();
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
