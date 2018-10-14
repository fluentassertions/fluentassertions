#if NETSTANDARD1_6

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace FluentAssertions.Common
{
    internal class NetStandardReflector : IReflector
    {
        public IEnumerable<Type> GetAllTypesFromAppDomain(Func<Assembly, bool> predicate)
        {
            return DependencyContext.Default.RuntimeLibraries
                .Where(IsRelevant)
                .Select(TryLoad)
                .Where(a => (a != null) && predicate(a))
                .SelectMany(GetExportedTypes)
                .ToArray();
        }

        private bool IsRelevant(RuntimeLibrary arg)
        {
            string assemblyName = arg.Name.ToLower();

            return !assemblyName.StartsWith("microsoft.") &&
                   !assemblyName.StartsWith("xunit") &&
                   !assemblyName.StartsWith("system.") &&
                   !assemblyName.StartsWith("runtime.") &&
                   !assemblyName.StartsWith("netstandard") &&
                   !assemblyName.StartsWith("newtonsoft");
        }

        private static Assembly TryLoad(RuntimeLibrary lib)
        {
            try
            {
                return Assembly.Load(new AssemblyName(lib.Name));
            }
            catch (FileNotFoundException)
            {
                // Ignore
            }

            return null;
        }

        private static IEnumerable<Type> GetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly?.GetExportedTypes() ?? new Type[0];
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

#endif
