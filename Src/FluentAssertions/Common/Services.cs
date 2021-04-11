using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Maintains the framework-specific services.
    /// </summary>
    public static class Services
    {
        private static readonly object Lockable = new();
        private static Configuration configuration;

        static Services()
        {
            ResetToDefaults();
        }

        public static IConfigurationStore ConfigurationStore { get; set; }

        public static Configuration Configuration
        {
            get
            {
                lock (Lockable)
                {
                    if (configuration is null)
                    {
                        configuration = new Configuration(ConfigurationStore);
                    }

                    return configuration;
                }
            }
        }

        public static Action<string> ThrowException { get; set; }

        public static IReflector Reflector { get; set; }

        public static void ResetToDefaults()
        {
            Reflector = new FullFrameworkReflector();
#if NETFRAMEWORK || NETCOREAPP
            ConfigurationStore = new ConfigurationStoreExceptionInterceptor(new AppSettingsConfigurationStore());
#else
            ConfigurationStore = new NullConfigurationStore();
#endif
            ThrowException = TestFrameworkProvider.Throw;
        }
    }
}
