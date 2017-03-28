using FluentAssertions.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
