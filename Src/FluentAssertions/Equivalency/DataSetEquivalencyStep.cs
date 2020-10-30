using System.Collections.Generic;
using System.Data;
using System.Linq;

using FluentAssertions.Data;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class DataSetEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return typeof(DataSet).IsAssignableFrom(config.GetExpectationType(context.RuntimeType, context.CompileTimeType));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "The code is easier to read without it.")]
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            var subject = context.Subject as DataSet;
            var expectation = context.Expectation as DataSet;

            if (expectation == null)
            {
                if (subject != null)
                {
                    AssertionScope.Current.FailWith("Expected {context:DataSet} value to be null, but found {0}", subject);
                }
            }
            else
            {
                if (subject == null)
                {
                    if (context.Subject == null)
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataSet} to be non-null, but found null");
                    }
                    else
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataSet} to be of type {0}, but found {1} instead", expectation.GetType(), context.Subject.GetType());
                    }
                }
                else
                {
                    var dataConfig = config as DataEquivalencyAssertionOptions<DataSet>;

                    if ((dataConfig == null) || !dataConfig.AllowMismatchedTypes)
                    {
                        AssertionScope.Current
                            .ForCondition(subject.GetType() == expectation.GetType())
                            .FailWith("Expected {context:DataSet} to be of type '{0}'{reason}, but found '{1}'", expectation.GetType(), subject.GetType());
                    }

                    var selectedMembers = GetMembersFromExpectation(context, config)
                        .ToDictionary(member => member.Name);

                    CompareScalarProperties(subject, expectation, selectedMembers);

                    CompareCollections(context, parent, config, subject, expectation, dataConfig, selectedMembers);
                }
            }

            return true;
        }

        private static void CompareScalarProperties(DataSet subject, DataSet expectation, Dictionary<string, IMember> selectedMembers)
        {
            // Note: The members here are listed in the XML documentation for the DataSet.BeEquivalentTo extension
            // method in DataSetAssertions.cs. If this ever needs to change, keep them in sync.
            if (selectedMembers.ContainsKey(nameof(expectation.DataSetName)))
            {
                AssertionScope.Current
                    .ForCondition(subject.DataSetName == expectation.DataSetName)
                    .FailWith("Expected {context:DataSet} to have DataSetName '{0}'{reason}, but found '{1}' instead", expectation.DataSetName, subject.DataSetName);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.CaseSensitive)))
            {
                AssertionScope.Current
                    .ForCondition(subject.CaseSensitive == expectation.CaseSensitive)
                    .FailWith("Expected {context:DataSet} to have CaseSensitive value of '{0}'{reason}, but found '{1}' instead", expectation.CaseSensitive, subject.CaseSensitive);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.EnforceConstraints)))
            {
                AssertionScope.Current
                    .ForCondition(subject.EnforceConstraints == expectation.EnforceConstraints)
                    .FailWith("Expected {context:DataSet} to have EnforceConstraints value of '{0}'{reason}, but found '{1}' instead", expectation.EnforceConstraints, subject.EnforceConstraints);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.HasErrors)))
            {
                AssertionScope.Current
                    .ForCondition(subject.HasErrors == expectation.HasErrors)
                    .FailWith("Expected {context:DataSet} to have HasErrors value of '{0}'{reason}, but found '{1}' instead", expectation.HasErrors, subject.HasErrors);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Locale)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Locale == expectation.Locale)
                    .FailWith("Expected {context:DataSet} to have Locale value of '{0}'{reason}, but found '{1}' instead", expectation.Locale, subject.Locale);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Namespace)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Namespace == expectation.Namespace)
                    .FailWith("Expected {context:DataSet} to have Namespace value of '{0}'{reason}, but found '{1}' instead", expectation.Namespace, subject.Namespace);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Prefix)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Prefix == expectation.Prefix)
                    .FailWith("Expected {context:DataSet} to have Prefix value of '{0}'{reason}, but found '{1}' instead", expectation.Prefix, subject.Prefix);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.RemotingFormat)))
            {
                AssertionScope.Current
                    .ForCondition(subject.RemotingFormat == expectation.RemotingFormat)
                    .FailWith("Expected {context:DataSet} to have RemotingFormat value of '{0}'{reason}, but found '{1}' instead", expectation.RemotingFormat, subject.RemotingFormat);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.SchemaSerializationMode)))
            {
                AssertionScope.Current
                    .ForCondition(subject.SchemaSerializationMode == expectation.SchemaSerializationMode)
                    .FailWith("Expected {context:DataSet} to have SchemaSerializationMode value of '{0}'{reason}, but found '{1}' instead", expectation.SchemaSerializationMode, subject.SchemaSerializationMode);
            }
        }

        private static void CompareCollections(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config, DataSet subject, DataSet expectation, DataEquivalencyAssertionOptions<DataSet> dataConfig, Dictionary<string, IMember> selectedMembers)
        {
            // Note: The collections here are listed in the XML documentation for the DataSet.BeEquivalentTo extension
            // method in DataSetAssertions.cs. If this ever needs to change, keep them in sync.
            CompareExtendedProperties(context, parent, config, expectation, selectedMembers);

            CompareTables(context, parent, subject, expectation, dataConfig, selectedMembers);
        }

        private static void CompareExtendedProperties(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config, DataSet expectation, Dictionary<string, IMember> selectedMembers)
        {
            foreach (var collectionName in new[] { nameof(expectation.ExtendedProperties), nameof(expectation.Relations) })
            {
                if (selectedMembers.TryGetValue(collectionName, out var expectationMember))
                {
                    var matchingMember = FindMatchFor(expectationMember, context, config);

                    if (matchingMember != null)
                    {
                        IEquivalencyValidationContext nestedContext =
                                context.AsNestedMember(expectationMember, matchingMember);

                        if (nestedContext != null)
                        {
                            parent.AssertEqualityUsing(nestedContext);
                        }
                    }
                }
            }
        }

        private static void CompareTables(IEquivalencyValidationContext context, IEquivalencyValidator parent, DataSet subject, DataSet expectation, DataEquivalencyAssertionOptions<DataSet> dataConfig, Dictionary<string, IMember> selectedMembers)
        {
            if (selectedMembers.ContainsKey(nameof(expectation.Tables)))
            {
                AssertionScope.Current
                    .ForCondition(subject.Tables.Count == expectation.Tables.Count)
                    .FailWith("Expected {context:DataSet} to contain {0}, but found {1} table(s)", expectation.Tables.Count, subject.Tables.Count);

                if (dataConfig != null)
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

                var expectationTableNames = expectation.Tables.OfType<DataTable>()
                    .Select(table => table.TableName);
                var subjectTableNames = subject.Tables.OfType<DataTable>()
                    .Select(table => table.TableName);

                foreach (string tableName in expectationTableNames.Union(subjectTableNames))
                {
                    if ((dataConfig != null) && dataConfig.ExcludeTableNames.Contains(tableName))
                    {
                        continue;
                    }

                    var expectationTable = expectation.Tables[tableName];
                    var subjectTable = subject.Tables[tableName];

                    AssertionScope.Current
                        .ForCondition(subjectTable != null)
                        .FailWith("Expected {context:DataSet} to contain table '{0}'{reason}, but did not find it", tableName);

                    AssertionScope.Current
                        .ForCondition(expectationTable != null)
                        .FailWith("Found unexpected table '{0}' in DataSet", tableName);

                    var nestedContext = context.AsCollectionItem(
                        tableName,
                        subjectTable,
                        expectationTable);

                    if (nestedContext != null)
                    {
                        parent.AssertEqualityUsing(nestedContext);
                    }
                }
            }
        }

        private static IMember FindMatchFor(IMember selectedMemberInfo, IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> query =
                from rule in config.MatchingRules
                let match = rule.Match(selectedMemberInfo, context.Subject, context.CurrentNode, config)
                where match != null
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
