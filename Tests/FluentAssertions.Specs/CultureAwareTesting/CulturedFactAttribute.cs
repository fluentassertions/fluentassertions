using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.CultureAwareTesting;

[XunitTestCaseDiscoverer("FluentAssertionsAsync.Specs.CultureAwareTesting.CulturedFactAttributeDiscoverer", "FluentAssertions.Specs")]
public sealed class CulturedFactAttribute : FactAttribute
{
#pragma warning disable CA1019 // Define accessors for attribute arguments
    public CulturedFactAttribute(params string[] _) { }
#pragma warning restore CA1019 // Define accessors for attribute arguments
}
