using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FluentAssertionsAsync;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.Net80)]
public class BeEquivalentToWithDeeplyNestedStructures
{
    public class ComplexType
    {
        public int A { get; set; }
        public ComplexType B { get; set; }
    }

    [Params(1, 10, 100, 500)]
    public int N { get; set; }

    [Params(1, 2, 6)]
    public int Depth { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        subject = Enumerable.Range(0, N).Select(_ => CreateComplex(Depth)).ToList();
        expectation = Enumerable.Range(0, N).Select(_ => CreateComplex(Depth)).ToList();
    }

    private static ComplexType CreateComplex(int i)
    {
        if (i == 0)
        {
            return new ComplexType();
        }

        return new ComplexType
        {
            A = i,
            B = CreateComplex(i - 1)
        };
    }

    private List<ComplexType> subject;
    private List<ComplexType> expectation;

    [Benchmark]
    public async Task BeEquivalentTo()
    {
        await subject.Should().BeEquivalentToAsync(expectation);
    }
}
