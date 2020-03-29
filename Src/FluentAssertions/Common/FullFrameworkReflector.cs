
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Common
{
    internal class FullFrameworkReflector : IReflector
    {
        public IEnumerable<Type> GetAllTypesFromAppDomain(Func<Assembly, bool> predicate)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !IsDynamic(a) && IsRelevant(a) && predicate(a))
                .SelectMany(GetExportedTypes).ToArray();
        }

        private bool IsRelevant(Assembly ass)
        {
            string assemblyName = ass.GetName().Name;

            return !assemblyName.StartsWith("microsoft.", StringComparison.OrdinalIgnoreCase) &&
                   !assemblyName.StartsWith("xunit", StringComparison.OrdinalIgnoreCase) &&
                   !assemblyName.StartsWith("jetbrains.", StringComparison.OrdinalIgnoreCase) &&
                   !assemblyName.StartsWith("system", StringComparison.OrdinalIgnoreCase) &&
                   !assemblyName.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase) &&
                   !assemblyName.StartsWith("newtonsoft", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsDynamic(Assembly assembly)
        {
            return (assembly.GetType().FullName == "System.Reflection.Emit.AssemblyBuilder") ||
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
    }
}

