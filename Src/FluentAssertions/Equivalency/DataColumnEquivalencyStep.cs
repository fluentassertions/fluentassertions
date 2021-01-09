using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using FluentAssertions.Data;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class DataColumnEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return typeof(DataColumn).IsAssignableFrom(config.GetExpectationType(context.RuntimeType, context.CompileTimeType));
        }

        [SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "The code is easier to read without it.")]
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            var subject = context.Subject as DataColumn;
            var expectation = context.Expectation as DataColumn;

            if (expectation is null)
            {
                if (subject is not null)
                {
                    AssertionScope.Current.FailWith("Expected {context:DataColumn} value to be null, but found {0}", subject);
                }
            }
            else
            {
                if (subject is null)
                {
                    if (context.Subject is null)
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataColumn} to be non-null, but found null");
                    }
                    else
                    {
                        AssertionScope.Current.FailWith("Expected {context:DataColumn} to be of type {0}, but found {1} instead", expectation.GetType(), context.Subject.GetType());
                    }
                }
                else
                {
                    CompareSubjectAndExpectationOfTypeDataColumn(context, parent, config, subject);
                }
            }

            return true;
        }

        private static void CompareSubjectAndExpectationOfTypeDataColumn(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config, DataColumn subject)
        {
            bool compareColumn = true;

            var dataSetConfig = config as DataEquivalencyAssertionOptions<DataSet>;
            var dataTableConfig = config as DataEquivalencyAssertionOptions<DataTable>;
            var dataColumnConfig = config as DataEquivalencyAssertionOptions<DataColumn>;

            if (((dataSetConfig is not null) && dataSetConfig.ShouldExcludeColumn(subject))
             || ((dataTableConfig is not null) && dataTableConfig.ShouldExcludeColumn(subject))
             || ((dataColumnConfig is not null) && dataColumnConfig.ShouldExcludeColumn(subject)))
            {
                compareColumn = false;
            }

            if (compareColumn)
            {
                foreach (IMember expectationMember in GetMembersFromExpectation(context, config))
                {
                    if (expectationMember.Name != nameof(subject.Table))
                    {
                        CompareMember(expectationMember, parent, context, config);
                    }
                }
            }
        }

        private static void CompareMember(IMember expectationMember, IEquivalencyValidator parent, IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
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

        private static IMember FindMatchFor(IMember selectedMemberInfo, IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            IEnumerable<IMember> query =
                from rule in config.MatchingRules
                let match = rule.Match(selectedMemberInfo, context.Subject, context.CurrentNode, config)
                where match is not null
                select match;

            return query.FirstOrDefault();
        }

        // Note: This list of candidate members is duplicated in the XML documentation for the
        // DataColumn.BeEquivalentTo extension method in DataColumnAssertions.cs. If this ever
        // needs to change, keep them in sync.
        private static readonly ISet<string> CandidateMembers =
            new HashSet<string>()
            {
                nameof(DataColumn.AllowDBNull),
                nameof(DataColumn.AutoIncrement),
                nameof(DataColumn.AutoIncrementSeed),
                nameof(DataColumn.AutoIncrementStep),
                nameof(DataColumn.Caption),
                nameof(DataColumn.ColumnName),
                nameof(DataColumn.DataType),
                nameof(DataColumn.DateTimeMode),
                nameof(DataColumn.DefaultValue),
                nameof(DataColumn.Expression),
                nameof(DataColumn.ExtendedProperties),
                nameof(DataColumn.MaxLength),
                nameof(DataColumn.Namespace),
                nameof(DataColumn.Prefix),
                nameof(DataColumn.ReadOnly),
                nameof(DataColumn.Unique),
            };

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

            members = members.Where(member => CandidateMembers.Contains(member.Name));

            return members;
        }
    }
}
