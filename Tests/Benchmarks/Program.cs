using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal static class Program
    {
        public static void Main()
        {
            _ = BenchmarkRunner.Run<Issue1657>();
        }
    }
}
