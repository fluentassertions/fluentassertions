using System;
using System.Data;
using System.Diagnostics;

using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Data
{
    /// <summary>
    /// Provides convenient assertion methods on a <see cref="DataColumn"/> that can be
    /// used to assert equivalency.
    /// </summary>
    [DebuggerNonUserCode]
    public class DataColumnAssertions : ReferenceTypeAssertions<DataColumn, DataColumnAssertions>
    {
        public DataColumnAssertions(DataColumn dataColumn)
            : base(dataColumn)
        {
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataColumn"/> is equivalent to another.
        /// </summary>
        /// <remarks>
        /// Data columns are equivalent when the following members have the same values:
        ///
        /// <list type="bullet">
        ///   <item><description>AllowDBNull</description></item>
        ///   <item><description>AutoIncrement</description></item>
        ///   <item><description>AutoIncrementSeed</description></item>
        ///   <item><description>AutoIncrementStep</description></item>
        ///   <item><description>Caption</description></item>
        ///   <item><description>ColumnName</description></item>
        ///   <item><description>DataType</description></item>
        ///   <item><description>DateTimeMode</description></item>
        ///   <item><description>DefaultValue</description></item>
        ///   <item><description>Expression</description></item>
        ///   <item><description>ExtendedProperties</description></item>
        ///   <item><description>MaxLength</description></item>
        ///   <item><description>Namespace</description></item>
        ///   <item><description>Prefix</description></item>
        ///   <item><description>ReadOnly</description></item>
        ///   <item><description>Unique</description></item>
        /// </list>
        /// </remarks>
        /// <param name="expectation">A <see cref="DataColumn"/> with the expected configuration.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<DataColumnAssertions> BeEquivalentTo(DataColumn expectation, string because = "", params object[] becauseArgs)
        {
            return BeEquivalentTo(
                expectation,
                options => options,
                because,
                becauseArgs);
        }

        /// <summary>
        /// Asserts that an instance of <see cref="DataColumn"/> is equivalent to another.
        /// </summary>
        /// <remarks>
        /// Data columns are equivalent when the following members have the same values:
        ///
        /// <list type="bullet">
        ///   <item><description>AllowDBNull</description></item>
        ///   <item><description>AutoIncrement</description></item>
        ///   <item><description>AutoIncrementSeed</description></item>
        ///   <item><description>AutoIncrementStep</description></item>
        ///   <item><description>Caption</description></item>
        ///   <item><description>ColumnName</description></item>
        ///   <item><description>DataType</description></item>
        ///   <item><description>DateTimeMode</description></item>
        ///   <item><description>DefaultValue</description></item>
        ///   <item><description>Expression</description></item>
        ///   <item><description>ExtendedProperties</description></item>
        ///   <item><description>MaxLength</description></item>
        ///   <item><description>Namespace</description></item>
        ///   <item><description>Prefix</description></item>
        ///   <item><description>ReadOnly</description></item>
        ///   <item><description>Unique</description></item>
        /// </list>
        ///
        /// Testing of any property can be overridden using the <paramref name="config"/> callback. Exclude specific properties using
        /// <see cref="IDataEquivalencyAssertionOptions{T}.Excluding(System.Linq.Expressions.Expression{Func{T, object}})"/>.
        ///
        /// If <see cref="IDataEquivalencyAssertionOptions{T}.ExcludingColumn(DataColumn)"/> or a related function is
        /// used and the exclusion matches the subject <see cref="DataColumn"/>, then the equivalency test will never
        /// fail.
        /// </remarks>
        /// <param name="expectation">A <see cref="DataColumn"/> with the expected configuration.</param>
        /// <param name="config">
        /// A reference to the <see cref="IDataEquivalencyAssertionOptions{DataColumn}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="IDataEquivalencyAssertionOptions{DataColumn}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<DataColumnAssertions> BeEquivalentTo(DataColumn expectation, Func<IDataEquivalencyAssertionOptions<DataColumn>, IDataEquivalencyAssertionOptions<DataColumn>> config, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            IDataEquivalencyAssertionOptions<DataColumn> options = config(AssertionOptions.CloneDefaults<DataColumn, DataEquivalencyAssertionOptions<DataColumn>>(e => new(e)));

            var context = new EquivalencyValidationContext(Node.From<DataColumn>(() => AssertionScope.Current.CallerIdentity), options)
            {
                Reason = new Reason(because, becauseArgs),
                TraceWriter = options.TraceWriter
            };

            var comparands = new Comparands
            {
                Subject = Subject,
                Expectation = expectation,
                CompileTimeType = typeof(DataColumn)
            };

            new EquivalencyValidator().AssertEquality(comparands, context);

            return new AndConstraint<DataColumnAssertions>(this);
        }

        protected override string Identifier => "DataColumn";
    }
}
