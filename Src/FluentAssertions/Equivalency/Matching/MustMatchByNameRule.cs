using System.Reflection;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Equivalency.Matching;

/// <summary>
/// Requires the subject to have a member with the exact same name as the expectation has.
/// </summary>
internal class MustMatchByNameRule : IMemberMatchingRule
{
    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyOptions options)
    {
        IMember subjectMember = null;

        if (options.IncludedProperties != MemberVisibility.None)
        {
            PropertyInfo propertyInfo = subject.GetType().FindProperty(
                expectedMember.Name,
                options.IncludedProperties | MemberVisibility.ExplicitlyImplemented);

            subjectMember = propertyInfo is not null && !propertyInfo.IsIndexer() ? new Property(propertyInfo, parent) : null;
        }

        if (subjectMember is null && options.IncludedFields != MemberVisibility.None)
        {
            FieldInfo fieldInfo = subject.GetType().FindField(
                expectedMember.Name,
                options.IncludedFields);

            subjectMember = fieldInfo is not null ? new Field(fieldInfo, parent) : null;
        }

        if (subjectMember is null)
        {
            Execute.Assertion.FailWith(
                $"Expectation has {expectedMember.Description} that the other object does not have.");
        }
        else if (options.IgnoreNonBrowsableOnSubject && !subjectMember.IsBrowsable)
        {
            Execute.Assertion.FailWith(
                $"Expectation has {expectedMember.Description} that is non-browsable in the other object, and non-browsable " +
                "members on the subject are ignored with the current configuration");
        }
        else
        {
            // Everything is fine
        }

        return subjectMember;
    }

    /// <inheritdoc />
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return "Match member by name (or throw)";
    }
}
