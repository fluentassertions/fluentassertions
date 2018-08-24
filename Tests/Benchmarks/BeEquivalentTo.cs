using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using FluentAssertions.Collections;

namespace Benchmarks
{
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
            list = Enumerable.Range(0, N).Select(_ => Nested.Create(1)).ToList();
            list2 = Enumerable.Range(0, N).Select(_ => Nested.Create(1)).ToList();
        }

        [Benchmark]
        public AndConstraint<GenericCollectionAssertions<Nested>> BeEquivalentTo() => list.Should().BeEquivalentTo(list2);
    }
}
