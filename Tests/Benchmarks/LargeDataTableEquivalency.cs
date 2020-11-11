using System;
using System.Data;
using System.Linq;

using BenchmarkDotNet.Attributes;

using Bogus;

using FluentAssertions;
using FluentAssertions.Data;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class LargeDataTableEquivalencyBenchmarks
    {
        private DataTable dataTable1;
        private DataTable dataTable2;

        [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
        public int RowCount { get; set; }

        [GlobalSetup]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0055:Fix formatting", Justification = "Big long list of one-liners")]
        public void GlobalSetup()
        {
            dataTable1 = CreateDataTable();
            dataTable2 = CreateDataTable();

            object[] rowData = new object[dataTable1.Columns.Count];

            var faker = new Faker();

            faker.Random = new Randomizer(localSeed: 1);

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < dataTable1.Columns.Count; j++)
                {
                    var column = dataTable1.Columns[j];

                    switch (Type.GetTypeCode(column.DataType))
                    {
                        case TypeCode.Empty:
                        case TypeCode.DBNull: rowData[j] = null; break;
                        case TypeCode.Boolean: rowData[j] = faker.Random.Bool(); break;
                        case TypeCode.Char: rowData[j] = faker.Lorem.Letter().Single(); break;
                        case TypeCode.SByte: rowData[j] = faker.Random.SByte(); break;
                        case TypeCode.Byte: rowData[j] = faker.Random.Byte(); break;
                        case TypeCode.Int16: rowData[j] = faker.Random.Short(); break;
                        case TypeCode.UInt16: rowData[j] = faker.Random.UShort(); break;
                        case TypeCode.Int32: rowData[j] = faker.Random.Int(); break;
                        case TypeCode.UInt32: rowData[j] = faker.Random.UInt(); break;
                        case TypeCode.Int64: rowData[j] = faker.Random.Long(); break;
                        case TypeCode.UInt64: rowData[j] = faker.Random.ULong(); break;
                        case TypeCode.Single: rowData[j] = faker.Random.Float(); break;
                        case TypeCode.Double: rowData[j] = faker.Random.Double(); break;
                        case TypeCode.Decimal: rowData[j] = faker.Random.Decimal(); break;
                        case TypeCode.DateTime: rowData[j] = faker.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(+30)); break;
                        case TypeCode.String: rowData[j] = faker.Lorem.Lines(1); break;

                        default:
                        {
                            if (column.DataType == typeof(TimeSpan))
                                rowData[j] = faker.Date.Future() - faker.Date.Future();
                            else if (column.DataType == typeof(Guid))
                                rowData[j] = faker.Random.Guid();
                            else
                                throw new Exception("Unable to populate column of type " + column.DataType);

                            break;
                        }
                    }
                }

                dataTable1.Rows.Add(rowData);
                dataTable2.Rows.Add(rowData);
            }
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
}
