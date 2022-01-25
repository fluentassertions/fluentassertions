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
            public void When_DataTables_are_identical_it_should_succeed()
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
            public void When_DataTables_are_both_null_it_should_succeed()
            {
                // Act & Assert
                ((DataTable)null).Should().BeEquivalentTo(null);
            }

            [Fact]
            public void When_DataTable_is_null_and_isnt_expected_to_be_it_should_fail()
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
            public void When_DataTable_is_expected_to_be_null_and_isnt_it_should_fail()
            {
                // Arrange
                var typedDataSet = CreateDummyDataSet<TypedDataSet>();

                var dataTable = typedDataSet.ToUntypedDataSet().Tables["TypedDataTable1"];

                // Act
                Action action = () => dataTable.Should().BeEquivalentTo(null);

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_DataTable_type_does_not_match_and_AllowMismatchedType_not_enabled_it_should_fail()
            {
                // Arrange
                var typedDataSet = CreateDummyDataSet<TypedDataSet>(identicalTables: true);
                var typedDataSet2 = new TypedDataSetSubclass(typedDataSet);

                var dataTable = typedDataSet.ToUntypedDataSet().Tables["TypedDataTable1"];
                var dataTableOfMismatchedType = typedDataSet2.TypedDataTable1;

                // Act
                Action action = () => dataTable.Should().BeEquivalentTo(dataTableOfMismatchedType);

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_DataTable_type_does_not_match_and_AllowMismatchedType_is_enabled_it_should_succeed()
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
            public void When_TableName_does_not_match_and_property_is_not_excluded_it_should_fail()
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
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_TableName_does_not_match_and_property_is_excluded_it_should_succeed()
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
            public void When_CaseSensitive_does_not_match_and_property_is_not_excluded_it_should_fail()
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
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_CaseSensitive_does_not_match_and_property_is_excluded_it_should_succeed()
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
            public void When_DisplayExpression_does_not_match_and_property_is_not_excluded_it_should_fail()
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
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_DisplayExpression_does_not_match_and_property_is_excluded_it_should_succeed()
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
            public void When_HasErrors_does_not_match_and_property_is_not_excluded_it_should_fail()
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
            public void When_HasErrors_does_not_match_and_property_is_excluded_it_should_succeed()
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
            public void When_Locale_does_not_match_and_property_is_not_excluded_it_should_fail()
            {
                // Arrange
                var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

                typedDataSet2.Locale = new CultureInfo("fr-CA");

                var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
                var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

                // Act
                Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_Locale_does_not_match_and_property_is_excluded_it_should_succeed()
            {
                // Arrange
                var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

                typedDataSet2.Locale = new CultureInfo("fr-CA");

                var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
                var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

                // Act & Assert
                dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.Locale));
            }

            [Fact]
            public void When_Namespace_does_not_match_and_property_is_not_excluded_it_should_fail()
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
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_Namespace_does_not_match_and_property_is_excluded_it_should_succeed()
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
            public void When_Prefix_does_not_match_and_property_is_not_excluded_it_should_fail()
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
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_Prefix_does_not_match_and_property_is_excluded_it_should_succeed()
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
            public void When_RemotingFormat_does_not_match_and_property_is_not_excluded_it_should_fail()
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
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_RemotingFormat_does_not_match_and_property_is_excluded_it_should_succeed()
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
            public void When_Columns_do_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
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
                action.Should().Throw<XunitException>();
            }

            [Theory]
            [MemberData(nameof(AllChangeTypes))]
            public void When_Columns_do_not_match_and_Columns_and_Rows_are_excluded_it_should_succeed(ChangeType changeType)
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
            public void When_ExtendedProperties_do_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
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
                action.Should().Throw<XunitException>();
            }

            [Theory]
            [MemberData(nameof(AllChangeTypes))]
            public void When_ExtendedProperties_do_not_match_and_property_is_excluded_it_should_succeed(ChangeType changeType)
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
            public void When_PrimaryKey_does_not_match_and_property_is_not_excluded_it_should_fail()
            {
                // Arrange
                var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

                var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable2"];
                var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable2"];

                dataTable2.PrimaryKey = dataTable2.Columns.Cast<DataColumn>().Skip(2).ToArray();
                dataTable2.Columns[0].Unique = true;

                // Act
                Action action = () =>
                    dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                        .Excluding(dataTable => dataTable.Constraints));

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_PrimaryKey_does_not_match_and_property_is_excluded_it_should_succeed()
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
            public void When_columns_for_constraint_do_not_match_message_should_list_all_columns_involved(NumberOfColumnsInConstraintDifference difference)
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

                var dataTable1ColumnsForConstraint = dataTable1.Columns.OfType<DataColumn>()
                    .Take(dataTable1.Columns.Count - differenceCount)
                    .ToArray();

                var dataTable2ColumnsForConstraint = dataTable2.Columns.OfType<DataColumn>()
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
            public void When_Constraints_do_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
            {
                // Arrange
                var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

                var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable2"];
                var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable2"];

                ApplyChange(dataTable2.Constraints, dataTable2.Columns["Decimal"], changeType);

                // Act
                Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Theory]
            [MemberData(nameof(AllChangeTypes))]
            public void When_Constraints_do_not_match_and_property_is_excluded_it_should_succeed(ChangeType changeType)
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
            public void When_Rows_do_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType, bool acceptChanges)
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

                // Act
                Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Theory]
            [MemberData(nameof(AllChangeTypesWithAcceptChangesValues))]
            public void When_Rows_do_not_match_and_property_is_excluded_it_should_succeed(ChangeType changeType, bool acceptChanges)
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
            public void When_data_matches_in_different_order_and_RowMatchMode_is_PrimaryKey_it_should_succeed()
            {
                // Arrange
                var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, randomizeRowOrder: true);

                var dataTable1 = typedDataSet1.ToUntypedDataSet().Tables["TypedDataTable1"];
                var dataTable2 = typedDataSet2.ToUntypedDataSet().Tables["TypedDataTable1"];

                // Act & Assert
                dataTable1.Should().BeEquivalentTo(dataTable2, options => options.UsingRowMatchMode(RowMatchMode.PrimaryKey));
            }
        }
    }
