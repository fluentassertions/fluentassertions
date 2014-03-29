using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace FluentAssertions.Common
{
    internal class DefaultReflector : IReflector
    {
        public IEnumerable<Type> GetAllTypesFromAppDomain(Func<Assembly, bool> predicate)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => predicate(a) && !IsDynamic(a))
                .SelectMany(GetExportedTypes).ToArray();
        }

        private static bool IsDynamic(Assembly assembly)
        {
#if __IOS__
            return false; // iOS does not support dynamic assemblies
#else
            return (assembly is AssemblyBuilder) ||
                   (assembly.GetType().FullName == "System.Reflection.Emit.InternalAssemblyBuilder");
#endif
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
    }
}