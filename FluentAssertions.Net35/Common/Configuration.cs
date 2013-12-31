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

        #endregion

        /// <summary>
        /// Gets the active configuration,
        /// </summary>
        public static Configuration Current
        {
            get { return Services.Configuration; }
        }

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
                if (!valueFormatterDetectionMode.HasValue)
                {
                    valueFormatterDetectionMode = DetermineFormatterDetectionMode();
                }

                return valueFormatterDetectionMode.Value;
            }
            set { valueFormatterDetectionMode = value; }
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
                if (valueFormatterAssembly == null)
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
                valueFormatterAssembly = value;
                valueFormatterDetectionMode = null;
            }
        }
    }
}