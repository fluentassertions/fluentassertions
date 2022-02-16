---
title: System.Data
permalink: /data/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

Fluent Assertions can be used to assert equivalence of System.Data types such as `DataSet` and `DataTable`.

## Basic Assertions

As with other reference types, you can assert a value of any of the core System.Data types to be null or not null:

```csharp
DataSet result = ...;

result.Should().NotBeNull();
```

You can also assert that two `DataSet` objects contain equivalent configuration and data, which, by default, will compare the rows contained by `DataTable` objects by their index within the collection.
Like this:

```csharp
var expected = GetExpectedDataSet();
var actual = GetActualDataSet();

actual.Should().BeEquivalentTo(expected);
```

The `BeEquivalentTo` test can be applied to `DataTable`, `DataColumn` and `DataRow` objects as well:

```csharp
actual.Tables["First"].Should().BeEquivalentTo(expected.Tables["First"]);
```

Chaining additional assertions is supported as well.

```csharp
dataTable.Should().HaveColumns("FirstName", "LastName")
    .And.Should().HaveRowCount(3);
```

## `DataSet` Assertions

The following assertions are available on `DataSet` objects:

* `.Should().HaveTableCount(n)`: Asserts that the `DataSet`'s `Tables` collection has the expected number of members.
* `.Should().HaveTable(tableName)`, `.Should().HaveTables(tableName, tableName, ...)`: Asserts that the `DataSet`'s `Tables` collection contains at least tables with the specified names. Additional tables are ignored.
* `.Should().BeEquivalentTo(dataSet)`: Performs a deep equivalency comparison between the subject and the expectation.

## `DataTable` Assertions

The following assertions are available on `DataTable` objects:

* `.Should().HaveColumn(columnName)`, `.Should().HaveColumns(columnName, columnName, ...)`: Asserts that the `DataTable`'s `Columns` collection contains at least columns with the specified names. Additional columns are ignored.
* `.Should().HaveRowCount(n)`: Asserts that the `DataTable`'s `Rows` collection has the expected number of elements.
* `.Should().BeEquivalentTo(dataTable)`: Performs a deep equivalency comparison between the subject and the expectation.

When comparing the rows of a `DataTable`, by default the rows are matched by their index in the `Rows` collection. But, if the subject and expectation tables contain equivalent `PrimaryKey` values, then it is possible to match the rows by their primary key field data, irrespective of the row order within the `Rows` collection. See "Equivalency Assertion Options" below for how to configure this.

## `DataColumn` Assertions

The following assertions are available on `DataColumn` objects:

* `.Should().BeEquivalentTo(dataColumn)`: Performs a deep equivalency comparison between the subject and the expectation.

## `DataRow` Assertions

The following assertions are available on `DataRow` objects:

* `.Should().HaveColumn(columnName)`, `.Should().HaveColumns(columnName, columnName, ...)`: Asserts that the `DataTable`'s `Columns` collection contains at least columns with the specified names. Additional columns are ignored.
* `.Should().BeEquivalentTo(dataRow)`: Performs a deep equivalency comparison between the subject and the expectation. This includes comparing field values, for which the defined columns must match.

When checking the equivalency of two `DataRow` objects, by default the `RowState` must match. But, if this is overridden using equivalency assertion options (`.Excluding(row => row.RowState)`), two `DataRow` objects with differing `RowState`s can still be considered equivalent based on their field values. FluentAssertions automatically determines which _version_ of field values to use in the subject and the expectation separately.

* For `DataRowState.Unchanged`, field values for `DataRowVersion.Current` are used.
* For `DataRowState.Added`, field values for `DataRowVersion.Current` are used.
* For `DataRowState.Deleted`, field values for `DataRowVersion.Original` are used.
* For `DataRowState.Modified`, field values for `DataRowVersion.Current` are used.

In addition, if both the subject and the expectation are in the `DataRowState.Modified` state, then the `DataRowVersion.Original` values are also compared, separately from the `DataRowVersion.Current` values. This can be disabled using the `.ExcludingOriginalData()` equivalency assertion option.

## Collections

Each `DataSet` has a `DataTableCollection` called `Tables`, and each `DataTable` has a `DataColumnCollection` called `Columns` and a `DataRowCollection` called `Rows`. Some assertions can be performed on these collection types.

The following assertions are in common to all three collection types:

