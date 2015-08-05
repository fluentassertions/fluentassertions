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
        private Assembly assembly = null;

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
#if CORE_CLR
                // For CoreCLR, we need to attempt to load the assembly
                try
                {
                    assembly = Assembly.Load(new AssemblyName(AssemblyName) { Version = new Version(0,0,0,0)});
                    return assembly != null;
                }
                catch
                {
                    return false;
                }
#else
                string prefix = AssemblyName + ",";

#if !PORTABLE
                assembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a => a.FullName.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase));
#endif
                return (assembly != null);
#endif
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
            StorageFolder folder = await TryToGetFolderFromAssemblyLocation();
            if (folder == null)
            {
                folder = TryToGetFolderFromPackageLocation();
            }

            if (folder == null)
            {
                throw new Exception("Could not determine the current directory to detect the test framework");
            }

            IEnumerable<Assembly> assemblies =
                from file in await folder.GetFilesAsync().AsTask().ConfigureAwait(false)
                where (file.FileType == ".dll") || (file.FileType == ".exe")
                let assm = TryLoadAssembly(new AssemblyName()
                {
                    Name = file.DisplayName
                })
                where assm != null
                select assm;
            
            return assemblies.ToArray();
        }

        private static Assembly TryLoadAssembly(AssemblyName name)
        {
            try
            {
                return Assembly.Load(name);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private StorageFolder TryToGetFolderFromPackageLocation()
        {
            try
            {
                return Windows.ApplicationModel.Package.Current.InstalledLocation;
            }
            catch
            {
                return null;
            }
        }

        private static async Task<StorageFolder> TryToGetFolderFromAssemblyLocation()
        {
            try
            {
                string assemblyPath = ((dynamic)typeof(LateBoundTestFramework).GetTypeInfo().Assembly).Location;
                string folderPath = Path.GetDirectoryName(assemblyPath);
                
                return await StorageFolder.GetFolderFromPathAsync(folderPath);
            }
            catch
            {
                return null;
            }
        }
    }
#endif


}