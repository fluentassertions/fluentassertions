using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Equivalency
{
    public class ConstraintEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return typeof(Constraint).IsAssignableFrom(config.GetExpectationType(context.RuntimeType, context.CompileTimeType));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0038:Use pattern matching", Justification = "Would decrease code clarity")]
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            if (!(context.Subject is Constraint))
            {
                AssertionScope.Current
                    .FailWith("Expected {context:constraint} to be a value of type Constraint, but found {0}", context.Subject.GetType());
            }
            else
            {
                var subject = (Constraint)context.Subject;
                var expectation = (Constraint)context.Expectation;

                var selectedMembers = GetMembersFromExpectation(context, config)
                    .ToDictionary(member => member.Name);

                CompareCommonProperties(context, parent, config, subject, expectation, selectedMembers);

                bool matchingType = subject.GetType() == expectation.GetType();

                AssertionScope.Current
                    .ForCondition(matchingType)
                    .FailWith("Expected {context:constraint} to be of type {0}, but found {1}", expectation.GetType(), subject.GetType());

                if (matchingType)
                {
                    if ((subject is UniqueConstraint subjectUniqueConstraint)
                     && (expectation is UniqueConstraint expectationUniqueConstraint))
                    {
                        CompareConstraints(parent, context, subjectUniqueConstraint, expectationUniqueConstraint, selectedMembers);
                    }
                    else if ((subject is ForeignKeyConstraint subjectForeignKeyConstraint)
                          && (expectation is ForeignKeyConstraint expectationForeignKeyConstraint))
                    {
                        CompareConstraints(parent, context, subjectForeignKeyConstraint, expectationForeignKeyConstraint, selectedMembers);
                    }
                    else
                    {
                        AssertionScope.Current
                            .FailWith("Don't know how to handle {constraint:a Constraint} of type {0}", subject.GetType());
                    }
                }
            }

            return true;
        }

        private static void CompareCommonProperties(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config, Constraint subject, Constraint expectation, Dictionary<string, IMember> selectedMembers)
        {
            if (selectedMembers.ContainsKey("ConstraintName"))
            {
                AssertionScope.Current
                    .ForCondition(subject.ConstraintName == expectation.ConstraintName)
                    .FailWith("Expected {context:constraint} to have a ConstraintName of {0}{reason}, but found {1}", expectation.ConstraintName, subject.ConstraintName);
            }

            if (selectedMembers.ContainsKey("Table"))
            {
                AssertionScope.Current
                    .ForCondition(subject.Table.TableName == expectation.Table.TableName)
                    .FailWith("Expected {context:constraint} to be associated with a Table with TableName of {0}{reason}, but found {1}", expectation.Table.TableName, subject.Table.TableName);
            }

            if (selectedMembers.TryGetValue("ExtendedProperties", out IMember expectationMember))
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

        private static void CompareConstraints(IEquivalencyValidator parent, IEquivalencyValidationContext context, UniqueConstraint subject, UniqueConstraint expectation, Dictionary<string, IMember> selectedMembers)
        {
            AssertionScope.Current
                .ForCondition(subject.ConstraintName == expectation.ConstraintName)
                .FailWith("Expected {context:constraint} to be named {0}{reason}, but found {1}", expectation.ConstraintName, subject.ConstraintName);

            parent.AssertEqualityUsing(
                CreateNestedContext(context, nameof(subject.ExtendedProperties)));

            if (selectedMembers.ContainsKey(nameof(expectation.IsPrimaryKey)))
            {
                AssertionScope.Current
                    .ForCondition(subject.IsPrimaryKey == expectation.IsPrimaryKey)
                    .FailWith("Expected {context:constraint} to be a {0} constraint{reason}, but found a {1} constraint",
                        expectation.IsPrimaryKey ? "Primary Key" : "Foreign Key",
                        subject.IsPrimaryKey ? "Primary Key" : "Foreign Key");
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Columns)))
            {
                CompareConstraintColumns(subject.Columns, expectation.Columns);
            }
        }

        private static void CompareConstraints(IEquivalencyValidator parent, IEquivalencyValidationContext context, ForeignKeyConstraint subject, ForeignKeyConstraint expectation, Dictionary<string, IMember> selectedMembers)
        {
            AssertionScope.Current
                .ForCondition(subject.ConstraintName == expectation.ConstraintName)
                .FailWith("Expected {context:constraint} to be named {0}{reason}, but found {1}", expectation.ConstraintName, subject.ConstraintName);

            parent.AssertEqualityUsing(
                CreateNestedContext(context, nameof(subject.ExtendedProperties)));

            if (selectedMembers.ContainsKey(nameof(expectation.RelatedTable)))
            {
                AssertionScope.Current
                    .ForCondition(subject.RelatedTable.TableName == expectation.RelatedTable.TableName)
                    .FailWith("Expected {context:constraint} to have a related table named {0}{reason}, but found {1}", expectation.RelatedTable.TableName, subject.RelatedTable.TableName);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.AcceptRejectRule)))
            {
                AssertionScope.Current
                    .ForCondition(subject.AcceptRejectRule == expectation.AcceptRejectRule)
                    .FailWith("Expected {context:constraint} to have AcceptRejectRule.{0}{reason}, but found AcceptRejectRule.{1}", expectation.AcceptRejectRule, subject.AcceptRejectRule);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.DeleteRule)))
            {
                AssertionScope.Current
                    .ForCondition(subject.DeleteRule == expectation.DeleteRule)
                    .FailWith("Expected {context:constraint} to have DeleteRule Rule.{0}{reason}, but found Rule.{1}", expectation.DeleteRule, subject.DeleteRule);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.UpdateRule)))
            {
                AssertionScope.Current
                    .ForCondition(subject.UpdateRule == expectation.UpdateRule)
                    .FailWith("Expected {context:constraint} to have UpdateRule Rule.{0}{reason}, but found Rule.{1}", expectation.UpdateRule, subject.UpdateRule);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.Columns)))
            {
                CompareConstraintColumns(subject.Columns, expectation.Columns);
            }

            if (selectedMembers.ContainsKey(nameof(expectation.RelatedColumns)))
            {
                CompareConstraintColumns(subject.RelatedColumns, expectation.RelatedColumns);
            }
        }

        private static void CompareConstraintColumns(DataColumn[] subjectColumns, DataColumn[] expectationColumns)
        {
            var subjectColumnNames = new HashSet<string>(subjectColumns.Select(col => col.ColumnName));
            var expectationColumnNames = new HashSet<string>(expectationColumns.Select(col => col.ColumnName));

            var missingColumnNames = expectationColumnNames.Except(subjectColumnNames).ToList();
            var extraColumnNames = subjectColumnNames.Except(expectationColumnNames).ToList();

            var failureMessage = new StringBuilder();

            if (missingColumnNames.Any())
            {
                failureMessage.Append("Expected {context:constraint} to include ");

                if (missingColumnNames.Count == 1)
                {
                    failureMessage.Append("column ").Append(missingColumnNames.Single());
                }
                else
                {
                    failureMessage.Append("columns ").Append(missingColumnNames.JoinUsingWritingStyle());
                }

                failureMessage.Append("{reason}, but constraint does not include ");
                failureMessage.Append((missingColumnNames.Count == 1)
                    ? "that column. "
                    : "these columns. ");
            }

            if (extraColumnNames.Any())
            {
                failureMessage.Append("Did not expect {context:constraint} to include ");

                if (extraColumnNames.Count == 1)
                {
                    failureMessage.Append("column ").Append(extraColumnNames.Single());
                }
                else
                {
                    failureMessage.Append("columns ").Append(extraColumnNames.JoinUsingWritingStyle());
                }

                failureMessage.Append("{reason}, but it does.");
            }

            bool successful = failureMessage.Length == 0;

            AssertionScope.Current
                .ForCondition(successful)
                .FailWith(failureMessage.ToString());
        }

        private static IEquivalencyValidationContext CreateNestedContext(IEquivalencyValidationContext context, string name)
        {
            var nestedMember = new Property(
                typeof(Constraint).GetProperty(name),
                context.CurrentNode);

            return context.AsNestedMember(nestedMember, nestedMember);
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
                // Within a ConstraintCollection, different types of Constraint are kept polymorphically.
                // As such, the concept of "compile-time type" isn't meaningful, and we override this
                // with the discovered type of the constraint at runtime.
                members = rule.SelectMembers(context.CurrentNode, members, new MemberSelectionContext
                {
                    CompileTimeType = context.RuntimeType, // intentional
                    RuntimeType = context.RuntimeType,
                    Options = config
                });
            }

            return members;
        }
    }
}