* `.Should().BeEmpty()`: Succeeds if the collection contains no items (tables, columns, rows).
* `.Should().NotBeEmpty()`: Succeeds if the collection contains at least one item (table, column, row).
* `.Should().ContainEquivalentOf(x)`: Succeeds if the collection contains an item (table, column, row) that is equivalent to the supplied item.
* `.Should().NotContainEquivalentOf(x)`: Succeeds if the item does not contain any item (table, column, row) that is equivalent to the supplied item.
* `.Should().HaveSameCount(x)`: Succeeds if the collection contains the same number of items as the supplied collection of the same type.
* `.Should().NotHaveSameCount(x)`: Succeeds if the collection does not contain the same number of items as the supplied collection of the same type.
* `.Should().HaveCount(x)`: Succeeds if the collection contains exactly the specified number of items.
* `.Should().HaveCount(predicate)`: Succeeds if the predicate returns true for the number of items in the collection.
* `.Should().NotHaveCount(x)`: Succeeds if the collection contains a different number of items than the supplied count.
* `.Should().HaveCountGreaterThan(x)`: Succeeds if the collection contains more items than the supplied count.
* `.Should().HaveCountGreaterThanOrEqualTo(x)`: Succeeds if the collection contains at least as many items as the supplied count.
* `.Should().HaveCountLessThan(x)`: Succeeds if the collection contains fewer items than the supplied count.
* `.Should().HaveCountLessThanOrEqualTo(x)`: Succeeds if the collection contains at most as many items as the supplied count.

There are also some assertions specific to each type of collection:

* `dataSet.Tables.Should().ContainTableWithName(x)`: Succeeds if the `DataSet` contains a table with the specified name.
* `dataSet.Tables.Should().NotContainTableWithName(x)`: Succeeds if the `DataSet` does not contain a table with the specified name.
* `dataTable.Columns.Should().ContainColumnWithName(x)`: Succeeds if the `DataTable` contains a column with the specified name.
* `dataTable.Columns.Should().NotContainColumnWithName(x)`: Succeeds if the `DataTable` does not contain a column with the specified name.
* `dataTable.Rows.Should().BeSubsetOf(x)`: Succeeds if the `DataTable` only contains rows equivalent to those in the supplied set of `DataRow`s.
* `dataTable.Rows.Should().NotBeSubsetOf(x)`: Succeeds if the `DataTable` contains at least one row that is not equivalent to any in the supplied set of `DataRow`s.
* `dataTable.Rows.Should().IntersectWith(x)`: Succeeds if the `DataTable` contains at least one row that is equivalent to any in the supplied set of `DataRow`s.
* `dataTable.Rows.Should().NotIntersectWith(x)`: Succeeds if the `DataTable` does not contain any rows equivalent to any in the supplied set of `DataRow`s.
* `dataTable.Rows.Should().ContainMatchingRow(predicate)`: Succeeds if the `DataTable` contains at least one row for which the supplied `predicate` returns `true`.
* `dataTable.Rows.Should().NotContainMatchingRow(predicate)`: Succeeds if the `DataTable` does not contain any rows for which the supplied `predicate` returns `true`.

## Equivalency Assertion Options

When checking equivalency, the operation can be fine-tuned by configuring the options provided to an optional configuration callback in the `.BeEquivalentTo` method.

In addition to standard equivalency assertion options, which are applied recursively to all subjects found in the object graph, the following options are available that are specific to equivalency tests for supported `System.Data` types:

