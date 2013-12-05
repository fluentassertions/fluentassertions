using System;

namespace FluentAssertions.Common
{
    internal class Configuration
    {
        private readonly IConfigurationStore store;
        private string valueFormatterAssembly;

        public Configuration(IConfigurationStore store)
        {
            this.store = store;
        }

        public ValueFormatterDetection ValueFormatterDetection
        {
            get
            {
                if (ValueFormatterAssembly != null)
                {
                    return ValueFormatterDetection.Specific;
                }
                else
                {
                    string setting = store.GetSetting("valueFormatters");
                    if (!string.IsNullOrEmpty(setting))
                    {
                        try
                        {
                            return (ValueFormatterDetection)Enum.Parse(typeof(ValueFormatterDetection), setting, true);
                        }
                        catch (ArgumentException)
                        {
                            throw new InvalidOperationException(string.Format(
                                "'{0}' is not a valid option for detecting value formatters. Valid options include Disabled, Specific and Scan.",
                                setting));
                        }
                    }
                }

                return ValueFormatterDetection.Disabled;
            }
        }

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
        }
    }

    internal enum ValueFormatterDetection
    {
        Disabled,
        Specific,
        Scan,
    }
}