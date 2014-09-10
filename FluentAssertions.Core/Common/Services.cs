using System;

using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Maintains the framework-specific services.
    /// </summary>
    public static class Services
    {
        #region Private Definitions

        private static bool initialized = false;
        private static readonly object lockable = new object();
        private static Configuration configuration;
        private static Action<string> throwException;
        private static IReflector reflector;

        #endregion

        public static void Initialize()
        {
            if (!initialized)
            {
                lock (lockable)
                {
                    if (!initialized)
                    {
                        var platform = PlatformAdapter.Resolve<IProvidePlatformServices>();
                        Configuration = platform.Configuration ?? new Configuration(new NullConfigurationStore());
                        Reflector = platform.Reflector ?? new NullReflector();
                        ThrowException = platform.Throw;
                        Formatter.AddPlatformFormatters(platform.Formatters);

                        initialized = true;
                    }
                }
            }
        }

        public static Configuration Configuration
        {
            get
            {
                Initialize();
                return configuration;
            }
            set { configuration = value; }
        }

        public static Action<string> ThrowException
        {
            get
            {
                Initialize();
                return throwException;
            }
            set { throwException = value; }
        }

        public static IReflector Reflector
        {
            get
            {
                Initialize();
                return reflector;
            }
            set { reflector = value; }
        }

        public static void ResetToDefaults()
        {
            initialized = false;
            Initialize();
        }
    }
}