* `.AllowingMismatchedTypes()`: Allows objects with equivalent data to be considered equivalent, even if their data types do not match. See "Typed `DataSet`, `DataTable`, `DataRow` objects" below.
* `.IgnoringUnmatchedColumns()`: Allows tables to be equivalent if one table has columns that the other does not. The column definitions and row data for these columns is ignored.
* `.UsingRowMatchMode(Index | PrimaryKey)`: Specifies the manner in which rows in two `DataTable`s should be matched up for equivalency testing.
* `.ExcludingOriginalData()`: Specifies that when comparing two `DataRow` objects whose `RowState` is both `Modified`, only the `Current` value of fields should be compared, ignoring any differences in the `Original` versions of those fields.
* `.Excluding(x => x.Member)`: Excludes the specified member of the subject type from comparison. This method can only be used to select members accessible from the subject type directly.
* `.ExcludingRelated((DataRelation x) => x.Member)`:
* `.ExcludingRelated((DataTable x) => x.Member)`:
* `.ExcludingRelated((DataColumn x) => x.Member)`:
* `.ExcludingRelated((DataRow x) => x.Member)`:
* `.ExcludingRelated((DataConstraint x) => x.Member)`: Excludes the specified member of a related System.Data type from comparison. This is to handle the case where a change to one object within a `DataSet` causes corresponding changes automatically in other places. For instance, setting `Unique` to `true` in a `DataColumn` has a side-effect of adding a new `UniqueConstraint` to the `Constraints` collection of the containing `DataTable`.
* `.ExcludingTable(tableName)`, `.ExcludingTables(tableName, tableName, ...)`: Excludes tables with the specified name(s) from comparison.
* `.ExcludingColumnInAllTables(columnName)`, `.ExcludingColumnsInAllTables(columnName, columnName, ...)`: Excludes all columns in any table within the `DataSet` with the specified name(s) from comparison.
* `.ExcludingColumn(tableName, columnName)`, `.ExcludingColumns(tableName, columnName, columnName, ...)`: Excludes the specified column(s) in tables with a specific name within the `DataSet`s from comparison.
* `.ExcludingColumn(dataColumn)`, `.ExcludingColumns(dataColumn, dataColumn, ...`): Excludes the specified column(s) in tables with names matching the owner `DataTable` of the supplied `DataColumn` object(s) from comparison. This method is supplied because with typed `DataTable` objects, it is a common pattern that accessors for `DataColumn` objects are provided.

These configuration methods follow a fluent initialization pattern; they return the same options object they were called on, so that configuration can be chained in a single expression.

See the "Examples" section for some uses of equivalency options.

## Typed `DataSet`, `DataTable`, `DataRow` objects

If some of your `DataSet`, `DataTable` or `DataRow` objects are _typed_ objects (e.g. `DataTable` classes that inherit from `TypedTableBase<TRow>`), by default, equivalency tests will fail if the data types do not also match. If you want to compare typed objects with untyped objects that otherwise contain equivalent data, then the option `AllowingMismatchedTypes` can be used:

```csharp
DataSet expected = GetExpectedDataSet();
EmployeeData actual = service.GetEmployeeData();

actual.Should().BeEquivalentTo(expected, options => options.AllowingMismatchedTypes());
```

Apart from the default check for matching types, whether a `DataSet`, `DataTable` or `DataRow` is a vanilla object or an instance of a custom subtype is irrelevant for the purposes of comparison.

## Examples

Asserting that a `DataTable` has certain columns:

```csharp
var table = GetDataTable();

MyTypedTableType templateTable = GetExpectedDataTable();

table.Should().HaveColumn("FirstName");

table.Should().HaveColumns(templateTable.FirstNameColumn, templateTable.LastNameColumn);
```

Excluding tables and columns from equivalency tests:

> You can exclude a column from consideration by name across all tables in a `DataSet`, or from a specific `DataTable`.

```csharp
var expected = GetExpectedDataSet();
var actual = GetActualDataSet();

actual.Should().BeEquivalentTo(expected, options => options
    .ExcludingTable("Employees")
    .ExcludingColumnInAllTables("EmployeeID")
    .ExcludingColumn(tableName: "Employees", columnName: "Address"));
```

Excluding fields from types other than the subject:

> `DataSet` objects have a hierarchy of objects representing the data and many interrelations between these objects. For instance, setting `Unique` to `true` in a `DataColumn` has a side-effect of adding a new `UniqueConstraint` to the `Constraints` collection of the containing `DataTable`. As such, to eliminate side-effects it may be necessary to ignore additional properties of types related to the subject. To support this, a method `ExcludingRelated` can be used that allows members to be selected on any related type.

```csharp
var expected = GetExpectedDataSet();
var actual = GetActualDataSet();

actual.Should().BeEquivalentTo(expected, options => options
    .Excluding(dataSet => dataSet.DataSetName)
    .ExcludingRelated((DataRelation relation) => relation.DataSet);
```

Matching the rows of `DataTable`s by their primary key values:

> When comparing the rows of a `DataTable`, by default, rows are expected to be occur at the same indices. If a `DataTable` has a `PrimaryKey` set, then it is possible to instead match rows by their primary key, irrespective of the order in which they occur.

```csharp
var expected = GetExpectedDataTable();
var actual = GetActualDataTable(0;

actual.Should().BeEquivalentTo(expected, options => options.UsingRowMatchMode(RowMatchMode.PrimaryKey));
```
