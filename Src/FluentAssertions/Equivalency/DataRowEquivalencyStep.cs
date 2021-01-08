using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using FluentAssertions.Data;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class DataRowEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return typeof(DataRow).IsAssignableFrom(config.GetExpectationType(context.RuntimeType, context.CompileTimeType));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "The code is easier to read without it.")]
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            var subject = context.Subject as DataRow;
            var expectation = context.Expectation as DataRow;

            if (expectation is null)
            {
                if (subject is not null)
                {
                    AssertionScope.Current.FailWith("Expected {context:DataRow} value to be null, but found {0}", subject);
                }
            }
            else
            {
                if (subject is null)
                {
                    if (context.Subject is null)
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataRow} to be non-null, but found null");
                    }
                    else
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataRow} to be of type {0}, but found {1} instead", expectation.GetType(), context.Subject.GetType());
                    }
                }
                else
                {
                    var dataSetConfig = config as DataEquivalencyAssertionOptions<DataSet>;
                    var dataTableConfig = config as DataEquivalencyAssertionOptions<DataTable>;
                    var dataRowConfig = config as DataEquivalencyAssertionOptions<DataRow>;

                    if (((dataSetConfig is null) || !dataSetConfig.AllowMismatchedTypes)
                     && ((dataTableConfig is null) || !dataTableConfig.AllowMismatchedTypes)
                     && ((dataRowConfig is null) || !dataRowConfig.AllowMismatchedTypes))
                    {
                        AssertionScope.Current
                            .ForCondition(subject.GetType() == expectation.GetType())
                            .FailWith("Expected {context:DataRow} to be of type '{0}'{reason}, but found '{1}'", expectation.GetType(), subject.GetType());
                    }

                    SelectedDataRowMembers selectedMembers = GetMembersFromExpectation(context, config);

                    CompareScalarProperties(subject, expectation, selectedMembers);

                    CompareFieldValues(context, parent, subject, expectation, dataSetConfig, dataTableConfig, dataRowConfig);
                }
            }

            return true;
        }

        private static void CompareScalarProperties(DataRow subject, DataRow expectation, SelectedDataRowMembers selectedMembers)
        {
            // Note: The members here are listed in the XML documentation for the DataRow.BeEquivalentTo extension
            // method in DataRowAssertions.cs. If this ever needs to change, keep them in sync.
            if (selectedMembers.HasErrors)
            {
                AssertionScope.Current
                    .ForCondition(subject.HasErrors == expectation.HasErrors)
                    .FailWith("Expected {context:DataRow} to have HasErrors value of '{0}'{reason}, but found '{1}' instead", expectation.HasErrors, subject.HasErrors);
            }

            if (selectedMembers.RowState)
            {
                AssertionScope.Current
                    .ForCondition(subject.RowState == expectation.RowState)
                    .FailWith("Expected {context:DataRow} to have RowState value of '{0}'{reason}, but found '{1}' instead", expectation.RowState, subject.RowState);
            }
        }

        private static void CompareFieldValues(IEquivalencyValidationContext context, IEquivalencyValidator parent, DataRow subject, DataRow expectation, DataEquivalencyAssertionOptions<DataSet> dataSetConfig, DataEquivalencyAssertionOptions<DataTable> dataTableConfig, DataEquivalencyAssertionOptions<DataRow> dataRowConfig)
        {
            IEnumerable<string> expectationColumnNames = expectation.Table.Columns.OfType<DataColumn>()
                .Select(col => col.ColumnName);

            IEnumerable<string> subjectColumnNames = subject.Table.Columns.OfType<DataColumn>()
                .Select(col => col.ColumnName);

            bool ignoreUnmatchedColumns =
                ((dataSetConfig is not null) && dataSetConfig.IgnoreUnmatchedColumns) ||
                ((dataTableConfig is not null) && dataTableConfig.IgnoreUnmatchedColumns) ||
                ((dataRowConfig is not null) && dataRowConfig.IgnoreUnmatchedColumns);

            DataRowVersion subjectVersion =
                (subject.RowState == DataRowState.Deleted)
                ? DataRowVersion.Original
                : DataRowVersion.Current;

            DataRowVersion expectationVersion =
                (expectation.RowState == DataRowState.Deleted)
                ? DataRowVersion.Original
                : DataRowVersion.Current;

            bool compareOriginalVersions = (subject.RowState == DataRowState.Modified) && (expectation.RowState == DataRowState.Modified);

            if (((dataSetConfig is not null) && dataSetConfig.ExcludeOriginalData)
             || ((dataTableConfig is not null) && dataTableConfig.ExcludeOriginalData)
             || ((dataRowConfig is not null) && dataRowConfig.ExcludeOriginalData))
            {
                compareOriginalVersions = false;
            }

            foreach (var columnName in expectationColumnNames.Union(subjectColumnNames))
            {
                DataColumn expectationColumn = expectation.Table.Columns[columnName];
                DataColumn subjectColumn = subject.Table.Columns[columnName];

                if (((dataSetConfig is not null) && dataSetConfig.ShouldExcludeColumn(subjectColumn))
                 || ((dataTableConfig is not null) && dataTableConfig.ShouldExcludeColumn(subjectColumn))
                 || ((dataRowConfig is not null) && dataRowConfig.ShouldExcludeColumn(subjectColumn)))
                {
                    continue;
                }

                if (!ignoreUnmatchedColumns)
                {
                    AssertionScope.Current
                        .ForCondition(subjectColumn is not null)
                        .FailWith("Expected {context:DataRow} to have column '{0}'{reason}, but found none", columnName);

                    AssertionScope.Current
                        .ForCondition(expectationColumn is not null)
                        .FailWith("Found unexpected column '{0}' in {context:DataRow}", columnName);
                }

                if ((subjectColumn is not null) && (expectationColumn is not null))
                {
                    CompareFieldValue(context, parent, subject, expectation, subjectColumn, subjectVersion, expectationColumn, expectationVersion);

                    if (compareOriginalVersions)
                    {
                        CompareFieldValue(context, parent, subject, expectation, subjectColumn, DataRowVersion.Original, expectationColumn, DataRowVersion.Original);
                    }
                }
            }
        }

        private static void CompareFieldValue(IEquivalencyValidationContext context, IEquivalencyValidator parent, DataRow subject, DataRow expectation, DataColumn subjectColumn, DataRowVersion subjectVersion, DataColumn expectationColumn, DataRowVersion expectationVersion)
        {
            IEquivalencyValidationContext nestedContext = context.AsCollectionItem(
                subjectVersion == DataRowVersion.Current
                ? subjectColumn.ColumnName
                : $"{subjectColumn.ColumnName}, DataRowVersion.Original",
                subject[subjectColumn, subjectVersion],
                expectation[expectationColumn, expectationVersion]);

            if (nestedContext is not null)
            {
                parent.AssertEqualityUsing(nestedContext);
            }
        }

        private class SelectedDataRowMembers
        {
            public bool HasErrors { get; set; }

            public bool RowState { get; set; }
        }

        private static readonly ConcurrentDictionary<(Type CompileTimeType, Type RuntimeType, IEquivalencyAssertionOptions Config), SelectedDataRowMembers> SelectedMembersCache
            = new ConcurrentDictionary<(Type CompileTimeType, Type RuntimeType, IEquivalencyAssertionOptions Config), SelectedDataRowMembers>();

        private static SelectedDataRowMembers GetMembersFromExpectation(IEquivalencyValidationContext context,
            IEquivalencyAssertionOptions config)
        {
            var cacheKey = (context.CompileTimeType, context.RuntimeType, config);

            if (!SelectedMembersCache.TryGetValue(cacheKey, out SelectedDataRowMembers selectedMembers))
            {
                var members = Enumerable.Empty<IMember>();

                foreach (IMemberSelectionRule rule in config.SelectionRules)
                {
                    members = rule.SelectMembers(context.CurrentNode, members, new MemberSelectionContext
                    {
                        CompileTimeType = context.CompileTimeType,
                        RuntimeType = context.RuntimeType,
                        Options = config
                    });
                }

                selectedMembers = new SelectedDataRowMembers();

                selectedMembers.HasErrors = members.Any(m => m.Name == nameof(DataRow.HasErrors));
                selectedMembers.RowState = members.Any(m => m.Name == nameof(DataRow.RowState));

                SelectedMembersCache.TryAdd(cacheKey, selectedMembers);
            }

            return selectedMembers;
        }
    }
}
