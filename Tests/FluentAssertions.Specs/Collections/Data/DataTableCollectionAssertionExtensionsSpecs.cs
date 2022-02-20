using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions.Execution;

using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections.Data
{
    public class DataTableCollectionAssertionExtensionsSpecs
    {
        private DataTable CreateTestDataTable(int seed)
        {
            var table = new DataTable("Table" + seed);

            var random = new Random(seed);

            for (int i = 0; i < 3; i++)
            {
                var columnType =
                    random.Next(4) switch
                    {
                        0 => typeof(int),
                        1 => typeof(string),
                        2 => typeof(bool),
                        _ => typeof(DateTime),
                    };

                table.Columns.Add($"Column{i}", columnType);
            }

            return table;
        }

        #region BeSameAs & NotBeSameAs
        [Fact]
        public void When_testing_that_references_to_the_same_object_are_the_same_it_should_succeed()
        {
            // Arrange
            var dataSet = new DataSet();

            dataSet.Tables.Add(CreateTestDataTable(seed: 1));

            var tableCollection1 = dataSet.Tables;
            var tableCollection2 = tableCollection1;

            // Act & Assert
            tableCollection1.Should().BeSameAs(tableCollection2);
        }

        [Fact]
        public void When_testing_that_references_to_the_same_object_are_not_the_same_it_should_fail()
        {
            // Arrange
            var dataSet = new DataSet();

            dataSet.Tables.Add(CreateTestDataTable(seed: 1));

            var tableCollection1 = dataSet.Tables;
            var tableCollection2 = tableCollection1;

            // Act
            Action action =
                () => tableCollection1.Should().NotBeSameAs(tableCollection2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_testing_that_references_to_different_objects_are_the_same_it_should_fail()
        {
            // Arrange
            var dataSet1 = new DataSet();
            var dataSet2 = new DataSet();

            dataSet1.Tables.Add(CreateTestDataTable(seed: 1));
            dataSet2.Tables.Add(CreateTestDataTable(seed: 1));

            var tableCollection1 = dataSet1.Tables;
            var tableCollection2 = dataSet2.Tables;

            // Act
            Action action =
                () => tableCollection1.Should().BeSameAs(tableCollection2);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_testing_that_references_to_different_objects_are_not_the_same_it_should_succeed()
        {
            // Arrange
            var dataSet1 = new DataSet();
            var dataSet2 = new DataSet();

            dataSet1.Tables.Add(CreateTestDataTable(seed: 1));
            dataSet2.Tables.Add(CreateTestDataTable(seed: 1));

            var tableCollection1 = dataSet1.Tables;
            var tableCollection2 = dataSet2.Tables;

            // Act & Assert
            tableCollection1.Should().NotBeSameAs(tableCollection2);
        }
        #endregion

        #region HaveSameCount & NotHaveSameCount
        [Fact]
        public void When_asserting_same_count_if_expectation_is_null_it_should_fail()
        {
            // Arrange
            var dataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                dataSet.Tables.Add(CreateTestDataTable(seed));
            }

            var nullReference = default(DataTableCollection);

            // Act
            Action action =
                () => dataSet.Tables.Should().HaveSameCount(nullReference);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }

        [Fact]
        public void When_two_DataTableCollections_have_the_same_number_elements_it_should_succeed()
        {
            // Arrange
            var firstDataSet = new DataSet();
            var secondDataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataSet.Tables.Add(CreateTestDataTable(seed));
                secondDataSet.Tables.Add(CreateTestDataTable(seed + 10));
            }

            // Act & Assert
            firstDataSet.Tables.Should().HaveSameCount(secondDataSet.Tables);
        }

        [Fact]
        public void When_two_DataTableCollections_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            // Arrange
            var firstDataSet = new DataSet();
            var secondDataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataSet.Tables.Add(CreateTestDataTable(seed));
                secondDataSet.Tables.Add(CreateTestDataTable(seed + 10));
            }

            secondDataSet.Tables.RemoveAt(1);

            // Act
            Action action =
                () => firstDataSet.Tables.Should().HaveSameCount(secondDataSet.Tables);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataSet.Tables to have 2 table(s), but found 3 table(s).");
        }

        [Fact]
        public void When_count_of_generic_data_column_collection_count_is_compared_with_null_it_should_fail()
        {
            // Arrange
            var dataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                dataSet.Tables.Add(CreateTestDataTable(seed));
            }

            List<DataTable> nullDataTables = null;

            // Act
            Action action =
                () => nullDataTables.Should().HaveSameCount(dataSet.Tables, because: "we {0}", "care");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected nullDataTables to have the same count as * because we care, but found <null>.");
        }

        [Fact]
        public void When_count_of_generic_data_column_collection_is_compared_with_DataTableCollection_with_same_number_of_elements_it_should_succeed()
        {
            // Arrange
            var firstDataSet = new DataSet();
            var secondDataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataSet.Tables.Add(CreateTestDataTable(seed));
                secondDataSet.Tables.Add(CreateTestDataTable(seed + 10));
            }

            var genericDataTableCollection = firstDataSet.Tables.Cast<DataTable>();

            // Act & Assert
            genericDataTableCollection.Should().HaveSameCount(secondDataSet.Tables);
        }

        [Fact]
        public void When_generic_data_column_collection_is_compared_with_DataTableCollection_with_different_number_of_elements_it_should_fail()
        {
            // Arrange
            var firstDataSet = new DataSet();
            var secondDataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataSet.Tables.Add(CreateTestDataTable(seed));
                secondDataSet.Tables.Add(CreateTestDataTable(seed + 10));
            }

            secondDataSet.Tables.RemoveAt(1);

            var genericDataTableCollection = firstDataSet.Tables.Cast<DataTable>();

            // Act
            Action action =
                () => genericDataTableCollection.Should().HaveSameCount(secondDataSet.Tables, because: "we {0}", "care");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected genericDataTableCollection to have 2 table(s) because we care, but found 3.");
        }

        [Fact]
        public void When_asserting_not_same_count_if_expectation_is_null_it_should_fail()
        {
            // Arrange
            var dataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                dataSet.Tables.Add(CreateTestDataTable(seed));
            }

            var nullReference = default(DataTableCollection);

            // Act
            Action action =
                () => dataSet.Tables.Should().NotHaveSameCount(nullReference);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage(
                "Cannot verify count against a <null> collection.*");
        }

        [Fact]
        public void When_asserting_not_same_count_and_two_DataTableCollections_have_different_number_elements_it_should_succeed()
        {
            // Arrange
            var firstDataSet = new DataSet();
            var secondDataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataSet.Tables.Add(CreateTestDataTable(seed));
                secondDataSet.Tables.Add(CreateTestDataTable(seed + 10));
            }

            secondDataSet.Tables.RemoveAt(1);

            // Act & Assert
            firstDataSet.Tables.Should().NotHaveSameCount(secondDataSet.Tables);
        }

        [Fact]
        public void When_asserting_not_same_count_and_two_DataTableCollections_have_the_same_number_columns_it_should_fail()
        {
            // Arrange
            var firstDataSet = new DataSet();
            var secondDataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataSet.Tables.Add(CreateTestDataTable(seed));
                secondDataSet.Tables.Add(CreateTestDataTable(seed + 10));
            }

            // Act
            Action action =
                () => firstDataSet.Tables.Should().NotHaveSameCount(secondDataSet.Tables, because: "we {0}", "care");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataSet.Tables to not have 3 table(s) because we care, but found 3 table(s).");
        }

        [Fact]
        public void When_asserting_not_same_count_and_count_of_generic_data_column_collection_count_is_compared_with_null_it_should_fail()
        {
            // Arrange
            var dataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                dataSet.Tables.Add(CreateTestDataTable(seed));
            }

            List<DataTable> nullDataTables = null;

            // Act
            Action action =
                () => nullDataTables.Should().NotHaveSameCount(dataSet.Tables, because: "we {0}", "care");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected nullDataTables to not have the same count as * because we care, but found <null>.");
        }

        [Fact]
        public void When_asserting_not_same_count_and_count_of_generic_data_column_collection_is_compared_with_DataTableCollection_with_same_number_of_elements_it_should_fail()
        {
            // Arrange
            var firstDataSet = new DataSet();
            var secondDataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataSet.Tables.Add(CreateTestDataTable(seed));
                secondDataSet.Tables.Add(CreateTestDataTable(seed + 10));
            }

            var genericDataTableCollection = firstDataSet.Tables.Cast<DataTable>();

            // Act
            Action action =
                () => genericDataTableCollection.Should().NotHaveSameCount(secondDataSet.Tables, because: "we {0}", "care");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected genericDataTableCollection to not have 3 table(s) because we care, but found 3.");
        }

        [Fact]
        public void When_asserting_not_same_count_and_generic_data_column_collection_is_compared_with_DataTableCollection_with_different_number_of_elements_it_should_succeed()
        {
            // Arrange
            var firstDataSet = new DataSet();
            var secondDataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataSet.Tables.Add(CreateTestDataTable(seed));
                secondDataSet.Tables.Add(CreateTestDataTable(seed + 10));
            }

            secondDataSet.Tables.RemoveAt(1);

            var genericDataTableCollection = firstDataSet.Tables.Cast<DataTable>();

            // Act & Assert
            genericDataTableCollection.Should().NotHaveSameCount(secondDataSet.Tables);
        }
        #endregion

        #region ContainTableWithName & NotContainTableWithName
        [Fact]
        public void Should_succeed_when_asserting_DataTableCollection_contains_a_table_from_the_collection()
        {
            // Arrange
            var dataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                dataSet.Tables.Add(CreateTestDataTable(seed));
            }

            // Act & Assert
            dataSet.Tables.Should().ContainTableWithName("Table1");
        }

        [Fact]
        public void When_a_DataTableCollection_does_not_contain_single_table_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                dataSet.Tables.Add(CreateTestDataTable(seed));
            }

            // Act
            Action action =
                () => dataSet.Tables.Should().ContainTableWithName("Table4", "because {0}", "we do");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected dataSet.Tables* to contain table named \"Table4\" because we do*");
        }

        [Fact]
        public void Should_succeed_when_asserting_DataTableCollection_does_not_contain_a_table_that_is_not_in_the_collection()
        {
            // Arrange
            var dataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                dataSet.Tables.Add(CreateTestDataTable(seed));
            }

            // Act & Assert
            dataSet.Tables.Should().NotContainTableWithName("Table4");
        }

        [Fact]
        public void When_collection_contains_an_unexpected_item_it_should_throw()
        {
            // Arrange
            var dataSet = new DataSet();

            for (int seed = 0; seed < 3; seed++)
            {
                dataSet.Tables.Add(CreateTestDataTable(seed));
            }

            // Act
            Action action =
                () => dataSet.Tables.Should().NotContainTableWithName("Table1", "because we {0} like it, but found it anyhow", "don't");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected dataSet.Tables* to not contain table named \"Table1\" because we don't like it, but found it anyhow*");
        }
        #endregion
    }
}
