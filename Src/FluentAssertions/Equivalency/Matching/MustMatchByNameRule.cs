using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Matching
{
    /// <summary>
    /// Requires the subject to have a member with the exact same name as the expectation has.
    /// </summary>
    internal class MustMatchByNameRule : IMemberMatchingRule
    {
        public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyAssertionOptions config)
        {
            IMember subjectMember = null;

            if (config.IncludeProperties)
            {
                PropertyInfo propertyInfo = subject.GetType().FindProperty(expectedMember.Name, expectedMember.Type);
                subjectMember = (propertyInfo != null) && !propertyInfo.IsIndexer() ? (IMember)new Property(propertyInfo, parent) : null;
            }

            if ((subjectMember is null) && config.IncludeFields)
            {
                FieldInfo fieldInfo = subject.GetType().FindField(expectedMember.Name, expectedMember.Type);
                subjectMember = (fieldInfo != null) ? (IMember)new Field(fieldInfo, parent) : null;
            }

            if ((subjectMember is null || !config.UseRuntimeTyping) && ExpectationImplementsMemberExplicitly(subject, expectedMember))
            {
                subjectMember = expectedMember;
            }

            if (subjectMember is null)
            {
                Execute.Assertion.FailWith(
                    $"Expectation has {expectedMember.Description} that the other object does not have.");
            }

            return subjectMember;
        }

        private static bool ExpectationImplementsMemberExplicitly(object expectation, IMember subjectMember)
        {
            return subjectMember.DeclaringType.IsInstanceOfType(expectation);
        }

        /// <inheritdoc />
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Match member by name (or throw)";
        }
    }
}
