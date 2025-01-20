using FluentAssertions.Specs;

[assembly: FluentAssertions.Extensibility.AssertionEngineInitializer(
    typeof(AssemblyInitializer),
    nameof(AssemblyInitializer.AcknowledgeSoftWarning))]

namespace FluentAssertions.Specs;

public static class AssemblyInitializer
{
    public static void AcknowledgeSoftWarning()
    {
        // Suppress the soft warning about the license requirements for commercial use
        License.Accepted = true;
    }
}
