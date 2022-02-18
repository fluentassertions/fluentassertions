using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal static class Program
    {
        public static void Main()
        {
            var exporter = new CsvExporter(
                CsvSeparator.CurrentCulture,
                new SummaryStyle(
                    cultureInfo: System.Globalization.CultureInfo.GetCultureInfo("nl-NL"),
                    printUnitsInHeader: true,
                    printUnitsInContent: false,
                    timeUnit: Perfolizer.Horology.TimeUnit.Microsecond,
                    sizeUnit: SizeUnit.KB
                ));

            var config = ManualConfig.CreateMinimumViable().AddExporter(exporter);

            _ = BenchmarkRunner.Run<CheckIfMemberIsBrowsableBenchmarks>(config);
        }
    }
}
