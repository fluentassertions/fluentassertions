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
    public class DataColumnCollectionAssertionExtensionsSpecs
    {
        private DataColumn CreateTestDataColumn(int seed)
        {
            var column = new DataColumn("Column" + seed);

            var random = new Random(seed);

            column.DataType =
                random.Next(4) switch
                {
                    0 => typeof(int),
                    1 => typeof(string),
                    2 => typeof(bool),
                    _ => typeof(DateTime),
                };

            return column;
        }

        #region HaveSameCount & NotHaveSameCount
        [Fact]
        public void When_DataColumnCollection_and_DataTable_have_the_same_number_elements_it_should_succeed()
        {
            // Arrange
            var firstDataTable = new DataTable();
            var secondDataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
            }

            // Act & Assert
            firstDataTable.Columns.Should().HaveSameCount(secondDataTable.Columns);
        }

        [Fact]
        public void When_two_DataColumnCollections_have_the_same_number_elements_it_should_succeed()
        {
            // Arrange
            var firstDataTable = new DataTable();
            var secondDataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
            }

            // Act & Assert
            firstDataTable.Columns.Should().HaveSameCount(secondDataTable.Columns);
        }

        [Fact]
        public void When_both_DataColumnCollections_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            // Arrange
            var firstDataTable = new DataTable();
            var secondDataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
            }

            secondDataTable.Columns.RemoveAt(1);

            // Act
            Action action =
                () => firstDataTable.Columns.Should().HaveSameCount(secondDataTable.Columns);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataTable.Columns to have 2 column(s), but found 3.");
        }

        [Fact]
        public void When_comparing_column_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            var firstDataTable = new DataTable();
            var secondDataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
            }

            secondDataTable.Columns.RemoveAt(1);

            // Act
            Action action =
                () => firstDataTable.Columns.Should().HaveSameCount(secondDataTable.Columns, "we want to test the {0}", "reason");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataTable.Columns to have 2 column(s) because we want to test the reason, but found 3.");
        }

        [Fact]
        public void When_asserting_not_same_count_and_DataColumnCollections_have_different_number_elements_it_should_succeed()
        {
            // Arrange
            var firstDataTable = new DataTable();
            var secondDataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
            }

            secondDataTable.Columns.RemoveAt(1);

            // Act & Assert
            firstDataTable.Columns.Should().NotHaveSameCount(secondDataTable.Columns);
        }

        [Fact]
        public void When_asserting_not_same_count_and_both_DataColumnCollections_have_the_same_number_columns_it_should_fail()
        {
            // Arrange
            var firstDataTable = new DataTable();
            var secondDataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
            }

            // Act
            Action action =
                () => firstDataTable.Columns.Should().NotHaveSameCount(secondDataTable.Columns);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataTable.Columns to not have 3 column(s), but found 3.");
        }

        [Fact]
        public void When_comparing_not_same_column_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            var firstDataTable = new DataTable();
            var secondDataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
            }

            // Act
            Action action =
                () => firstDataTable.Columns.Should().NotHaveSameCount(secondDataTable.Columns, "we want to test the {0}", "reason");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected * to not have 3 column(s) because we want to test the reason, but found 3.");
        }
        #endregion

        #region ContainColumnWithName & NotContainColumnWithName
        [Fact]
        public void Should_succeed_when_asserting_DataColumnCollection_contains_a_column_from_the_collection()
        {
            // Arrange
            var dataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                dataTable.Columns.Add(CreateTestDataColumn(seed));
            }

            // Act & Assert
            dataTable.Columns.Should().ContainColumnWithName("Column1");
        }

        [Fact]
        public void When_a_DataColumnCollection_does_not_contain_single_column_it_should_throw_with_clear_explanation()
        {
            // Arrange
            var dataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                dataTable.Columns.Add(CreateTestDataColumn(seed));
            }

            // Act
            Action action =
                () => dataTable.Columns.Should().ContainColumnWithName("Column4", "because {0}", "we do");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected dataTable.Columns to contain column named \"Column4\" because we do*");
        }

        [Fact]
        public void Should_succeed_when_asserting_DataColumnCollection_does_not_contain_a_column_that_is_not_in_the_collection()
        {
            // Arrange
            var dataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                dataTable.Columns.Add(CreateTestDataColumn(seed));
            }

            // Act & Assert
            dataTable.Columns.Should().NotContainColumnWithName("Column4");
        }

        [Fact]
        public void When_collection_contains_an_unexpected_item_it_should_throw()
        {
            // Arrange
            var dataTable = new DataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                dataTable.Columns.Add(CreateTestDataColumn(seed));
            }

            // Act
            Action action =
                () => dataTable.Columns.Should().NotContainColumnWithName("Column1", "because we {0} like it, but found it anyhow", "don't");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected dataTable.Columns* to not contain column named \"Column1\" because we don't like it, but found it anyhow*");
        }
        #endregion
    }
}
