using System.Linq;
using Xunit;

namespace FluentAssertions.Equivalency.Specs;

public class PerformanceSpecs
{
    [Fact(Skip = "Performance test")]
    public void Compare_complex_collection()
    {
        int n = 500;
        var subject = Enumerable.Range(0, n).Select(index => Create(index)).ToArray();
        var expected = Enumerable.Range(n, n).Select(index => Create(index)).ToArray();

        subject.Should().BeEquivalentTo(expected, e => e.WithoutStrictOrdering());
    }

    private object Create(int index) => new
    {
        Value = index,
        Name = $"Item-{index}-{new string((char)('A' + (index % 26)), 50)}",
        Description = $"Description for item number {index} with extra padding {new string('x', 100)}",
        Category = $"Category-{index % 10}",
        Nested = new
        {
            Id = index * 100,
            Label = $"Nested-{index}-{new string('z', 50)}",
            SubNested = new
            {
                Code = index * 1000,
                Data = $"Data-{index}-{new string('y', 80)}"
            }
        },
        Meta = new
        {
            K1 = index,
            K2 = index * 2,
            K3 = index * 3,
            K4 = index * 4,
            K5 = index * 5
        }
    };
}
