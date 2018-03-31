using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal static class Program
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<BeEquivalentToBenchmarks>();
        }
    }
}
