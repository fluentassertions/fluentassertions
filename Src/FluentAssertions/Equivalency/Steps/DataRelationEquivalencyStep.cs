using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    public class DataRelationEquivalencyStep : EquivalencyStep<DataRelation>
    {
        [SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "The code is easier to read without it.")]
        protected override EquivalencyResult OnHandle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            var subject = comparands.Subject as DataRelation;
            var expectation = comparands.Expectation as DataRelation;

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
                    if (comparands.Subject is null)
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataRelation} value to be non-null, but found null");
                    }
                    else
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataRelation} of type {0}, but found {1} instead",
                            expectation.GetType(), comparands.Subject.GetType());
                    }
                }
                else
                {
                    var selectedMembers = GetMembersFromExpectation(context.CurrentNode, comparands, context.Options)
                        .ToDictionary(member => member.Name);

                    CompareScalarProperties(subject, expectation, selectedMembers);

                    CompareCollections(context, comparands, nestedValidator, context.Options, selectedMembers);

                    CompareRelationConstraints(context, nestedValidator, subject, expectation, selectedMembers);
                }
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static void CompareScalarProperties(DataRelation subject, DataRelation expectation,
            Dictionary<string, IMember> selectedMembers)
        {
            if (selectedMembers.ContainsKey(nameof(expectation.RelationName)))
            {
                AssertionScope.Current
                    .ForCondition(subject.RelationName == expectation.RelationName)
                    .FailWith("Expected {context:DataRelation} to have RelationName of {0}{reason}, but found {1}",
                        expectation.RelationName, subject.RelationName);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Nested)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Nested == expectation.Nested)
                    .FailWith("Expected {context:DataRelation} to have Nested value of {0}{reason}, but found {1}",
                        expectation.Nested, subject.Nested);
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

        private static void CompareCollections(IEquivalencyValidationContext context, Comparands comparands,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config, Dictionary<string, IMember> selectedMembers)
        {
            if (selectedMembers.TryGetValue(nameof(DataRelation.ExtendedProperties), out IMember expectationMember))
            {
                IMember matchingMember = FindMatchFor(expectationMember, context.CurrentNode, comparands.Subject, config);
                if (matchingMember is not null)
                {
                    var nestedComparands = new Comparands
                    {
                        Subject = matchingMember.GetValue(comparands.Subject),
                        Expectation = expectationMember.GetValue(comparands.Expectation),
                        CompileTimeType = expectationMember.Type
                    };

                    parent.RecursivelyAssertEquality(nestedComparands, context.AsNestedMember(expectationMember));
                }
            }
        }

        private static void CompareRelationConstraints(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            DataRelation subject, DataRelation expectation,
            Dictionary<string, IMember> selectedMembers)
        {
            CompareDataRelationConstraints(
                parent, context, subject, expectation, selectedMembers,
                "Child",
                selectedMembers.ContainsKey(nameof(DataRelation.ChildTable)),
                selectedMembers.ContainsKey(nameof(DataRelation.ChildColumns)),
                selectedMembers.ContainsKey(nameof(DataRelation.ChildKeyConstraint)),
                r => r.ChildColumns,
                r => r.ChildTable);

            CompareDataRelationConstraints(
                parent, context, subject, expectation, selectedMembers,
                "Parent",
                selectedMembers.ContainsKey(nameof(DataRelation.ParentTable)),
                selectedMembers.ContainsKey(nameof(DataRelation.ParentColumns)),
                selectedMembers.ContainsKey(nameof(DataRelation.ParentKeyConstraint)),
                r => r.ParentColumns,
                r => r.ParentTable);
        }

        private static void CompareDataRelationConstraints(
            IEquivalencyValidator parent, IEquivalencyValidationContext context,
            DataRelation subject, DataRelation expectation, Dictionary<string, IMember> selectedMembers,
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
                CompareDataRelationKeyConstraint(subject, expectation, parent, context, selectedMembers, relationDirection);
            }
        }

        private static void CompareDataRelationColumns(DataRelation subject, DataRelation expectation,
            Func<DataRelation, DataColumn[]> getColumns)
        {
            DataColumn[] subjectColumns = getColumns(subject);
            DataColumn[] expectationColumns = getColumns(expectation);

            // These column references are in different tables in different data sets that _should_ be equivalent
            // to one another.
            bool success = AssertionScope.Current
                .ForCondition(subjectColumns.Length == expectationColumns.Length)
                .FailWith("Expected {context:DataRelation} to reference {0} column(s){reason}, but found {subjectColumns.Length}",
                    expectationColumns.Length, subjectColumns.Length);

            if (success)
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
                        .FailWith(
                            "Expected {context:DataRelation} to reference column {0} in table {1}{reason}, but found a reference to {2} in table {3} instead",
                            expectationColumn.ColumnName,
                            expectationColumn.Table.TableName,
                            subjectColumn.ColumnName,
                            subjectColumn.Table.TableName);
                }
            }
        }

        private static void CompareDataRelationTable(DataRelation subject, DataRelation expectation,
            Func<DataRelation, DataTable> getOtherTable)
        {
            DataTable subjectTable = getOtherTable(subject);
            DataTable expectationTable = getOtherTable(expectation);

            AssertionScope.Current
                .ForCondition(subjectTable.TableName == expectationTable.TableName)
                .FailWith("Expected {context:DataRelation} to reference a table named {0}{reason}, but found {1} instead",
                    expectationTable.TableName, subjectTable.TableName);
        }

        private static void CompareDataRelationKeyConstraint(DataRelation subject, DataRelation expectation,
            IEquivalencyValidator parent, IEquivalencyValidationContext context, Dictionary<string, IMember> selectedMembers,
            string relationDirection)
        {
            if (selectedMembers.TryGetValue(relationDirection + "KeyConstraint", out IMember expectationMember))
            {
                IMember subjectMember = FindMatchFor(expectationMember, context.CurrentNode, subject, context.Options);

                var newComparands = new Comparands
                {
                    Subject = subjectMember.GetValue(subject),
                    Expectation = expectationMember.GetValue(expectation),
                    CompileTimeType = expectationMember.Type
                };

                parent.RecursivelyAssertEquality(newComparands, context.AsNestedMember(expectationMember));
            }
        }

        private static IMember FindMatchFor(IMember selectedMemberInfo, INode currentNode, object subject,
            IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> query =
                from rule in config.MatchingRules
                let match = rule.Match(selectedMemberInfo, subject, currentNode, config)
                where match is not null
                select match;

            return query.FirstOrDefault();
        }

        private static IEnumerable<IMember> GetMembersFromExpectation(INode currentNode, Comparands comparands,
            IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> members = Enumerable.Empty<IMember>();

            foreach (IMemberSelectionRule rule in config.SelectionRules)
            {
                members = rule.SelectMembers(currentNode, members,
                    new MemberSelectionContext(comparands.CompileTimeType, comparands.RuntimeType, config));
            }

            return members;
        }
    }
}
