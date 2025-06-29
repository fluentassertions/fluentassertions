using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

[Collection("ConfigurationSpecs")]
public class TracingSpecs
{
    [Fact]
    public void Tracing_must_be_safe_when_executed_concurrently()
    {
        try
        {
            // Arrange
            AssertionConfiguration.Current.Equivalency.Modify(e => e.WithTracing());

            Parallel.For(1, 10_000, (_, _) =>
            {
                try
                {
                    new { A = "a" }.Should().BeEquivalentTo(new { A = "b" });
                }
                catch (XunitException)
                {
                }
            });
        }
        finally
        {
            AssertionEngine.ResetToDefaults();
        }
    }
}
