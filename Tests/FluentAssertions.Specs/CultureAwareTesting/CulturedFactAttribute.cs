using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.CultureAwareTesting;

[XunitTestCaseDiscoverer("FluentAssertions.Specs.CultureAwareTesting.CulturedFactAttributeDiscoverer", "FluentAssertions.Specs")]
public sealed class CulturedFactAttribute : FactAttribute
{
#pragma warning disable CA1019 // Define accessors for attribute arguments
    // ReSharper disable once UnusedParameter.Local
    public CulturedFactAttribute(params string[] _) { }
#pragma warning restore CA1019 // Define accessors for attribute arguments
}
