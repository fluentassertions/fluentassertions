using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FluentAssertions.Xml;

namespace FluentAssertions;

[DebuggerNonUserCode]
public static class XmlAssertionExtensions
{
    public static XmlNodeAssertions Should([NotNull] this XmlNode actualValue)
    {
        return new XmlNodeAssertions(actualValue);
    }

    public static XmlElementAssertions Should([NotNull] this XmlElement actualValue)
    {
        return new XmlElementAssertions(actualValue);
    }
}
