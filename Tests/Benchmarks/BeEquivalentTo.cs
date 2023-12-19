using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentAssertionsAsync;
using FluentAssertionsAsync.Collections;

namespace Benchmarks;

[MemoryDiagnoser]
[RyuJitX86Job]
public class BeEquivalentToBenchmarks
{
    private List<Nested> list;
    private List<Nested> list2;

    [Params(10, 100, 1_000, 5_000, 10_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        int objectCount = 0;

        list = Enumerable.Range(0, N).Select(_ => Nested.Create(1, ref objectCount)).ToList();

        objectCount = 0;

        list2 = Enumerable.Range(0, N).Select(_ => Nested.Create(1, ref objectCount)).ToList();
    }

    [Benchmark]
    public async Task<AndConstraint<GenericCollectionAssertions<Nested>>> BeEquivalentTo() => await list.Should().BeEquivalentToAsync(list2);
}
