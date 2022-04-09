using System;
using System.Data;
using System.Globalization;
using System.Linq;
using FluentAssertions.Data;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class DataTableSpecs : DataSpecs
    {
        [Fact]
        public void When_data_tables_are_identical_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2);
        }

        [Fact]
        public void When_data_tables_are_both_null_equivalence_test_should_succeed()
        {
            // Act & Assert
            ((DataTable)null).Should().BeEquivalentTo(null);
        }

        [Fact]
        public void When_data_table_is_null_and_isnt_expected_to_be_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataTable = typedDataSet.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act
            Action action = () => ((DataTable)null).Should().BeEquivalentTo(dataTable);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected *to be non-null, but found null*");
        }

        [Fact]
        public void When_data_table_is_expected_to_be_null_and_isnt_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataTable = typedDataSet.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act
            Action action = () => dataTable.Should().BeEquivalentTo(null);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable value to be null, but found TypedDataTable1*");
        }

        [Fact]
        public void When_data_table_type_does_not_match_and_assertion_is_not_configured_to_allow_mismatched_types_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>(identicalTables: true);
            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet);

            var dataTable = typedDataSet.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTableOfMismatchedType = typedDataSet2.TypedDataTable1;

            // Act
            Action action = () => dataTable.Should().BeEquivalentTo(dataTableOfMismatchedType);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable to be of type *TypedDataTable1*, but found *System.Data.DataTable*");
        }

        [Fact]
        public void When_data_table_type_does_not_match_but_assertion_is_configured_to_allow_mismatched_types_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>(identicalTables: true);
            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet);

            var dataTable = typedDataSet.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTableOfMismatchedType = typedDataSet2.TypedDataTable1;

            // Act & Assert
            dataTable.Should().BeEquivalentTo(dataTableOfMismatchedType, options => options.AllowingMismatchedTypes());
        }

        [Fact]
        public void When_data_table_name_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            dataTable2.TableName += "different";

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable1 to have TableName *different*, but found *TypedDataTable1* instead*");
        }

        [Fact]
        public void When_data_table_name_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>(identicalTables: true);
            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            dataTable2.TableName += "different";

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(
                dataTable2,
                options => options
                .Excluding(dataTable => dataTable.TableName)
                .ExcludingRelated((DataColumn dataColumn) => dataColumn.Table)
                .ExcludingRelated((Constraint constraint) => constraint.Table));
        }

        [Fact]
        public void When_data_table_case_sensitivity_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.CaseSensitive = !typedDataSet2.CaseSensitive;

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable1 to have CaseSensitive value of True, but found False instead*");
        }

        [Fact]
        public void When_data_table_case_sensitivity_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.CaseSensitive = !typedDataSet2.CaseSensitive;

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.CaseSensitive));
        }

        [Fact]
        public void When_data_table_display_expression_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            dataTable2.DisplayExpression = typedDataSet2.TypedDataTable1.StringColumn.ColumnName;

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable1 to have DisplayExpression value of *String*");
        }

        [Fact]
        public void When_data_table_display_expression_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            dataTable2.DisplayExpression = typedDataSet2.TypedDataTable1.StringColumn.ColumnName;

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.DisplayExpression));
        }

        [Fact]
        public void When_one_data_table_has_errors_and_the_other_does_not_and_the_property_that_indicates_the_presence_of_errors_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            dataTable2.Rows[0].RowError = "Manually added error";

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("HasErrors");
        }

        [Fact]
        public void When_one_data_table_has_errors_and_the_other_does_not_but_the_property_that_indicates_the_presence_of_errors_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            dataTable2.Rows[0].RowError = "Manually added error";

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, config => config
                .Excluding(dataTable => dataTable.HasErrors)
                .ExcludingRelated((DataRow dataRow) => dataRow.HasErrors));
        }

        [Fact]
        public void When_data_table_locale_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet1.Locale = new CultureInfo("en-US");
            typedDataSet2.Locale = new CultureInfo("fr-CA");

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable1 to have Locale value of *fr-CA*, but found *en-US* instead*");
        }

        [Fact]
        public void When_data_table_locale_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet1.Locale = new CultureInfo("en-US");
            typedDataSet2.Locale = new CultureInfo("fr-CA");

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.Locale));
        }

        [Fact]
        public void When_data_table_namespace_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Namespace += "different";

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable1 to have Namespace value of *different*, but found *");
        }

        [Fact]
        public void When_data_table_namespace_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Namespace += "different";

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                .Excluding(dataTable => dataTable.Namespace)
                .ExcludingRelated((DataColumn dataColumn) => dataColumn.Namespace));
        }

        [Fact]
        public void When_data_table_prefix_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            dataTable2.Prefix += "different";

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable1 to have Prefix value of *different*, but found * instead*");
        }

        [Fact]
        public void When_data_table_prefix_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            dataTable2.Prefix += "different";

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.Prefix));
        }

        [Fact]
        public void When_data_table_remoting_format_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.RemotingFormat =
                (typedDataSet2.RemotingFormat == SerializationFormat.Binary)
                ? SerializationFormat.Xml
                : SerializationFormat.Binary;

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable1 to have RemotingFormat value of *Binary*, but found *Xml* instead*");
        }

        [Fact]
        public void When_data_table_remoting_format_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.RemotingFormat =
                (typedDataSet2.RemotingFormat == SerializationFormat.Binary)
                ? SerializationFormat.Xml
                : SerializationFormat.Binary;

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // LAST ONE

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.RemotingFormat));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_data_table_columns_do_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            ApplyChange(dataTable2.Columns, changeType);

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected property dataTable1.Column* to be *, but *");
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_data_table_columns_do_not_match_but_columns_and_rows_are_excluded_equivalence_test_should_succeed(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            ApplyChange(dataTable2.Columns, changeType);

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                .Excluding(dataTable => dataTable.Columns)
                .Excluding(dataTable => dataTable.Rows));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_data_table_extended_properties_do_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            ApplyChange(dataTable2.ExtendedProperties, changeType);

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected *dataTable1.ExtendedProperties* to be *, but *");
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_data_table_extended_properties_do_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            ApplyChange(dataTable2.ExtendedProperties, changeType);

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.ExtendedProperties));
        }

        [Fact]
        public void When_data_table_primary_key_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable2"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable2"];

            dataTable1.Columns.Cast<DataColumn>().Skip(2).ToList()
                .ForEach(col => col.AllowDBNull = false);

            dataTable2.PrimaryKey = dataTable2.Columns.Cast<DataColumn>().Skip(2).ToArray();
            dataTable2.Columns[0].Unique = true;

            // Act
            Action action = () =>
                dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                    .Excluding(dataTable => dataTable.Constraints));

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected property dataTable1.PrimaryKey to be a collection with * item(s), but *contains * item(s) less than*");
        }

        [Fact]
        public void When_data_table_primary_key_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            for (int i = 2; i < typedDataSet1.TypedDataTable2.Columns.Count; i++)
            {
                typedDataSet1.TypedDataTable2.Columns[i].AllowDBNull = false;
            }

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable2"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable2"];

            dataTable2.PrimaryKey = dataTable2.Columns.Cast<DataColumn>().Skip(2).ToArray();
            dataTable2.Columns[0].Unique = true;

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                .Excluding(dataTable => dataTable.PrimaryKey)
                .Excluding(dataTable => dataTable.Constraints));
        }

        public enum NumberOfColumnsInConstraintDifference
        {
            SingleColumn,
            MultipleColumns,
        }

        [Theory]
        [InlineData(NumberOfColumnsInConstraintDifference.SingleColumn)]
        [InlineData(NumberOfColumnsInConstraintDifference.MultipleColumns)]
        public void When_columns_for_constraint_in_data_table_do_not_match_message_should_list_all_columns_involved(NumberOfColumnsInConstraintDifference difference)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable2"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable2"];

            int differenceCount =
                difference switch
                {
                    NumberOfColumnsInConstraintDifference.SingleColumn => 1,
                    NumberOfColumnsInConstraintDifference.MultipleColumns => 2,

                    _ => throw new Exception("Sanity failure")
                };

            var dataTable1ColumnsForConstraint = dataTable1.Columns.Cast<DataColumn>()
                .Take(dataTable1.Columns.Count - differenceCount)
                .ToArray();

            var dataTable2ColumnsForConstraint = dataTable2.Columns.Cast<DataColumn>()
                .Skip(differenceCount)
                .ToArray();

            const string ConstraintName = "TestSubjectConstraint";

            dataTable1.Constraints.Add(new UniqueConstraint(ConstraintName, dataTable1ColumnsForConstraint));
            dataTable2.Constraints.Add(new UniqueConstraint(ConstraintName, dataTable2ColumnsForConstraint));

            var missingColumnNames = dataTable2ColumnsForConstraint.Select(col => col.ColumnName)
                .Except(dataTable1ColumnsForConstraint.Select(col => col.ColumnName));
            var extraColumnNames = dataTable1ColumnsForConstraint.Select(col => col.ColumnName)
                .Except(dataTable2ColumnsForConstraint.Select(col => col.ColumnName));

            string columnsNoun = differenceCount == 1
                ? "column"
                : "columns";

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                $"Expected *{ConstraintName}* to include {columnsNoun} {string.Join("*", missingColumnNames)}*" +
                $"Did not expect *{ConstraintName}* to include {columnsNoun} {string.Join("*", extraColumnNames)}*");
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_data_table_constraints_do_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable2"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable2"];

            ApplyChange(dataTable2.Constraints, dataTable2.Columns["Decimal"], changeType);

            string expectedExceptionPattern =
                changeType == ChangeType.Changed
                ? "Found unexpected constraint named *Constraint2* in property dataTable1.Constraints*"
                : "Expected property dataTable1.Columns[*].Unique to be *, but found *";

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(expectedExceptionPattern);
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_data_table_constraints_do_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable2"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable2"];

            ApplyChange(dataTable2.Constraints, dataTable2.Columns["Decimal"], changeType);

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                .Excluding(dataTable => dataTable.Constraints)
                .ExcludingRelated((DataColumn dataColumn) => dataColumn.Unique));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypesWithAcceptChangesValues))]
        public void When_data_table_rows_do_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail(ChangeType changeType, bool acceptChanges)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            ApplyChange(dataTable2.Rows, dataTable2, changeType);

            if (acceptChanges)
            {
                dataTable2.AcceptChanges();
            }

            string exceptionPattern;

            if (changeType == ChangeType.Changed)
            {
                exceptionPattern =
                    acceptChanges
                    ? "Expected dataTable1.Rows[1][String] to be *different* with a length of *, but * has a length of *, differs near *"
                    : "Expected dataTable1.Rows[1] to have RowState value of *Modified*, but found *Unchanged* instead*";
            }
            else
            {
                exceptionPattern =
                    "Expected property dataTable1.Rows to contain * row(s), but found 10*";
            }

            // Act
            Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(exceptionPattern);
        }

        [Theory]
        [MemberData(nameof(AllChangeTypesWithAcceptChangesValues))]
        public void When_data_table_rows_do_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed(ChangeType changeType, bool acceptChanges)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            ApplyChange(dataTable2.Rows, dataTable2, changeType);

            if (acceptChanges)
            {
                dataTable2.AcceptChanges();
            }

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.Rows));
        }

        [Fact]
        public void When_data_table_data_matches_in_different_order_and_the_row_match_mode_is_by_primary_key_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, randomizeRowOrder: true);

            var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
            var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options.UsingRowMatchMode(RowMatchMode.PrimaryKey));
        }

        [Fact]
        public void Data_table_is_not_equivalent_to_another_type()
        {
            // Arrange
            var subject = new
            {
                DataTable = "foobar"
            };

            var expected = new
            {
                DataTable = new DataTable()
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*System.Data.DataTable*found System.String*");
        }

        [Fact]
        public void When_data_table_has_expected_row_count_it_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataTable = dataSet.TypedDataTable1;

            int correctRowCount = dataTable.Rows.Count;

            // Act & Assert
            dataTable.Should().HaveRowCount(correctRowCount);
        }

        [Fact]
        public void When_empty_data_table_has_expected_row_count_of_zero_it_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataTable = dataSet.TypedDataTable2;

            dataTable.Rows.Clear();
            dataTable.AcceptChanges();

            // Act & Assert
            dataTable.Should().HaveRowCount(0);
        }

        [Fact]
        public void When_data_table_does_not_have_expected_row_count_does_not_match_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataTable = dataSet.TypedDataTable1;

            int correctRowCount = dataTable.Rows.Count;

            int incorrectRowCount = correctRowCount * 2;

            // Act
            Action action =
                () => dataTable.Should().HaveRowCount(incorrectRowCount);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable to contain exactly * row(s), but found *");
        }

        [Fact]
        public void When_data_table_has_expected_column_it_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataTable = dataSet.TypedDataTable1;

            var expectedColumnName = dataTable.Columns[0].ColumnName;

            // Act & Assert
            dataTable.Should().HaveColumn(expectedColumnName);
        }

        [Fact]
        public void When_data_table_does_not_have_expected_column_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataTable = dataSet.TypedDataTable1;

            // Act
            Action action =
                () => dataTable.Should().HaveColumn("Unicorn");

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable to contain a column named *Unicorn*, but it does not.");
        }

        [Fact]
        public void When_data_table_has_all_expected_columns_it_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataTable = dataSet.TypedDataTable1;

            var existingColumnNames = dataTable.Columns.Cast<DataColumn>()
                .Select(column => column.ColumnName);

            // Act & Assert
            dataTable.Should().HaveColumns(existingColumnNames);
        }

        [Fact]
        public void When_data_table_has_only_some_expected_columns_then_asserting_that_it_has_all_of_them_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataTable = dataSet.TypedDataTable1;

            var columnNames = dataTable.Columns.Cast<DataColumn>()
                .Select(column => column.ColumnName)
                .Concat(new[] { "Unicorn" });

            // Act
            Action action =
                () => dataTable.Should().HaveColumns(columnNames);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable to contain a column named *Unicorn*, but it does not.");
        }

        [Fact]
        public void When_data_table_has_none_of_the_expected_columns_then_asserting_that_it_has_all_of_them_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataTable = dataSet.TypedDataTable1;

            var nonExistingColumnNames = new[] { "Unicorn", "Dragon" };

            // Act
            Action action =
                () => dataTable.Should().HaveColumns(nonExistingColumnNames);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataTable to contain a column named *Unicorn*, but it does not.");
        }
    }
}
