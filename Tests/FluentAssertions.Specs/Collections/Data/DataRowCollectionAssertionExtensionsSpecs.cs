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
    public static class DataRowCollectionAssertionExtensionsSpecs
    {
        private static DataTable CreateTestDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("RowID", typeof(Guid));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Number", typeof(int));
            dataTable.Columns.Add("Flag", typeof(bool));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));

            return dataTable;
        }

        private static DataRow AddTestDataRow(DataTable dataTable, int seed)
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

        public class BeSameAs
        {
            [Fact]
            public void When_references_are_the_same_it_should_succeed()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var rowCollection1 = dataTable.Rows;
                var rowCollection2 = rowCollection1;

                // Act & Assert
                rowCollection1.Should().BeSameAs(rowCollection2);
            }

            [Fact]
            public void When_references_are_different_it_should_fail()
            {
                // Arrange
                var dataTable1 = new DataTable("Test1");
                var dataTable2 = new DataTable("Test2");

                var rowCollection1 = dataTable1.Rows;
                var rowCollection2 = dataTable2.Rows;

                // Act
                Action action =
                    () => rowCollection1.Should().BeSameAs(rowCollection2);

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected rowCollection1 to refer to *, but found * (different underlying object).");
            }
        }

        public class NotBeSameAs
        {
            [Fact]
            public void When_references_are_the_same_object_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var rowCollection1 = dataTable.Rows;
                var rowCollection2 = rowCollection1;

                // Act
                Action action =
                    () => rowCollection1.Should().NotBeSameAs(rowCollection2);

                // Assert
                action.Should().Throw<XunitException>().WithMessage("Did not expect rowCollection1 to refer to *.");
            }

            [Fact]
            public void When_references_are_different_it_should_succeed()
            {
                // Arrange
                var dataTable1 = new DataTable("Test1");
                var dataTable2 = new DataTable("Test2");

                var rowCollection1 = dataTable1.Rows;
                var rowCollection2 = dataTable2.Rows;

                // Act & Assert
                rowCollection1.Should().NotBeSameAs(rowCollection2);
            }
        }

        public class HaveSameCount
        {
            [Fact]
            public void When_expectation_is_null_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable();

                for (int seed = 0; seed < 3; seed++)
                {
                    AddTestDataRow(dataTable, seed);
                }

                var nullReference = default(DataRowCollection);

                // Act
                Action action =
                    () => dataTable.Rows.Should().HaveSameCount(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify count against a <null> collection.*");
            }

            public class DataRowCollectionAssertions
            {
                [Fact]
                public void When_two_collections_have_the_same_number_of_rows_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(firstDataTable, seed);
                        AddTestDataRow(secondDataTable, seed + 10);
                    }

                    // Act & Assert
                    firstDataTable.Rows.Should().HaveSameCount(secondDataTable.Rows);
                }

                [Fact]
                public void When_two_collections_do_not_have_the_same_number_of_rows_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

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
            }

            public class GenericCollectionAssertions
            {
                [Fact]
                public void When_collection_is_compared_with_null_it_should_fail()
                {
                    // Arrange
                    var dataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(dataTable, seed);
                    }

                    List<DataRow> nullDataRows = null;

                    // Act
                    Action action =
                        () => nullDataRows.Should().HaveSameCount(dataTable.Rows, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected nullDataRows to have the same count as * because we care, but found <null>.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_same_number_of_rows_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(firstDataTable, seed);
                        AddTestDataRow(secondDataTable, seed + 10);
                    }

                    var genericDataRowCollection = firstDataTable.Rows.Cast<DataRow>();

                    // Act & Assert
                    genericDataRowCollection.Should().HaveSameCount(secondDataTable.Rows);
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_different_number_of_rows_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(firstDataTable, seed);
                        AddTestDataRow(secondDataTable, seed + 10);
                    }

                    secondDataTable.Rows.RemoveAt(1);

                    var genericDataRowCollection = firstDataTable.Rows.Cast<DataRow>();

                    // Act
                    Action action =
                        () => genericDataRowCollection.Should().HaveSameCount(secondDataTable.Rows, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected genericDataRowCollection to have 2 row(s) because we care, but found 3.");
                }
            }
        }

        public class NotHaveSameCount
        {
            [Fact]
            public void When_expectation_is_null_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable();

                for (int seed = 0; seed < 3; seed++)
                {
                    AddTestDataRow(dataTable, seed);
                }

                var nullReference = default(DataRowCollection);

                // Act
                Action action =
                    () => dataTable.Rows.Should().NotHaveSameCount(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify count against a <null> collection.*");
            }

            public class DataRowCollectionAssertions
            {
                [Fact]
                public void When_two_collections_have_different_number_of_rows_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(firstDataTable, seed);
                        AddTestDataRow(secondDataTable, seed + 10);
                    }

                    secondDataTable.Rows.RemoveAt(1);

                    // Act & Assert
                    firstDataTable.Rows.Should().NotHaveSameCount(secondDataTable.Rows);
                }

                [Fact]
                public void When_two_collections_have_the_same_number_rows_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(firstDataTable, seed);
                        AddTestDataRow(secondDataTable, seed + 10);
                    }

                    // Act
                    Action action =
                        () => firstDataTable.Rows.Should().NotHaveSameCount(secondDataTable.Rows, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected firstDataTable.Rows to not have 3 row(s) because we care, but found 3.");
                }
            }

            public class GenericCollectionAssertions
            {
                [Fact]
                public void When_collection_is_compared_with_null_it_should_fail()
                {
                    // Arrange
                    var dataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(dataTable, seed);
                    }

                    List<DataRow> nullDataRows = null;

                    // Act
                    Action action =
                        () => nullDataRows.Should().NotHaveSameCount(dataTable.Rows, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected nullDataRows to not have the same count as * because we care, but found <null>.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_same_number_of_rows_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(firstDataTable, seed);
                        AddTestDataRow(secondDataTable, seed + 10);
                    }

                    var genericDataRowCollection = firstDataTable.Rows.Cast<DataRow>();

                    // Act
                    Action action =
                        () => genericDataRowCollection.Should().NotHaveSameCount(secondDataTable.Rows, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected genericDataRowCollection to not have 3 row(s) because we care, but found 3.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_different_number_of_rows_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        AddTestDataRow(firstDataTable, seed);
                        AddTestDataRow(secondDataTable, seed + 10);
                    }

                    secondDataTable.Rows.RemoveAt(1);

                    var genericDataRowCollection = firstDataTable.Rows.Cast<DataRow>();

                    // Act & Assert
                    genericDataRowCollection.Should().NotHaveSameCount(secondDataTable.Rows);
                }
            }
        }

        public class BeSubsetOf
        {
            [Fact]
            public void When_expectation_is_null_it_should_fail()
            {
                // Arrange
                var dataTable = CreateTestDataTable();

                for (int i = 0; i < 10; i++)
                {
                    AddTestDataRow(dataTable, i);
                }

                var nullReference = default(DataRowCollection);

                // Act
                Action action =
                    () => dataTable.Rows.Should().BeSubsetOf(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify a subset against a <null> collection.*");
            }

            [Fact]
            public void When_subject_is_subset_it_should_succeed()
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
            public void When_subject_is_not_subset_it_should_fail()
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
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected dataTable1.Rows to be a subset of *, but items * are not part of the superset.");
            }
        }

        public class NotBeSubsetOf
        {
            [Fact]
            public void When_expectation_is_null_it_should_fail()
            {
                // Arrange
                var dataTable = CreateTestDataTable();

                for (int i = 0; i < 10; i++)
                {
                    AddTestDataRow(dataTable, i);
                }

                var nullReference = default(DataRowCollection);

                // Act
                Action action =
                    () => dataTable.Rows.Should().NotBeSubsetOf(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify a subset against a <null> collection.*");
            }

            [Fact]
            public void When_subject_is_not_subset_it_should_succeed()
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
            public void When_subject_is_subset_it_should_fail()
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
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected dataTable1.Rows to not be a subset of the supplied set, but all items are part of the superset.");
            }
        }

        public class IntersectWith
        {
            [Fact]
            public void When_subject_intersects_expectation_it_should_succeed()
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
            public void When_subject_does_not_intersect_expectation_it_should_fail()
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
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected dataTable1.Rows to intersect the supplied set, but all items are part of the superset.");
            }
        }

        public class NotIntersectWith
        {
            [Fact]
            public void When_subject_does_not_intersect_expectation_it_should_succeed()
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
            public void When_subject_intersects_expectation_it_should_fail()
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
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected dataTable1.Rows to not intersect the supplied set, but at least one row is in common.");
            }
        }
    }
}
