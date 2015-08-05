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