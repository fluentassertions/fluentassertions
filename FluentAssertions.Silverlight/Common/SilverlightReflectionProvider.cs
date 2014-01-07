using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Common
{
    internal class SilverlightReflectionProvider : IReflectionProvider
    {
        public IEnumerable<Type> GetAllTypesFromAppDomain(Func<Assembly, bool> predicate)
        {
            return ((Assembly[])((dynamic)AppDomain.CurrentDomain).GetAssemblies())
                .Where(predicate)
                .SelectMany(GetExportedTypes).ToArray();
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
    }
}