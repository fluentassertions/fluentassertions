using System;
using System.Data;
using System.Linq;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class DataRowSpecs : DataSpecs
{
    [Fact]
    public void When_data_rows_are_identical_equivalency_test_should_succeed()
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
    public void When_data_rows_are_both_null_equivalency_test_should_succeed()
    {
        // Act & Assert
        ((DataRow)null).Should().BeEquivalentTo(null);
    }

    [Fact]
    public void When_data_row_is_null_and_isnt_expected_to_be_then_equivalency_test_should_fail()
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
    public void When_data_row_is_expected_to_be_null_and_isnt_then_equivalency_test_should_fail()
    {
        // Arrange
        var dataSet = CreateDummyDataSet<TypedDataSet>();

        var dataTable = dataSet.TypedDataTable1;

        // Act
        Action action = () => dataTable[0].Should().BeEquivalentTo(null);

        // Assert
        action.Should().Throw<XunitException>().WithMessage("Expected dataTable[0] value to be null, but found *");
    }

    [Fact]
    public void
        When_data_row_subject_is_deleted_and_expectation_is_not_but_the_row_state_is_excluded_equivalency_test_should_succeed()
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
    public void
        When_data_row_expectation_is_deleted_and_subject_is_not_but_the_row_state_is_excluded_equivalency_test_should_succeed()
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
    public void When_data_row_is_deleted_equivalency_test_should_succeed()
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
    public void When_data_row_is_modified_and_original_data_differs_equivalency_test_should_fail()
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
        action.Should().Throw<XunitException>()
            .WithMessage("Expected dataRow1[Decimal, DataRowVersion.Original] to be *, but found *");
    }

    [Fact]
    public void
        When_data_row_is_modified_and_original_data_differs_but_original_data_is_excluded_then_equivalency_test_should_succeed()
    {
        // Arrange
        var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
        var dataSet2 = new TypedDataSetSubclass(dataSet1);

        var dataTable1 = dataSet1.TypedDataTable2;
        var dataTable2 = dataSet2.TypedDataTable2;

        var dataRow1 = dataTable1[0];
        var dataRow2 = dataTable2[0];

        // Set the Decimal property to be the same for both rows, but by doing ++ and -- in
        // different orders, the AcceptChanges call will load different values into the
        // original data for each row.
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
    public void When_data_row_type_does_not_match_and_mismatched_types_are_not_allowed_then_equivalency_test_should_fail()
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
        action.Should().Throw<XunitException>()
            .WithMessage("Expected dataTable[0] to be of type *TypedDataRow2, but found *TypedDataRow1*");
    }

    [Fact]
    public void When_data_row_type_does_not_match_but_mismatched_types_are_allowed_then_equivalency_test_should_succeed()
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
    public void
        When_one_data_row_has_errors_and_the_other_does_not_and_the_corresponding_property_is_not_excluded_then_equivalency_test_should_fail()
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
    public void
        When_one_data_row_has_errors_and_the_other_does_not_but_the_corresponding_property_is_excluded_then_equivalency_test_should_succeed()
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
    public void
        When_the_data_row_state_does_not_match_and_the_corresponding_property_is_not_excluded_equivalency_test_should_fail()
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
    public void
        When_the_data_row_state_does_not_match_but_the_corresponding_property_is_excluded_equivalency_test_should_succeed()
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
    public void When_data_row_data_does_not_match_and_the_column_in_question_is_not_excluded_then_equivalency_test_should_fail()
    {
        // Arrange
        var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

        var dataSet2 = new TypedDataSetSubclass(dataSet1);

        var dataRow1 = dataSet1.TypedDataTable1[0];
        var dataRow2 = dataSet2.TypedDataTable1[0];

        dataRow2.String = Guid.NewGuid().ToString().Substring(0, 15);
        dataSet2.AcceptChanges();

        // Act
        Action action = () => dataRow1.Should().BeEquivalentTo(dataRow2);

        // Assert
        action.Should().Throw<XunitException>().Which.Message.Should().Contain(dataRow2.String);
    }

    [Fact]
    public void When_data_row_data_does_not_match_but_the_column_is_excluded_then_equivalency_test_should_succeed()
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
    public void Data_row_is_not_equivalent_to_another_type()
    {
        // Arrange
        var table = new DataTable();
        var dataRow = table.NewRow();

        var subject = new
        {
            DataRow = "foobar"
        };

        var expected = new
        {
            DataRow = dataRow
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected*System.Data.DataRow*found System.String*");
    }

    [Fact]
    public void Any_type_is_not_equivalent_to_data_row_colletion()
    {
        // Arrange
        var o = new object();

        // Act
        Action act = () => o.Should().BeEquivalentTo((DataRowCollection)null);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected* to be of type DataRowCollection, but found*");
    }

    [Fact]
    public void When_data_row_has_column_then_asserting_that_it_has_that_column_should_succeed()
    {
        // Arrange
        var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

        var dataRow = dataSet.TypedDataTable1[0];

        string expectedColumnName = dataSet.TypedDataTable1.Columns.Cast<DataColumn>().Last().ColumnName;

        // Act & Assert
        dataRow.Should().HaveColumn(expectedColumnName);
    }

    [Fact]
    public void When_data_row_does_not_have_column_then_asserting_that_it_has_that_column_should_fail()
    {
        // Arrange
        var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

        var dataRow = dataSet.TypedDataTable1[0];

        // Act
        Action action =
            () => dataRow.Should().HaveColumn("Unicorn");

        // Assert
        action.Should().Throw<XunitException>().WithMessage("Expected dataRow to contain a column named *Unicorn*");
    }

    [Fact]
    public void Null_data_row_does_not_have_column()
    {
        // Arrange
        var dataRow = (DataRow)null;

        // Act
        Action action =
            () => dataRow.Should().HaveColumn("Does not matter");

        // Assert
        action.Should().Throw<XunitException>()
            .WithMessage("Expected dataRow to contain a column named *Does not matter*, but found <null>*");
    }

    [Fact]
    public void When_data_row_data_has_all_columns_being_asserted_then_it_should_succeed()
    {
        // Arrange
        var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

        var dataRow = dataSet.TypedDataTable1[0];

        var subsetOfColumnNames = dataRow.Table.Columns.Cast<DataColumn>()
            .Take(dataRow.Table.Columns.Count - 2)
            .Select(column => column.ColumnName);

        // Act & Assert
        dataRow.Should().HaveColumns(subsetOfColumnNames);
    }

    [Fact]
    public void Data_row_with_all_colums_asserted_and_using_the_array_overload_passes()
    {
        // Arrange
        var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

        var dataRow = dataSet.TypedDataTable1[0];

        string[] subsetOfColumnNames = dataRow.Table.Columns.Cast<DataColumn>()
            .Take(dataRow.Table.Columns.Count - 2)
            .Select(column => column.ColumnName)
            .ToArray();

        // Act & Assert
        dataRow.Should().HaveColumns(subsetOfColumnNames);
    }

    [Fact]
    public void Null_data_row_and_using_the_array_overload_fails()
    {
        // Arrange
        var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

        var dataRow = dataSet.TypedDataTable1[0];

        var actual = (DataRow)null;

        string[] subsetOfColumnNames = dataRow.Table.Columns.Cast<DataColumn>()
            .Take(dataRow.Table.Columns.Count - 2)
            .Select(column => column.ColumnName)
            .ToArray();

        // Act
        Action act = () =>
        {
            using var _ = new AssertionScope();
            actual.Should().HaveColumns(subsetOfColumnNames);
        };

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected actual to be in a table containing *column*, but found <null>*");
    }

    [Fact]
    public void When_data_row_data_has_only_some_of_the_columns_being_asserted_then_it_should_fail()
    {
        // Arrange
        var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

        var dataRow = dataSet.TypedDataTable1[0];

        var subsetOfColumnNamesWithUnicorn = dataRow.Table.Columns.Cast<DataColumn>()
            .Take(dataRow.Table.Columns.Count - 2)
            .Select(column => column.ColumnName)
            .Concat(new[] { "Unicorn" });

        // Act
        Action action =
            () => dataRow.Should().HaveColumns(subsetOfColumnNamesWithUnicorn);

        // Assert
        action.Should().Throw<XunitException>()
            .WithMessage("Expected table containing dataRow to contain a column named *Unicorn*");
    }

    [Fact]
    public void When_data_row_data_has_none_of_the_columns_being_asserted_then_it_should_fail()
    {
        // Arrange
        var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

        var dataRow = dataSet.TypedDataTable1[0];

        var columnNames = new[] { "Unicorn", "Dragon" };

        // Act
        Action action =
            () => dataRow.Should().HaveColumns(columnNames);

        // Assert
        action.Should().Throw<XunitException>()
            .WithMessage("Expected table containing dataRow to contain a column named *Unicorn*");
    }
}
