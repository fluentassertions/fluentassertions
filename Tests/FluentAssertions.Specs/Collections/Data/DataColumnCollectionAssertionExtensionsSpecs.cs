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
    public static class DataColumnCollectionAssertionExtensionsSpecs
    {
        private static DataColumn CreateTestDataColumn(int seed)
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

        public class BeSameAs
        {
            [Fact]
            public void When_references_are_the_same_it_should_succeed()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var columnCollection1 = dataTable.Columns;
                var columnCollection2 = columnCollection1;

                // Act & Assert
                columnCollection1.Should().BeSameAs(columnCollection2);
            }

            [Fact]
            public void When_references_are_different_it_should_fail()
            {
                // Arrange
                var dataTable1 = new DataTable("Test1");
                var dataTable2 = new DataTable("Test2");

                var columnCollection1 = dataTable1.Columns;
                var columnCollection2 = dataTable2.Columns;

                // Act
                Action action =
                    () => columnCollection1.Should().BeSameAs(columnCollection2);

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected columnCollection1 to refer to *, but found * (different underlying object).");
            }

            [Fact]
            public void When_generic_collection_is_tested_against_typed_collection_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var columnCollection = dataTable.Columns;

                var genericCollection = columnCollection.Cast<DataColumn>();

                // Act
                Action action =
                    () => genericCollection.Should().BeSameAs(columnCollection, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected genericCollection to refer to DataColumnCollection because we care, but found * (different type).");
            }
        }

        public class NotBeSameAs
        {
            [Fact]
            public void When_references_are_the_same_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable("Test");

                var columnCollection1 = dataTable.Columns;
                var columnCollection2 = columnCollection1;

                // Act
                Action action =
                    () => columnCollection1.Should().NotBeSameAs(columnCollection2);

                // Assert
                action.Should().Throw<XunitException>().WithMessage("Did not expect columnCollection1 to refer to *.");
            }

            [Fact]
            public void When_references_are_different_it_should_succeed()
            {
                // Arrange
                var dataTable1 = new DataTable("Test1");
                var dataTable2 = new DataTable("Test2");

                var columnCollection1 = dataTable1.Columns;
                var columnCollection2 = dataTable2.Columns;

                // Act & Assert
                columnCollection1.Should().NotBeSameAs(columnCollection2);
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
                    dataTable.Columns.Add(CreateTestDataColumn(seed));
                }

                var nullReference = default(DataColumnCollection);

                // Act
                Action action =
                    () => dataTable.Columns.Should().HaveSameCount(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify count against a <null> collection.*");
            }

            public class DataColumnCollectionAssertions
            {
                [Fact]
                public void When_two_collections_have_the_same_number_of_columns_it_should_succeed()
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
                public void When_two_collections_do_not_have_the_same_number_of_columns_it_should_fail()
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
            }

            public class GeneicCollectionAssertions
            {
                [Fact]
                public void When_collection_is_compared_with_null_it_should_fail()
                {
                    // Arrange
                    var dataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        dataTable.Columns.Add(CreateTestDataColumn(seed));
                    }

                    List<DataColumn> nullDataColumns = null;

                    // Act
                    Action action =
                        () => nullDataColumns.Should().HaveSameCount(dataTable.Columns, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected nullDataColumns to have the same count as * because we care, but found <null>.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_same_number_of_columns_it_should_succeed()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                        secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
                    }

                    var genericDataColumnCollection = firstDataTable.Columns.Cast<DataColumn>();

                    // Act & Assert
                    genericDataColumnCollection.Should().HaveSameCount(secondDataTable.Columns);
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_different_number_of_columns_it_should_fail()
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

                    var genericDataColumnCollection = firstDataTable.Columns.Cast<DataColumn>();

                    // Act
                    Action action =
                        () => genericDataColumnCollection.Should().HaveSameCount(secondDataTable.Columns,
                            because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected genericDataColumnCollection to have 2 column(s) because we care, but found 3.");
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
                    dataTable.Columns.Add(CreateTestDataColumn(seed));
                }

                var nullReference = default(DataColumnCollection);

                // Act
                Action action =
                    () => dataTable.Columns.Should().NotHaveSameCount(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify count against a <null> collection.*");
            }

            public class DataColumnCollectionAssertions
            {
                [Fact]
                public void When_two_collections_have_different_number_of_columns_it_should_succeed()
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
                public void When_two_collections_have_the_same_number_of_columns_it_should_fail()
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
                        () => firstDataTable.Columns.Should().NotHaveSameCount(secondDataTable.Columns,
                            because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected firstDataTable.Columns to not have 3 column(s) because we care, but found 3.");
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
                        dataTable.Columns.Add(CreateTestDataColumn(seed));
                    }

                    List<DataColumn> nullDataColumns = null;

                    // Act
                    Action action =
                        () => nullDataColumns.Should().NotHaveSameCount(dataTable.Columns, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected nullDataColumns to not have the same count as * because we care, but found <null>.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_same_number_of_columns_it_should_fail()
                {
                    // Arrange
                    var firstDataTable = new DataTable();
                    var secondDataTable = new DataTable();

                    for (int seed = 0; seed < 3; seed++)
                    {
                        firstDataTable.Columns.Add(CreateTestDataColumn(seed));
                        secondDataTable.Columns.Add(CreateTestDataColumn(seed + 10));
                    }

                    var genericDataColumnCollection = firstDataTable.Columns.Cast<DataColumn>();

                    // Act
                    Action action =
                        () => genericDataColumnCollection.Should().NotHaveSameCount(secondDataTable.Columns,
                            because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected genericDataColumnCollection to not have 3 column(s) because we care, but found 3.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_different_number_of_columns_it_should_succeed()
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

                    var genericDataColumnCollection = firstDataTable.Columns.Cast<DataColumn>();

                    // Act & Assert
                    genericDataColumnCollection.Should().NotHaveSameCount(secondDataTable.Columns);
                }
            }
        }

        public class ContainColumnWithName
        {
            [Fact]
            public void When_column_exists_it_should_succeed()
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
            public void When_column_does_not_exist_it_should_fail()
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
        }

        public class NotContainColumnWithName
        {
            [Fact]
            public void When_column_does_not_exist_it_should_succeed()
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
            public void When_column_exists_it_should_fail()
            {
                // Arrange
                var dataTable = new DataTable();

                for (int seed = 0; seed < 3; seed++)
                {
                    dataTable.Columns.Add(CreateTestDataColumn(seed));
                }

                // Act
                Action action =
                    () => dataTable.Columns.Should().NotContainColumnWithName(
                        "Column1", "because we {0} like it, but found it anyhow", "don't");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected dataTable.Columns* to not contain column named \"Column1\" because we don't like it, but found it anyhow*");
            }
        }
    }
}
