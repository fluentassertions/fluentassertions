using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FluentAssertionsAsync;
using FluentAssertionsAsync.Common;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net80)]
public class CollectionEqualBenchmarks
{
    private IEnumerable<int> collection1;
    private IEnumerable<int> collection2;

    [Params(10, 100, 1_000, 5_000, 10_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        collection1 = new int[N];
        collection2 = new int[N];
    }

    [Benchmark(Baseline = true)]
    public void CollectionEqual_Generic()
    {
        collection1.Should().Equal(collection2);
    }

    [Benchmark]
    public void CollectionEqual_Optimized()
    {
        collection1.Should().Equal(collection2, ObjectExtensions.GetComparer<int>());
    }

    [Benchmark]
    public void CollectionEqual_CustomComparer()
    {
        collection1.Should().Equal(collection2, (a, b) => a == b);
    }
}
