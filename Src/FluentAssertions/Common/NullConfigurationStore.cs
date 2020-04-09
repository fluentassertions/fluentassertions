#if !(NET47 || NETCOREAPP2_1 || NETCOREAPP3_0)
namespace FluentAssertions.Common
{
    internal class NullConfigurationStore : IConfigurationStore
    {
        public string GetSetting(string name)
        {
            return "";
        }
    }
}
#endif
