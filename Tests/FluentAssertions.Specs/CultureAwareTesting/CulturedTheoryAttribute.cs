using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.CultureAwareTesting;

[XunitTestCaseDiscoverer("FluentAssertionsAsync.Specs.CultureAwareTesting.CulturedTheoryAttributeDiscoverer",
    "FluentAssertionsAsync.Specs")]
public sealed class CulturedTheoryAttribute : TheoryAttribute
{
#pragma warning disable CA1019 // Define accessors for attribute arguments
    public CulturedTheoryAttribute(params string[] _) { }
#pragma warning restore CA1019 // Define accessors for attribute arguments
}
