using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class DataRelationEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return typeof(DataRelation).IsAssignableFrom(config.GetExpectationType(context.RuntimeType, context.CompileTimeType));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "The code is easier to read without it.")]
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            var subject = context.Subject as DataRelation;
            var expectation = context.Expectation as DataRelation;

            if (expectation is null)
            {
                if (subject is not null)
                {
                    AssertionScope.Current.FailWith("Expected {context:DataRelation} to be null, but found {0}", subject);
                }
            }
            else
            {
                if (subject is null)
                {
                    if (context.Subject is null)
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataRelation} value to be non-null, but found null");
                    }
                    else
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataRelation} of type {0}, but found {1} instead", expectation.GetType(), context.Subject.GetType());
                    }
                }
                else
                {
                    var selectedMembers = GetMembersFromExpectation(context, config)
                        .ToDictionary(member => member.Name);

                    CompareScalarProperties(subject, expectation, selectedMembers);

                    CompareCollections(context, parent, config, selectedMembers);

                    CompareRelationConstraints(context, parent, config, subject, expectation, selectedMembers);
                }
            }

            return true;
        }

        private static void CompareScalarProperties(DataRelation subject, DataRelation expectation, Dictionary<string, IMember> selectedMembers)
        {
            if (selectedMembers.ContainsKey(nameof(expectation.RelationName)))
            {
                AssertionScope.Current
                    .ForCondition(subject.RelationName == expectation.RelationName)
                    .FailWith("Expected {context:DataRelation} to have RelationName of '{0}'{reason}, but found '{1}'", expectation.RelationName, subject.RelationName);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Nested)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Nested == expectation.Nested)
                    .FailWith("Expected {context:DataRelation} to have Nested value of '{0}'{reason}, but found '{1}'", expectation.Nested, subject.Nested);
            }

            // Special case: Compare name only
            if (selectedMembers.ContainsKey(nameof(expectation.DataSet)))
            {
                AssertionScope.Current
                    .ForCondition(subject.DataSet?.DataSetName == expectation.DataSet?.DataSetName)
                    .FailWith("Expected containing DataSet of {context:DataRelation} to be {0}{reason}, but found {1}",
                        expectation.DataSet?.DataSetName ?? "<null>",
                        subject.DataSet?.DataSetName ?? "<null>");
            }
        }

        private static void CompareCollections(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config, Dictionary<string, IMember> selectedMembers)
        {
            if (selectedMembers.TryGetValue(nameof(DataRelation.ExtendedProperties), out IMember expectationMember))
            {
                IMember matchingMember = FindMatchFor(expectationMember, context, config);

                if (matchingMember is not null)
                {
                    IEquivalencyValidationContext nestedContext =
                            context.AsNestedMember(expectationMember, matchingMember);

                    if (nestedContext is not null)
                    {
                        parent.AssertEqualityUsing(nestedContext);
                    }
                }
            }
        }

        private static void CompareRelationConstraints(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config, DataRelation subject, DataRelation expectation, Dictionary<string, IMember> selectedMembers)
        {
            CompareDataRelationConstraints(
                parent, context, config, subject, expectation, selectedMembers,
                "Child",
                selectedMembers.ContainsKey(nameof(DataRelation.ChildTable)),
                selectedMembers.ContainsKey(nameof(DataRelation.ChildColumns)),
                selectedMembers.ContainsKey(nameof(DataRelation.ChildKeyConstraint)),
                r => r.ChildColumns,
                r => r.ChildTable);

            CompareDataRelationConstraints(
                parent, context, config, subject, expectation, selectedMembers,
                "Parent",
                selectedMembers.ContainsKey(nameof(DataRelation.ParentTable)),
                selectedMembers.ContainsKey(nameof(DataRelation.ParentColumns)),
                selectedMembers.ContainsKey(nameof(DataRelation.ParentKeyConstraint)),
                r => r.ParentColumns,
                r => r.ParentTable);
        }

        private static void CompareDataRelationConstraints(
            IEquivalencyValidator parent, IEquivalencyValidationContext context, IEquivalencyAssertionOptions config, DataRelation subject, DataRelation expectation, Dictionary<string, IMember> selectedMembers,
            string relationDirection,
            bool compareTable, bool compareColumns, bool compareKeyConstraint,
            Func<DataRelation, DataColumn[]> getColumns,
            Func<DataRelation, DataTable> getOtherTable)
        {
            if (compareColumns)
            {
                CompareDataRelationColumns(subject, expectation, getColumns);
            }

            if (compareTable)
            {
                CompareDataRelationTable(subject, expectation, getOtherTable);
            }

            if (compareKeyConstraint)
            {
                CompareDataRelationKeyConstraint(parent, context, config, selectedMembers, relationDirection);
            }
        }

        private static void CompareDataRelationColumns(DataRelation subject, DataRelation expectation, Func<DataRelation, DataColumn[]> getColumns)
        {
            DataColumn[] subjectColumns = getColumns(subject);
            DataColumn[] expectationColumns = getColumns(expectation);

            // These column references are in different tables in different data sets that _should_ be equivalent
            // to one another.
            AssertionScope.Current
                .ForCondition(subjectColumns.Length == expectationColumns.Length)
                .FailWith("Expected {context:DataRelation} to reference {0} column(s){reason}, but found {subjectColumns.Length}", expectationColumns.Length, subjectColumns.Length);

            if (subjectColumns.Length == expectationColumns.Length)
            {
                for (int i = 0; i < expectationColumns.Length; i++)
                {
                    DataColumn subjectColumn = subjectColumns[i];
                    DataColumn expectationColumn = expectationColumns[i];

                    bool columnsAreEquivalent =
                        (subjectColumn.Table.TableName == expectationColumn.Table.TableName) &&
                        (subjectColumn.ColumnName == expectationColumn.ColumnName);

                    AssertionScope.Current
                        .ForCondition(columnsAreEquivalent)
                        .FailWith("Expected {context:DataRelation} to reference column {0} in table {1}{reason}, but found a reference to {2} in table {3} instead",
                            expectationColumn.ColumnName,
                            expectationColumn.Table.TableName,
                            subjectColumn.ColumnName,
                            subjectColumn.Table.TableName);
                }
            }
        }

        private static void CompareDataRelationTable(DataRelation subject, DataRelation expectation, Func<DataRelation, DataTable> getOtherTable)
        {
            DataTable subjectTable = getOtherTable(subject);
            DataTable expectationTable = getOtherTable(expectation);

            AssertionScope.Current
                .ForCondition(subjectTable.TableName == expectationTable.TableName)
                .FailWith("Expected {context:DataRelation} to reference a table named {0}{reason}, but found {1} instead", expectationTable.TableName, subjectTable.TableName);
        }

        private static void CompareDataRelationKeyConstraint(IEquivalencyValidator parent, IEquivalencyValidationContext context, IEquivalencyAssertionOptions config, Dictionary<string, IMember> selectedMembers, string relationDirection)
        {
            if (selectedMembers.TryGetValue(relationDirection + "KeyConstraint", out IMember expectationMember))
            {
                IMember subjectMember = FindMatchFor(expectationMember, context, config);

                IEquivalencyValidationContext nestedContext = context.AsNestedMember(expectationMember, subjectMember);

                if (nestedContext is not null)
                {
                    parent.AssertEqualityUsing(nestedContext);
                }
            }
        }

        private static IMember FindMatchFor(IMember selectedMemberInfo, IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> query =
                from rule in config.MatchingRules
                let match = rule.Match(selectedMemberInfo, context.Subject, context.CurrentNode, config)
                where match is not null
                select match;

            return query.FirstOrDefault();
        }

        private static IEnumerable<IMember> GetMembersFromExpectation(IEquivalencyValidationContext context,
            IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> members = Enumerable.Empty<IMember>();

            foreach (IMemberSelectionRule rule in config.SelectionRules)
            {
                members = rule.SelectMembers(context.CurrentNode, members, new MemberSelectionContext
                {
                    CompileTimeType = context.CompileTimeType,
                    RuntimeType = context.RuntimeType,
                    Options = config
                });
            }

            return members;
        }
    }
}
