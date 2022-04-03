using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FluentAssertions;
using FluentAssertions.Collections;

namespace Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.Net60)]
    public class Issue1657
    {
        private List<ExampleObject> list;
        private List<ExampleObject> list2;

        [Params(1, 10, 50)]
        public int N { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            list = Enumerable.Range(0, N).Select(i => GetObject(i)).ToList();
            list2 = Enumerable.Range(0, N).Select(i => GetObject(N - 1 - i)).ToList();
        }

        [Benchmark]
        public AndConstraint<GenericCollectionAssertions<ExampleObject>> BeEquivalentTo() =>
            list.Should().BeEquivalentTo(list2);

        private static ExampleObject GetObject(int i)
        {
            return new ExampleObject
            {
                Id = i.ToString("D2"),
                Value1 = i.ToString("D2"),
                Value2 = i.ToString("D2"),
                Value3 = i.ToString("D2"),
                Value4 = i.ToString("D2"),
                Value5 = i.ToString("D2"),
                Value6 = i.ToString("D2"),
                Value7 = i.ToString("D2"),
                Value8 = i.ToString("D2"),
                Value9 = i.ToString("D2"),
                Value10 = i.ToString("D2"),
                Value11 = i.ToString("D2"),
                Value12 = i.ToString("D2"),
            };
        }
    }

    public class ExampleObject
    {
        public string Id { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public string Value3 { get; set; }

        public string Value4 { get; set; }

        public string Value5 { get; set; }

        public string Value6 { get; set; }

        public string Value7 { get; set; }

        public string Value8 { get; set; }

        public string Value9 { get; set; }

        public string Value10 { get; set; }

        public string Value11 { get; set; }

        public string Value12 { get; set; }
    }
}
