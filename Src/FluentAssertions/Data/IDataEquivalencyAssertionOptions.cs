using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

using FluentAssertions.Equivalency;

namespace FluentAssertions.Data
{
    /// <summary>
    /// Provides access to configuration for equivalency assertions on System.Data types (<see cref="DataSet"/>,
    /// <see cref="DataTable"/>, <see cref="DataRow"/>, <see cref="DataColumn"/>, <see cref="DataRelation"/>,
    /// <see cref="Constraint"/>).
    /// </summary>
    /// <typeparam name="T">The System.Data type being tested for equivalency.</typeparam>
    public interface IDataEquivalencyAssertionOptions<T> : IEquivalencyAssertionOptions
    {
        /// <summary>
        /// Specifies that the subject and the expectation should not be considered non-equivalent if their exact data types do not match.
        /// </summary>
        IDataEquivalencyAssertionOptions<T> AllowingMismatchedTypes();

        /// <summary>
        /// Specifies that when comparing <see cref="DataTable.Columns"/>, columns that are unmatched between the subject and the expectation should be ignored.
        /// </summary>
        IDataEquivalencyAssertionOptions<T> IgnoringUnmatchedColumns();

        /// <summary>
        /// Specifies the <see cref="RowMatchMode"/> that should be used when comparing <see cref="DataTable.Rows"/>. By default, rows are matched by their index in the <see cref="DataTable.Rows"/> collection. But, if the <see cref="DataTable"/> has a <see cref="DataTable.PrimaryKey"/> set, it is possible to use <see cref="RowMatchMode.PrimaryKey"/> to indicate that rows should be matched by their primary key values, irrespective of their index within the <see cref="DataTable.Rows"/> collection.
        /// </summary>
        /// <param name="rowMatchMode">The <see cref="RowMatchMode"/> to use when comparing <see cref="DataTable.Rows"/> between the subject and the expectation.</param>
        IDataEquivalencyAssertionOptions<T> UsingRowMatchMode(RowMatchMode rowMatchMode);

        /// <summary>
        /// Specifies that when comparing <see cref="DataRow"/> objects that are in the <see cref="DataRowState.Modified"/> state, only the current field values should be compared. Original field values are excluded from comparison. This only affects comparisons where both the subject and the expectation are in the modified state.
        /// </summary>
        IDataEquivalencyAssertionOptions<T> ExcludingOriginalData();

        /// <summary>
        /// Excludes members of the objects under test from comparison by means of a predicate that selects members based on <see cref="IMemberInfo"/> objects describing them.
        /// </summary>
        /// <param name="predicate">A functor that returns true if the <see cref="IMemberInfo"/> parameter refers to a member that should be excluded.</param>
        IDataEquivalencyAssertionOptions<T> Excluding(Expression<Func<IMemberInfo, bool>> predicate);

        /// <summary>
        /// Excludes a member of the objects under test from comparison by means of an <see cref="Expression{TDelegate}"/> that refers to the member in question.
        /// </summary>
        /// <param name="expression">An <see cref="Expression{TDelegate}"/> that accesses the member to be excluded.</param>
        IDataEquivalencyAssertionOptions<T> Excluding(Expression<Func<T, object>> expression);

        /// <summary>
        /// Excludes an entire table from comparison. When comparing <see cref="DataSet"/> objects, if a table is present by the supplied name, it is not considered for the purpose of determining equivalency. This configuration option has no effect when comparing other types of object, including <see cref="DataTable"/>.
        /// </summary>
        /// <param name="tableName">The value for <see cref="DataTable.TableName"/> for which tables within a <see cref="DataSet"/> should be ignored.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingTable(string tableName);

        /// <summary>
        /// Excludes tables from comparison using names in an <see cref="IEnumerable{T}"/> set. When comparing <see cref="DataSet"/> objects, if a table is present by one of the supplied names, it is not considered for the purpose of determining equivalency. This configuration option has no effect when comparing other types of object, including <see cref="DataTable"/>.
        /// </summary>
        /// <param name="tableNames">An <see cref="IEnumerable{T}"/> of <see cref="string"/> values for <see cref="DataTable.TableName"/> for which tables within a <see cref="DataSet"/> should be ignored.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingTables(IEnumerable<string> tableNames);

        /// <summary>
        /// Excludes tables from comparison using an array of table names. When comparing <see cref="DataSet"/> objects, if a table is present by one of the supplied names, it is not considered for the purpose of determining equivalency. This configuration option has no effect when comparing other types of object, including <see cref="DataTable"/>.
        /// </summary>
        /// <param name="tableNames">An array of <see cref="string"/> values for <see cref="DataTable.TableName"/> for which tables within a <see cref="DataSet"/> should be ignored.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingTables(params string[] tableNames);

        /// <summary>
        /// Excludes a column from comparison by <see cref="DataColumn"/>. The column to be excluded is matched by the name of its associated <see cref="DataTable"/> and its own <see cref="DataColumn.ColumnName"/>.
        /// </summary>
        /// <param name="column">A <see cref="DataColumn"/> object that specifies which column should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumn(DataColumn column);

        /// <summary>
        /// Excludes a column from comparison by the name of its associated <see cref="DataTable"/> and its own <see cref="DataColumn.ColumnName"/>.
        /// </summary>
        /// <param name="tableName">The value for <see cref="DataTable.TableName"/> for which columns should be ignored.</param>
        /// <param name="columnName">The value for <see cref="DataColumn.ColumnName"/> for which columns should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumn(string tableName, string columnName);

