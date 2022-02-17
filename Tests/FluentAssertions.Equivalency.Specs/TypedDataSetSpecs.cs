using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class TypedDataSetSpecs : DataSpecs
    {
        [Fact]
        public void When_DataSets_are_identical_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2);
        }

        [Fact]
        public void When_DataSets_are_both_null_it_should_succeed()
        {
            // Act & Assert
            ((TypedDataSet)null).Should().BeEquivalentTo(null);
        }

        [Fact]
        public void When_DataSet_is_null_and_isnt_expected_to_be_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>();

            // Act
            Action action = () => ((TypedDataSet)null).Should().BeEquivalentTo(dataSet);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected *to be non-null, but found null*");
        }

        [Fact]
        public void When_DataSet_is_expected_to_be_null_and_isnt_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>();

            // Act
            Action action = () => dataSet.Should().BeEquivalentTo(null);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataSet_type_does_not_match_and_AllowMismatchedType_not_enabled_it_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSetOfMismatchedType = new TypedDataSetSubclass(dataSet);

            // Act
            Action action = () => dataSet.Should().BeEquivalentTo(dataSetOfMismatchedType);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataSet_type_does_not_match_and_AllowMismatchedType_is_enabled_it_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSetOfMismatchedType = new TypedDataSetSubclass(dataSet);

            // Act & Assert
            dataSet.Should().BeEquivalentTo(dataSetOfMismatchedType, options => options.AllowingMismatchedTypes());
        }

        [Fact]
        public void When_DataSetName_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.DataSetName += "different";

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataSetName_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.DataSetName += "different";

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.DataSetName)
                .ExcludingRelated((DataRelation dataRelation) => dataRelation.DataSet));
        }

        [Fact]
        public void When_CaseSensitive_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.CaseSensitive = !dataSet2.CaseSensitive;

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_CaseSensitive_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.CaseSensitive = !dataSet2.CaseSensitive;

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.CaseSensitive));
        }

        [Fact]
        public void When_EnforceConstraints_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.EnforceConstraints = !dataSet2.EnforceConstraints;

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_EnforceConstraints_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.EnforceConstraints = !dataSet2.EnforceConstraints;

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.EnforceConstraints));
        }

        [Fact]
        public void When_HasErrors_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.TypedDataTable1.Rows[0].RowError = "Manually added error";

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2, config => config.ExcludingTables("TypedDataTable1"));

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("HasErrors");
        }

        [Fact]
        public void When_HasErrors_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.TypedDataTable1.Rows[0].RowError = "Manually added error";

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2,
                config => config.Excluding(dataSet => dataSet.HasErrors).ExcludingTables("TypedDataTable1"));
        }

        [Fact]
        public void When_Locale_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.Locale = new CultureInfo("fr-CA");

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Locale_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.Locale = new CultureInfo("fr-CA");

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.Locale));
        }

        [Fact]
        public void When_Namespace_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.Namespace += "different";

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Namespace_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.Namespace += "different";

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.Namespace)
                .ExcludingRelated((DataTable dataTable) => dataTable.Namespace)
                .ExcludingRelated((DataColumn dataColumn) => dataColumn.Namespace));
        }

        [Fact]
        public void When_Prefix_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.Prefix += "different";

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Prefix_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.Prefix += "different";

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.Prefix));
        }

        [Fact]
        public void When_RemotingFormat_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.RemotingFormat =
                (dataSet2.RemotingFormat == SerializationFormat.Binary)
                    ? SerializationFormat.Xml
                    : SerializationFormat.Binary;

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_RemotingFormat_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.RemotingFormat =
                (dataSet2.RemotingFormat == SerializationFormat.Binary)
                    ? SerializationFormat.Xml
                    : SerializationFormat.Binary;

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.RemotingFormat)
                .ExcludingRelated((DataTable dataTable) => dataTable.RemotingFormat));
        }

        [Fact]
        public void When_SchemaSerializationMode_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.SchemaSerializationMode =
                (dataSet2.SchemaSerializationMode == SchemaSerializationMode.ExcludeSchema)
                    ? SchemaSerializationMode.IncludeSchema
                    : SchemaSerializationMode.ExcludeSchema;

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_SchemaSerializationMode_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            dataSet2.SchemaSerializationMode =
                (dataSet2.SchemaSerializationMode == SchemaSerializationMode.ExcludeSchema)
                    ? SchemaSerializationMode.IncludeSchema
                    : SchemaSerializationMode.ExcludeSchema;

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.SchemaSerializationMode));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_ExtendedProperties_do_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            ApplyChange(dataSet2.ExtendedProperties, changeType);

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_ExtendedProperties_do_not_match_and_property_is_excluded_it_should_succeed(ChangeType changeType)
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            ApplyChange(dataSet2.ExtendedProperties, changeType);

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.ExtendedProperties));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "All enum values are accounted for.")]
        public void When_Relations_does_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            switch (changeType)
            {
                case ChangeType.Added:
                    dataSet1.Relations.RemoveAt(0);
                    break;

                case ChangeType.Changed:
                    dataSet2.Relations[0].RelationName += "different";
                    break;

                case ChangeType.Removed:
                    dataSet2.Relations.RemoveAt(0);
                    break;
            }

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "All enum values are accounted for.")]
        public void When_Relations_does_not_match_and_property_is_excluded_it_should_succeed(ChangeType changeType)
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1);

            switch (changeType)
            {
                case ChangeType.Added:
                    dataSet1.Relations.RemoveAt(0);
                    break;

                case ChangeType.Changed:
                    dataSet2.Relations[0].RelationName += "different";
                    break;

                case ChangeType.Removed:
                    dataSet2.Relations.RemoveAt(0);
                    break;
            }

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.Relations)
                .ExcludingRelated((DataTable dataTable) => dataTable.ParentRelations)
                .ExcludingRelated((DataTable dataTable) => dataTable.ChildRelations));
        }

        [Fact]
        public void When_Tables_are_the_same_but_in_a_different_order_it_should_succeed()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1, swapTableOrder: true);

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2);
        }

        [Fact]
        public void When_Tables_count_does_not_match_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1, swapTableOrder: true);

            dataSet2.Tables.Add(new DataTable("ThirdWheel"));

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("to contain " + dataSet2.Tables.Count);
        }

        [Fact]
        public void When_Tables_count_matches_but_tables_are_different_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1, swapTableOrder: true);

            dataSet2.TypedDataTable2.TableName = "DifferentTableName";

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Tables_contain_different_data_it_should_fail()
        {
            // Arrange
            var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var dataSet2 = new TypedDataSetSubclass(dataSet1, swapTableOrder: true);

            dataSet2.TypedDataTable2[0].Guid = Guid.NewGuid();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
