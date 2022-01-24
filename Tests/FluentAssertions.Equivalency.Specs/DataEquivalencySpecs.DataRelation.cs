using System;
using System.Data;

using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    /// <summary>
    /// DataRelationEquivalency specs.
    /// </summary>
    public partial class DataEquivalencySpecs
    {
        public class DataRelationEquivalencySpecs : DataEquivalencySpecs
        {
            [Fact]
            public void When_RelationName_does_not_match_and_property_is_not_excluded_it_should_fail()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                dataSet2.Relations[0].RelationName += "different";

                // Act
                Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_RelationName_does_not_match_and_property_is_excluded_it_should_succeed()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                dataSet2.Relations[0].RelationName += "different";

                // Act & Assert
                dataTable1.Should().BeEquivalentTo(
                    dataTable2,
                    options => options
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.RelationName));
            }

            [Fact]
            public void When_Nested_does_not_match_and_property_is_not_excluded_it_should_fail()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                dataSet2.Relations[0].Nested = !dataSet2.Relations[0].Nested;

                // Act
                Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2);

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_Nested_does_not_match_and_property_is_excluded_it_should_succeed()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                dataSet2.Relations[0].Nested = !dataSet2.Relations[0].Nested;

                // Act & Assert
                dataTable1.Should().BeEquivalentTo(
                    dataTable2,
                    options => options
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.Nested));
            }

            [Fact]
            public void When_DataSet_does_not_match_and_property_is_not_excluded_it_should_fail()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                dataSet2.DataSetName += "different";

                // Act
                Action action = () => dataTable1.Should().BeEquivalentTo(dataTable2, options => options.Excluding(dataTable => dataTable.DataSet));

                // Assert
                action.Should().Throw<XunitException>();
            }

            [Fact]
            public void When_DataSet_does_not_match_and_property_is_excluded_it_should_succeed()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>(identicalTables: true);
                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                dataSet2.DataSetName += "different";

                // Act & Assert
                dataTable1.Should().BeEquivalentTo(
                    dataTable2,
                    options => options
                    .Excluding(dataTable => dataTable.DataSet)
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.DataSet));
            }

            [Theory]
            [MemberData(nameof(AllChangeTypes))]
            public void When_ExtendedProperties_do_not_match_and_property_is_not_excluded_it_should_fail(ChangeType changeType)
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                ApplyChange(dataTable2.ChildRelations[0].ExtendedProperties, changeType);

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
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                ApplyChange(dataTable2.ChildRelations[0].ExtendedProperties, changeType);

                // Act & Assert
                dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.ExtendedProperties));
            }

            [Fact]
            public void When_ParentColumns_do_not_match_and_property_is_not_excluded_it_should_fail()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable2;
                var dataTable2 = dataSet2.TypedDataTable2;

                dataSet2.TypedDataTable1.RowIDColumn.ColumnName += "different";

                // Act
                Action action = () =>
                    dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                        .Excluding(dataTable => dataTable.ChildRelations)
                        .Excluding(dataTable => dataTable.Constraints));

                // Assert
                action.Should().Throw<XunitException>().Which.Message.Should().Contain(dataSet2.TypedDataTable1.Columns[0].ColumnName);
            }

            [Fact]
            public void When_ParentColumns_do_not_match_and_property_is_excluded_it_should_succeed()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable2;
                var dataTable2 = dataSet2.TypedDataTable2;

                dataSet2.TypedDataTable1.RowIDColumn.ColumnName += "different";

                // Act & Assert
                dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                    .Excluding(dataTable => dataTable.ChildRelations)
                    .Excluding(dataTable => dataTable.Constraints)
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.ParentColumns)
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.ParentKeyConstraint)
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.ChildKeyConstraint));
            }

            [Fact]
            public void When_ChildColumns_do_not_match_and_property_is_not_excluded_it_should_fail()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                dataSet2.TypedDataTable2.ForeignRowIDColumn.ColumnName += "different";

                // Act
                Action action = () =>
                    dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                        .Excluding(dataTable => dataTable.ParentRelations)
                        .Excluding(dataTable => dataTable.Constraints));

                // Assert
                action.Should().Throw<XunitException>().Which.Message.Should().Contain(dataSet2.TypedDataTable1.Columns[0].ColumnName);
            }

            [Fact]
            public void When_ChildColumns_do_not_match_and_property_is_excluded_it_should_succeed()
            {
                // Arrange
                var dataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

                var dataSet2 = new TypedDataSetSubclass(dataSet1);

                var dataTable1 = dataSet1.TypedDataTable1;
                var dataTable2 = dataSet2.TypedDataTable1;

                dataSet2.TypedDataTable2.ForeignRowIDColumn.ColumnName += "different";

                // Act & Assert
                dataTable1.Should().BeEquivalentTo(dataTable2, options => options
                    .Excluding(dataTable => dataTable.ParentRelations)
                    .Excluding(dataTable => dataTable.Constraints)
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.ChildColumns)
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.ParentKeyConstraint)
                    .ExcludingRelated((DataRelation dataRelation) => dataRelation.ChildKeyConstraint));
            }
        }
    }
}
