using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAssertions.Common
{
    internal static class Services
    {
        static Services()
        {
            Configuration = new Configuration(new DefaultConfigurationStore());
        }

        public static Configuration Configuration { get; set; }
    }

    public class DefaultConfigurationStore : IConfigurationStore
    {
        public string GetSetting(string name)
        {
            return "";
        }
    }
}