        /// <summary>
        /// Exclude an enumerable set of columns from comparison by <see cref="DataColumn"/>. For each item in the enumeration, the column to be excluded is matched by the name of its associated <see cref="DataTable"/> and its own <see cref="DataColumn.ColumnName"/>.
        /// </summary>
        /// <param name="columns">An <see cref="IEnumerable{T}"/> object that specifies which column(s) should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumns(IEnumerable<DataColumn> columns);

        /// <summary>
        /// Excludes an array of columns from comparison by <see cref="DataColumn"/>. For each element in the array, the column to be excluded is matched by the name of its associated <see cref="DataTable"/> and its own <see cref="DataColumn.ColumnName"/>.
        /// </summary>
        /// <param name="columns">An array of <see cref="DataColumn"/> objects that specify which columns should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumns(params DataColumn[] columns);

        /// <summary>
        /// Excludes an enumerable set of columns from comparison by name, within tables with a specified name./>.
        /// </summary>
        /// <param name="tableName">The value for <see cref="DataTable.TableName"/> for which columns should be ignored.</param>
        /// <param name="columnNames">An <see cref="IEnumerable{T}"/> of <see cref="string"/> values that specify the <see cref="DataColumn.ColumnName"/> values for which columns should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumns(string tableName, IEnumerable<string> columnNames);

        /// <summary>
        /// Excludes an array of columns from comparison by name, within tables with a specified name./>.
        /// </summary>
        /// <param name="tableName">The value for <see cref="DataTable.TableName"/> for which columns should be ignored.</param>
        /// <param name="columnNames">An array of <see cref="string"/> values that specify the <see cref="DataColumn.ColumnName"/> values for which columns should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumns(string tableName, params string[] columnNames);

        /// <summary>
        /// Excludes columns from comparison by <see cref="DataColumn"/> comparing only the <see cref="DataColumn.ColumnName"/>. If columns exist by the same name in multiple <see cref="DataTable"/> objects within a <see cref="DataSet"/>, they are all excluded from comparison.
        /// </summary>
        /// <param name="columnName">The value for <see cref="DataColumn.ColumnName"/> for which columns should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumnInAllTables(string columnName);

        /// <summary>
        /// Excludes columns from comparison by <see cref="DataColumn"/> comparing only the <see cref="DataColumn.ColumnName"/>. If columns exist by the same name in multiple <see cref="DataTable"/> objects within a <see cref="DataSet"/>, they are all excluded from comparison.
        /// </summary>
        /// <param name="columnNames">An <see cref="IEnumerable{T}"/> of <see cref="string"/> values that specify the <see cref="DataColumn.ColumnName"/> values for which columns should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumnsInAllTables(IEnumerable<string> columnNames);

        /// <summary>
        /// Excludes columns from comparison by <see cref="DataColumn"/> comparing only the <see cref="DataColumn.ColumnName"/>. If columns exist by the same name in multiple <see cref="DataTable"/> objects within a <see cref="DataSet"/>, they are all excluded from comparison.
        /// </summary>
        /// <param name="columnNames">An array of <see cref="string"/> values that specify the <see cref="DataColumn.ColumnName"/> values for which columns should be ignored.</param>
        /// <remarks>
        /// When comparing <see cref="DataColumn"/> objects (e.g. within <see cref="DataColumnCollection"/>), excluded columns are ignored completely. When comparing <see cref="DataRow"/> objects, the data associated with excluded columns is ignored.
        /// </remarks>
        IDataEquivalencyAssertionOptions<T> ExcludingColumnsInAllTables(params string[] columnNames);

        /// <summary>
        /// Excludes properties of <see cref="Constraint"/> from comparison by means of an<see cref= "Expression{TDelegate}" /> that refers to the member in question.
        /// </summary>
        /// <param name="expression">An <see cref="Expression{TDelegate}"/> that accesses the member to be excluded.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<Constraint, object>> expression);

        /// <summary>
        /// Excludes properties of <see cref="Constraint"/> from comparison by means of an<see cref= "Expression{TDelegate}" /> that refers to the member in question.
        /// </summary>
        /// <param name="expression">An <see cref="Expression{TDelegate}"/> that accesses the member to be excluded.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<ForeignKeyConstraint, object>> expression);

        /// <summary>
        /// Excludes properties of <see cref="Constraint"/> from comparison by means of an<see cref= "Expression{TDelegate}" /> that refers to the member in question.
        /// </summary>
        /// <param name="expression">An <see cref="Expression{TDelegate}"/> that accesses the member to be excluded.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<UniqueConstraint, object>> expression);

        /// <summary>
        /// Excludes properties of <see cref="DataColumn"/> from comparison by means of an<see cref= "Expression{TDelegate}" /> that refers to the member in question.
        /// </summary>
        /// <param name="expression">An <see cref="Expression{TDelegate}"/> that accesses the member to be excluded.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<DataColumn, object>> expression);

        /// <summary>
        /// Excludes properties of <see cref="DataRelation"/> from comparison by means of an<see cref= "Expression{TDelegate}" /> that refers to the member in question.
        /// </summary>
        /// <param name="expression">An <see cref="Expression{TDelegate}"/> that accesses the member to be excluded.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<DataRelation, object>> expression);

        /// <summary>
        /// Excludes properties of <see cref="DataRow"/> from comparison by means of an<see cref= "Expression{TDelegate}" /> that refers to the member in question.
        /// </summary>
        /// <param name="expression">An <see cref="Expression{TDelegate}"/> that accesses the member to be excluded.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<DataRow, object>> expression);

        /// <summary>
        /// Excludes properties of <see cref="DataTable"/> from comparison by means of an<see cref= "Expression{TDelegate}" /> that refers to the member in question.
        /// </summary>
        /// <param name="expression">An <see cref="Expression{TDelegate}"/> that accesses the member to be excluded.</param>
        IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<DataTable, object>> expression);
    }
}
