using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Allows for fluent selection of types from an assembly.
    /// </summary>
    public class TypeSelector
    {
        private IEnumerable<Type> selectedTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSelector"/> class.
        /// </summary>
        /// <param name="assembly">The assembly from which to select types.</param>
        public TypeSelector(Assembly assembly)
        {
            Subject = assembly;
            selectedTypes = assembly.GetTypes();
        }

        /// <summary>
        /// Gets the <see cref="Assembly"/> from which to select types.
        /// </summary>
        public Assembly Subject { get; private set; }

        /// <summary>
        /// The resulting <see cref="Type"/> objects.
        /// </summary>
        public Type[] ToArray()
        {
            return selectedTypes.ToArray();
        }

        /// <summary>
        /// Only select the types that derive from a class of the specified type.
        /// </summary>
        public TypeSelector DerivingFrom<TBase>()
        {
            selectedTypes = selectedTypes.Where(type => type.IsSubclassOf(typeof(TBase)));
            return this;
        }

        /// <summary>
        /// Only select the types that implement the specified interface.
        /// </summary>
        public TypeSelector Implementing<TInterface>()
        {
            selectedTypes = selectedTypes.Where(type => typeof(TInterface).IsAssignableFrom(type) && (type != typeof(TInterface)));
            return this;
        }

        /// <summary>
        /// Only select the types that are decorated with the specified attribute.
        /// </summary>
        public TypeSelector DecoratedWith<TAttribute>()
        {
            selectedTypes = selectedTypes.Where(type => (type.GetCustomAttributes(typeof(TAttribute), true).Length > 0));
            return this;
        }
    }
}