using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FluentAssertionsAsync;
using FluentAssertionsAsync.Primitives;

namespace Benchmarks;

[MemoryDiagnoser]
public class LargeObjectGraphBenchmarks
{
    [Params(16, 18, 20, 24, 28)]
    public int N { get; set; }

    private Nested copy1;
    private Nested copy2;

    [GlobalSetup]
    public void GlobalSetup()
    {
        int objectCount = 0;

        copy1 = Nested.Create(N, ref objectCount);

        objectCount = 0;

        copy2 = Nested.Create(N, ref objectCount);

        Console.WriteLine("N = {0} ; Graph size: {1} objects", N, objectCount);
    }

    [Benchmark]
    public async Task<AndConstraint<ObjectAssertions>> BeEquivalentTo() =>
        await copy1.Should().BeEquivalentToAsync(copy2, config => config.AllowingInfiniteRecursion());
}
