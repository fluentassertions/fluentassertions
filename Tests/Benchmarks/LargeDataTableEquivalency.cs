using System;
using System.Data;
using System.Linq;

using BenchmarkDotNet.Attributes;

using Bogus;

using FluentAssertions;
using FluentAssertions.Data;

namespace Benchmarks;

[MemoryDiagnoser]
public class LargeDataTableEquivalencyBenchmarks
{
    private DataTable dataTable1;
    private DataTable dataTable2;

    [Params(10, 100, 1_000, 10_000, 100_000)]
    public int RowCount { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        dataTable1 = CreateDataTable();
        dataTable2 = CreateDataTable();

        object[] rowData = new object[dataTable1.Columns.Count];

        var faker = new Faker
        {
            Random = new Randomizer(localSeed: 1)
        };

        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < dataTable1.Columns.Count; j++)
            {
                Type columnType = dataTable1.Columns[j].DataType;
                rowData[j] = GetData(faker, columnType);
            }

            dataTable1.Rows.Add(rowData);
            dataTable2.Rows.Add(rowData);
        }
    }

    private static object GetData(Faker faker, Type columnType)
    {
        return (Type.GetTypeCode(columnType)) switch
        {
            TypeCode.Empty or TypeCode.DBNull => null,
            TypeCode.Boolean => faker.Random.Bool(),
            TypeCode.Char => faker.Lorem.Letter().Single(),
            TypeCode.SByte => faker.Random.SByte(),
            TypeCode.Byte => faker.Random.Byte(),
            TypeCode.Int16 => faker.Random.Short(),
            TypeCode.UInt16 => faker.Random.UShort(),
            TypeCode.Int32 => faker.Random.Int(),
            TypeCode.UInt32 => faker.Random.UInt(),
            TypeCode.Int64 => faker.Random.Long(),
            TypeCode.UInt64 => faker.Random.ULong(),
            TypeCode.Single => faker.Random.Float(),
            TypeCode.Double => faker.Random.Double(),
            TypeCode.Decimal => faker.Random.Decimal(),
            TypeCode.DateTime => faker.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(+30)),
            TypeCode.String => faker.Lorem.Lines(1),
            _ => GetDefault(faker, columnType),
        };
    }

    private static object GetDefault(Faker faker, Type columnType)
    {
        if (columnType == typeof(TimeSpan))
            return faker.Date.Future() - faker.Date.Future();
        else if (columnType == typeof(Guid))
            return faker.Random.Guid();

        throw new Exception("Unable to populate column of type " + columnType);
    }

    private static DataTable CreateDataTable()
    {
        var table = new DataTable("Test");

        table.Columns.Add("A", typeof(bool));
        table.Columns.Add("B", typeof(char));
        table.Columns.Add("C", typeof(sbyte));
        table.Columns.Add("D", typeof(byte));
        table.Columns.Add("E", typeof(short));
        table.Columns.Add("F", typeof(ushort));
        table.Columns.Add("G", typeof(int));
        table.Columns.Add("H", typeof(uint));
        table.Columns.Add("I", typeof(long));
        table.Columns.Add("J", typeof(ulong));
        table.Columns.Add("K", typeof(float));
        table.Columns.Add("L", typeof(double));
        table.Columns.Add("M", typeof(decimal));
        table.Columns.Add("N", typeof(DateTime));
        table.Columns.Add("O", typeof(string));
        table.Columns.Add("P", typeof(TimeSpan));
        table.Columns.Add("Q", typeof(Guid));

        return table;
    }

    [Benchmark]
    public AndConstraint<DataTableAssertions<DataTable>> BeEquivalentTo() => dataTable1.Should().BeEquivalentTo(dataTable2);
}
