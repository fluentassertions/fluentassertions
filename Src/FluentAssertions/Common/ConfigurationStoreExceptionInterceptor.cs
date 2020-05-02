namespace FluentAssertions.Common
{
    internal class ConfigurationStoreExceptionInterceptor : IConfigurationStore
    {
        private readonly IConfigurationStore configurationStore;

        private bool underlyingStoreUnavailable;

        public ConfigurationStoreExceptionInterceptor(IConfigurationStore configurationStore)
        {
            this.configurationStore = configurationStore;
        }

        public string GetSetting(string name)
        {
            if (underlyingStoreUnavailable)
            {
                return null;
            }

            try
            {
                return configurationStore.GetSetting(name);
            }
            catch
            {
                underlyingStoreUnavailable = true;
                return null;
            }
        }
    }
}
