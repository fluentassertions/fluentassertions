using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FluentAssertions.Equivalency.Specs;

/// <summary>
/// Class containing specs related to System.Data, including many common declarations and methods.
/// </summary>
public class DataSpecs
{
    // Non-static member to avoid getting flagged as a "static holder type". This
    // type is inherited by the specialized specs classes it contains.
    public int Dummy { get; set; }

    #region Subject Types
    /*

    NB: TypedDataTable1 and TypedDataTable2 intentionally have the same schema. This allows testing of
        Should().BeEquivalentTo(options => options.AllowMismatchedTypes).

    */

    public class TypedDataRow1 : DataRow
    {
        private readonly TypedDataTable1 ownerTable;

        public TypedDataRow1(DataRowBuilder rowBuilder)
            : base(rowBuilder)
        {
            ownerTable = (TypedDataTable1)Table;
        }

        public int RowID
        {
            get => (int)this[ownerTable.RowIDColumn];
            set => this[ownerTable.RowIDColumn] = value;
        }

        public decimal Decimal
        {
            get => (decimal)this[ownerTable.DecimalColumn];
            set => this[ownerTable.DecimalColumn] = value;
        }

        public string String
        {
            get => (string)this[ownerTable.StringColumn];
            set => this[ownerTable.StringColumn] = value;
        }

        public Guid Guid
        {
            get => (Guid)this[ownerTable.GuidColumn];
            set => this[ownerTable.GuidColumn] = value;
        }

        public DateTime DateTime
        {
            get => (DateTime)this[ownerTable.DateTimeColumn];
            set => this[ownerTable.DateTimeColumn] = value;
        }

