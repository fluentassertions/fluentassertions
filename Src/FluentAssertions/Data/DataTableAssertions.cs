using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Data
{
    /// <summary>
    /// Provides convenient assertion methods on a <see cref="DataTable"/> that can be
    /// used to assert equivalency and the presence of rows and columns.
    /// </summary>
    [DebuggerNonUserCode]
    public class DataTableAssertions<TDataTable> : ReferenceTypeAssertions<DataTable, DataTableAssertions<TDataTable>>
        where TDataTable : DataTable
    {
        public DataTableAssertions(TDataTable dataTable)
            : base(dataTable)
        {
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataTable"/> contains exactly the expected number of rows in its <see cref="DataTable.Rows"/> collection.
        /// </summary>
        /// <param name="expected">The expected number of rows.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<DataTableAssertions<TDataTable>> HaveRowCount(int expected, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:DataTable} to contain exactly {0} row(s){reason}, but found <null>.", expected);
            }

            int actualCount = Subject.Rows.Count;

            Execute.Assertion
                .ForCondition(actualCount == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:DataTable} to contain exactly {0} row(s){reason}, but found {1}.", expected, actualCount);

            return new AndConstraint<DataTableAssertions<TDataTable>>(this);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataTable"/> has a column with the expected column name.
        /// </summary>
        /// <param name="expectedColumnName">The value that is expected in <see cref="DataColumn.ColumnName"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndWhichConstraint<DataTableAssertions<TDataTable>, DataColumn> HaveColumn(string expectedColumnName, string because = "", params object[] becauseArgs)
        {
            var subjectColumn = default(DataColumn);

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:DataTable} to contain a column named {0}{reason}, but found <null>.", expectedColumnName);
            }
            else if (!Subject.Columns.Contains(expectedColumnName))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:DataTable} to contain a column named {0}{reason}, but it does not.", expectedColumnName);
            }
            else
            {
                subjectColumn = Subject.Columns[expectedColumnName];
            }

            return new AndWhichConstraint<DataTableAssertions<TDataTable>, DataColumn>(this, subjectColumn);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataTable"/> has columns with all of the supplied expected column names.
        /// </summary>
        /// <param name="expectedColumnNames">An array of values expected in <see cref="DataColumn.ColumnName"/>.</param>
        public AndConstraint<DataTableAssertions<TDataTable>> HaveColumns(params string[] expectedColumnNames)
        {
            return HaveColumns((IEnumerable<string>)expectedColumnNames);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataTable"/> has columns with all of the supplied expected column names.
        /// </summary>
        /// <param name="expectedColumnNames">An <see cref="IEnumerable{T}"/> of string values expected in <see cref="DataColumn.ColumnName"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<DataTableAssertions<TDataTable>> HaveColumns(IEnumerable<string> expectedColumnNames, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:DataTable} to contain {0} column(s) with specific names{reason}, but found <null>.", expectedColumnNames.Count());
            }

            foreach (var expectedColumnName in expectedColumnNames)
            {
                Execute.Assertion
                    .ForCondition(Subject.Columns.Contains(expectedColumnName))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:DataTable} to contain a column named {0}{reason}, but it does not.", expectedColumnName);
            }

            return new AndConstraint<DataTableAssertions<TDataTable>>(this);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataTable"/> is equivalent to another.
        /// </summary>
        /// <remarks>
        /// Data tables are equivalent when the following members have the same values:
        ///
        /// <list type="bullet">
        ///   <item><description>TableName</description></item>
        ///   <item><description>CaseSensitive</description></item>
        ///   <item><description>DisplayExpression</description></item>
        ///   <item><description>HasErrors</description></item>
        ///   <item><description>Locale</description></item>
        ///   <item><description>Namespace</description></item>
        ///   <item><description>Prefix</description></item>
        ///   <item><description>RemotingFormat</description></item>
        /// </list>
        ///
        /// In addition, the following collections must contain equivalent data:
        ///
        /// <list type="type=bullet">
        ///   <item><description>ChildRelations</description></item>
        ///   <item><description>Columns</description></item>
        ///   <item><description>Constraints</description></item>
        ///   <item><description>ExtendedProperties</description></item>
        ///   <item><description>ParentRelations</description></item>
        ///   <item><description>PrimaryKey</description></item>
        ///   <item><description>Rows</description></item>
        /// </list>
        ///
        /// The <see cref="DataTable"/> objects must be of the same type; if two <see cref="DataTable"/> objects
        /// are equivalent in all ways, except that one is a typed <see cref="DataTable"/> that is a subclass
        /// of <see cref="DataTable"/>, then by default, they will not be considered equivalent. This can be overridden
        /// with the <see cref="BeEquivalentTo(DataTable, Func{IDataEquivalencyAssertionOptions{DataTable}, IDataEquivalencyAssertionOptions{DataTable}}, string, object[])"/>
        /// overload.
        /// </remarks>
        /// <param name="expectation">A <see cref="DataColumn"/> with the expected configuration.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<DataTableAssertions<TDataTable>> BeEquivalentTo(DataTable expectation, string because = "", params object[] becauseArgs)
        {
            return BeEquivalentTo(
                expectation,
                options => options,
                because,
                becauseArgs);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataTable"/> is equivalent to another.
        /// </summary>
        /// <remarks>
        /// Data tables are equivalent when the following members have the same values:
        ///
        /// <list type="bullet">
        ///   <item><description>TableName</description></item>
        ///   <item><description>CaseSensitive</description></item>
        ///   <item><description>DisplayExpression</description></item>
        ///   <item><description>HasErrors</description></item>
        ///   <item><description>Locale</description></item>
        ///   <item><description>Namespace</description></item>
        ///   <item><description>Prefix</description></item>
        ///   <item><description>RemotingFormat</description></item>
        /// </list>
        ///
        /// In addition, the following collections must contain equivalent data:
        ///
        /// <list type="type=bullet">
        ///   <item><description>ChildRelations</description></item>
        ///   <item><description>Columns</description></item>
        ///   <item><description>Constraints</description></item>
        ///   <item><description>ExtendedProperties</description></item>
        ///   <item><description>ParentRelations</description></item>
        ///   <item><description>PrimaryKey</description></item>
        ///   <item><description>Rows</description></item>
        /// </list>
        ///
        /// The <see cref="DataTable"/> objects must be of the same type; if two <see cref="DataTable"/> objects
        /// are equivalent in all ways, except that one is a typed <see cref="DataTable"/> that is a subclass
        /// of <see cref="DataTable"/>, then by default, they will not be considered equivalent.
        ///
        /// This, as well as testing of any property can be overridden using the <paramref name="config"/> callback.
        /// By calling <see cref="IDataEquivalencyAssertionOptions{T}.AllowingMismatchedTypes"/>, two <see cref="DataTable"/>
        /// objects of differing types can be considered equivalent. Exclude specific properties using
        /// <see cref="IDataEquivalencyAssertionOptions{T}.Excluding(System.Linq.Expressions.Expression{Func{T, object}})"/>.
        /// Exclude columns of the data table using <see cref="IDataEquivalencyAssertionOptions{T}.ExcludingColumn(DataColumn)"/>
        /// or a related function -- this excludes both the <see cref="DataColumn"/> objects in <see cref="DataTable.Columns"/>
        /// and associated field data in <see cref="DataRow"/> objects within the <see cref="DataTable"/>.
        ///
        /// You can use <see cref="IDataEquivalencyAssertionOptions{T}.ExcludingRelated(System.Linq.Expressions.Expression{Func{DataTable, object}})"/>
        /// and related functions to exclude properties on other related System.Data types.
        /// </remarks>
        /// <param name="expectation">A <see cref="DataColumn"/> with the expected configuration.</param>
        /// <param name="config">
        /// A reference to the <see cref="IDataEquivalencyAssertionOptions{DataTable}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="IDataEquivalencyAssertionOptions{DataTable}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<DataTableAssertions<TDataTable>> BeEquivalentTo(DataTable expectation, Func<IDataEquivalencyAssertionOptions<DataTable>, IDataEquivalencyAssertionOptions<DataTable>> config, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            IDataEquivalencyAssertionOptions<DataTable> options = config(AssertionOptions.CloneDefaults<DataTable, DataEquivalencyAssertionOptions<DataTable>>(e => new(e)));

            var context = new EquivalencyValidationContext(Node.From<DataTable>(() => AssertionScope.Current.CallerIdentity), options)
            {
                Reason = new Reason(because, becauseArgs),
                TraceWriter = options.TraceWriter
            };

            var comparands = new Comparands
            {
                Subject = Subject,
                Expectation = expectation,
                CompileTimeType = typeof(TDataTable),
            };

            new EquivalencyValidator().AssertEquality(comparands, context);

            return new AndConstraint<DataTableAssertions<TDataTable>>(this);
        }

        protected override string Identifier => "DataTable";
    }
}
