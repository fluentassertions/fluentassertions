using BenchmarkDotNet.Attributes;
using FluentAssertions;
using FluentAssertions.Common;

namespace Benchmarks
{
    [MemoryDiagnoser]
    [RyuJitX86Job]
    public class CollectionEqualBenchmarks
    {
        private int[] collection1;
        private int[] collection2;

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
        public void CollectionEqual_Generic_IsSameOrEqualTo()
        {
            collection1.Should().Equal(collection2, (s, e) => ((object)s).IsSameOrEqualTo(e));
        }

        [Benchmark]
        public void CollectionEqual_Generic_Equality()
        {
            collection1.Should().Equal(collection2, (a, b) => a == b);
        }
    }
}
