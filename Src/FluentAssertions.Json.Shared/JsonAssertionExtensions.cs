using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace FluentAssertions.Json
{
    /// <summary>
    ///     Contains extension methods for JToken assertions.
    /// </summary>
    [DebuggerNonUserCode]
    public static class JsonAssertionExtensions
    {
        /// <summary>
        ///     Returns an <see cref="JTokenAssertions"/> object that can be used to assert the current <see cref="JToken"/>.
        /// </summary>
        public static JTokenAssertions Should(this JToken jToken)
        {
            return new JTokenAssertions(jToken);
        }
    }
}