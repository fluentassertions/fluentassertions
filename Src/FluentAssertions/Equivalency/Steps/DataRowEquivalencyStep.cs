using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Data;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    public class DataRowEquivalencyStep : EquivalencyStep<DataRow>
    {
        [SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "The code is easier to read without it.")]
        protected override EquivalencyResult OnHandle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            var subject = comparands.Subject as DataRow;
            var expectation = comparands.Expectation as DataRow;

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
                    if (comparands.Subject is null)
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataRow} to be non-null, but found null");
                    }
                    else
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataRow} to be of type {0}, but found {1} instead",
                            expectation.GetType(), comparands.Subject.GetType());
                    }
                }
                else
                {
                    var dataSetConfig = context.Options as DataEquivalencyAssertionOptions<DataSet>;
                    var dataTableConfig = context.Options as DataEquivalencyAssertionOptions<DataTable>;
                    var dataRowConfig = context.Options as DataEquivalencyAssertionOptions<DataRow>;

                    if (dataSetConfig?.AllowMismatchedTypes != true
                        && dataTableConfig?.AllowMismatchedTypes != true
                        && dataRowConfig?.AllowMismatchedTypes != true)
                    {
                        AssertionScope.Current
                            .ForCondition(subject.GetType() == expectation.GetType())
                            .FailWith("Expected {context:DataRow} to be of type {0}{reason}, but found {1}",
                                expectation.GetType(), subject.GetType());
                    }

                    SelectedDataRowMembers selectedMembers = GetMembersFromExpectation(comparands, context.CurrentNode, context.Options);

                    CompareScalarProperties(subject, expectation, selectedMembers);

                    CompareFieldValues(context, nestedValidator, subject, expectation, dataSetConfig, dataTableConfig,
                        dataRowConfig);
                }
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static void CompareScalarProperties(DataRow subject, DataRow expectation, SelectedDataRowMembers selectedMembers)
        {
            // Note: The members here are listed in the XML documentation for the DataRow.BeEquivalentTo extension
            // method in DataRowAssertions.cs. If this ever needs to change, keep them in sync.
            if (selectedMembers.HasErrors)
            {
                AssertionScope.Current
                    .ForCondition(subject.HasErrors == expectation.HasErrors)
                    .FailWith("Expected {context:DataRow} to have HasErrors value of {0}{reason}, but found {1} instead",
                        expectation.HasErrors, subject.HasErrors);
            }

            if (selectedMembers.RowState)
            {
                AssertionScope.Current
                    .ForCondition(subject.RowState == expectation.RowState)
                    .FailWith("Expected {context:DataRow} to have RowState value of {0}{reason}, but found {1} instead",
                        expectation.RowState, subject.RowState);
            }
        }

        private static void CompareFieldValues(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            DataRow subject, DataRow expectation, DataEquivalencyAssertionOptions<DataSet> dataSetConfig,
            DataEquivalencyAssertionOptions<DataTable> dataTableConfig, DataEquivalencyAssertionOptions<DataRow> dataRowConfig)
        {
            IEnumerable<string> expectationColumnNames = expectation.Table.Columns.Cast<DataColumn>()
                .Select(col => col.ColumnName);

            IEnumerable<string> subjectColumnNames = subject.Table.Columns.Cast<DataColumn>()
                .Select(col => col.ColumnName);

            bool ignoreUnmatchedColumns =
                (dataSetConfig?.IgnoreUnmatchedColumns == true) ||
                (dataTableConfig?.IgnoreUnmatchedColumns == true) ||
                (dataRowConfig?.IgnoreUnmatchedColumns == true);

            DataRowVersion subjectVersion =
                (subject.RowState == DataRowState.Deleted)
                    ? DataRowVersion.Original
                    : DataRowVersion.Current;

            DataRowVersion expectationVersion =
                (expectation.RowState == DataRowState.Deleted)
                    ? DataRowVersion.Original
                    : DataRowVersion.Current;

            bool compareOriginalVersions =
                (subject.RowState == DataRowState.Modified) && (expectation.RowState == DataRowState.Modified);

            if ((dataSetConfig?.ExcludeOriginalData == true)
                || (dataTableConfig?.ExcludeOriginalData == true)
                || (dataRowConfig?.ExcludeOriginalData == true))
            {
                compareOriginalVersions = false;
            }

            foreach (var columnName in expectationColumnNames.Union(subjectColumnNames))
            {
                DataColumn expectationColumn = expectation.Table.Columns[columnName];
                DataColumn subjectColumn = subject.Table.Columns[columnName];

                if (subjectColumn is not null
                    && ((dataSetConfig?.ShouldExcludeColumn(subjectColumn) == true)
                        || (dataTableConfig?.ShouldExcludeColumn(subjectColumn) == true)
                        || (dataRowConfig?.ShouldExcludeColumn(subjectColumn) == true)))
                {
                    continue;
                }

                if (!ignoreUnmatchedColumns)
                {
                    AssertionScope.Current
                        .ForCondition(subjectColumn is not null)
                        .FailWith("Expected {context:DataRow} to have column {0}{reason}, but found none", columnName);

                    AssertionScope.Current
                        .ForCondition(expectationColumn is not null)
                        .FailWith("Found unexpected column {0} in {context:DataRow}", columnName);
                }

                if ((subjectColumn is not null) && (expectationColumn is not null))
                {
                    CompareFieldValue(context, parent, subject, expectation, subjectColumn, subjectVersion, expectationColumn,
                        expectationVersion);

                    if (compareOriginalVersions)
                    {
                        CompareFieldValue(context, parent, subject, expectation, subjectColumn, DataRowVersion.Original,
                            expectationColumn, DataRowVersion.Original);
                    }
                }
            }
        }

        private static void CompareFieldValue(IEquivalencyValidationContext context, IEquivalencyValidator parent, DataRow subject,
            DataRow expectation, DataColumn subjectColumn, DataRowVersion subjectVersion, DataColumn expectationColumn,
            DataRowVersion expectationVersion)
        {
            IEquivalencyValidationContext nestedContext = context.AsCollectionItem<object>(
                subjectVersion == DataRowVersion.Current
                    ? subjectColumn.ColumnName
                    : $"{subjectColumn.ColumnName}, DataRowVersion.Original");

            if (nestedContext is not null)
            {
                parent.RecursivelyAssertEquality(
                    new Comparands(subject[subjectColumn, subjectVersion], expectation[expectationColumn, expectationVersion], typeof(object)),
                    nestedContext);
            }
        }

        private class SelectedDataRowMembers
        {
            public bool HasErrors { get; set; }

            public bool RowState { get; set; }
        }

        private static readonly ConcurrentDictionary<(Type CompileTimeType, Type RuntimeType, IEquivalencyAssertionOptions Config),
                SelectedDataRowMembers> SelectedMembersCache = new();

        private static SelectedDataRowMembers GetMembersFromExpectation(Comparands comparands, INode currentNode,
            IEquivalencyAssertionOptions config)
        {
            var cacheKey = (comparands.CompileTimeType, comparands.RuntimeType, config);

            if (!SelectedMembersCache.TryGetValue(cacheKey, out SelectedDataRowMembers selectedMembers))
            {
                var members = Enumerable.Empty<IMember>();

                foreach (IMemberSelectionRule rule in config.SelectionRules)
                {
                    members = rule.SelectMembers(currentNode, members,
                        new MemberSelectionContext(comparands.CompileTimeType, comparands.RuntimeType, config));
                }

                selectedMembers = new SelectedDataRowMembers
                {
                    HasErrors = members.Any(m => m.Name == nameof(DataRow.HasErrors)),
                    RowState = members.Any(m => m.Name == nameof(DataRow.RowState))
                };

                SelectedMembersCache.TryAdd(cacheKey, selectedMembers);
            }

            return selectedMembers;
        }
    }
}
