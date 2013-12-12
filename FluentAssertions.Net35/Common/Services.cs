namespace FluentAssertions.Common
{
    /// <summary>
    /// Maintains the framework-specific services.
    /// </summary>
    internal static class Services
    {
        static Services()
        {
            Configuration = new Configuration(new NullConfigurationStore());
        }

        public static Configuration Configuration { get; set; }
    }

    internal class NullConfigurationStore : IConfigurationStore
    {
        public string GetSetting(string name)
        {
            return "";
        }
    }
}