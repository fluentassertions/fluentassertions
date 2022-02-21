using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions.Data;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    public class DataSetEquivalencyStep : EquivalencyStep<DataSet>
    {
        protected override EquivalencyResult OnHandle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            var subject = comparands.Subject as DataSet;
            var expectation = comparands.Expectation as DataSet;

            if (expectation is null)
            {
                if (subject is not null)
                {
                    AssertionScope.Current.FailWith("Expected {context:DataSet} value to be null, but found {0}", subject);
                }
            }
            else
            {
                if (subject is null)
                {
                    if (comparands.Subject is null)
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataSet} to be non-null, but found null");
                    }
                    else
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataSet} to be of type {0}, but found {1} instead", expectation.GetType(), comparands.Subject.GetType());
                    }
                }
                else
                {
                    var dataConfig = context.Options as DataEquivalencyAssertionOptions<DataSet>;

                    if (dataConfig?.AllowMismatchedTypes != true)
                    {
                        AssertionScope.Current
                            .ForCondition(subject.GetType() == expectation.GetType())
                            .FailWith("Expected {context:DataSet} to be of type {0}{reason}, but found {1}", expectation.GetType(), subject.GetType());
                    }

                    var selectedMembers = GetMembersFromExpectation(comparands, context.CurrentNode, context.Options)
                        .ToDictionary(member => member.Name);

                    CompareScalarProperties(subject, expectation, selectedMembers);

                    CompareCollections(context, nestedValidator, context.Options, subject, expectation, dataConfig, selectedMembers);
                }
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static void CompareScalarProperties(DataSet subject, DataSet expectation, Dictionary<string, IMember> selectedMembers)
        {
            // Note: The members here are listed in the XML documentation for the DataSet.BeEquivalentTo extension
            // method in DataSetAssertions.cs. If this ever needs to change, keep them in sync.
            if (selectedMembers.ContainsKey(nameof(expectation.DataSetName)))
            {
                AssertionScope.Current
                    .ForCondition(subject.DataSetName == expectation.DataSetName)
                    .FailWith("Expected {context:DataSet} to have DataSetName {0}{reason}, but found {1} instead", expectation.DataSetName, subject.DataSetName);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.CaseSensitive)))
            {
                AssertionScope.Current
                    .ForCondition(subject.CaseSensitive == expectation.CaseSensitive)
                    .FailWith("Expected {context:DataSet} to have CaseSensitive value of {0}{reason}, but found {1} instead", expectation.CaseSensitive, subject.CaseSensitive);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.EnforceConstraints)))
            {
                AssertionScope.Current
                    .ForCondition(subject.EnforceConstraints == expectation.EnforceConstraints)
                    .FailWith("Expected {context:DataSet} to have EnforceConstraints value of {0}{reason}, but found {1} instead", expectation.EnforceConstraints, subject.EnforceConstraints);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.HasErrors)))
            {
                AssertionScope.Current
                    .ForCondition(subject.HasErrors == expectation.HasErrors)
                    .FailWith("Expected {context:DataSet} to have HasErrors value of {0}{reason}, but found {1} instead", expectation.HasErrors, subject.HasErrors);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Locale)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Locale == expectation.Locale)
                    .FailWith("Expected {context:DataSet} to have Locale value of {0}{reason}, but found {1} instead", expectation.Locale, subject.Locale);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Namespace)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Namespace == expectation.Namespace)
                    .FailWith("Expected {context:DataSet} to have Namespace value of {0}{reason}, but found {1} instead", expectation.Namespace, subject.Namespace);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Prefix)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Prefix == expectation.Prefix)
                    .FailWith("Expected {context:DataSet} to have Prefix value of {0}{reason}, but found {1} instead", expectation.Prefix, subject.Prefix);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.RemotingFormat)))
            {
                AssertionScope.Current
                    .ForCondition(subject.RemotingFormat == expectation.RemotingFormat)
                    .FailWith("Expected {context:DataSet} to have RemotingFormat value of {0}{reason}, but found {1} instead", expectation.RemotingFormat, subject.RemotingFormat);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.SchemaSerializationMode)))
            {
                AssertionScope.Current
                    .ForCondition(subject.SchemaSerializationMode == expectation.SchemaSerializationMode)
                    .FailWith("Expected {context:DataSet} to have SchemaSerializationMode value of {0}{reason}, but found {1} instead", expectation.SchemaSerializationMode, subject.SchemaSerializationMode);
            }
        }

        private static void CompareCollections(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config, DataSet subject, DataSet expectation, DataEquivalencyAssertionOptions<DataSet> dataConfig, Dictionary<string, IMember> selectedMembers)
        {
            // Note: The collections here are listed in the XML documentation for the DataSet.BeEquivalentTo extension
            // method in DataSetAssertions.cs. If this ever needs to change, keep them in sync.
            CompareExtendedProperties(new Comparands(subject, expectation, typeof(DataSet)), context, parent, config, selectedMembers);

            CompareTables(context, parent, subject, expectation, dataConfig, selectedMembers);
        }

        private static void CompareExtendedProperties(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config, Dictionary<string, IMember> selectedMembers)
        {
            foreach (var collectionName in new[] { nameof(DataSet.ExtendedProperties), nameof(DataSet.Relations) })
            {
                if (selectedMembers.TryGetValue(collectionName, out IMember expectationMember))
                {
                    IMember matchingMember = FindMatchFor(expectationMember, comparands.Subject, context.CurrentNode, config);
                    if (matchingMember is not null)
                    {
                        var nestedComparands = new Comparands
                        {
                            Subject = matchingMember.GetValue(comparands.Subject),
                            Expectation = expectationMember.GetValue(comparands.Expectation),
                            CompileTimeType = expectationMember.Type
                        };

                        IEquivalencyValidationContext nestedContext = context.AsNestedMember(expectationMember);
                        parent.RecursivelyAssertEquality(nestedComparands, nestedContext);
                    }
                }
            }
        }

        private static void CompareTables(IEquivalencyValidationContext context, IEquivalencyValidator parent, DataSet subject, DataSet expectation, DataEquivalencyAssertionOptions<DataSet> dataConfig, Dictionary<string, IMember> selectedMembers)
        {
            if (selectedMembers.ContainsKey(nameof(expectation.Tables)))
            {
                bool success = AssertionScope.Current
                    .ForCondition(subject.Tables.Count == expectation.Tables.Count)
                    .FailWith("Expected {context:DataSet} to contain {0}, but found {1} table(s)", expectation.Tables.Count, subject.Tables.Count);

                if (!success)
                {
                    return;
                }

                if (dataConfig is not null)
                {
                    bool excludeCaseSensitive = !selectedMembers.ContainsKey(nameof(DataSet.CaseSensitive));
                    bool excludeLocale = !selectedMembers.ContainsKey(nameof(DataSet.Locale));

                    if (excludeCaseSensitive || excludeLocale)
                    {
                        dataConfig.Excluding((IMemberInfo memberInfo) =>
                                (memberInfo.DeclaringType == typeof(DataTable)) &&
                            (
                                    (excludeCaseSensitive && (memberInfo.Name == nameof(DataTable.CaseSensitive)))
                            ||
                                    (excludeLocale && (memberInfo.Name == nameof(DataTable.Locale)))));
                    }
                }

                IEnumerable<string> expectationTableNames = expectation.Tables.Cast<DataTable>()
                    .Select(table => table.TableName);
                IEnumerable<string> subjectTableNames = subject.Tables.Cast<DataTable>()
                    .Select(table => table.TableName);

                foreach (string tableName in expectationTableNames.Union(subjectTableNames))
                {
                    if (dataConfig?.ExcludeTableNames.Contains(tableName) == true)
                    {
                        continue;
                    }

                    CompareTable(context, parent, subject, expectation, tableName);
                }
            }
        }

        private static void CompareTable(IEquivalencyValidationContext context, IEquivalencyValidator parent, DataSet subject,
            DataSet expectation, string tableName)
        {
            DataTable expectationTable = expectation.Tables[tableName];
            DataTable subjectTable = subject.Tables[tableName];

            bool success = AssertionScope.Current
                .ForCondition(subjectTable is not null)
                .FailWith("Expected {context:DataSet} to contain table {0}{reason}, but did not find it", tableName)
                .Then
                .ForCondition(expectationTable is not null)
                .FailWith("Found unexpected table {0} in DataSet", tableName);

            if (success)
            {
                IEquivalencyValidationContext nestedContext = context.AsCollectionItem<DataTable>(tableName);
                parent.RecursivelyAssertEquality(new Comparands(subjectTable, expectationTable, typeof(DataTable)), nestedContext);
            }
        }

        private static IMember FindMatchFor(IMember selectedMemberInfo, object subject, INode currentNode, IEquivalencyAssertionOptions options)
        {
            IEnumerable<IMember> query =
                from rule in options.MatchingRules
                let match = rule.Match(selectedMemberInfo, subject, currentNode, options)
                where match is not null
                select match;

            return query.FirstOrDefault();
        }

        private static IEnumerable<IMember> GetMembersFromExpectation(Comparands comparands,
            INode contextCurrentNode,
            IEquivalencyAssertionOptions options)
        {
            IEnumerable<IMember> members = Enumerable.Empty<IMember>();

            foreach (IMemberSelectionRule rule in options.SelectionRules)
            {
                members = rule.SelectMembers(contextCurrentNode, members,
                    new MemberSelectionContext(comparands.CompileTimeType, comparands.RuntimeType, options));
            }

            return members;
        }
    }
}
