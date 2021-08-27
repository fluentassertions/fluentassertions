namespace FluentAssertions.Common
{
    internal class NullConfigurationStore : IConfigurationStore
    {
        public string GetSetting(string name)
        {
            return string.Empty;
        }
    }
}
