using System.Diagnostics;
using System.Xml;
using FluentAssertions.Execution;
using FluentAssertions.Xml;

namespace FluentAssertions;

[DebuggerNonUserCode]
public static class XmlAssertionExtensions
{
    public static XmlNodeAssertions Should(this XmlNode actualValue)
    {
        return new XmlNodeAssertions(actualValue, AssertionChain.GetOrCreate());
    }

    public static XmlElementAssertions Should(this XmlElement actualValue)
    {
        return new XmlElementAssertions(actualValue, AssertionChain.GetOrCreate());
    }
}
