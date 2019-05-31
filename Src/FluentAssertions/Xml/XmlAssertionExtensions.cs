#if !NETSTANDARD1_3 && !NETSTANDARD1_6

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
