using System;
using System.Linq;
using System.Reflection;

#if WINRT
using System.Collections.Generic;
#endif

namespace FluentAssertions.Frameworks
{
    internal abstract class LateBoundTestFramework : ITestFramework
    {
        private Assembly assembly;

        public void Throw(string message)
        {
            Type exceptionType = assembly.GetType(ExceptionFullName);
            if (exceptionType == null)
            {
                throw new Exception(string.Format(
                    "Failed to create the assertion exception for the current test framework: \"{0}, {1}\"",
                    ExceptionFullName, assembly.FullName));
            }

            throw (Exception)Activator.CreateInstance(exceptionType, message);
        }

        public bool IsAvailable
        {
            get
            {
                assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == AssemblyName);
                return (assembly != null);
            }
        }

        protected abstract string AssemblyName { get; }
        protected abstract string ExceptionFullName { get; }

#if WINRT
        private sealed class AppDomain
        {
            public static AppDomain CurrentDomain { get; private set; }

            static AppDomain()
            {
                CurrentDomain = new AppDomain();
            }

            public Assembly[] GetAssemblies()
            {
                return GetAssemblyListAsync().Result.ToArray();
            }

            private async System.Threading.Tasks.Task<IEnumerable<Assembly>> GetAssemblyListAsync()
            {
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                
                List<Assembly> assemblies = new List<Assembly>();
                foreach (Windows.Storage.StorageFile file in await folder.GetFilesAsync())
                {
                    if (file.FileType == ".dll" || file.FileType == ".exe")
                    {
                        AssemblyName name = new AssemblyName() { Name = file.Name };
                        Assembly asm = Assembly.Load(name);
                        assemblies.Add(asm);
                    }
                }

                return assemblies;
            }
        }
#endif
    }
}