        public int? ForeignRowID
        {
            get => IsNull(ownerTable.ForeignRowIDColumn) ? null : (int?)this[ownerTable.ForeignRowIDColumn];
            set => this[ownerTable.ForeignRowIDColumn] = value;
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.Naming", "SA1306", Justification = "DataColumn accessors are named after the columns.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.Naming", "SA1516", Justification = "DataColumn accessors are grouped in a block of lines.")]
    public class TypedDataTable1 : TypedTableBase<TypedDataRow1>
    {
        public TypedDataTable1()
        {
            TableName = "TypedDataTable1";

            Columns.Add(new DataColumn("RowID", typeof(int)));
            Columns.Add(new DataColumn("Decimal", typeof(decimal)));
            Columns.Add(new DataColumn("String", typeof(string)));
            Columns.Add(new DataColumn("Guid", typeof(Guid)));
            Columns.Add(new DataColumn("DateTime", typeof(DateTime)));
            Columns.Add(new DataColumn("ForeignRowID", typeof(int)));

            PrimaryKey = new[] { RowIDColumn };

            Constraints.Add(new UniqueConstraint(GuidColumn));
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new TypedDataRow1(builder);
        }

        public new TypedDataRow1 NewRow()
        {
            return (TypedDataRow1)base.NewRow();
        }

        public TypedDataRow1 this[int index]
        {
            get => (TypedDataRow1)Rows[index];
        }

        public DataColumn RowIDColumn => Columns["RowID"];
        public DataColumn DecimalColumn => Columns["Decimal"];
        public DataColumn StringColumn => Columns["String"];
        public DataColumn GuidColumn => Columns["Guid"];
        public DataColumn DateTimeColumn => Columns["DateTime"];
        public DataColumn ForeignRowIDColumn => Columns["ForeignRowID"];
    }

    public class TypedDataRow2 : DataRow
    {
        private readonly TypedDataTable2 ownerTable;

        public TypedDataRow2(DataRowBuilder rowBuilder)
            : base(rowBuilder)
        {
            ownerTable = (TypedDataTable2)Table;
        }

        public int RowID
        {
            get => (int)this[ownerTable.RowIDColumn];
            set => this[ownerTable.RowIDColumn] = value;
        }

        public decimal Decimal
        {
            get => (decimal)this[ownerTable.DecimalColumn];
            set => this[ownerTable.DecimalColumn] = value;
        }

        public string String
        {
            get => (string)this[ownerTable.StringColumn];
            set => this[ownerTable.StringColumn] = value;
        }

        public Guid Guid
        {
            get => (Guid)this[ownerTable.GuidColumn];
            set => this[ownerTable.GuidColumn] = value;
        }

        public DateTime DateTime
        {
            get => (DateTime)this[ownerTable.DateTimeColumn];
            set => this[ownerTable.DateTimeColumn] = value;
        }

        public int? ForeignRowID
        {
            get => IsNull(ownerTable.ForeignRowIDColumn) ? null : (int?)this[ownerTable.ForeignRowIDColumn];
            set => this[ownerTable.ForeignRowIDColumn] = value;
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.Naming", "SA1306", Justification = "DataColumn accessors are named after the columns.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.Naming", "SA1516", Justification = "DataColumn accessors are grouped in a block of lines.")]
    public class TypedDataTable2 : TypedTableBase<TypedDataRow2>
    {
        public TypedDataTable2()
        {
            TableName = "TypedDataTable2";

            Columns.Add(new DataColumn("RowID", typeof(int)));
            Columns.Add(new DataColumn("Decimal", typeof(decimal)));
            Columns.Add(new DataColumn("String", typeof(string)));
            Columns.Add(new DataColumn("Guid", typeof(Guid)));
            Columns.Add(new DataColumn("DateTime", typeof(DateTime)));
            Columns.Add(new DataColumn("ForeignRowID", typeof(int)));

            PrimaryKey = new[] { RowIDColumn };

            Constraints.Add(new UniqueConstraint(GuidColumn));
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new TypedDataRow2(builder);
        }

        public new TypedDataRow2 NewRow()
        {
            return (TypedDataRow2)base.NewRow();
        }

        public TypedDataRow2 this[int index]
        {
            get => (TypedDataRow2)Rows[index];
        }

        public DataColumn RowIDColumn => Columns["RowID"];
        public DataColumn DecimalColumn => Columns["Decimal"];
        public DataColumn StringColumn => Columns["String"];
        public DataColumn GuidColumn => Columns["Guid"];
        public DataColumn DateTimeColumn => Columns["DateTime"];
        public DataColumn ForeignRowIDColumn => Columns["ForeignRowID"];
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.Naming", "SA1516", Justification = "DataTable accessors are grouped in a block of lines.")]
    public class TypedDataSet : DataSet
    {
        public override SchemaSerializationMode SchemaSerializationMode { get; set; }

        public TypedDataSet()
            : this(swapTableOrder: false)
        {
        }

        public TypedDataSet(bool swapTableOrder)
        {
            if (swapTableOrder)
            {
                Tables.Add(new TypedDataTable2());
                Tables.Add(new TypedDataTable1());
            }
            else
            {
                Tables.Add(new TypedDataTable1());
                Tables.Add(new TypedDataTable2());
            }
        }

        public TypedDataTable1 TypedDataTable1 => (TypedDataTable1)Tables["TypedDataTable1"];
        public TypedDataTable2 TypedDataTable2 => (TypedDataTable2)Tables["TypedDataTable2"];

        public DataSet ToUntypedDataSet()
        {
            var dataSet = new DataSet();

            dataSet.DataSetName = DataSetName;
            dataSet.CaseSensitive = CaseSensitive;
            dataSet.EnforceConstraints = EnforceConstraints;
            dataSet.Locale = Locale;
            dataSet.Namespace = Namespace;
            dataSet.Prefix = Prefix;
            dataSet.RemotingFormat = RemotingFormat;

            foreach (var typedTable in Tables.Cast<DataTable>())
            {
                var dataTable = new DataTable();

                dataTable.TableName = typedTable.TableName;
                dataTable.DisplayExpression = typedTable.DisplayExpression;
                dataTable.MinimumCapacity = typedTable.MinimumCapacity;
                dataTable.Namespace = typedTable.Namespace;
                dataTable.Prefix = typedTable.Prefix;
                dataTable.RemotingFormat = typedTable.RemotingFormat;

                foreach (var column in typedTable.Columns.Cast<DataColumn>())
                {
                    var dataColumn = new DataColumn();

                    dataColumn.ColumnName = column.ColumnName;
                    dataColumn.AllowDBNull = column.AllowDBNull;
                    dataColumn.AutoIncrement = column.AutoIncrement;
                    dataColumn.AutoIncrementSeed = column.AutoIncrementSeed;
                    dataColumn.AutoIncrementStep = column.AutoIncrementStep;
                    dataColumn.Caption = column.Caption;
                    dataColumn.ColumnMapping = column.ColumnMapping;
                    dataColumn.DataType = column.DataType;
                    dataColumn.DateTimeMode = column.DateTimeMode;
                    dataColumn.DefaultValue = column.DefaultValue;
                    dataColumn.Expression = column.Expression;
                    dataColumn.MaxLength = column.MaxLength;
                    dataColumn.Namespace = column.Namespace;
                    dataColumn.Prefix = column.Prefix;
                    dataColumn.ReadOnly = column.ReadOnly;

                    foreach (var property in column.ExtendedProperties.Cast<DictionaryEntry>())
                    {
                        dataColumn.ExtendedProperties.Add(property.Key, property.Value);
                    }

                    dataTable.Columns.Add(dataColumn);
                }

                foreach (var row in typedTable.Rows.Cast<DataRow>())
                {
                    dataTable.ImportRow(row);
                }

                dataTable.PrimaryKey = typedTable.PrimaryKey
                    .Select(col => dataTable.Columns[col.ColumnName])
                    .ToArray();

                foreach (var property in typedTable.ExtendedProperties.Cast<DictionaryEntry>())
                {
                    dataTable.ExtendedProperties.Add(property.Key, property.Value);
                }

                foreach (var constraint in typedTable.Constraints.Cast<Constraint>())
                {
                    if (!Relations.Cast<DataRelation>().Any(rel =>
                        (rel.ChildKeyConstraint == constraint) ||
                        (rel.ParentKeyConstraint == constraint)))
                    {
                        if (constraint is UniqueConstraint uniqueConstraint)
                        {
                            if (!uniqueConstraint.IsPrimaryKey)
                            {
                                dataTable.Constraints.Add(new UniqueConstraint(
                                    name: uniqueConstraint.ConstraintName,
                                    column: dataTable.Columns[uniqueConstraint.Columns.Single().ColumnName]));
                            }
                        }
                        else
                        {
                            throw new Exception($"Don't know how to clone a constraint of type {constraint.GetType()}");
                        }
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            foreach (var property in ExtendedProperties.Cast<DictionaryEntry>())
            {
                dataSet.ExtendedProperties.Add(property.Key, property.Value);
            }

            foreach (var relation in Relations.Cast<DataRelation>())
            {
                // NB: In the context of the unit test, this is assuming that there is only one column in a relation.
                var dataRelation = new DataRelation(
                    relation.RelationName,
                    parentColumn: dataSet.Tables[relation.ParentTable.TableName].Columns[relation.ParentColumns.Single().ColumnName],
                    childColumn: dataSet.Tables[relation.ChildTable.TableName].Columns[relation.ChildColumns.Single().ColumnName]);

                foreach (var property in relation.ExtendedProperties.Cast<DictionaryEntry>())
                {
                    dataRelation.ExtendedProperties.Add(property.Key, property.Value);
                }

                dataSet.Relations.Add(dataRelation);
            }

            return dataSet;
        }
    }

    public class TypedDataSetSubclass : TypedDataSet
    {
        public TypedDataSetSubclass()
        {
        }

        public TypedDataSetSubclass(TypedDataSet copyFrom, bool swapTableOrder = false, bool randomizeRowOrder = false)
            : base(swapTableOrder)
        {
            DataSetName = copyFrom.DataSetName;
            CaseSensitive = copyFrom.CaseSensitive;
            EnforceConstraints = copyFrom.EnforceConstraints;
            Locale = copyFrom.Locale;
            Namespace = copyFrom.Namespace;
            Prefix = copyFrom.Prefix;
            RemotingFormat = copyFrom.RemotingFormat;
            SchemaSerializationMode = copyFrom.SchemaSerializationMode;

            CopyTable<TypedDataTable1, TypedDataRow1>(
                @from: copyFrom.TypedDataTable1,
                to: TypedDataTable1,
                randomizeRowOrder);

            CopyTable<TypedDataTable2, TypedDataRow2>(
                @from: copyFrom.TypedDataTable2,
                to: TypedDataTable2,
                randomizeRowOrder);

            foreach (var property in copyFrom.ExtendedProperties.Cast<DictionaryEntry>())
            {
                ExtendedProperties.Add(property.Key, property.Value);
            }

            foreach (var copyFromRelation in copyFrom.Relations.Cast<DataRelation>())
            {
                // NB: In the context of the unit test, this is assuming that there is only one column in a relation.
                var relation = new DataRelation(
                    copyFromRelation.RelationName,
                    parentColumn: Tables[copyFromRelation.ParentTable.TableName].Columns[copyFromRelation.ParentColumns.Single().ColumnName],
                    childColumn: Tables[copyFromRelation.ChildTable.TableName].Columns[copyFromRelation.ChildColumns.Single().ColumnName]);

                foreach (var property in copyFromRelation.ExtendedProperties.Cast<DictionaryEntry>())
                {
                    relation.ExtendedProperties.Add(property.Key, property.Value);
                }

                Relations.Add(relation);
            }
        }

        private void CopyTable<TDataTable, TDataRow>(TDataTable from, TDataTable to, bool randomizeRowOrder)
            where TDataTable : DataTable, IEnumerable<TDataRow>
            where TDataRow : DataRow
        {
            if (randomizeRowOrder)
            {
                foreach (var row in from.OrderBy(row => Guid.NewGuid()))
                {
                    to.ImportRow(row);
                }
            }
            else
            {
                foreach (var row in from)
                {
                    to.ImportRow(row);
                }
            }

            foreach (var property in from.ExtendedProperties.Cast<DictionaryEntry>())
            {
                to.ExtendedProperties.Add(property.Key, property.Value);
            }

            foreach (var column in to.Columns.Cast<DataColumn>())
            {
                foreach (var property in from.Columns[column.ColumnName].ExtendedProperties.Cast<DictionaryEntry>())
                {
                    column.ExtendedProperties.Add(property.Key, property.Value);
                }
            }
        }
    }
    #endregion

    private static readonly Random Random = new Random();

    internal static TDataSet CreateDummyDataSet<TDataSet>(bool identicalTables = false, bool includeDummyData = true, bool includeRelation = true)
        where TDataSet : TypedDataSet, new()
    {
        var ret = new TDataSet();

        foreach (var dataColumn in ret.TypedDataTable1.Columns.Cast<DataColumn>())
        {
            AddExtendedProperties(dataColumn.ExtendedProperties);
        }

        if (includeDummyData)
        {
            InsertDummyRows(ret.TypedDataTable1);
        }

        AddExtendedProperties(ret.TypedDataTable1.ExtendedProperties);

        if (identicalTables)
        {
            foreach (var dataColumn in ret.TypedDataTable2.Columns.Cast<DataColumn>())
            {
                foreach (var property in ret.TypedDataTable1.ExtendedProperties.Cast<DictionaryEntry>())
                {
                    dataColumn.ExtendedProperties.Add(property.Key, property.Value);
                }
            }

            foreach (var row in ret.TypedDataTable1.Cast<DataRow>())
            {
                ret.TypedDataTable2.ImportRow(row);
            }

            foreach (var property in ret.TypedDataTable1.ExtendedProperties.Cast<DictionaryEntry>())
            {
                ret.TypedDataTable2.ExtendedProperties.Add(property.Key, property.Value);
            }
        }
        else
        {
            foreach (var dataColumn in ret.TypedDataTable2.Columns.Cast<DataColumn>())
            {
                AddExtendedProperties(dataColumn.ExtendedProperties);
            }

            if (includeDummyData)
            {
                InsertDummyRows(ret.TypedDataTable2, ret.TypedDataTable1);
            }

            if (includeRelation)
            {
                AddRelation(ret);
            }

            AddExtendedProperties(ret.TypedDataTable2.ExtendedProperties);
        }

        AddExtendedProperties(ret.ExtendedProperties);

        return ret;
    }

    protected static void AddRelation(TypedDataSet dataSet)
    {
        var relation = new DataRelation("TestRelation", dataSet.TypedDataTable1.RowIDColumn, dataSet.TypedDataTable2.ForeignRowIDColumn);

        AddExtendedProperties(relation.ExtendedProperties);

        dataSet.Relations.Add(relation);
    }

    private static void AddExtendedProperties(PropertyCollection extendedProperties)
    {
        extendedProperties.Add(Guid.NewGuid(), Guid.NewGuid());
        extendedProperties.Add(Guid.NewGuid(), Guid.NewGuid().ToString());
        extendedProperties.Add(Guid.NewGuid().ToString(), Guid.NewGuid());
    }

    private static void InsertDummyRows(DataTable table, DataTable foreignRows = null)
    {
        for (int i = 0; i < 10; i++)
        {
            InsertDummyRow(table, foreignRows?.Rows[i]);
        }

        table.AcceptChanges();
    }

    private static void InsertDummyRow(DataTable table, DataRow foreignRow = null)
    {
        var row = table.NewRow();

        foreach (var column in table.Columns.Cast<DataColumn>())
        {
            if (column.ColumnName.StartsWith("Foreign", StringComparison.Ordinal))
            {
                row[column] = foreignRow?[column.ColumnName.Substring(7)] ?? DBNull.Value;
            }
            else
            {
                row[column] = GetDummyValueOfType(column.DataType);
            }
        }

        table.Rows.Add(row);
    }

    [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "TypedDataTable test types have a limited set of columns.")]
    private static object GetDummyValueOfType(Type dataType)
    {
        switch (Type.GetTypeCode(dataType))
        {
            case TypeCode.Int32:
                return Random.Next();
            case TypeCode.DateTime:
                return DateTime.MinValue.AddTicks((Random.Next() & 0x7FFFFFFF) * 1469337835L).AddTicks((Random.Next() & 0x7FFFFFFF) * 1469337835L / 2147483648);
            case TypeCode.Decimal:
                return new decimal(Random.NextDouble());
            case TypeCode.String:
                return Guid.NewGuid().ToString();

            default:
                if (dataType == typeof(Guid))
                {
                    return Guid.NewGuid();
                }

                throw new Exception($"Don't know how to generate dummy value of type {dataType}");
        }
    }

    public enum ChangeType
    {
        Added,
        Changed,
        Removed,
    }

    public static IEnumerable<object[]> AllChangeTypes
        => Enum.GetValues(typeof(ChangeType)).Cast<ChangeType>().Select(t => new object[] { t });

    public static IEnumerable<object[]> AllChangeTypesWithAcceptChangesValues
        => Enum.GetValues(typeof(ChangeType)).Cast<ChangeType>().Join(
            new[] { true, false },
            changeType => true,
            acceptChanges => true,
            (changeType, acceptChanges) => new object[] { changeType, acceptChanges });

    [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "Every enum value is covered")]
    protected static void ApplyChange(DataColumnCollection columns, ChangeType changeType)
    {
        switch (changeType)
        {
            case ChangeType.Added:
                columns.Add("Test", typeof(int));
                break;
            case ChangeType.Changed:
                columns[1].ColumnName += "different";
                break;
            case ChangeType.Removed:
                columns.RemoveAt(1);
                break;
        }
    }

    [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "Every enum value is covered")]
    protected static void ApplyChange(PropertyCollection extendedProperties, ChangeType changeType)
    {
        switch (changeType)
        {
            case ChangeType.Added:
                extendedProperties[Guid.NewGuid()] = Guid.NewGuid();
                break;
            case ChangeType.Changed:
                extendedProperties[extendedProperties.Keys.Cast<object>().First()] = Guid.NewGuid();
                break;
            case ChangeType.Removed:
                extendedProperties.Remove(extendedProperties.Keys.Cast<object>().First());
                break;
        }
    }

    [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "Every enum value is covered")]
    protected static void ApplyChange(ConstraintCollection constraints, DataColumn columnForNewConstraint, ChangeType changeType)
    {
        switch (changeType)
        {
            case ChangeType.Added:
                constraints.Add(
                    new UniqueConstraint(
                        "Test",
                        columnForNewConstraint));
                break;
            case ChangeType.Changed:
                constraints[1].ConstraintName += "different";
                break;
            case ChangeType.Removed:
                constraints.RemoveAt(1);
                break;
        }
    }

    [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "Every enum value is covered")]
    protected static void ApplyChange(DataRowCollection rows, DataTable dataTable, ChangeType changeType)
    {
        switch (changeType)
        {
            case ChangeType.Added:
                InsertDummyRow(dataTable);
                break;
            case ChangeType.Changed:
                rows[1]["String"] += "different";
                break;
            case ChangeType.Removed:
                rows.RemoveAt(0);
                break;
        }
    }
}
