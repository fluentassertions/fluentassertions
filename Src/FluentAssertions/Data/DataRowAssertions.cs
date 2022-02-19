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
    /// Provides convenient assertion methods on a <see cref="DataRow"/> that can be
    /// used to assert equivalency and the presence of columns.
    /// </summary>
    [DebuggerNonUserCode]
    public class DataRowAssertions<TDataRow> : ReferenceTypeAssertions<TDataRow, DataRowAssertions<TDataRow>>
        where TDataRow : DataRow
    {
        public DataRowAssertions(TDataRow dataRow)
            : base(dataRow)
        {
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataRow"/> has a column with the expected column name.
        /// </summary>
        /// <param name="expectedColumnName">The value that is expected in <see cref="DataColumn.ColumnName"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndWhichConstraint<DataRowAssertions<TDataRow>, DataColumn> HaveColumn(string expectedColumnName, string because = "", params object[] becauseArgs)
        {
            var subjectColumn = default(DataColumn);

            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:DataRow} to contain a column named {0}{reason}, but found <null>.", expectedColumnName);
            }
            else if (!Subject.Table.Columns.Contains(expectedColumnName))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:DataRow} to contain a column named {0}{reason}, but it does not.", expectedColumnName);
            }
            else
            {
                subjectColumn = Subject.Table.Columns[expectedColumnName];
            }

            return new AndWhichConstraint<DataRowAssertions<TDataRow>, DataColumn>(this, subjectColumn);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataRow"/> has columns with all of the supplied expected column names.
        /// </summary>
        /// <param name="expectedColumnNames">An array of values expected in <see cref="DataColumn.ColumnName"/>.</param>
        public AndConstraint<DataRowAssertions<TDataRow>> HaveColumns(params string[] expectedColumnNames)
        {
            return HaveColumns((IEnumerable<string>)expectedColumnNames);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataRow"/> has columns with all of the supplied expected column names.
        /// </summary>
        /// <param name="expectedColumnNames">An <see cref="IEnumerable{T}"/> of string values expected in <see cref="DataColumn.ColumnName"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<DataRowAssertions<TDataRow>> HaveColumns(IEnumerable<string> expectedColumnNames, string because = "", params object[] becauseArgs)
        {
            if (Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:DataRow} to be in a table containing {0} column(s) with specific names{reason}, but found <null>.", expectedColumnNames.Count());
            }

            foreach (var expectedColumnName in expectedColumnNames)
            {
                Execute.Assertion
                    .ForCondition(Subject.Table.Columns.Contains(expectedColumnName))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected table containing {context:DataRow} to contain a column named {0}{reason}, but it does not.", expectedColumnName);
            }

            return new AndConstraint<DataRowAssertions<TDataRow>>(this);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataRow"/> is equivalent to another.
        /// </summary>
        /// <remarks>
        /// Data rows are equivalent when they contain identical field data for the row they represent, and
        /// the following members have the same values:
        ///
        /// <list type="bullet">
        ///   <item><description>HasErrors</description></item>
        ///   <item><description>RowState</description></item>
        /// </list>
        ///
        /// The <see cref="DataRow"/> objects must be of the same type; if two <see cref="DataRow"/> objects
        /// are equivalent in all ways, except that one is part of a typed <see cref="DataTable"/> and is of a subclass
        /// of <see cref="DataRow"/>, then by default, they will not be considered equivalent. This can be overridden
        /// with the <see cref="BeEquivalentTo(DataRow, Func{IDataEquivalencyAssertionOptions{DataRow}, IDataEquivalencyAssertionOptions{DataRow}}, string, object[])"/>
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
        public AndConstraint<DataRowAssertions<TDataRow>> BeEquivalentTo(DataRow expectation, string because = "", params object[] becauseArgs)
        {
            return BeEquivalentTo(
                expectation,
                options => options,
                because,
                becauseArgs);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataRow"/> is equivalent to another.
        /// </summary>
        /// <remarks>
        /// Data rows are equivalent when they contain identical field data for the row they represent, and
        /// the following members have the same values:
        ///
        /// <list type="bullet">
        ///   <item><description>HasErrors</description></item>
        ///   <item><description>RowState</description></item>
        /// </list>
        ///
        /// The <see cref="DataRow"/> objects must be of the same type; if two <see cref="DataRow"/> objects
        /// are equivalent in all ways, except that one is part of a typed <see cref="DataTable"/> and is of a subclass
        /// of <see cref="DataRow"/>, then by default, they will not be considered equivalent.
        ///
        /// This, as well as testing of any property can be overridden using the <paramref name="config"/> callback.
        /// By calling <see cref="IDataEquivalencyAssertionOptions{T}.AllowingMismatchedTypes"/>, two <see cref="DataRow"/>
        /// objects of differing types can be considered equivalent. Exclude specific properties using
        /// <see cref="IDataEquivalencyAssertionOptions{T}.Excluding(System.Linq.Expressions.Expression{Func{T, object}})"/>.
        /// Exclude columns of the data table (which also excludes the related field data in <see cref="DataRow"/>
        /// objects) using <see cref="IDataEquivalencyAssertionOptions{T}.ExcludingColumn(DataColumn)"/> or a related function.
        /// </remarks>
        ///
        /// You can use <see cref="IDataEquivalencyAssertionOptions{T}.ExcludingRelated(System.Linq.Expressions.Expression{Func{DataTable, object}})"/>
        /// and related functions to exclude properties on other related System.Data types.
        /// <param name="expectation">A <see cref="DataColumn"/> with the expected configuration.</param>
        /// <param name="config">
        /// A reference to the <see cref="IDataEquivalencyAssertionOptions{DataRow}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="IDataEquivalencyAssertionOptions{DataRow}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<DataRowAssertions<TDataRow>> BeEquivalentTo(DataRow expectation, Func<IDataEquivalencyAssertionOptions<DataRow>, IDataEquivalencyAssertionOptions<DataRow>> config, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            IDataEquivalencyAssertionOptions<DataRow> options = config(AssertionOptions.CloneDefaults<DataRow, DataEquivalencyAssertionOptions<DataRow>>(e => new(e)));

            var context = new EquivalencyValidationContext(Node.From<DataRow>(() => AssertionScope.Current.CallerIdentity), options)
            {
                Reason = new Reason(because, becauseArgs),
                TraceWriter = options.TraceWriter
            };

            var comparands = new Comparands
            {
                Subject = Subject,
                Expectation = expectation,
                CompileTimeType = typeof(TDataRow),
            };

            new EquivalencyValidator().AssertEquality(comparands, context);

            return new AndConstraint<DataRowAssertions<TDataRow>>(this);
        }

        protected override string Identifier => "DataRow";
    }
}
