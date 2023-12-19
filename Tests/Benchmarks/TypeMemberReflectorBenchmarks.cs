using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FluentAssertionsAsync.Common;
using static FluentAssertionsAsync.Equivalency.MemberVisibility;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class TypeMemberReflectorBenchmarks
{
    [Benchmark]
    public object Publicc() => new TypeMemberReflector(typeof(A), Public);

    [Benchmark]
    public object Public_Internal() => new TypeMemberReflector(typeof(A), Public | Internal);

    [Benchmark]
    public object Public_Internal_ExplicitlyImplemented() => new TypeMemberReflector(typeof(A), Public | Internal | ExplicitlyImplemented);
}

internal interface I
{
    int Interface0 { get; set; }
    int Interface1 { get; set; }
    int Interface2 { get; set; }
    int Interface3 { get; set; }
    int Interface4 { get; set; }
    int Interface5 { get; set; }
    int Interface6 { get; set; }
    int Interface7 { get; set; }
    int Interface8 { get; set; }
    int Interface9 { get; set; }
}

internal class A : I
{
    public int Public0 { get; set; }
    public int Public1 { get; set; }
    public int Public2 { get; set; }
    public int Public3 { get; set; }
    public int Public4 { get; set; }
    public int Public5 { get; set; }
    public int Public6 { get; set; }
    public int Public7 { get; set; }
    public int Public8 { get; set; }
    public int Public9 { get; set; }

    internal int Internal0 { get; set; }
    internal int Internal1 { get; set; }
    internal int Internal2 { get; set; }
    internal int Internal3 { get; set; }
    internal int Internal4 { get; set; }
    internal int Internal5 { get; set; }
    internal int Internal6 { get; set; }
    internal int Internal7 { get; set; }
    internal int Internal8 { get; set; }
    internal int Internal9 { get; set; }

    int I.Interface0 { get; set; }
    int I.Interface1 { get; set; }
    int I.Interface2 { get; set; }
    int I.Interface3 { get; set; }
    int I.Interface4 { get; set; }
    int I.Interface5 { get; set; }
    int I.Interface6 { get; set; }
    int I.Interface7 { get; set; }
    int I.Interface8 { get; set; }
    int I.Interface9 { get; set; }
}
