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

namespace FluentAssertions.Specs.Collections.Data;

public static class DataRowCollectionAssertionSpecs
{
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
                "Invalid expectation: Expected genericCollection to refer to an instance of DataRowCollection " +
                "because we care, but found *.");
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

        [Fact]
        public void When_generic_collection_is_tested_against_typed_collection_it_should_fail()
        {
            // Arrange
            var dataTable = new DataTable("Test");

            var rowCollection = dataTable.Rows;

            var genericCollection = rowCollection.Cast<DataRow>();

            // Act
            Action action =
                () => genericCollection.Should().NotBeSameAs(rowCollection, because: "we {0}", "care");

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Invalid expectation: Expected genericCollection to refer to a different instance of " +
                "DataRowCollection because we care, but found *.");
        }
    }

    public class HaveSameCount
    {
        [Fact]
        public void When_subject_is_null_it_should_fail()
        {
            // Arrange
            var subject = default(DataRowCollection);

            var expectation = new DataTable().Rows;

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
            var dataTable = new DataTable();

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

                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();

                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();

                // Act & Assert
                firstDataTable.Rows.Should().HaveSameCount(secondDataTable.Rows);
            }

            [Fact]
            public void When_two_collections_do_not_have_the_same_number_of_rows_it_should_fail()
            {
                // Arrange
                var firstDataTable = new DataTable();
                var secondDataTable = new DataTable();

                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();

                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();

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

                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();

                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();

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

                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();

                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();

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
        public void When_subject_is_null_it_should_fail()
        {
            // Arrange
            var subject = default(DataRowCollection);

            var expectation = new DataTable().Rows;

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
            var dataTable = new DataTable();

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

                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();

                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();

                // Act & Assert
                firstDataTable.Rows.Should().NotHaveSameCount(secondDataTable.Rows);
            }

            [Fact]
            public void When_two_collections_have_the_same_number_rows_it_should_fail()
            {
                // Arrange
                var firstDataTable = new DataTable();
                var secondDataTable = new DataTable();

                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();

                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();

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

                dataTable.Rows.Add();
                dataTable.Rows.Add();
                dataTable.Rows.Add();

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

                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();

                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();

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

                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();
                firstDataTable.Rows.Add();

                secondDataTable.Rows.Add();
                secondDataTable.Rows.Add();

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
            var dataTable = new DataTable();

            dataTable.Columns.Add("RowID", typeof(Guid));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Number", typeof(int));
            dataTable.Columns.Add("Flag", typeof(bool));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));

            dataTable.Rows.Add(new Guid("6f460c1a-755d-d8e4-ad67-65d5f519dbc8"), "1851925803", 2137491580, true, new DateTime(638898932425580731));
            dataTable.Rows.Add(new Guid("8286d046-9740-a3e4-95cf-ff46699c73c4"), "607156385", 1321446349, true, new DateTime(641752306337096884));
            dataTable.Rows.Add(new Guid("95c69371-b924-6fe3-7c38-98b7dd200bc1"), "1509870614", 505401118, true, new DateTime(623130841631129390));

            var subjectTable = new DataTable();

            subjectTable.Columns.Add("RowID", typeof(Guid));
            subjectTable.Columns.Add("Description", typeof(string));
            subjectTable.Columns.Add("Number", typeof(int));
            subjectTable.Columns.Add("Flag", typeof(bool));
            subjectTable.Columns.Add("Timestamp", typeof(DateTime));

            subjectTable.Rows.Add(new Guid("95c69371-b924-6fe3-7c38-98b7dd200bc1"), "1509870614", 505401118, true, new DateTime(623130841631129390));

            var subjectRow = subjectTable.Rows[0];

            // Act & Assert
            dataTable.Rows.Should().ContainEquivalentOf(subjectRow);
        }

        [Fact]
        public void When_collection_does_not_contain_equivalent_row_it_should_fail()
        {
            // Arrange
            var dataTable = new DataTable();

            dataTable.Columns.Add("RowID", typeof(Guid));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Number", typeof(int));
            dataTable.Columns.Add("Flag", typeof(bool));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));

            dataTable.Rows.Add(new Guid("8286d046-9740-a3e4-95cf-ff46699c73c4"), "607156385", 1321446349, true, new DateTime(641752306337096884));
            dataTable.Rows.Add(new Guid("95c69371-b924-6fe3-7c38-98b7dd200bc1"), "1509870614", 505401118, true, new DateTime(623130841631129390));
            dataTable.Rows.Add(new Guid("a905569d-db07-3ae3-63a0-322750a4a3bd"), "265101196", 1836839534, true, new DateTime(625984215542645543));

            var subjectTable = new DataTable();

            subjectTable.Columns.Add("RowID", typeof(Guid));
            subjectTable.Columns.Add("Description", typeof(string));
            subjectTable.Columns.Add("Number", typeof(int));
            subjectTable.Columns.Add("Flag", typeof(bool));
            subjectTable.Columns.Add("Timestamp", typeof(DateTime));

            subjectTable.Rows.Add(new Guid("bc4519c8-fdeb-06e2-4a08-cc98c4273aba"), "1167815425", 1020794303, true, new DateTime(628837589454161696));

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
            var dataTable = new DataTable();

            dataTable.Columns.Add("RowID", typeof(Guid));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Number", typeof(int));
            dataTable.Columns.Add("Flag", typeof(bool));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));

            dataTable.Rows.Add(new Guid("8286d046-9740-a3e4-95cf-ff46699c73c4"), "607156385", 1321446349, true, new DateTime(641752306337096884));
            dataTable.Rows.Add(new Guid("95c69371-b924-6fe3-7c38-98b7dd200bc1"), "1509870614", 505401118, true, new DateTime(623130841631129390));
            dataTable.Rows.Add(new Guid("a905569d-db07-3ae3-63a0-322750a4a3bd"), "265101196", 1836839534, true, new DateTime(625984215542645543));

            var subjectTable = new DataTable();

            subjectTable.Columns.Add("RowID", typeof(Guid));
            subjectTable.Columns.Add("Description", typeof(string));
            subjectTable.Columns.Add("Number", typeof(int));
            subjectTable.Columns.Add("Flag", typeof(bool));
            subjectTable.Columns.Add("Timestamp", typeof(DateTime));

            subjectTable.Rows.Add(new Guid("95c69371-b924-6fe3-7c38-98b7dd200bc1"), "1509870614", 505401118, true, new DateTime(623130841631129390));

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
            var dataTable = new DataTable();

            dataTable.Columns.Add("RowID", typeof(Guid));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Number", typeof(int));
            dataTable.Columns.Add("Flag", typeof(bool));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));

            dataTable.Rows.Add(new Guid("8286d046-9740-a3e4-95cf-ff46699c73c4"), "607156385", 1321446349, true, new DateTime(641752306337096884));
            dataTable.Rows.Add(new Guid("95c69371-b924-6fe3-7c38-98b7dd200bc1"), "1509870614", 505401118, true, new DateTime(623130841631129390));
            dataTable.Rows.Add(new Guid("a905569d-db07-3ae3-63a0-322750a4a3bd"), "265101196", 1836839534, true, new DateTime(625984215542645543));

            var subjectTable = new DataTable();

            subjectTable.Columns.Add("RowID", typeof(Guid));
            subjectTable.Columns.Add("Description", typeof(string));
            subjectTable.Columns.Add("Number", typeof(int));
            subjectTable.Columns.Add("Flag", typeof(bool));
            subjectTable.Columns.Add("Timestamp", typeof(DateTime));

            subjectTable.Rows.Add(new Guid("bc4519c8-fdeb-06e2-4a08-cc98c4273aba"), "1167815425", 1020794303, true, new DateTime(628837589454161696));

            var subjectRow = subjectTable.Rows[0];

            // Act & Assert
            dataTable.Rows.Should().NotContainEquivalentOf(subjectRow);
        }
    }
}
