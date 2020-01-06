#if NETSTANDARD1_3 || NETSTANDARD1_6
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
