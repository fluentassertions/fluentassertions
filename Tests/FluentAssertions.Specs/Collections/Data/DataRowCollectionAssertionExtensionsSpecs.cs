using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions.Execution;

using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Collections.Data
{
    public class DataRowCollectionAssertionExtensionsSpecs
    {
        private DataTable CreateTestDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("RowID", typeof(Guid));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Number", typeof(int));
            dataTable.Columns.Add("Flag", typeof(bool));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));

            return dataTable;
        }

        private DataRow AddTestDataRow(DataTable dataTable, int seed)
        {
            var row = dataTable.Rows.Add();

            var random = new Random(seed);

            foreach (var column in dataTable.Columns.OfType<DataColumn>())
            {
                if (column.DataType == typeof(Guid))
                {
                    byte[] guidBytes = new byte[16];

                    random.NextBytes(guidBytes);

                    row[column] = new Guid(guidBytes);
                }
                else if (column.DataType == typeof(int))
                {
                    row[column] = random.Next();
                }
                else if (column.DataType == typeof(string))
                {
                    row[column] = random.Next().ToString(CultureInfo.InvariantCulture);
                }
                else if (column.DataType == typeof(bool))
                {
                    row[column] = (random.Next() & 2) != 0;
                }
                else if (column.DataType == typeof(DateTime))
                {
                    row[column] = new DateTime(1970, 1, 1).AddTicks(random.Next() & 0x7FFFFFFF).AddSeconds(random.Next() & 0x7FFFFFFF);
                }
                else
                {
                    throw new Exception("Unable to populate column data because the data type is unexpected: " + column.DataType);
                }
            }

            row.AcceptChanges();

            return row;
        }

        #region HaveSameCount & NotHaveSameCount
        [Fact]
        public void When_DataRowCollection_and_DataTable_have_the_same_number_elements_it_should_succeed()
        {
            // Arrange
            var firstDataTable = CreateTestDataTable();
            var secondDataTable = CreateTestDataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                AddTestDataRow(firstDataTable, seed);
                AddTestDataRow(secondDataTable, seed + 10);
            }

            // Act / Assert
            firstDataTable.Rows.Should().HaveSameCount(secondDataTable.Rows);
        }

        [Fact]
        public void When_two_DataRowCollections_have_the_same_number_elements_it_should_succeed()
        {
            // Arrange
            var firstDataTable = CreateTestDataTable();
            var secondDataTable = CreateTestDataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                AddTestDataRow(firstDataTable, seed);
                AddTestDataRow(secondDataTable, seed + 10);
            }

            // Act / Assert
            firstDataTable.Rows.Should().HaveSameCount(secondDataTable.Rows);
        }

        [Fact]
        public void When_both_DataRowCollections_do_not_have_the_same_number_of_elements_it_should_fail()
        {
            // Arrange
            var firstDataTable = CreateTestDataTable();
            var secondDataTable = CreateTestDataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                AddTestDataRow(firstDataTable, seed);
                AddTestDataRow(secondDataTable, seed + 10);
            }

            secondDataTable.Rows.RemoveAt(1);

            // Act
            Action action =
                () => firstDataTable.Rows.Should().HaveSameCount(secondDataTable.Rows);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataTable.Rows to have 2 row(s), but found 3.");
        }

        [Fact]
        public void When_comparing_row_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            var firstDataTable = CreateTestDataTable();
            var secondDataTable = CreateTestDataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                AddTestDataRow(firstDataTable, seed);
                AddTestDataRow(secondDataTable, seed + 10);
            }

            secondDataTable.Rows.RemoveAt(1);

            // Act
            Action action =
                () => firstDataTable.Rows.Should().HaveSameCount(secondDataTable.Rows, "we want to test the {0}", "reason");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataTable.Rows to have 2 row(s) because we want to test the reason, but found 3.");
        }

        [Fact]
        public void When_asserting_not_same_count_and_DataRowCollections_have_different_number_elements_it_should_succeed()
        {
            // Arrange
            var firstDataTable = CreateTestDataTable();
            var secondDataTable = CreateTestDataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                AddTestDataRow(firstDataTable, seed);
                AddTestDataRow(secondDataTable, seed + 10);
            }

            secondDataTable.Rows.RemoveAt(1);

            // Act / Assert
            firstDataTable.Rows.Should().NotHaveSameCount(secondDataTable.Rows);
        }

        [Fact]
        public void When_asserting_not_same_count_and_both_DataRowCollections_have_the_same_number_rows_it_should_fail()
        {
            // Arrange
            var firstDataTable = CreateTestDataTable();
            var secondDataTable = CreateTestDataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                AddTestDataRow(firstDataTable, seed);
                AddTestDataRow(secondDataTable, seed + 10);
            }

            // Act
            Action action =
                () => firstDataTable.Rows.Should().NotHaveSameCount(secondDataTable.Rows);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataTable.Rows to not have 3 row(s), but found 3.");
        }

        [Fact]
        public void When_comparing_not_same_row_counts_and_a_reason_is_specified_it_should_it_in_the_exception()
        {
            // Arrange
            var firstDataTable = CreateTestDataTable();
            var secondDataTable = CreateTestDataTable();

            for (int seed = 0; seed < 3; seed++)
            {
                AddTestDataRow(firstDataTable, seed);
                AddTestDataRow(secondDataTable, seed + 10);
            }

            // Act
            Action action =
                () => firstDataTable.Rows.Should().NotHaveSameCount(secondDataTable.Rows, "we want to test the {0}", "reason");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected firstDataTable.Rows to not have 3 row(s) because we want to test the reason, but found 3.");
        }
        #endregion

        #region BeSubsetOf & NotBeSubsetOf
        [Fact]
        public void Should_succeed_when_asserting_DataRowCollection_that_is_a_subset_of_another_collection_is_a_subset()
        {
            // Arrange
            var dataTable1 = CreateTestDataTable();
            var dataTable2 = CreateTestDataTable();

            for (int i = 0; i < 10; i++)
            {
                AddTestDataRow(dataTable1, i);
                AddTestDataRow(dataTable2, i);

                AddTestDataRow(dataTable2, i + 10);
            }

            // Act & Assert
            dataTable1.Rows.Should().BeSubsetOf(dataTable2.Rows);
        }

        [Fact]
        public void Should_fail_when_asserting_DataRowCollection_that_is_not_a_subset_of_another_collection_is_a_subset()
        {
            // Arrange
            var dataTable1 = CreateTestDataTable();
            var dataTable2 = CreateTestDataTable();

            for (int i = 0; i < 10; i++)
            {
                AddTestDataRow(dataTable1, i);
                AddTestDataRow(dataTable2, i);

                AddTestDataRow(dataTable1, i + 10);
            }

            // Act
            Action action =
                () => dataTable1.Rows.Should().BeSubsetOf(dataTable2.Rows);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_succeed_when_asserting_DataRowCollection_that_is_not_a_subset_of_another_collection_is_not_a_subset()
        {
            // Arrange
            var dataTable1 = CreateTestDataTable();
            var dataTable2 = CreateTestDataTable();

            for (int i = 0; i < 10; i++)
            {
                AddTestDataRow(dataTable1, i);
                AddTestDataRow(dataTable2, i);

                AddTestDataRow(dataTable1, i + 10);
            }

            // Act & Assert
            dataTable1.Rows.Should().NotBeSubsetOf(dataTable2.Rows);
        }

        [Fact]
        public void Should_fail_when_asserting_DataRowCollection_that_is_a_subset_of_another_collection_is_not_a_subset()
        {
            // Arrange
            var dataTable1 = CreateTestDataTable();
            var dataTable2 = CreateTestDataTable();

            for (int i = 0; i < 10; i++)
            {
                AddTestDataRow(dataTable1, i);
                AddTestDataRow(dataTable2, i);

                AddTestDataRow(dataTable2, i + 10);
            }

            // Act
            Action action =
                () => dataTable1.Rows.Should().NotBeSubsetOf(dataTable2.Rows);

            // Assert
            action.Should().Throw<XunitException>();
        }
        #endregion

        #region IntersectWith & NotIntersectWith
        [Fact]
        public void Should_succeed_when_asserting_DataRowCollection_that_intersects_another_collection_intersects_it()
        {
            // Arrange
            var dataTable1 = CreateTestDataTable();
            var dataTable2 = CreateTestDataTable();

            for (int i = 0; i < 10; i++)
            {
                // Seeds overlap
                AddTestDataRow(dataTable1, i);
                AddTestDataRow(dataTable2, i + 5);
            }

            // Act & Assert
            dataTable1.Rows.Should().IntersectWith(dataTable2.Rows);
        }

        [Fact]
        public void Should_fail_when_asserting_DataRowCollection_that_does_not_intersect_another_collection_intersects_it()
        {
            // Arrange
            var dataTable1 = CreateTestDataTable();
            var dataTable2 = CreateTestDataTable();

            for (int i = 0; i < 10; i++)
            {
                // Seeds do not overlap
                AddTestDataRow(dataTable1, i);
                AddTestDataRow(dataTable2, i + 10);
            }

            // Act
            Action action =
                () => dataTable1.Rows.Should().IntersectWith(dataTable2.Rows);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_succeed_when_asserting_DataRowCollection_that_does_not_intersect_another_collection_does_not_intersect_it()
        {
            // Arrange
            var dataTable1 = CreateTestDataTable();
            var dataTable2 = CreateTestDataTable();

            for (int i = 0; i < 10; i++)
            {
                // Seeds do not overlap
                AddTestDataRow(dataTable1, i);
                AddTestDataRow(dataTable2, i + 10);
            }

            // Act & Assert
            dataTable1.Rows.Should().NotIntersectWith(dataTable2.Rows);
        }

        [Fact]
        public void Should_fail_when_asserting_DataRowCollection_that_intersects_another_collection_does_not_intersect_it()
        {
            // Arrange
            var dataTable1 = CreateTestDataTable();
            var dataTable2 = CreateTestDataTable();

            for (int i = 0; i < 10; i++)
            {
                // Seeds overlap
                AddTestDataRow(dataTable1, i);
                AddTestDataRow(dataTable2, i + 5);
            }

            // Act
            Action action =
                () => dataTable1.Rows.Should().NotIntersectWith(dataTable2.Rows);

            // Assert
            action.Should().Throw<XunitException>();
        }
        #endregion
    }
}
