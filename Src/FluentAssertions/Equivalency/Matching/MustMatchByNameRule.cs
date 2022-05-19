using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Matching;

/// <summary>
/// Requires the subject to have a member with the exact same name as the expectation has.
/// </summary>
internal class MustMatchByNameRule : IMemberMatchingRule
{
    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyAssertionOptions config)
    {
        IMember subjectMember = null;

        if (config.IncludedProperties != MemberVisibility.None)
        {
            PropertyInfo propertyInfo = subject.GetType().FindProperty(expectedMember.Name, expectedMember.Type);
            subjectMember = (propertyInfo is not null) && !propertyInfo.IsIndexer() ? new Property(propertyInfo, parent) : null;
        }

        if ((subjectMember is null) && config.IncludedFields != MemberVisibility.None)
        {
            FieldInfo fieldInfo = subject.GetType().FindField(expectedMember.Name, expectedMember.Type);
            subjectMember = (fieldInfo is not null) ? new Field(fieldInfo, parent) : null;
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

        if (config.IgnoreNonBrowsableOnSubject && !subjectMember.IsBrowsable)
        {
            Execute.Assertion.FailWith(
                $"Expectation has {expectedMember.Description} that is non-browsable in the other object, and non-browsable " +
                $"members on the subject are ignored with the current configuration");
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
