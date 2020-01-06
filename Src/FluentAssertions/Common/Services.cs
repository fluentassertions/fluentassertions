using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Maintains the framework-specific services.
    /// </summary>
    public static class Services
    {
        private static readonly object lockable = new object();
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
                lock (lockable)
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
#if NETSTANDARD1_3
            Reflector = new NullReflector();
            ConfigurationStore = new NullConfigurationStore();
#elif NETSTANDARD1_6
            Reflector = new NetStandardReflector();
            ConfigurationStore = new NullConfigurationStore();
#else
            Reflector = new FullFrameworkReflector();
            ConfigurationStore = new ConfigurationStoreExceptionInterceptor(new AppSettingsConfigurationStore());
#endif

            ThrowException = TestFrameworkProvider.Throw;
        }
    }
}
