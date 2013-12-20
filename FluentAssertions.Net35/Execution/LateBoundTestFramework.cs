using System;
using System.IO;
using System.Linq;
using System.Reflection;

#if WINRT

using Windows.Storage;
using System.Threading.Tasks;
using System.Collections.Generic;

#endif

namespace FluentAssertions.Execution
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
                string prefix = AssemblyName + ",";

                assembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a => a.FullName.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase));

                return (assembly != null);
            }
        }

        protected abstract string AssemblyName { get; }
        protected abstract string ExceptionFullName { get; }
    }

#if WINRT
    /// <summary>
    /// Simulates the AppDomain class that is not available in Windows Store apps.
    /// </summary>
    internal sealed class AppDomain
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

        private async Task<IEnumerable<Assembly>> GetAssemblyListAsync()
        {
            string assemblyPath = ((dynamic)typeof(LateBoundTestFramework).GetTypeInfo().Assembly).Location;
            string folderPath = Path.GetDirectoryName(assemblyPath);

            var folder = await StorageFolder.GetFolderFromPathAsync(folderPath);

            IEnumerable<Assembly> assemblies = 
                from file in await folder.GetFilesAsync() 
                where (file.FileType == ".dll") || (file.FileType == ".exe") 
                select Assembly.Load(new AssemblyName() { Name = file.DisplayName });
            
            return assemblies.ToArray();
        }
    }
#endif
}