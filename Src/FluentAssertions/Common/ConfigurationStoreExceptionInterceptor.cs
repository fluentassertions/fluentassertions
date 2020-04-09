#if NET47 || NETCOREAPP2_1 || NETCOREAPP3_0
namespace FluentAssertions.Common
{
    internal class ConfigurationStoreExceptionInterceptor : IConfigurationStore
    {
        private bool underlyingStoreUnavailable;

        private readonly IConfigurationStore configurationStore;

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
#endif
