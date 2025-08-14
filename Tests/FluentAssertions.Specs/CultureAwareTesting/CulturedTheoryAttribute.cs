using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.CultureAwareTesting;

[XunitTestCaseDiscoverer("FluentAssertions.Specs.CultureAwareTesting.CulturedTheoryAttributeDiscoverer",
    "FluentAssertions.Specs")]
public sealed class CulturedTheoryAttribute : TheoryAttribute
{
#pragma warning disable CA1019 // Define accessors for attribute arguments
    // ReSharper disable once UnusedParameter.Local
    public CulturedTheoryAttribute(params string[] _) { }
#pragma warning restore CA1019 // Define accessors for attribute arguments
}
