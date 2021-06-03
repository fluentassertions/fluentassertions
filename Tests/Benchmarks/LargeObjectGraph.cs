using System;

using BenchmarkDotNet.Attributes;

using FluentAssertions;
using FluentAssertions.Primitives;

namespace Benchmarks
{
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
        public AndConstraint<ObjectAssertions> BeEquivalentTo()
            => copy1.Should().BeEquivalentTo(copy2, config => config.AllowingInfiniteRecursion());
    }
}
