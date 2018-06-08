#if NET45 || NET47 || NETSTANDARD2_0 || NETCOREAPP2_0

using System.Diagnostics;
using System.Xml;
using FluentAssertions.Xml;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public static class XmlAssertionExtensions
    {
        public static XmlNodeAssertions Should(this XmlNode actualValue)
        {
            return new XmlNodeAssertions(actualValue);
        }

        public static XmlElementAssertions Should(this XmlElement actualValue)
        {
            return new XmlElementAssertions(actualValue);
        }
    }
}

#endif
