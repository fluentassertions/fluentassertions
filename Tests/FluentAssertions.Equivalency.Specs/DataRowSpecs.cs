using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class DataRowSpecs : DataSpecs
    {
        [Fact]
        public void When_DataRows_are_identical_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable1 = dataSet1.TypedDataTable1;
            var dataTable2 = dataSet2.TypedDataTable1;

            // Act & Assert
            dataTable1[0].Should().BeEquivalentTo(dataTable2[0]);
        }

        [Fact]
        public void When_DataRows_are_both_null_it_should_succeed()
        {
            // Act & Assert
            ((DataRow)null).Should().BeEquivalentTo(null);
        }

        [Fact]
        public void When_DataRow_is_null_and_isnt_expected_to_be_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>();

            var dataTable = dataSet.TypedDataTable1;

            // Act
            Action action = () => ((DataRow)null).Should().BeEquivalentTo(dataTable[0]);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected *to be non-null, but found null*");
        }

        [Fact]
        public void When_DataRow_is_expected_to_be_null_and_isnt_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>();

            var dataTable = dataSet.TypedDataTable1;

            // Act
            Action action = () => dataTable[0].Should().BeEquivalentTo(null);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataRow_subject_is_deleted_and_RowState_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable1 = dataSet1.TypedDataTable2;
            var dataTable2 = dataSet2.TypedDataTable2;

            var dataRow1 = dataTable1[0];
            var dataRow2 = dataTable2[0];

            dataRow1.Delete();

            // Act & Assert
            dataRow1.Should().BeEquivalentTo(dataRow2, config => config
                .Excluding(row => row.RowState));
        }

        [Fact]
        public void When_DataRow_expectation_is_deleted_and_RowState_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable1 = dataSet1.TypedDataTable2;
            var dataTable2 = dataSet2.TypedDataTable2;

            var dataRow1 = dataTable1[0];
            var dataRow2 = dataTable2[0];

            dataRow2.Delete();

            // Act & Assert
            dataRow1.Should().BeEquivalentTo(dataRow2, config => config
                .Excluding(row => row.RowState));
        }

        [Fact]
        public void When_DataRow_is_deleted_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable1 = dataSet1.TypedDataTable2;
            var dataTable2 = dataSet2.TypedDataTable2;

            var dataRow1 = dataTable1[0];
            var dataRow2 = dataTable2[0];

            dataRow1.Delete();
            dataRow2.Delete();

            // Act & Assert
            dataRow1.Should().BeEquivalentTo(dataRow2);
        }

        [Fact]
        public void When_DataRow_is_modified_and_original_data_differs_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable1 = dataSet1.TypedDataTable2;
            var dataTable2 = dataSet2.TypedDataTable2;

            var dataRow1 = dataTable1[0];
            var dataRow2 = dataTable2[0];

            dataRow1.Decimal++;
            dataTable1.AcceptChanges();
            dataRow1.Decimal--;

            dataRow2.Decimal--;
            dataTable2.AcceptChanges();
            dataRow2.Decimal++;

            // Act
            Action action = () => dataRow1.Should().BeEquivalentTo(dataRow2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataRow_is_modified_and_original_data_differs_and_original_data_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable1 = dataSet1.TypedDataTable2;
            var dataTable2 = dataSet2.TypedDataTable2;

            var dataRow1 = dataTable1[0];
            var dataRow2 = dataTable2[0];

            dataRow1.Decimal++;
            dataTable1.AcceptChanges();
            dataRow1.Decimal--;

            dataRow2.Decimal--;
            dataTable2.AcceptChanges();
            dataRow2.Decimal++;

            // Act & Assert
            dataRow1.Should().BeEquivalentTo(dataRow2, config => config
                .ExcludingOriginalData());
        }

        [Fact]
        public void When_DataRow_type_does_not_match_and_AllowMismatchedType_not_enabled_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>(identicalTables: true);
            var dataSet2 = new TypedDataSetSubclass(dataSet);

            var dataTable = dataSet.TypedDataTable1;
            var dataTableOfMismatchedType = dataSet2.TypedDataTable2;

            dataSet2.Tables.Remove(dataTable.TableName);
            dataTableOfMismatchedType.TableName = dataTable.TableName;

            // Act
            Action action = () => dataTable[0].Should().BeEquivalentTo(dataTableOfMismatchedType[0]);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataRow_type_does_not_match_and_AllowMismatchedType_is_enabled_it_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>(identicalTables: true);
            var dataSet2 = new TypedDataSetSubclass(dataSet);

            var dataTable = dataSet.TypedDataTable1;
            var dataTableOfMismatchedType = dataSet2.TypedDataTable2;

            dataSet2.Tables.Remove(dataTable.TableName);
            dataTableOfMismatchedType.TableName = dataTable.TableName;

            // Act & Assert
            dataTable[0].Should().BeEquivalentTo(dataTableOfMismatchedType[0], options => options.AllowingMismatchedTypes());
        }

        [Fact]
        public void When_HasErrors_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataRow1 = dataSet1.TypedDataTable1[0];
            var dataRow2 = dataSet2.TypedDataTable1[0];

            dataRow2.RowError = "Manually added error";

            // Act
            Action action = () => dataRow1.Should().BeEquivalentTo(dataRow2);

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("HasErrors");
        }

        [Fact]
        public void When_HasErrors_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataRow1 = dataSet1.TypedDataTable1[0];
            var dataRow2 = dataSet2.TypedDataTable1[0];

            dataRow2.RowError = "Manually added error";

            // Act & Assert
            dataRow1.Should().BeEquivalentTo(dataRow2, config => config.Excluding(dataRow => dataRow.HasErrors));
        }

        [Fact]
        public void When_RowState_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataRow1 = dataSet1.TypedDataTable1[0];
            var dataRow2 = dataSet2.TypedDataTable1[0];

            dataRow1.DateTime = DateTime.UtcNow;
            dataRow2.DateTime = dataRow1.DateTime;

            dataSet2.AcceptChanges();

            // Act
            Action action = () => dataRow1.Should().BeEquivalentTo(dataRow2);

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("RowState");
        }

        [Fact]
        public void When_RowState_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataRow1 = dataSet1.TypedDataTable1[0];
            var dataRow2 = dataSet2.TypedDataTable1[0];

            dataRow1.DateTime = DateTime.UtcNow;
            dataRow2.DateTime = dataRow1.DateTime;

            dataSet2.AcceptChanges();

            // Act & Assert
            dataRow1.Should().BeEquivalentTo(dataRow2, config => config.Excluding(dataRow => dataRow.RowState));
        }

        [Fact]
        public void When_data_does_not_match_and_column_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataRow1 = dataSet1.TypedDataTable1[0];
            var dataRow2 = dataSet2.TypedDataTable1[0];

            dataRow2.String = Guid.NewGuid().ToString();
            dataSet2.AcceptChanges();

            // Act
            Action action = () => dataRow1.Should().BeEquivalentTo(dataRow2);

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain(dataRow2.String);
        }

        [Fact]
        public void When_data_does_not_match_and_column_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataRow1 = dataSet1.TypedDataTable1[0];
            var dataRow2 = dataSet2.TypedDataTable1[0];

            dataRow2.DateTime = DateTime.UtcNow;
            dataSet2.AcceptChanges();

            // Act & Assert
            dataRow1.Should().BeEquivalentTo(dataRow2, config => config.ExcludingColumn(dataSet2.TypedDataTable1.DateTimeColumn));
        }

        [Fact]
        public void When_data_has_column_asserting_HaveColumn_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataRow = dataSet.TypedDataTable1[0];

            // Act & Assert
            dataRow.Should().HaveColumn("ForeignRowID");
        }

        [Fact]
        public void When_data_does_not_have_column_asserting_HaveColumn_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataRow = dataSet.TypedDataTable1[0];

            // Act
            Action action =
                () => dataRow.Should().HaveColumn("Unicorn");

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_data_has_all_columns_asserting_HaveColumns_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataRow = dataSet.TypedDataTable1[0];

            var columnNames = dataRow.Table.Columns.OfType<DataColumn>()
                .Take(3)
                .Select(column => column.ColumnName);

            // Act & Assert
            dataRow.Should().HaveColumns(columnNames);
        }

        [Fact]
        public void When_data_has_only_some_columns_asserting_HaveColumns_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataRow = dataSet.TypedDataTable1[0];

            var columnNames = dataRow.Table.Columns.OfType<DataColumn>()
                .Take(3)
                .Select(column => column.ColumnName)
                .Concat(new[] { "Unicorn" });

            // Act
            Action action =
                () => dataRow.Should().HaveColumns(columnNames);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_data_has_no_matching_columns_asserting_HaveColumns_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataRow = dataSet.TypedDataTable1[0];

            var columnNames = new List<string>();

            columnNames.Add("Unicorn");
            columnNames.Add("Dragon");

            // Act
            Action action =
                () => dataRow.Should().HaveColumns(columnNames);

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
