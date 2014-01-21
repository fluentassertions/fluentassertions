using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Common
{
    internal class WpReflector : IReflector
    {
        public IEnumerable<Type> GetAllTypesFromAppDomain(Func<Assembly, bool> predicate)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(predicate)
                .SelectMany(GetExportedTypes).ToArray();
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
    }
}