using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.CultureAwareTesting
{
    [XunitTestCaseDiscoverer("FluentAssertions.Specs.CultureAwareTesting.CulturedTheoryAttributeDiscoverer", "FluentAssertions.Specs")]
    public sealed class CulturedTheoryAttribute : TheoryAttribute
    {
        public CulturedTheoryAttribute(params string[] cultures) { }
    }
}
