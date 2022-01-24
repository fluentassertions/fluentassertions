using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class DataSetSpecs : DataSpecs
    {
        [Fact]
        public void When_DataSets_are_identical_it_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2);
        }

        [Fact]
        public void When_DataSets_are_both_null_it_should_succeed()
        {
            // Act & Assert
            ((DataSet)null).Should().BeEquivalentTo(null);
        }

        [Fact]
        public void When_DataSet_is_null_and_isnt_expected_to_be_it_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSet = typedDataSet.ToUntypedDataSet();

            // Act
            Action action = () => ((DataSet)null).Should().BeEquivalentTo(dataSet);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected *to be non-null, but found null*");
        }

        [Fact]
        public void When_DataSet_is_expected_to_be_null_and_isnt_it_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSet = typedDataSet.ToUntypedDataSet();

            // Act
            Action action = () => dataSet.Should().BeEquivalentTo(null);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataSet_type_does_not_match_and_AllowMismatchedType_not_enabled_it_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSet = typedDataSet.ToUntypedDataSet();

            var dataSetOfMismatchedType = new TypedDataSetSubclass(typedDataSet);

            // Act
            Action action = () => dataSet.Should().BeEquivalentTo(dataSetOfMismatchedType);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataSet_type_does_not_match_and_AllowMismatchedType_is_enabled_it_should_succeed()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSet = typedDataSet.ToUntypedDataSet();

            var dataSetOfMismatchedType = new TypedDataSetSubclass(typedDataSet);

            // Act & Assert
            dataSet.Should().BeEquivalentTo(dataSetOfMismatchedType, options => options
                .AllowingMismatchedTypes()
                .Excluding(dataSet => dataSet.SchemaSerializationMode));
        }

        [Fact]
        public void When_DataSetName_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.DataSetName += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_DataSetName_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.DataSetName += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.DataSetName)
                .ExcludingRelated((DataRelation dataRelation) => dataRelation.DataSet));
        }

        [Fact]
        public void When_CaseSensitive_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.CaseSensitive = !typedDataSet2.CaseSensitive;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

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

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.CaseSensitive));
        }

        [Fact]
        public void When_EnforceConstraints_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.EnforceConstraints = !typedDataSet2.EnforceConstraints;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_EnforceConstraints_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.EnforceConstraints = !typedDataSet2.EnforceConstraints;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.EnforceConstraints));
        }

        [Fact]
        public void When_HasErrors_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.TypedDataTable1.Rows[0].RowError = "Manually added error";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2, config => config.ExcludingTables("TypedDataTable1"));

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("HasErrors");
        }

        [Fact]
        public void When_HasErrors_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.TypedDataTable1.Rows[0].RowError = "Manually added error";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2,
                config => config.Excluding(dataSet => dataSet.HasErrors).ExcludingTables("TypedDataTable1"));
        }

        [Fact]
        public void When_Locale_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Locale = new CultureInfo("fr-CA");

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

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

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.Locale));
        }

        [Fact]
        public void When_Namespace_does_not_match_and_property_is_not_excluded_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Namespace += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

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

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

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
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Prefix += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Prefix_does_not_match_and_property_is_excluded_it_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Prefix += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.Prefix));
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

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

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

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.RemotingFormat)
                .ExcludingRelated((DataTable dataTable) => dataTable.RemotingFormat));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_ExtendedProperties_do_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            ApplyChange(typedDataSet2.ExtendedProperties, changeType);

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

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
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            ApplyChange(typedDataSet2.ExtendedProperties, changeType);

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.ExtendedProperties));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "All enum values are accounted for.")]
        public void When_Relations_does_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
        {
            // Arrange
            TypedDataSetSubclass typedDataSet1;
            TypedDataSetSubclass typedDataSet2;

            if (changeType == ChangeType.Changed)
            {
                typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

                typedDataSet2.Relations[0].RelationName += "different";
            }
            else
            {
                var doesNotHaveRelation = CreateDummyDataSet<TypedDataSetSubclass>(includeRelation: false);
                var hasRelation = new TypedDataSetSubclass(doesNotHaveRelation);

                AddRelation(hasRelation);

                if (changeType == ChangeType.Added)
                {
                    typedDataSet1 = doesNotHaveRelation;
                    typedDataSet2 = hasRelation;
                }
                else
                {
                    typedDataSet1 = hasRelation;
                    typedDataSet2 = doesNotHaveRelation;
                }
            }

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

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
            TypedDataSetSubclass typedDataSet1;
            TypedDataSetSubclass typedDataSet2;

            if (changeType == ChangeType.Changed)
            {
                typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

                typedDataSet2.Relations[0].RelationName += "different";
            }
            else
            {
                var doesNotHaveRelation = CreateDummyDataSet<TypedDataSetSubclass>(includeRelation: false);
                var hasRelation = new TypedDataSetSubclass(doesNotHaveRelation);

                AddRelation(hasRelation);

                if (changeType == ChangeType.Added)
                {
                    typedDataSet1 = doesNotHaveRelation;
                    typedDataSet2 = hasRelation;
                }
                else
                {
                    typedDataSet1 = hasRelation;
                    typedDataSet2 = doesNotHaveRelation;
                }
            }

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.Relations)
                .ExcludingRelated((DataTable dataTable) => dataTable.Constraints)
                .ExcludingRelated((DataTable dataTable) => dataTable.ParentRelations)
                .ExcludingRelated((DataTable dataTable) => dataTable.ChildRelations)
                .ExcludingRelated((DataColumn dataColumn) => dataColumn.Unique));
        }

        [Fact]
        public void When_Tables_are_the_same_but_in_a_different_order_it_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, swapTableOrder: true);

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2);
        }

        [Fact]
        public void When_Tables_count_does_not_match_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, swapTableOrder: true);

            typedDataSet2.Tables.Add(new DataTable("ThirdWheel"));

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("to contain " + dataSet2.Tables.Count);
        }

        [Fact]
        public void When_Tables_count_matches_but_tables_are_different_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, swapTableOrder: true);

            typedDataSet2.TypedDataTable2.TableName = "DifferentTableName";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_Tables_contain_different_data_it_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, swapTableOrder: true);

            typedDataSet2.TypedDataTable2[0].Guid = Guid.NewGuid();

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>();
        }
    }
}
