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
        private static Action<string> throwException;

        private static IReflector reflector;
        private static IConfigurationStore configurationStore;

        static Services()
        {
            ResetToDefaults();
        }

        public static IConfigurationStore ConfigurationStore
        {
            get => configurationStore;
            set => configurationStore = value;
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
            get { return reflector; }
            set { reflector = value; }
        }

        public static void ResetToDefaults()
        {
#if NETSTANDARD1_3
            reflector = new NullReflector();
            configurationStore = new NullConfigurationStore();
#elif NETSTANDARD1_6
            reflector = new NetStandardReflector();
            configurationStore = new NullConfigurationStore();
#else
            reflector = new FullFrameworkReflector();
            configurationStore = new AppSettingsConfigurationStore();
#endif

            throwException = TestFrameworkProvider.Throw;
        }
    }
}
