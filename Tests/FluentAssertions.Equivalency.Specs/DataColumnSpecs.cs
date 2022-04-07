using System;
using System.Data;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class DataColumnSpecs : DataSpecs
    {
        [Fact]
        public void When_DataColumns_are_identical_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable1 = dataSet1.TypedDataTable1;
            var dataTable2 = dataSet2.TypedDataTable1;

            // Act & Assert
            dataTable1.RowIDColumn.Should().BeEquivalentTo(dataTable2.RowIDColumn);
        }

        [Fact]
        public void When_DataColumns_are_both_null_it_should_succeed()
        {
            // Act & Assert
            ((DataColumn)null).Should().BeEquivalentTo(null);
        }

        [Fact]
        public void When_DataColumn_is_null_and_isnt_expected_to_be_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>();

            var dataTable = dataSet.TypedDataTable1;

            // Act
            Action action = () => ((DataColumn)null).Should().BeEquivalentTo(dataTable.RowIDColumn);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected *to be non-null, but found null*");
        }

        [Fact]
        public void When_DataColumn_is_expected_to_be_null_and_isnt_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>();

            var dataTable = dataSet.TypedDataTable1;

            // Act
            Action action = () => dataTable.RowIDColumn.Should().BeEquivalentTo(null);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataColumn_has_changes_but_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Unique = true;
            dataColumn2.Caption = "Test";

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .ExcludingColumn(dataColumn2));
        }

        [Fact]
        public void When_DataColumn_has_changes_but_is_excluded_it_should_succeed_when_compared_via_DataTable()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable1 = dataSet1.TypedDataTable1;
            var dataTable2 = dataSet2.TypedDataTable1;

            dataTable2.DecimalColumn.Unique = true;
            dataTable2.DecimalColumn.Caption = "Test";

            // Act & Assert
            dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                .ExcludingColumn(dataTable2.DecimalColumn)
                .ExcludingRelated((DataTable dataTable) => dataTable.Constraints));
        }

        [Fact]
        public void When_DataColumn_has_changes_but_is_excluded_it_should_succeed_when_compared_via_DataSet()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataTable2 = dataSet2.TypedDataTable1;

            dataTable2.DecimalColumn.Unique = true;
            dataTable2.DecimalColumn.Caption = "Test";

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .ExcludingColumn(dataTable2.DecimalColumn)
                .ExcludingRelated((DataTable dataTable) => dataTable.Constraints));
        }

        [Fact]
        public void When_ColumnName_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.ColumnName += "different";

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_ColumnName_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.ColumnName += "different";

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.ColumnName)
                .Excluding(dataColumn => dataColumn.Caption));
        }

        [Fact]
        public void When_AllowDBNull_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.AllowDBNull = !dataColumn2.AllowDBNull;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_AllowDBNull_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.AllowDBNull = !dataColumn2.AllowDBNull;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.AllowDBNull));
        }

        [Fact]
        public void When_AutoIncrement_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.AutoIncrement = !dataColumn2.AutoIncrement;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_AutoIncrement_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.AutoIncrement = !dataColumn2.AutoIncrement;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.AutoIncrement));
        }

        [Fact]
        public void When_AutoIncrementSeed_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.AutoIncrementSeed++;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_AutoIncrementSeed_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.AutoIncrementSeed++;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.AutoIncrementSeed));
        }

        [Fact]
        public void When_AutoIncrementStep_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.AutoIncrementStep++;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_AutoIncrementStep_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.AutoIncrementStep++;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.AutoIncrementStep));
        }

        [Fact]
        public void When_Caption_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Caption += "different";

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Caption_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Caption += "different";

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.Caption));
        }

        [Fact]
        public void When_DataType_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>(includeDummyData: false);
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable2.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable2.DecimalColumn;

            dataColumn2.DataType = typeof(double);

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataType_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>(includeDummyData: false);
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable2.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable2.DecimalColumn;

            dataColumn2.DataType = typeof(double);

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.DataType));
        }

        [Fact]
        public void When_DateTimeMode_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>(includeDummyData: false);
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable2.DateTimeColumn;
            var dataColumn2 = dataSet2.TypedDataTable2.DateTimeColumn;

            dataColumn2.DateTimeMode =
                dataColumn2.DateTimeMode == DataSetDateTime.Local
                    ? DataSetDateTime.Utc
                    : DataSetDateTime.Local;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DateTimeMode_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>(includeDummyData: false);
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable2.DateTimeColumn;
            var dataColumn2 = dataSet2.TypedDataTable2.DateTimeColumn;

            dataColumn2.DateTimeMode =
                dataColumn2.DateTimeMode == DataSetDateTime.Local
                    ? DataSetDateTime.Utc
                    : DataSetDateTime.Local;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.DateTimeMode));
        }

        [Fact]
        public void When_DefaultValue_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.DefaultValue = 10M;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DefaultValue_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.DefaultValue = 10M;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.DefaultValue));
        }

        [Fact]
        public void When_Expression_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Expression = "RowID";

            // Act
            Action action = () =>
                dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                    .Excluding(dataColumn => dataColumn.ReadOnly));

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Expression_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Expression = "RowID";

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.Expression)
                .Excluding(dataColumn => dataColumn.ReadOnly));
        }

        [Fact]
        public void When_MaxLength_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.StringColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.StringColumn;

            dataColumn2.MaxLength = 250;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_MaxLength_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.StringColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.StringColumn;

            dataColumn2.MaxLength = 250;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.MaxLength));
        }

        [Fact]
        public void When_Namespace_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Namespace += "different";

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Namespace_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Namespace += "different";

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.Namespace));
        }

        [Fact]
        public void When_Prefix_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Prefix += "different";

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Prefix_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Prefix += "different";

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.Prefix));
        }

        [Fact]
        public void When_ReadOnly_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.ReadOnly = !dataColumn2.ReadOnly;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_ReadOnly_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.ReadOnly = !dataColumn2.ReadOnly;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.ReadOnly));
        }

        [Fact]
        public void When_Unique_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Unique = !dataColumn2.Unique;

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Unique_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            var dataColumn1 = dataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = dataSet2.TypedDataTable1.DecimalColumn;

            dataColumn2.Unique = !dataColumn2.Unique;

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2, options => options
                .Excluding(dataColumn => dataColumn.Unique));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_ExtendedProperties_do_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataColumn1 = typedDataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = typedDataSet2.TypedDataTable1.DecimalColumn;

            ApplyChange(dataColumn2.ExtendedProperties, changeType);

            // Act
            Action action = () => dataColumn1.Should().BeEquivalentTo(dataColumn2);

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

            var dataColumn1 = typedDataSet1.TypedDataTable1.DecimalColumn;
            var dataColumn2 = typedDataSet2.TypedDataTable1.DecimalColumn;

            ApplyChange(dataColumn2.ExtendedProperties, changeType);

            // Act & Assert
            dataColumn1.Should().BeEquivalentTo(dataColumn2,
                options => options.Excluding(dataColumn => dataColumn.ExtendedProperties));
        }

        [Fact]
        public void Data_column_is_not_equivalent_to_another_type()
        {
            // Arrange
            var subject = new
            {
                DataColumn = "foobar"
            };

            var expected = new
            {
                DataColumn = new DataColumn()
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*System.Data.DataColumn*found System.String*");
        }
    }
}
