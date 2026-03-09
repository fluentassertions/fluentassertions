using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;

namespace Benchmarks;

[MemoryDiagnoser]
public class PR_3133_Benchmarks
{
    private object[] subject;
    private object[] expected;

    [Params(10, 100, 200)]
    public int N { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        subject = Enumerable.Range(0, N).Select(index => Create(index)).ToArray();
        expected = Enumerable.Range(N, N).Select(index => Create(index)).ToArray();
    }

    [Benchmark]
    public void BeEquivalentTo()
    {
        try
        {
            subject.Should().BeEquivalentTo(expected, e => e.WithoutStrictOrdering());
        }
        catch
        {
        }
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
