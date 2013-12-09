using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    /// <summary>
    /// Specialized value formatter that looks for static methods in the caller's assembly marked with the 
    /// <see cref="ValueFormatterAttribute"/>.
    /// </summary>
    public class AttributeBasedFormatter : IValueFormatter
    {
        private MethodInfo[] formatters;

        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return (Services.Configuration.ValueFormatterDetection != ValueFormatterDetection.Disabled) && 
                (value != null) && (GetFormatter(value) != null);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="useLineBreaks"> </param>
        /// <param name="processedObjects">
        /// A collection of objects that 
        /// </param>
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, bool useLineBreaks, IList<object> processedObjects = null, int nestedPropertyLevel = 0)
        {
            MethodInfo method = GetFormatter(value);
            return (string)method.Invoke(null, new[]
            {
                value
            });
        }

        private MethodInfo GetFormatter(object value)
        {
            return Formatters.FirstOrDefault(m => m.GetParameters().Single().ParameterType == value.GetType());
        }

        public MethodInfo[] Formatters
        {
            get { return formatters ?? (formatters = FindCustomFormatters()); }
        }

        private MethodInfo[] FindCustomFormatters()
        {
            var query =
                from type in AllTypes
                where type != null
                from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                where method.IsStatic
                where method.HasAttribute<ValueFormatterAttribute>()
                where method.GetParameters().Count() == 1
                select method;

            return query.ToArray();
        }

#if SILVERLIGHT && !WINDOWS_PHONE
        private static IEnumerable<Type> AllTypes
        {
            get
            {
                return ((Assembly[])((dynamic)AppDomain.CurrentDomain).GetAssemblies())
                    .SelectMany(GetExportedTypes).ToArray();
            }
        }

        private static IEnumerable<Type> GetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types;
            }
            catch (NotSupportedException)
            {
                return new Type[0];
            }
        }
#elif WINDOWS_PHONE
        private static IEnumerable<Type> AllTypes
        {
            get { return AppDomain.CurrentDomain.GetAssemblies().SelectMany(GetExportedTypes).ToArray(); }
        }

        private static IEnumerable<Type> GetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return new Type[0];
            }
        }
#else
        private static IEnumerable<Type> AllTypes
        {
            get
            {
                return AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !IsDynamic(a))
                    .SelectMany(GetExportedTypes).ToArray();
            }
        }

        static bool IsDynamic(Assembly assembly)
        {
            return (assembly is System.Reflection.Emit.AssemblyBuilder) ||
                (assembly.GetType().FullName == "System.Reflection.Emit.InternalAssemblyBuilder");
        }

        private static IEnumerable<Type> GetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types;
            }
            catch (FileLoadException)
            {
                return new Type[0];
            }
            catch (Exception)
            {
                return new Type[0];
            }
        }
#endif

    }
}