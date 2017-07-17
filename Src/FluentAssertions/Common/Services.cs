using System;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Maintains the framework-specific services.
    /// </summary>
    public static class Services
    {
        private static readonly object lockable = new object();
        private static Configuration configuration;
        private static Action<string> throwException;

        private static IReflector reflector;
        private static IConfigurationStore configurationStore;

        static Services()
        {
            ResetToDefaults();
        }

        public static IConfigurationStore ConfigurationStore
        {
            get
            {
                return configurationStore;
            }
            set { configurationStore = value; }
        }

        public static Configuration Configuration
        {
            get
            {
                lock (lockable)
                {
                    if (configuration == null)
                    {
                        configuration = new Configuration(ConfigurationStore);
                    }

                    return configuration;
                }
            }
        }

        public static Action<string> ThrowException
        {
            get
            {
                return throwException;
            }
            set
            {
                    throwException = value;
            }
        }

        public static IReflector Reflector
        {
            get
            {
                return reflector;
            }
            set { reflector = value; }
        }

        public static void ResetToDefaults()
        {
#if NET45
            reflector = new FullFrameworkReflector();
            configurationStore = new AppSettingsConfigurationStore();
#else
            reflector = new NullReflector();
            configurationStore = new NullConfigurationStore();
#endif


            throwException = TestFrameworkProvider.Throw;
        }
    }
}