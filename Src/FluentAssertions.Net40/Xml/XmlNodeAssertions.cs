using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace FluentAssertions.Xml
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="XmlNode"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class XmlNodeAssertions : XmlNodeAssertions<XmlNode, XmlNodeAssertions>
    {
        public XmlNodeAssertions(XmlNode xmlNode)
            : base(xmlNode)
        { }
    }
}
