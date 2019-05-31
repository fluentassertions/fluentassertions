#if !NETSTANDARD1_3 && !NETSTANDARD1_6

using System.Configuration;

namespace FluentAssertions.Common
{
    internal class AppSettingsConfigurationStore : IConfigurationStore
    {
        public string GetSetting(string name)
        {
            string value = ConfigurationManager.AppSettings[name];
            return !string.IsNullOrEmpty(value) ? value : null;
        }
    }
}

#endif
