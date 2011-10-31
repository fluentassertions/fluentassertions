using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Allows for fluent selection of properties of a type through reflection.
    /// </summary>
    public class PropertyInfoSelector
    {
        private IEnumerable<PropertyInfo> selectedProperties = new List<PropertyInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfoSelector"/> class.
        /// </summary>
        /// <param name="type">The type from which to select properties.</param>
        public PropertyInfoSelector(Type type)
        {
            selectedProperties = type
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyInfoSelector"/> class.
        /// </summary>
        /// <param name="types">The types from which to select properties.</param>
        public PropertyInfoSelector(IEnumerable<Type> types)
        {
            selectedProperties = types.SelectMany(t => t
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
        }

        /// <summary>
        /// Only select the properties that have a public or internal getter.
        /// </summary>
        public PropertyInfoSelector ThatArePublicOrInternal
        {
            get
            {
                selectedProperties = selectedProperties.Where(property =>
                {
                    MethodInfo getter = property.GetGetMethod(true);

                    return ((getter != null) && (getter.IsPublic || getter.IsAssembly));
                });

                return this;
            }
        }

        /// <summary>
        /// Only select the properties that are decorated with an attribute of the specified type.
        /// </summary>
        public PropertyInfoSelector ThatAreDecoratedWith<TAttribute>()
        {
            selectedProperties = selectedProperties.Where(property => property.GetCustomAttributes(false).OfType<TAttribute>().Any());
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
        /// The resulting <see cref="PropertyInfo"/> objects.
        /// </summary>
        public PropertyInfo[] ToArray()
        {
            return selectedProperties.ToArray();
        }
    }
}