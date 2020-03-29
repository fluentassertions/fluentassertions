using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.CultureAwareTesting
{
    [XunitTestCaseDiscoverer("FluentAssertions.Specs.CultureAwareTesting.CulturedFactAttributeDiscoverer", "FluentAssertions.Specs")]
    public sealed class CulturedFactAttribute : FactAttribute
    {
        public CulturedFactAttribute(params string[] cultures) { }
    }
}
