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
    public static class DataTableCollectionAssertionSpecs
    {
        public class BeSameAs
        {
            [Fact]
            public void When_references_are_the_same_it_should_succeed()
            {
                // Arrange
                var dataSet = new DataSet();

                dataSet.Tables.Add(new DataTable("Table1"));

                var tableCollection1 = dataSet.Tables;
                var tableCollection2 = tableCollection1;

                // Act & Assert
                tableCollection1.Should().BeSameAs(tableCollection2);
            }

            [Fact]
            public void When_references_are_different_it_should_fail()
            {
                // Arrange
                var dataSet1 = new DataSet();
                var dataSet2 = new DataSet();

                dataSet1.Tables.Add(new DataTable("Table1"));
                dataSet2.Tables.Add(new DataTable("Table1"));

                var tableCollection1 = dataSet1.Tables;
                var tableCollection2 = dataSet2.Tables;

                // Act
                Action action =
                    () => tableCollection1.Should().BeSameAs(tableCollection2);

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected tableCollection1 to refer to *, but found * (different underlying object).");
            }

            [Fact]
            public void When_generic_collection_is_tested_against_typed_collection_it_should_fail()
            {
                // Arrange
                var dataSet = new DataSet();

                var tableCollection = dataSet.Tables;

                var genericCollection = tableCollection.Cast<DataTable>();

                // Act
                Action action =
                    () => genericCollection.Should().BeSameAs(tableCollection, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected genericCollection to refer to DataTableCollection because we care, but found * (different type).");
            }
        }

        public class NotBeSameAs
        {
            [Fact]
            public void When_references_are_the_same_it_should_fail()
            {
                // Arrange
                var dataSet = new DataSet();

                dataSet.Tables.Add(new DataTable("Table1"));

                var tableCollection1 = dataSet.Tables;
                var tableCollection2 = tableCollection1;

                // Act
                Action action =
                    () => tableCollection1.Should().NotBeSameAs(tableCollection2);

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Did not expect tableCollection1 to refer to *.");
            }

            [Fact]
            public void When_references_are_different_it_should_succeed()
            {
                // Arrange
                var dataSet1 = new DataSet();
                var dataSet2 = new DataSet();

                dataSet1.Tables.Add(new DataTable("Table1"));
                dataSet2.Tables.Add(new DataTable("Table1"));

                var tableCollection1 = dataSet1.Tables;
                var tableCollection2 = dataSet2.Tables;

                // Act & Assert
                tableCollection1.Should().NotBeSameAs(tableCollection2);
            }
        }

        public class HaveSameCount
        {
            [Fact]
            public void When_subject_is_null_it_should_fail()
            {
                // Arrange
                var subject = default(DataTableCollection);

                var expectation = new DataSet().Tables;

                // Act
                Action action =
                    () => subject.Should().HaveSameCount(expectation, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected * to have the same count as * because we care, but found <null>.*");
            }

            [Fact]
            public void When_expectation_is_null_it_should_fail()
            {
                // Arrange
                var dataSet = new DataSet();

                var nullReference = default(DataTableCollection);

                // Act
                Action action =
                    () => dataSet.Tables.Should().HaveSameCount(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify count against a <null> collection.*");
            }

            public class DataSetAssertions
            {
                [Fact]
                public void When_collections_have_the_same_number_of_tables_it_should_succeed()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table11"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Ensure that the table schema isn't important for the count comparison.
                    secondDataSet.Tables[0].Columns.Add("Column1", typeof(int));

                    // Act & Assert
                    firstDataSet.Tables.Should().HaveSameCount(secondDataSet, because: "we {0}", "care");
                }

                [Fact]
                public void When_collections_do_not_have_the_same_number_of_tables_it_should_fail()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Act
                    Action action =
                        () => firstDataSet.Tables.Should().HaveSameCount(secondDataSet, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected firstDataSet.Tables to have 2 table(s) because we care, but found 3.");
                }
            }

            public class DataTableCollectionAssertions
            {
                [Fact]
                public void When_collections_have_the_same_number_of_tables_it_should_succeed()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table11"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Ensure that the table schema isn't important for the count comparison.
                    secondDataSet.Tables[0].Columns.Add("Column1", typeof(int));

                    // Act & Assert
                    firstDataSet.Tables.Should().HaveSameCount(secondDataSet.Tables);
                }

                [Fact]
                public void When_collections_do_not_have_the_same_number_of_tables_it_should_fail()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Act
                    Action action =
                        () => firstDataSet.Tables.Should().HaveSameCount(secondDataSet.Tables);

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected firstDataSet.Tables to have 2 table(s), but found 3.");
                }
            }

            public class GenericCollectionAssertions
            {
                [Fact]
                public void When_collection_is_compared_with_null_it_should_fail()
                {
                    // Arrange
                    var dataSet = new DataSet();

                    dataSet.Tables.Add(new DataTable("Table0"));
                    dataSet.Tables.Add(new DataTable("Table1"));
                    dataSet.Tables.Add(new DataTable("Table2"));

                    List<DataTable> nullDataTables = null;

                    // Act
                    Action action =
                        () => nullDataTables.Should().HaveSameCount(dataSet.Tables, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected nullDataTables to have the same count as * because we care, but found <null>.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_same_number_of_tables_it_should_succeed()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table11"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Ensure that the table schema isn't important for the count comparison.
                    secondDataSet.Tables[0].Columns.Add("Column1", typeof(int));

                    var genericDataTableCollection = firstDataSet.Tables.Cast<DataTable>();

                    // Act & Assert
                    genericDataTableCollection.Should().HaveSameCount(secondDataSet.Tables);
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_different_number_of_tables_it_should_fail()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    var genericDataTableCollection = firstDataSet.Tables.Cast<DataTable>();

                    // Act
                    Action action =
                        () => genericDataTableCollection.Should().HaveSameCount(secondDataSet.Tables, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected genericDataTableCollection to have 2 table(s) because we care, but found 3.");
                }
            }
        }

        public class NotHaveSameCount
        {
            [Fact]
            public void When_subject_is_null_it_should_fail()
            {
                // Arrange
                var subject = default(DataTableCollection);

                var expectation = new DataSet().Tables;

                // Act
                Action action =
                    () => subject.Should().NotHaveSameCount(expectation, because: "we {0}", "care");

                // Assert
                action.Should().Throw<XunitException>().WithMessage(
                    "Expected * to not have the same count as * because we care, but found <null>.*");
            }

            [Fact]
            public void When_expectation_is_null_it_should_fail()
            {
                // Arrange
                var dataSet = new DataSet();

                dataSet.Tables.Add(new DataTable("Table0"));
                dataSet.Tables.Add(new DataTable("Table1"));
                dataSet.Tables.Add(new DataTable("Table2"));

                var nullReference = default(DataTableCollection);

                // Act
                Action action =
                    () => dataSet.Tables.Should().NotHaveSameCount(nullReference);

                // Assert
                action.Should().Throw<ArgumentNullException>().WithMessage(
                    "Cannot verify count against a <null> collection.*");
            }

            public class DataTableCollectionAssertions
            {
                [Fact]
                public void When_two_collections_have_different_number_of_tables_it_should_succeed()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Act & Assert
                    firstDataSet.Tables.Should().NotHaveSameCount(secondDataSet.Tables);
                }

                [Fact]
                public void When_two_collections_have_the_same_number_of_tables_it_should_fail()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table11"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Act
                    Action action =
                        () => firstDataSet.Tables.Should().NotHaveSameCount(secondDataSet.Tables, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected firstDataSet.Tables to not have 3 table(s) because we care, but found 3.");
                }
            }

            public class DataSetAssertions
            {
                [Fact]
                public void When_two_collections_have_different_number_of_tables_it_should_succeed()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Act & Assert
                    firstDataSet.Tables.Should().NotHaveSameCount(secondDataSet, because: "we {0}", "care");
                }

                [Fact]
                public void When_two_collections_have_the_same_number_of_tables_it_should_fail()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table11"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    // Act
                    Action action =
                        () => firstDataSet.Tables.Should().NotHaveSameCount(secondDataSet, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected firstDataSet.Tables to not have 3 table(s) because we care, but found 3.");
                }
            }

            public class GenericCollectionAssertions
            {
                [Fact]
                public void When_collection_is_compared_with_null_it_should_fail()
                {
                    // Arrange
                    var dataSet = new DataSet();

                    dataSet.Tables.Add(new DataTable("Table0"));
                    dataSet.Tables.Add(new DataTable("Table1"));
                    dataSet.Tables.Add(new DataTable("Table2"));

                    List<DataTable> nullDataTables = null;

                    // Act
                    Action action =
                        () => nullDataTables.Should().NotHaveSameCount(dataSet.Tables, because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected nullDataTables to not have the same count as * because we care, but found <null>.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_same_number_of_tables_it_should_fail()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table11"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    var genericDataTableCollection = firstDataSet.Tables.Cast<DataTable>();

                    // Act
                    Action action =
                        () => genericDataTableCollection.Should().NotHaveSameCount(secondDataSet.Tables,
                        because: "we {0}", "care");

                    // Assert
                    action.Should().Throw<XunitException>().WithMessage(
                        "Expected genericDataTableCollection to not have 3 table(s) because we care, but found 3.");
                }

                [Fact]
                public void When_collection_is_compared_with_typed_collection_with_different_number_of_tables_it_should_succeed()
                {
                    // Arrange
                    var firstDataSet = new DataSet();
                    var secondDataSet = new DataSet();

                    firstDataSet.Tables.Add(new DataTable("Table0"));
                    firstDataSet.Tables.Add(new DataTable("Table1"));
                    firstDataSet.Tables.Add(new DataTable("Table2"));

                    secondDataSet.Tables.Add(new DataTable("Table10"));
                    secondDataSet.Tables.Add(new DataTable("Table12"));

                    var genericDataTableCollection = firstDataSet.Tables.Cast<DataTable>();

                    // Act & Assert
                    genericDataTableCollection.Should().NotHaveSameCount(secondDataSet.Tables);
                }
            }
        }
    }
}
