using System;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    public class Configuration
    {
        #region Private Definitions

        private readonly IConfigurationStore store;
        private string valueFormatterAssembly;
        private ValueFormatterDetectionMode? valueFormatterDetectionMode;
        private string testFrameworkName;
        private readonly object propertiesAccessLock = new object();

        #endregion

        /// <summary>
        /// Gets the active configuration,
        /// </summary>
        public static Configuration Current => Services.Configuration;

        public Configuration(IConfigurationStore store)
        {
            this.store = store;
        }

        /// <summary>
        /// Gets or sets the mode on how Fluent Assertions will find custom implementations of
        /// <see cref="IValueFormatter"/>.
        /// </summary>
        public ValueFormatterDetectionMode ValueFormatterDetectionMode
        {
            get
            {
                lock (propertiesAccessLock)
                {
                    if (!valueFormatterDetectionMode.HasValue)
                    {
                        valueFormatterDetectionMode = DetermineFormatterDetectionMode();
                    }

                    return valueFormatterDetectionMode.Value;
                }
            }

            set
            {
                valueFormatterDetectionMode = value;
            }
        }

        private ValueFormatterDetectionMode DetermineFormatterDetectionMode()
        {
            if (ValueFormatterAssembly != null)
            {
                return ValueFormatterDetectionMode.Specific;
            }

            string setting = store.GetSetting("valueFormatters");
            if (!string.IsNullOrEmpty(setting))
            {
                try
                {
                    return (ValueFormatterDetectionMode)Enum.Parse(typeof(ValueFormatterDetectionMode), setting, true);
                }
                catch (ArgumentException)
                {
                    throw new InvalidOperationException(string.Format(
                        "'{0}' is not a valid option for detecting value formatters. Valid options include Disabled, Specific and Scan.",
                        setting));
                }
            }

            return ValueFormatterDetectionMode.Disabled;
        }

        /// <summary>
        /// Gets or sets the assembly name to scan for custom value formatters in case <see cref="ValueFormatterDetectionMode"/>
        /// is set to <see cref="ValueFormatterDetectionMode.Specific"/>.
        /// </summary>
        public string ValueFormatterAssembly
        {
            get
            {
                if (valueFormatterAssembly is null)
                {
                    string assemblyName = store.GetSetting("valueFormattersAssembly");
                    if (!string.IsNullOrEmpty(assemblyName))
                    {
                        valueFormatterAssembly = assemblyName;
                    }
                }

                return valueFormatterAssembly;
            }

            set
            {
                lock (propertiesAccessLock)
                {
                    valueFormatterAssembly = value;
                    valueFormatterDetectionMode = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the test framework to use.
        /// </summary>
        /// <remarks>
        /// If no name is provided, Fluent Assertions
        /// will try to detect it by scanning the currently loaded assemblies. If it can't find a suitable provider,
        /// and the run-time platform supports it, it'll try to get it from the <see cref="IConfigurationStore"/>.
        /// </remarks>
        public string TestFrameworkName
        {
            get
            {
                if (string.IsNullOrEmpty(testFrameworkName))
                {
                    testFrameworkName = store.GetSetting("FluentAssertions.TestFramework");
                }

                return testFrameworkName;
            }
            set => testFrameworkName = value;
        }
    }
}
