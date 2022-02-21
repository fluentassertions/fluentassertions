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
                    row[column] = new DateTime(1970, 1, 1)
                        .AddTicks(random.Next() & 0x7FFFFFFF)
                        .AddSeconds(random.Next() & 0x7FFFFFFF);
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

            [Fact]
            public void When_generic_collection_is_tested_against_typed_collection_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var rowCollection = dataTable.Rows;

                var genericCollection = rowCollection.Cast<DataRow>();

                // Act
                Action action =
                    () => genericCollection.Should().BeSameAs(rowCollection, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected genericCollection to refer to DataRowCollection because we care, but found * (different type).");
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

        // These tests are present to ensure that we can trust DataRow equivalency in the context of ContainEquivalentOf.
        public class ContainEquivalentOf
        {
            [Fact]
            public void When_subject_is_null_it_should_fail()
            {
                // Arrange
                var subject = default(DataRowCollection);

                var expectation = new DataTable().Rows;

                // Act
                Action action =
                    () => subject.Should().ContainEquivalentOf(expectation, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected * to contain equivalent of * because we care, but found <null>.*");
            }

            [Fact]
            public void When_collection_contains_equivalent_row_it_should_succeed()
            {
                // Arrange
                var dataTable = CreateTestDataTable();

                const int MaxSeed = 3;

                for (int seed = 1; seed <= MaxSeed; seed++)
                {
                    AddTestDataRow(dataTable, seed);
                }

                var subjectTable = CreateTestDataTable();

                AddTestDataRow(subjectTable, MaxSeed - 1);

                var subjectRow = subjectTable.Rows[0];

                // Act & Assert
                dataTable.Rows.Should().ContainEquivalentOf(subjectRow);
            }

            [Fact]
            public void When_collection_does_not_contain_equivalent_row_it_should_fail()
            {
                // Arrange
                var dataTable = CreateTestDataTable();

                const int MaxSeed = 3;

                for (int seed = 1; seed <= MaxSeed; seed++)
                {
                    AddTestDataRow(dataTable, seed);
                }

                var subjectTable = CreateTestDataTable();

                int seedNotInTable = MaxSeed + 1;

                AddTestDataRow(subjectTable, seedNotInTable);

                var subjectRow = subjectTable.Rows[0];

                // Act
                Action action =
                    () => dataTable.Rows.Should().ContainEquivalentOf(subjectRow, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected dataTable.Rows * to contain equivalent of System.Data.DataRow* because we care.*");
            }
        }

        // These tests are present to ensure that we can trust DataRow equivalency in the context of NotContainEquivalentOf.
        public class NotContainEquivalentOf
        {
            [Fact]
            public void When_subject_is_null_it_should_fail()
            {
                // Arrange
                var subject = default(DataRowCollection);

                var expectation = new DataTable().Rows;

                // Act
                Action action =
                    () => subject.Should().NotContainEquivalentOf(expectation, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected * not to contain equivalent of * because we care, but collection is <null>.*");
            }

            [Fact]
            public void When_collection_contains_equivalent_row_it_should_fail()
            {
                // Arrange
                var dataTable = CreateTestDataTable();

                const int MaxSeed = 3;

                for (int seed = 1; seed <= MaxSeed; seed++)
                {
                    AddTestDataRow(dataTable, seed);
                }

                var subjectTable = CreateTestDataTable();

                AddTestDataRow(subjectTable, MaxSeed - 1);

                var subjectRow = subjectTable.Rows[0];

                // Act
                Action action =
                    () => dataTable.Rows.Should().NotContainEquivalentOf(subjectRow, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>()
                    .WithMessage("Expected dataTable.Rows * not to contain equivalent of System.Data.DataRow* because we " +
                        "care, but found one at index 1.*");
            }

            [Fact]
            public void When_collection_does_not_contain_equivalent_row_it_should_succeed()
            {
                // Arrange
                var dataTable = CreateTestDataTable();

                const int MaxSeed = 3;

                for (int seed = 1; seed <= MaxSeed; seed++)
                {
                    AddTestDataRow(dataTable, seed);
                }

                var subjectTable = CreateTestDataTable();

                int seedNotInTable = MaxSeed + 1;

                AddTestDataRow(subjectTable, seedNotInTable);

                var subjectRow = subjectTable.Rows[0];

                // Act & Assert
                dataTable.Rows.Should().NotContainEquivalentOf(subjectRow);
            }
        }
    }
}
