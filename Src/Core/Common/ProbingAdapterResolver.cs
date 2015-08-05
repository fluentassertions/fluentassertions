// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// http://pclcontrib.codeplex.com/
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FluentAssertions.Common
{
    /// <summary>
    /// An implementation of <see cref="IAdapterResolver"/> that probes for platforms-specific adapters by dynamically
    /// looking for concrete types in platform-specific assemblies.
    /// </summary>
    internal class ProbingAdapterResolver : IAdapterResolver
    {
        #region Private Definitions

#if NEW_REFLECTION
        private readonly Func<AssemblyName, Assembly> assemblyLoader;
#else
        private readonly Func<string, Assembly> assemblyLoader;
#endif
        private readonly object lockable = new object();
        private readonly Dictionary<Type, object> adapters = new Dictionary<Type, object>();
        private Assembly assembly;

#endregion

        public ProbingAdapterResolver() : this(Assembly.Load)
        {            
        }

#if NEW_REFLECTION
        public ProbingAdapterResolver(Func<AssemblyName, Assembly> assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;
        }
#else
        public ProbingAdapterResolver(Func<string, Assembly> assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;
        }
#endif

        public object Resolve(Type type)
        {
            lock (lockable)
            {
                object instance;
                if (!adapters.TryGetValue(type, out instance))
                {
                    Assembly assembly = GetPlatformSpecificAssembly();
                    instance = ResolveAdapter(assembly, type);
                    adapters.Add(type, instance);
                }

                return instance;
            }
        }

        private static object ResolveAdapter(Assembly assembly, Type interfaceType)
        {
            string typeName = MakeAdapterTypeName(interfaceType);

#if NEW_REFLECTION
            Type type = assembly.GetType(typeName, throwOnError: false, ignoreCase: false);
#else
            Type type = assembly.GetType(typeName, throwOnError: false);
#endif
            if (type != null)
            {
                return Activator.CreateInstance(type);
            }

            return type;
        }

        private static string MakeAdapterTypeName(Type interfaceType)
        {
            // For example, if we're looking for an implementation of System.Security.Cryptography.ICryptographyFactory, 
            // then we'll look for System.Security.Cryptography.CryptographyFactory
            return interfaceType.Namespace + "." + interfaceType.Name.Substring(1);
        }

        private Assembly GetPlatformSpecificAssembly()
        {
            if (assembly == null)
            {
                assembly = ProbeForPlatformSpecificAssembly();
                if (assembly == null)
                {
                    throw new InvalidOperationException("No platform-specific assembly detected");
                }
            }

            return assembly;
        }

        private Assembly ProbeForPlatformSpecificAssembly()
        {
#if NEW_REFLECTION
            var assemblyName = new AssemblyName(GetType().GetTypeInfo().Assembly.FullName) { Name = "FluentAssertions" };
#else
            var assemblyName = new AssemblyName(GetType().Assembly.FullName) { Name = "FluentAssertions" };
#endif

            try
            {
#if NEW_REFLECTION
                return assemblyLoader(assemblyName);
#else
                return assemblyLoader(assemblyName.FullName);
#endif
            }
            catch (FileNotFoundException)
            {
            }

            return null;
        }
    }
}